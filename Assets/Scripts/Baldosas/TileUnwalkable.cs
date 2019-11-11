using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileUnwalkableType
{
    WALL,IMPENETRABLEWALL
}
public class TileUnwalkable : Tile
{
   
    public TileUnwalkableType tileType;
    public TileUnwalkable(int x, int y) : base( walkable: false) {  }

    public void Awake()
    {
        walkable = false;
        tileRender.sprite = sprites[(int)tileType];
    }
   
}
