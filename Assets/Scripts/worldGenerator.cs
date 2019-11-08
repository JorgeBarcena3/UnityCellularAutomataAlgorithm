using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldGenerator : MonoBehaviour
{
    [Header("Datos del mapa")]
    /// <summary>
    /// Tamaño Y de la cueva
    /// </summary>
    public int tamanioY;

    /// <summary>
    /// Tamaño X de la cueva
    /// </summary>
    public int tamanioX;

    /// <summary>
    /// Radio por el que se calcularan las casillas vecinas
    /// </summary>
    public int radioVecino;

    /// <summary>
    /// Probabilidades de que sea suelo inicial
    /// </summary>
    public float probabilidades_de_ser_suelo_inicial = 0.5f;

    /// <summary>
    /// Semilla de generacion aleatoria
    /// </summary>
    public int seed;

    [Header("Regla de generacion de mundo (Cells Atomata - B/S)")]

    /// <summary>
    /// Regla para la generacion del mundo
    /// </summary>
    public string reglaDeGeneracion = "4567/345";

    [Header("Iteraciones para la creacion del mundo")]

    /// <summary>
    /// Numero de iteraciones del mapa
    /// </summary>
    public int interaciones;

    /// <summary>
    /// Se daran iteracciones iniciales o no
    /// </summary>
    public bool iteracionesIniciales;

    /// <summary>
    /// Representacion del mundo generado
    /// </summary>
    private Tablero board;

    void generateWorld()
    {
        //Creamos la matriz de las celdas
        this.board = new Tablero(tamanioX, tamanioY, radioVecino, reglaDeGeneracion, probabilidades_de_ser_suelo_inicial);

        //Creamos un tablero random
        this.board.createRandomWorld();

        if (iteracionesIniciales)
            refreshBoard();

        //Dibujamos el mundo
        drawBoard();

    }

    /// <summary>
    /// Sprite con la que se representaran todo
    /// </summary>
    public GameObject[] sprites;

    /// <summary>
    /// Sprites generados
    /// </summary>
    private List<GameObject> spritesBoard;

    /// <summary>
    /// Función que pinta el tablero
    /// </summary>
    void drawBoard()
    {
        if (spritesBoard == null)
            spritesBoard = new List<GameObject>();
        else
        {
            for (int i = 0; i < spritesBoard.Count; i++)
            {
                Destroy(spritesBoard[i]);
            }
            spritesBoard = new List<GameObject>();

        }

        for (int i = 0; i < this.board.world_cell.GetLength(0); i++)
        {
            for (int j = 0; j < this.board.world_cell.GetLength(1); j++)
            {
                Vector3 size = sprites[0].GetComponent<SpriteRenderer>().bounds.size;

                Vector3 position = this.transform.position + new Vector3(i * size.x / 2, j * size.y / 2, 0);

                if (this.board.world_cell[i, j].value == CellsType.dead)
                {
                    spritesBoard.Add(Instantiate(sprites[0], position, Quaternion.identity));
                }
                else
                {
                    spritesBoard.Add(Instantiate(sprites[1], position, Quaternion.identity));

                }

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(seed);
        generateWorld();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            refreshBoard();
        }
    }

    /// <summary>
    /// Computamos los vecinos otra vez
    /// </summary>
    public void refreshBoard()
    {
        for (int i = 0; i < interaciones; ++i)
        {
            this.board.computeNeighbors();
        }

        Debug.Log("ITERACIONES TERMINADAS");
        drawBoard();
    }
}
