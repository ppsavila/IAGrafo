using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    Graph graph;

    public Transform seeker, target;
    public List<Node> path = new List<Node>();
    public List<Node> openSet = new List<Node>();
    HashSet<Node> closedSet = new HashSet<Node>();
    public Node startNode;
    public Node endNode;

    private void Awake()
    {
        graph = GetComponent<Graph>();
    }

    private void Update()
    {
        findPath(seeker.position, target.position);
    }

    void findPath(Vector3 startPos, Vector3 targetPos)
    {
         startNode = graph.NodeFromWorldPoint(startPos);
         endNode = graph.NodeFromWorldPoint(targetPos);
       
        openSet.Add(startNode);
        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost() <= currentNode.fCost() && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if( currentNode == endNode)
            {
                retracePath(startNode, endNode);

                return;
            }
           
            foreach (Node vizinho in currentNode.vizinhos)
            {

                if (!vizinho.walkable || closedSet.Contains(vizinho))
                {
                    
                    continue;
                }
                    
                float newMovementCost = currentNode.gCost + getDistance(currentNode, vizinho);
                if (newMovementCost < vizinho.gCost || !openSet.Contains(vizinho))
                {
                    vizinho.gCost = newMovementCost;
                    vizinho.hCost = getDistance(vizinho, endNode);
                    vizinho.parent = currentNode;

                    if (!openSet.Contains(vizinho))
                        openSet.Add(vizinho);
                }
            }
        }
    }

    void retracePath(Node startNode, Node endNode)
    {
        path = new List<Node>();
        Node currentNode = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        graph.path = path;
    }


    float getDistance(Node nodeA,Node nodeB)
    {
        float disX = Mathf.Abs(nodeA.posX - nodeB.posX);
        float disY = Mathf.Abs(nodeA.posY - nodeB.posY);
        if (disX > disY)
            return 14 * disY + 10 * (disX - disY);
        return 14 * disX + 10 * (disY - disX);
    }
}
