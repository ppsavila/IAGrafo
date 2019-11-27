using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Path : MonoBehaviour
{
    public GameObject ajuda;

    public static Path instance;
    Graph graph;

    public Transform seeker, target;
    public List<Node> path = new List<Node>();
    public List<Node> openSet = new List<Node>();
    HashSet<Node> closedSet = new HashSet<Node>();
    public Node startNode;
    public Node endNode;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        graph = GetComponent<Graph>();
        aest = false;
        deij = false;
    }

    bool aest, deij;
    private void Update()
    {
        startNode = graph.nodesList.Find(x => x.seeker == true);
        endNode = graph.nodesList.Find(x => x.target == true);

        DefinirPorClique();
        PainelAjuda();

        try
        {
            if (startNode != null && endNode != null && aest)
                findPathA();
            if (startNode != null && endNode != null && deij)
                findPathD();
        }
        catch { }
    }

    public void PainelAjuda()
    {
        if (Input.GetKeyDown(KeyCode.H))
            ajuda.SetActive(!ajuda.activeSelf);
    }

    public void DefinirA()
    {
        openSet.Clear();
        closedSet.Clear();
        aest = true;
        deij = false;
    }

    public void DefinirD()
    {
        openSet.Clear();
        closedSet.Clear();
        aest = false;
        deij = true;
    }

    void DefinirPorClique()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Node")
                {
                    if (hit.transform.gameObject.GetComponent<Node>().walkable)
                    {
                        LimparCliquesEsq();
                        hit.transform.gameObject.GetComponent<Node>().seeker = true;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Node")
                {
                    if (hit.transform.gameObject.GetComponent<Node>().walkable)
                    {
                        LimparCliquesDir();
                        hit.transform.gameObject.GetComponent<Node>().target = true;
                    }
                }
            }
        }
    }

    void LimparCliquesEsq()
    {
        foreach (Node n in graph.nodesList)
        {
            n.seeker = false;
        }
    }
    void LimparCliquesDir()
    {
        foreach (Node n in graph.nodesList)
        {
            n.target = false;
        }
    }

    void findPathA()
    {
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost() <= currentNode.fCost() && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
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

    void findPathD()
    {
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].gCost <= currentNode.gCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
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
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        graph.path = path;
    }


    float getDistance(Node nodeA, Node nodeB)
    {
        float disX = Mathf.Abs(nodeA.posX - nodeB.posX);
        float disY = Mathf.Abs(nodeA.posY - nodeB.posY);
        if (disX > disY)
            return 14 * disY + 10 * (disX - disY);
        return 14 * disX + 10 * (disY - disX);
    }
}
