using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public int x;
    public int y;

    public List<Node> edges;

    public GameObject stander;

    void Start()
    {
        if (stander == null)
            return;
        GameObject go = (GameObject)Instantiate(stander, this.transform.position, Quaternion.identity);
        go.GetComponent<Unit>().Tile = this;
        stander = go;
    }

    public float DistanceTo(Node n)
    {
        foreach (Node v in edges)
        {
            if (v == n)
            {
                return 1f;
            }
        }
        return Mathf.Infinity;
    }

    public Vector2 GetWorldPosition()
    {
        return this.transform.position;
    }

}
