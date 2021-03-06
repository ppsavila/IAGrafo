﻿using System.Collections;
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
    public Node parent;
    public bool inPath=false;

    public bool seeker = false ,target = false;

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
        if (walkable && inPath || seeker || target)
            GetComponent<Renderer>().material.color = Color.yellow;
        else if(walkable && inPath)
            GetComponent<Renderer>().material.color = Color.black;
        else if(walkable )
            GetComponent<Renderer>().material.color = Color.green;
        else
            GetComponent<Renderer>().material.color = Color.red;    

        if (!ligarVerifacaodecolisao)
        {

            if (Physics.CheckSphere(this.transform.position, Graph.instance.Radius, dontWalk) )
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
            if (transform.position.y - vizinho.transform.position.y >= Graph.instance.Slope)
                if (vizinho.transform.position.x - transform.position.x <= Graph.instance.Slope || vizinho.transform.position.z - transform.position.z <= Graph.instance.Slope)
                    return true;
                else
                    return false;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, Graph.instance.Radius);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    posX = transform.position.x;
    //    posY = transform.position.z;
    //    if (other.tag == "DontWalk")
    //    {
    //        GetComponent<Renderer>().material.color = Color.red;
    //    }
    //    else
    //        GetComponent<Renderer>().material.color = Color.green;
    //}
}

