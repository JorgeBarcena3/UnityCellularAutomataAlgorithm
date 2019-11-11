using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileWalkableType
{
    TILEFLOOR, SPAWNTILE, EXITTILE, SHRINE, HALLWAY
}
public class TileWalkable : Tile
{
    public TileWalkableType tileType;
    public TileWalkable(int x, int y) : base( walkable: true) { }
    public void Awake()
    {
        walkable = true;
        tileRender.sprite = sprites[(int)tileType];
    }
}
