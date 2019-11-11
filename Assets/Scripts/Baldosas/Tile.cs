using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Posicion x
    public float x { get; private set; }
    //Posicion y
    public float y { get; private set; }
    //Profundidaz 
    public float z { get; private set; }
    //Indica si esta baldosa es transitable
    public bool walkable { get; set; }
    //Referencia al spriterender de la baldosa
    [HideInInspector]
    public SpriteRenderer tileRender;
    //Referencia al transform de la baldosa
    [HideInInspector]
    public Transform tileTransform;
    //array de posibles sprites
    public Sprite[] sprites;
    public Tile(bool walkable = true)
    {
        this.walkable = walkable;
        

    }
    public void Start()
    {
        tileRender = GetComponent<SpriteRenderer>();
        tileTransform = GetComponent<Transform>();
        x = tileTransform.position.x;
        y = tileTransform.position.y;
        z = tileTransform.position.z;
        if (!walkable)
            Elevate();

    }
    public Vector2 GetPosition()
    {
        return new Vector2(x, y);
    }
    public void SetPosition(Vector2 position)
    {
        x = position.x;
        y = position.y;
        tileTransform.position = position;
    }
    public void Elevate()
    {
        Vector3 size = GetComponent<SpriteRenderer>().bounds.size;
        tileTransform.position = new Vector3(x, y + size.y * 9 / 40, z-1);

    }

}
