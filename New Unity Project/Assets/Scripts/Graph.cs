using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Graph : MonoBehaviour
{
    public static Graph instance;
    [Range(0, 0)]
    public float Grain;
    [Range(0, 5)]
    public float GrainFactor ;
    [Range(0,5)]
    public float Radius;
    [Range(0, 5)]
    public float Slope;
    public int Size  = 15;
    public GameObject Node;
    public Transform nodes;
    public Node[,] nodesMatriz;
    public List<Node> nodesList = new List<Node>();
    int gridSize;
    float nodeDiametre;


    void Awake ()
    {
        instance = this;
       
    }

    private void Start()
    {
        nodeDiametre = Radius * 2;
        Grain = (nodeDiametre / GrainFactor);
        gridSize = Mathf.RoundToInt((Size * Size) / Grain);

    }


    private void FixedUpdate()
    {

    }

    public void salvarGrafo()
    {
        XmlSerializer serializador = new XmlSerializer(typeof(List<Node>));
        StreamWriter arqDados = new StreamWriter("Grafo.xml");
        serializador.Serialize(arqDados.BaseStream, nodesList);
        arqDados.Close();
    }

    public void carregarGrafo()
    {
        XmlSerializer serializador = new XmlSerializer(typeof(List<Node>));
        StreamReader arqLeit = new StreamReader("Grafo.xml");
        nodesList= (List<Node>) serializador.Deserialize(arqLeit.BaseStream);
        arqLeit.Close();



    }

    void instanciarNovos()
    {
        for (int i = 0; i < nodes.childCount; i++)
        {
            Destroy(nodes.GetChild(i));
        }
        RaycastHit ray;
        foreach (Node node in nodesList)
        {
            if (Physics.Raycast(new Vector3(node.posX, 0, node.posY), Vector3.down, out ray, 100.0f))
            {
                Instantiate(Node, ray.point, Quaternion.identity, nodes);
            }
        }
    }



    public void GenerateGraph2()
    {
        nodesMatriz = new Node[gridSize, gridSize];
        Vector3 worldBottomLeft = transform.position - Vector3.right * Size / 2 - Vector3.forward * Size / 2;

        RaycastHit ray;

        for (int i = 0; i <= gridSize; i++)
        {
            for (int j = 0; j <= gridSize; j++)
            {
                Vector3 worldPoint = (worldBottomLeft + Vector3.right *( i * Grain)  + Vector3.forward * (j * Grain));
                if (Physics.Raycast(worldPoint, Vector3.down, out ray, 100.0f))
                {
                    GameObject aux = Instantiate(Node, ray.point, Quaternion.identity,nodes);
                    aux.GetComponent<Node>().posX = i;
                    aux.GetComponent<Node>().posY = j;
                    nodesList.Add(aux.GetComponent<Node>());
                }
            }
        }

        foreach (Node node in nodesList)
        {
            node.vizinhos = nodeVizinhos(node);
        }
    }

    public List<Node> nodeVizinhos(Node node)
    {
        List<Node> vizinhos = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 & y == 0)
                    continue;
                float checkX = node.posX + x;
                float checkY = node.posY + y;
                if (checkX >= 0 && checkX < gridSize && checkY >= 0 && checkY < gridSize)
                {
                    vizinhos.Add(nodesList.Find(z => z.posX == checkX && z.posY == checkY));
                }
                    
            }
        }
        return vizinhos;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(Size, 1, Size));
        Gizmos.color = Color.red;
        foreach (Node node in nodesList)
        {
            foreach (Node vizinho in node.vizinhos)
            {
                if (vizinho != null)
                {                 
                    Debug.DrawLine(node.transform.position, vizinho.transform.position);
                }
            }
        }

        
    }

}
