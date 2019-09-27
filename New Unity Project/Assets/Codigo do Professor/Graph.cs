using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public float Grain = 1.0f;
    public int Size = 10;
    public GameObject Node;
    public Material Walk, DontWalk;
    public Node[,] Grid;
    public float nodeRadius; // Raio do node
    float nodeDiametre;
    int gridSizeX, gridSizeY;

    void Awake ()
    {
        nodeDiametre = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(Size / nodeDiametre); 
        gridSizeY = Mathf.RoundToInt(Size / nodeDiametre);
        Grid = new Node[gridSizeX, gridSizeY];
    }

    private void Start()
    {
         GenerateGraph();
    }


    public void GenerateGraph ()
    {
        RaycastHit ray;
        Vector3 worldBottomLeft = transform.position - Vector3.right * Size/ 2 - Vector3.forward * Size / 2;
        for (int x = 0; x < gridSizeX; x++)
        { 
            for (int y = 0; y < gridSizeY; y++)
            {
                GameObject aux = null;
                if (Physics.Raycast(worldBottomLeft + new Vector3(x * Grain, 0, y* Grain),Vector3.down, out ray, 100.0f))
                {
                    aux = Instantiate(Node, ray.point, Quaternion.identity);

                    if (ray.collider.tag == "Walk")
                    {
                        aux.GetComponent<Renderer>().material = Walk;
                        aux.GetComponent<Node>().walkable = true;
                    }
                    else
                    {
                        aux.GetComponent<Renderer>().material = DontWalk;
                        aux.GetComponent<Node>().walkable = false;
                    }
                    Grid[x, y] = aux.GetComponent<Node>();
                }


            }
        }
    }




    public void UpdateGraph()
    {

    }

}
