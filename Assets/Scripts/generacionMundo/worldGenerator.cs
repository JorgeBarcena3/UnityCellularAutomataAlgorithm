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

    /// <summary>
    /// Los pasillos seran estrechos o amplios
    /// </summary>
    public bool pasillosEstrechos = false;

    /// <summary>
    /// Determina si despues de cada iteracion se generan pasillos o no
    /// </summary>
    public bool generarPasillosPorIteracion = true;

    /// <summary>
    /// Determina si suavizamos o no el mundo
    /// </summary>
    public bool suavizarMundo = true;



    [Range(0, 1)]
    /// <summary>
    /// Probabilidades de que sea suelo inicial
    /// </summary>
    public float probabilidadesDeSerSueloRnd = 0.45f;

    [Header("Opciones de debug")]

    /// <summary>
    ///  Determina si se mostraran los colores del modo debug
    /// </summary>
    public bool mostrarColoresDebug = false;

    /// <summary>
    ///  Determina si se mostraran los colores de cada sala
    /// </summary>
    public bool colorDeSalasYPasillos = false;


    [Header("Semilla del mundo")]

    /// <summary>
    /// Semilla de generacion aleatoria
    /// </summary>
    public int seed;

    /// <summary>
    /// Determinamos si cogemos una seed al azar
    /// </summary>
    public bool useRandomSeed;

    /// <summary>
    /// Ver progreso de creacion de mapa
    /// </summary>
    public bool verProcesoCreacionMapa = false;

    /// <summary>
    /// Numero de iteraciones que ha hecho en la primera generacion del mapa
    /// </summary>
    private int countIteracionesIniciales = 0;

    [Header("Regla de generacion de mundo (Cells Atomata - B/S)")]

    /// <summary>
    /// Regla para la generacion del mundo
    /// </summary>
    public string reglaDeGeneracion = "5678/35678";


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
        this.board = new Tablero(tamanioX, tamanioY, radioVecino, reglaDeGeneracion, probabilidadesDeSerSueloRnd, pasillosEstrechos);

        //Creamos un tablero random
        this.board.createRandomWorld();

        if (iteracionesIniciales && !verProcesoCreacionMapa)
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
        /*
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

                if (!mostrarColoresDebug)
                {
                    if (this.board[x, y].value == CellsType.alive)
                    {
                        spritesBoard.Last().GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        spritesBoard.Last().GetComponent<SpriteRenderer>().color = Color.black;
                    }
                }
                else if (colorDeSalasYPasillos)
                {
                    if (this.board[x, y].value == CellsType.alive)
                    {
                        spritesBoard.Last().GetComponent<SpriteRenderer>().color = this.board[x, y].color;

                    }
                }



            }
        }
        */

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

                if (!mostrarColoresDebug)
                {
                    if (this.board[x, y].value == CellsType.alive)
                    {
                        spritesBoard.Last().GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        spritesBoard.Last().GetComponent<SpriteRenderer>().color = Color.black;
                    }
                }
                else if (colorDeSalasYPasillos)
                {
                    if (this.board[x, y].value == CellsType.alive)
                    {
                        spritesBoard.Last().GetComponent<SpriteRenderer>().color = this.board[x, y].color;

                    }
                }



            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!useRandomSeed)
            UnityEngine.Random.InitState(seed);

        if (verProcesoCreacionMapa)
            InvokeRepeating("doAStep", 0.0f, 0.5f);


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

        if (Input.GetKeyDown(KeyCode.R))
        {
            board.roomManager.checkRooms(board);
            drawBoard();

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            board.roomManager.checkRooms(board, true);
            drawBoard();
        }
    }

    /// <summary>
    /// Hace una genracion del algoritmo
    /// </summary>
    public void doAStep()
    {
        this.board.computeNeighbors();
        board.roomManager.checkRooms(board);
        if (++countIteracionesIniciales >= interaciones)
        {
            board.roomManager.checkRooms(board, this.generarPasillosPorIteracion);
            if (suavizarMundo)
                board.smoothOutTheMap();

            CancelInvoke("doAStep");
        }
        drawBoard();

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

        if (generarPasillosPorIteracion)
            board.roomManager.checkRooms(board, true);
        else
            board.roomManager.checkRooms(board);


        drawBoard();

    }

}
