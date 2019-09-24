using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public Node[,] grid; // matriz de nodes
    public Vector2 gridWorldSize; // tamanho do grid
    public float nodeRadius; // Raio do node
    public LayerMask unWalkableMask; // Layer de não andavel

    [Range(0,3)]
    public float Slope=0.2f;

    float nodeDiametre; // diametro do node 
    int gridSizeX, gridSizeY; 

    private void Start()
    {
        nodeDiametre = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiametre); // cria a quantidade de nodes possiveis de serem criados no eixo x 
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiametre); // cria a quantidade de nodes possiveis de serem criados no eixo z 
        CreateGrid();
    }

    /// <summary>
    /// Retorna todos os vizinhos de um determinado node 
    /// </summary>
    /// <param name="node"> o node </param>
    /// <returns> lista de vizinhos </returns>
    public List<Node> nodeVizinhos(Node node)
    {
        List<Node> vizinhos = new List<Node>();
        for (int x = -1; x <=1 ; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 & y == 0)
                    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    vizinhos.Add(grid[checkX,checkY]);
            }
        }
        return vizinhos;
    }

    /// <summary>
    /// Cria o grid
    /// </summary>
    void CreateGrid()
    {

        grid = new Node[gridSizeX, gridSizeY]; // crio nosso grid com o numero maximo de node em cada eixo
        Vector3 worldBottomLeft = transform.position - Vector3.right*gridWorldSize.x/2 - Vector3.forward*gridWorldSize.y/2; // me retorna o canto esquerdo do nosso grid
        RaycastHit ray;
        bool walkable =false;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiametre + nodeRadius) + Vector3.forward * (y * nodeDiametre + nodeRadius) + Vector3.down;// me retorna cada ponto no mundo

                if (Physics.Raycast(worldPoint,Vector3.up,out ray,unWalkableMask))
                {
                    if (ray.collider.gameObject.tag != "Player")
                        worldPoint.y = 1 + ray.point.y;
                    else
                        worldPoint.y = 0;
                }
                else
                {
                    worldPoint.y = 0;
                }

                walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkableMask)); // checo usando o chekSphere se colidiu com algum obstaculo setando assim a variavel walkable como true ou false
                grid[x, y] = new Node(walkable, worldPoint,x,y); // Crio o nove na cordena x,y passando o bool se é walkable ou nao e passando tambem sua posição no mundo
            }
        }

        foreach(Node node in grid)
        {
            List<Node> vizinhos = nodeVizinhos(node);

            foreach (Node nodeV in vizinhos)
            {
                float distY = nodeV.worldPosition.y - node.worldPosition.y;
                if (distY > Slope)
                    nodeV.walkable = false;
            }
        }

    }

    /// <summary>
    /// Retorno um node apartir de uma posição do mundo 
    /// </summary>
    /// <param name="worldPosition">posição </param>
    /// <returns>Node</returns>
    public Node nodeFromWorldPoint(Vector3 worldPosition) 
    {
        float percenteX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percenteY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percenteX = Mathf.Clamp01(percenteX);
        percenteY = Mathf.Clamp01(percenteY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percenteX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percenteY);
        return grid[x, y];
    }


    /// <summary>
    /// Serve pra desenhar no view da unity um cubo parar mostrar a area do grid
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); // bota o cubo no tamanho total do grid



        if(grid != null)
        {
            Node playerNode = nodeFromWorldPoint(player.position);

            foreach (Node node in grid)
            {
                List<Node> vizinhos = nodeVizinhos(node);

                foreach (Node nodeV in vizinhos)
                {
                    
                    if (node.walkable && nodeV.walkable)
                        Gizmos.color = Color.blue;
                    else
                        Gizmos.color = Color.black;
                    Gizmos.DrawLine(node.worldPosition, nodeV.worldPosition);
                }


                if (node.walkable)
                {
                    if (playerNode == node) // colore onde o player esta
                        Gizmos.color = Color.cyan;
                    else
                        Gizmos.color = Color.green;
                }   
                else
                    Gizmos.color = Color.red;    

                Gizmos.DrawSphere(node.worldPosition, nodeDiametre -.7f);
            }
        }  // bota as esferas no mapa
    }
}
