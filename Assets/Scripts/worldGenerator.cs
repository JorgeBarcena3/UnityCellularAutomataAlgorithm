using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class worldGenerator : MonoBehaviour
{
    [Header("Datos del mapa")]
    /// <summary>
    /// Tamaño Y de la cueva
    /// </summary>
    public int tamanioY = 50;

    /// <summary>
    /// Tamaño X de la cueva
    /// </summary>
    public int tamanioX = 50;

    /// <summary>
    /// Radio por el que se calcularan las casillas vecinas
    /// </summary>
    public int radioVecino = 1;

    [Range(0, 1)]
    /// <summary>
    /// Probabilidades de que sea suelo inicial
    /// </summary>
    public float probabilidades_de_ser_suelo_inicial = 0.45f;

    [Header("Semilla del mundo")]

    /// <summary>
    /// Semilla de generacion aleatoria
    /// </summary>
    public int seed;

    /// <summary>
    /// Determinamos si cogemos una seed al azar
    /// </summary>
    public bool useRandomSeed;

    [Header("Regla de generacion de mundo (Cells Atomata - B/S)")]

    /// <summary>
    /// Regla para la generacion del mundo
    /// </summary>
    public string reglaDeGeneracion = "5678/35678";

    /// <summary>
    /// Determina si suavizamos o no el mundo
    /// </summary>
    public bool suavizarMundo = true;

    [Header("Iteraciones para la creacion del mundo")]

    /// <summary>
    /// Numero de iteraciones del mapa
    /// </summary>
    public int interaciones = 15;

    /// <summary>
    /// Se daran iteracciones iniciales o no
    /// </summary>
    public bool iteracionesIniciales = true;

    /// <summary>
    /// Representacion del mundo generado
    /// </summary>
    private Tablero board;

    /// <summary>
    /// Genera un mapa aleatorio
    /// </summary>
    void generateWorld()
    {
        //Creamos la matriz de las celdas
        this.board = new Tablero(tamanioX, tamanioY, radioVecino, reglaDeGeneracion, probabilidades_de_ser_suelo_inicial);

        //Creamos un tablero random
        this.board.createRandomWorld();

        if (iteracionesIniciales)
            refreshBoard();

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

        for (int y = 0; y < this.board.world_cell.GetLength(0); y++)
        {
            for (int x = 0; x < this.board.world_cell.GetLength(1); x++)
            {
                Vector3 size = sprites[0].GetComponent<SpriteRenderer>().bounds.size;
                Vector3 position = this.transform.position + new Vector3(x * size.x, -(y * size.y), 0);

                if (this.board.world_cell[x, y].value == CellsType.dead)
                {
                    spritesBoard.Add(Instantiate(sprites[0], position, Quaternion.identity, this.transform));
                }
                else
                {
                    spritesBoard.Add(Instantiate(sprites[1], position, Quaternion.identity, this.transform));

                }

                if (
                       this.board.world_cell[x, y].countNeighborsAlive == 8 && this.board[x, y].value == CellsType.dead
                    || this.board[x, y].countNeighborsAlive == 0 && this.board[x, y].value == CellsType.alive)

                    spritesBoard.Last().GetComponent<SpriteRenderer>().color = Color.red;



            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!useRandomSeed)
            UnityEngine.Random.InitState(seed);

        generateWorld();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            refreshBoard();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            board.smoothOutTheMap();
            drawBoard();

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

        if (suavizarMundo)
            this.board.smoothOutTheMap();

        drawBoard();

    }

}
