using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public int Move;
    public int Health;

    public int Attack;
    public int Defence;

    public Node Tile;

    public bool HasMoved;
    public bool HasActed;

    public void MoveTo(Node n)
    {
        Tile.stander = null;
        this.transform.position = n.GetWorldPosition();
        Tile = n;
        Tile.stander = this.gameObject;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

}
