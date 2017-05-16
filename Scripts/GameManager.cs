using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static Unit selected;
    public static Unit possibleSelection;
    public bool playerTurn;

    List<Node> currentPath;
    public List<Node> graph;

    public GameObject[] DicePrefabs;
    public GameObject YourPanel;
    public GameObject EnemyPanel;
    public GameObject SelectionPromt;
    public Text AnalyzerText;

    List<int> PlayersDice = new List<int>();
    List<int> EnemysDice = new List<int>();

    // Use this for initialization
    void Start () {
        playerTurn = true;
        GameObject map = GameObject.Find("Map");
        graph.AddRange(map.GetComponentsInChildren<Node>());
	}



    public void NextTurn()
    {



        playerTurn = !playerTurn;
    }

    public void GeneratePathTo(Node target)
    {
        currentPath = null;
        Dictionary<Node, int> Dist = new Dictionary<Node, int>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        Node source = selected.Tile;


        List<Node> frontier = new List<Node>();
        frontier.Add(source);

        List<Node> visited = new List<Node>();

        Dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                Dist[v] = int.MaxValue;
                prev[v] = null;
            }
        }

        while (frontier.Count > 0)
        {
            Node current = frontier[0];
            frontier.RemoveAt(0);

            foreach (Node u in current.edges)
            {
                bool add = true;
                for (int i = 0; i < visited.Count; i++)
                {
                    if (u == visited[i])
                        add = false;
                }
                if (add)
                {
                    if ( Dist[u] == int.MaxValue ) {
                        frontier.Add(u);
                        Dist[u] = Dist[current] + 1;
                        prev[u] = current;
                    }

                }
                
            }

            if (prev[target] != null)
                break;

            visited.Add(current);
        }

        if (prev[target] == null)
        {
            // no route to target
            return;
        }

        currentPath = new List<Node>();

        Node curr = target;

        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }
        currentPath.Reverse();
    }

    public void Attack(Unit u)
    {
        ClearDiceFromUI();
        PlayersDice.Clear();
        EnemysDice.Clear();
        for (int i = 0; i < selected.Attack; i++)
        {
            PlayersDice.Add(Random.Range(0, 6));
        }
        PlayersDice.Sort();
        PlayersDice.Reverse();
        for (int i = 0; i < selected.Attack; i++)
        {
            GameObject go = (GameObject)Instantiate(DicePrefabs[PlayersDice[i]]);
            go.transform.SetParent(YourPanel.transform);
        }

        for (int i = 0; i < u.Defence; i++)
        {
            EnemysDice.Add(Random.Range(0, 6));
        }
        EnemysDice.Sort();
        EnemysDice.Reverse();
        for (int i = 0; i < u.Defence; i++)
        {
            GameObject go = (GameObject)Instantiate(DicePrefabs[EnemysDice[i]]);
            go.transform.SetParent(EnemyPanel.transform);
        }
        PlayersDice.Reverse();
        EnemysDice.Reverse();

    }

    void ClearDiceFromUI()
    {
        foreach (Transform child in YourPanel.transform)
        {
            if (child.tag == "Die")
                Destroy(child.gameObject);
        }
        foreach (Transform child in EnemyPanel.transform)
        {
            if (child.tag == "Die")
                Destroy(child.gameObject);
        }
    }

    void Deselect()
    {
        if (selected == null)
            return;
        ClearDiceFromUI();
        selected.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        selected = null;
    }
    void Select(Unit u)
    {
        selected = u;
        selected.GetComponentInChildren<SpriteRenderer>().color = Color.green;

    }

    public void InPromptSelectNewSelected()
    {
        selected.HasActed = true;
        Deselect();
        Select(possibleSelection);
        SelectionPromt.SetActive(false);
    }
    public void InPromptCancel()
    {
        SelectionPromt.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            Deselect();

        if (Input.GetMouseButtonDown(0))
        {
            if (!playerTurn)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 mapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mapPoint, Vector2.zero);

            if (hit.collider != null)
            {
                Node hitNode = hit.collider.transform.parent.gameObject.GetComponent<Node>();

                if (selected != null)
                {
                    if (hitNode.stander == null)
                    {
                        GeneratePathTo(hitNode);
                        selected.MoveTo(currentPath[Mathf.Min(selected.Move, currentPath.Count - 1)]);
                    }
                    else if (hitNode.stander.tag == "Player")
                    {
                        possibleSelection = hitNode.stander.GetComponent<Unit>();
                        SelectionPromt.SetActive(true);
                        return;
                    }
                    else if (hitNode.stander.tag == "Enemy")
                    {
                        foreach (Node e in hitNode.edges)
                        {
                            if (e == selected.Tile)
                                Attack(hitNode.stander.GetComponent<Unit>());
                        }
                    }
                    else
                        return;
                }

                else if (selected == null)
                {
                    if (hitNode.stander == null)
                        return;
                    else if (hitNode.stander.tag == "Player")
                        Select(hitNode.stander.GetComponent<Unit>());
                    else if (hitNode.stander.tag == "Enemy")
                        return;
                }
            }
        }

    }
}
