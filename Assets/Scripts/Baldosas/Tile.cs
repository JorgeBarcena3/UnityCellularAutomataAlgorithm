using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Posicion x
    public int x { get; private set; }
    //Posicion y
    public int y { get; private set; }
    //Profundidaz 
    public int z { get; private set; }
    //Indica si esta baldosa es transitable
    public bool walkable { get; private set; }
    //Referencia al spriterender de la baldosa
    public SpriteRenderer tileRender;
    //array de posibles sprites
    public Sprite[] sprites;
    public Tile(int x, int y, bool walkable = true)
    {
        this.x = x;
        this.y = y;
        this.z = -(y);
        this.walkable = walkable;
        tileRender = GetComponent<SpriteRenderer>();

    }

}
