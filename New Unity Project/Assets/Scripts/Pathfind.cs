using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind : MonoBehaviour
{
    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void FindPath(Vector3 startPos,Vector3 targetPos)
    {
        Node startNode = grid.nodeFromWorldPoint(startPos); // Node inical do meu caminho

        Node targetNode = grid.nodeFromWorldPoint(targetPos); // Node final do meu caminho

        List<Node> openSet = new List<Node>(); // Lista de nodes que ja abri

        HashSet<Node> closeSet = new HashSet<Node>();// Lista de nodes que ainda não abri

        openSet.Add(startNode);

        while (closeSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost() < currentNode.fCost() || currentNode.hCost == openSet[i].hCost && openSet[i].hCost < currentNode.hCost)
                    currentNode = openSet[i];

                openSet.Remove(currentNode);
                closeSet.Add(currentNode);

                if (currentNode == targetNode)
                    return;
            }
        }

    }
}
