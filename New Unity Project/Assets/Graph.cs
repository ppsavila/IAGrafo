using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public float      Grain = 1.0f;
    public int        Size  = 10;
    public GameObject Node;
    public Material   Walk, DontWalk;

    public void Awake ()
    {
        GenerateGraph();
    }

    public void GenerateGraph ()
    {
        RaycastHit ray;

        for (int i = 0; i < Size; i++)
        { 
            for (int j = 0; j < Size; j++)
            {
                if (Physics.Raycast(this.transform.position + new Vector3(i * Grain, 0, j * Grain),
                                    Vector3.down, out ray, 100.0f))
                {
                    GameObject aux = Instantiate(Node, ray.point, Quaternion.identity);

                    if (ray.collider.tag == "Walk")
                        aux.GetComponent<Renderer>().material = Walk;
                    else
                        aux.GetComponent<Renderer>().material = DontWalk;
                }
            }
        }
    }
}
