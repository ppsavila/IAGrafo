using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public static Graph instance;
    public float Grain = 1.0f;
    [Range(0,5)]
    public float Radius = 1f;
    public int Size  = 15;
    public GameObject Node;
    public Transform nodes;
    public GameObject[,] nodesMatriz;
    int gridSize;

    public void Awake ()
    {
        instance = this;
        GenerateGraph2();
    }



    public void GenerateGraph2()
    {
        float nodeDiametre = Radius * 2;
        gridSize = Mathf.RoundToInt(Size *  Grain);

        nodesMatriz = new GameObject[gridSize, gridSize];
        Vector3 worldBottomLeft = transform.position - Vector3.right * Size / 2 - Vector3.forward * Size / 2;

        RaycastHit ray;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiametre ) + Vector3.forward * (j * nodeDiametre );


                if (Physics.Raycast(worldPoint, Vector3.down, out ray, 100.0f))
                {
                    GameObject aux = Instantiate(Node, ray.point, Quaternion.identity,nodes);
                    nodesMatriz[i, j] = aux;
                    
                    aux.GetComponent<Node>().posX = i;
                    aux.GetComponent<Node>().posY = j;



                    
                }
            }
        }
    }

    public List<GameObject> nodeVizinhos(GameObject node, int gX,int gY)
    {
        List<GameObject> vizinhos = new List<GameObject>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 & y == 0)
                    continue;
                int checkX = gX + x;
                int checkY = gY + y;
                if (checkX >= 0 && checkX < gridSize && checkY >= 0 && checkY < gridSize)
                    vizinhos.Add(nodesMatriz[checkX, checkY]);
            }
        }
        return vizinhos;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position,new Vector3(Size,1,Size));

        foreach (GameObject node in nodesMatriz) // que isso gente 
        {
            //Codigo super leve 
            List<GameObject> vizinhos = nodeVizinhos(node, node.GetComponent<Node>().posX, node.GetComponent<Node>().posY);

            if(vizinhos!= null)
            {
                foreach (GameObject vizinho in vizinhos)
                {
                    Gizmos.DrawLine(node.transform.position, vizinho.transform.position); // cu
                }
            }
            
        }
    }
}
