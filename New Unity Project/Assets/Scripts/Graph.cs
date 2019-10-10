using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    Vector3 worldPoint;
    Vector3 worldBottomLeft;

    public static Graph instance;
    [Range(0, 1)]
    public float Grain; // Granuliadade 
    [Range(0, 5)]
    public float GrainFactor; //Quanto de granuliadade vai ter
    [Range(0, 5)]
    public float Radius; // Raio de cada nodo
    [HideInInspector]
    [Range(0.01f, 5)]
    public float Slope; // Altura de cada nodo

    public int Size = 15; // Area do grid

    int gridSize; //Tamanho do grid

    float nodeDiametre;//Diametro do Nodo

    public GameObject Node; // GameObject do nodo 

    List<GameObject> nodesGO = new List<GameObject>();

    public Transform nodes; //Organizador de nodes

    public List<Node> nodesList = new List<Node>(); //Lista de nodes criados

    Button saveB, criarB, loadB;
    Slider slopeS;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Usado pra atualizar os botões no canvas, quando carregado o grafo salvo
        criarB = GameObject.Find("Criar").GetComponent<Button>();
        saveB = GameObject.Find("Salvar").GetComponent<Button>();
        loadB = GameObject.Find("Carregar").GetComponent<Button>();
        slopeS = GameObject.Find("Slope").GetComponent<Slider>();

        criarB.onClick.AddListener(GenerateGraph2);
        saveB.onClick.AddListener(SalvarGrafo);
        loadB.onClick.AddListener(CarregarGrafoSalvo);

        //

        //Definindo valor mínimo e máximo pro slope (slope é até q grau/altura é permitido a conexão entre vértices/nodos
        slopeS.minValue = 0.01f; 
        slopeS.maxValue = 5;
        slopeS.value = Slope;

        nodeDiametre = Radius * 2;

        Grain = (nodeDiametre / GrainFactor);

        gridSize = Mathf.RoundToInt((Size * Size) / Grain); // Calculo de quantos nodes podem ter dentro de uma area do grid
    }

    void Update()
    {
        Slope = slopeS.value;
    }

    public void SalvarGrafo()
    {
        string localPath = "Assets/Resources/Save/" + "Graph" + ".prefab";

        //localPath = AssetDatabase.GenerateUniqueAssetPath(localPath); //Geraria um asset com novo nome, evitando substituição de arquivo existente

        // Cria um novo prefab
        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
    }

    public void CarregarGrafoSalvo()
    {
        var localPath = Resources.Load("Save/" + "Graph"); //Busca onde está salvo o grafo
        Instantiate(localPath);
        Destroy(gameObject);
    }

    /// <summary>
    /// Criar um grafo
    /// </summary>
    public void GenerateGraph2()
    {
        //Calculo o ponto esquerdo do meu grid
        worldBottomLeft = transform.position - Vector3.right * Size / 2 - Vector3.forward * Size / 2;

        RaycastHit ray;
        for (int i = 0; i <= gridSize; i++)
        {   // Rodo 2 loops para o eixo X e Y do nosso grid, veja que da forma que esta nosso grid sempre vai ser quadrado s
            for (int j = 0; j <= gridSize; j++)
            {

                //Calculo o ponto no mundo, somando o ponto esquerdo do meu grid com o calculo mt loco ai pra saber cada ponto no meu grid
                worldPoint = (worldBottomLeft + Vector3.right * (i * Grain) + Vector3.forward * (j * Grain));

                //Depois jogo meu raycast apartir desse ponto para baixo com o range de 100 caso acerte em algo eu faço o que esta dentro do if
                if (Physics.Raycast(worldPoint, Vector3.down, out ray, 100.0f))
                {
                    //instancio uma bolinha no ponto onde o raio bateu
                    GameObject aux = Instantiate(Node, ray.point, Quaternion.identity, nodes);
                    //Guardo as posições x e y dela para uso futuro dentro dela mesmo
                    aux.GetComponent<Node>().posX = i;
                    aux.GetComponent<Node>().posY = j;
                    //Adciono o node instanciado na lista de nodes ^-^
                    nodesList.Add(aux.GetComponent<Node>());
                    nodesGO.Add(aux);
                }
            }
        }

        //Essa parte é apenas pra evitar problemas de List com posição vazia, limpando ela toda antes de adicionar novos elementos
        for (int i = 0; i < nodesList.Count; i++)
        {
            nodesList[i].vizinhos.Clear();
        }
        
        foreach (Node node in nodesList)
        {
            node.vizinhos = nodeVizinhos(node);
        }

    }


    /// <summary>
    /// Retorna todos os vizinhos de um determinado node
    /// </summary>
    /// <param name="node"> Node </param>
    /// <returns>Uma lista de vizinhos </returns>
    public List<Node> nodeVizinhos(Node node)
    {
        List<Node> vizinhos = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 & y == 0)
                    continue;
                float checkX = node.posX + x; //a posição do meu node mais 1 no eixo x 
                float checkY = node.posY + y;//a posição do meu node mais 1 no eixo y 
                if (checkX >= 0 && checkX < gridSize && checkY >= 0 && checkY < gridSize)
                {
                    //Adciono na lista de vizinhos um node que possui a posição igual a da checkX e checkY
                    vizinhos.Add(nodesList.Find(z => z.posX == checkX && z.posY == checkY));
                    
                }

            }
        }
        return vizinhos;
    }

    /// <summary>
    /// Função da unity usada geralmente parar desenhar coisas no scene, usada no projeto para desenhar a area que vai ser 
    /// gerado o grafo e as arestas 
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(Size, 1, Size));

        foreach (Node node in nodesList)
        {
            foreach (Node vizinho in node.vizinhos)
            {
                if (vizinho != null)
                {
                    if (node.walkable && vizinho.walkable)
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;
                    Gizmos.DrawLine(node.transform.position, vizinho.transform.position);
                }
            }
        }


    }
    
}