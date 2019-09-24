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

    public void Awake ()
    {
        Grid = new Node[Size, Size];
        GenerateGraph();
    }

    public void GenerateGraph ()
    {
        RaycastHit ray;

        for (int x = 0; x < Size; x++)
        { 
            for (int y = 0; y < Size; y++)
            {
                GameObject aux = null;
                if (Physics.Raycast(this.transform.position + new Vector3(x * Grain, 0, y* Grain),Vector3.down, out ray, 100.0f))
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
                }

                Grid[x, y] = aux.GetComponent<Node>();
            }
        }
    }




    public void UpdateGraph()
    {

    }

}
