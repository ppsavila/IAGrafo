using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class Node : MonoBehaviour
{
    public bool walkable;
    public bool ligarVerifacaodecolisao;
    [XmlAttribute]
    public LayerMask dontWalk;
    [XmlAttribute]
    public List<Node> vizinhos = new List<Node>();
    public float posX;
    public float posY;
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
    private void Start()
    {
        ligarVerifacaodecolisao = false;
    }

    private void FixedUpdate()
    {
        if (walkable)
            GetComponent<Renderer>().material.color = Color.green;
        else
            GetComponent<Renderer>().material.color = Color.red;

        if (!ligarVerifacaodecolisao)
        {

            if (Physics.CheckSphere(this.transform.position, Graph.instance.Radius, dontWalk) || testeVizinhos())
            {
                walkable = false;
            }
            else
            {
                walkable = true;
            }
        }
        posX = transform.position.x;
        posY = transform.position.z;
    }

    bool testeVizinhos()
    {
        foreach (Node vizinho in vizinhos)
        {
            if ((this.transform.position.y - vizinho.transform.position.y) < Graph.instance.Slope)
                return false;
            else
                return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, Graph.instance.Radius);
    }
}

