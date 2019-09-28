using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool walkable;
    public bool ligarVerifacaodecolisao;
    public LayerMask dontWalk;
    public int posX;
    public int posY;

    public float gCost;
    public float hCost;
    

    public void setRadius(float radius)
    {
        GetComponent<SphereCollider>().radius = radius;
    }

    public float fCost()
    {
        return gCost + hCost;
    }

    private void Update()
    {
        if (walkable)
            GetComponent<Renderer>().material.color = Color.green;
        else
            GetComponent<Renderer>().material.color = Color.red;

        if (!ligarVerifacaodecolisao)
        {
            if (Physics.CheckSphere(this.transform.position, Graph.instance.Radius, dontWalk))
            {
                walkable = false;
            }
            else
            {
                walkable = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, Graph.instance.Radius);
    }
}

