using System;


/// <summary>
/// Clase tablero del juego
/// </summary>
public class Tablero
{

    /// <summary>
    /// Representacion del mundo generado
    /// </summary>
    public Cell[,] world_cell;

    /// <summary>
    /// Ancho del tableor
    /// </summary>
    public int width { get; private set; }

    /// <summary>
    /// Altura del tablero
    /// </summary>
    public int height { get; private set; }

    /// <summary>
    /// Radio por el que se calcularan los vecinos
    /// </summary>
    public int radioVecino { get; set; }

    /// <summary>
    /// Probabilidades de empezar vivo
    /// </summary>
    private float chanceToLive { get; set; }

    /// <summary>
    /// Manager que controla la aplicacion de las reglas
    /// </summary>
    private RuleManager ruleManager { get; set; }

    /// <summary>
    /// Sobrecarga operador de []
    /// </summary>
    /// <param name="x">Valor de X</param>
    /// <param name="y">Valor de Y</param>
    /// <returns></returns>
    public Cell this[int x, int y]
    {
        get { return this.world_cell[x, y]; }
        private set { }
    }

    /// <summary>
    /// Constructor para crear el tablero
    /// </summary>
    /// <param name="tamanioX"></param>
    /// <param name="tamanioY"></param>
    public Tablero(int tamanioX, int tamanioY, int _radioVecino, string _reglaDeGeneracion, float probabilidades_de_ser_suelo_inicial)
    {
        this.ruleManager = new RuleManager(_reglaDeGeneracion, 'S', 'B');
        this.width = tamanioX;
        this.height = tamanioY;
        this.world_cell = new Cell[width, height];
        this.radioVecino = _radioVecino;
        this.chanceToLive = probabilidades_de_ser_suelo_inicial;

    }

    /// <summary>
    /// Llena un array con cells aleatorias
    /// </summary>
    public void createRandomWorld()
    {
        for (int y = 0; y < this.world_cell.GetLength(0); ++y)
        {
            for (int x = 0; x < this.world_cell.GetLength(1); ++x)
            {
                this.world_cell[x,y] = new Cell(x,y, chanceToLive);
            }
        }

        searchNeighbors();

    }

    /// <summary>
    /// Una vez completado el tablero buscamos y almacenamos los vecinos
    /// </summary>
    private void searchNeighbors()
    {

        for (int y = 0; y < this.world_cell.GetLength(0); ++y)
        {
            for (int x = 0; x < this.world_cell.GetLength(1); ++x)
            {
                setNeighbors(ref this.world_cell[x,y]);
            }
        }
    }

    /// <summary>
    /// Computa los vecinos de una celda
    /// </summary>
    /// <param name="myCell">La celda base</param>
    private void setNeighbors(ref Cell myCell)
    {
        int radioVecinos = radioVecino;

        myCell.countNeighborsAlive = 0;

        //Cogemos todos los vecinos
        for (int y = radioVecinos; y >= -radioVecinos; --y)
        {
            for (int x = radioVecinos; x >= -radioVecinos; --x)
            {
                int NeighborX = myCell.cellInfo.x + x;
                int NeighborY = myCell.cellInfo.y + y;

                if (
                    (NeighborX >= 0 && NeighborX < world_cell.GetLength(0))
                 && (NeighborY >= 0 && NeighborY < world_cell.GetLength(1))
                 && (Math.Abs(x) + Math.Abs(y) != 0)
                 )
                {
                    if (this.world_cell[NeighborX, NeighborY].value == CellsType.alive)
                        ++myCell.countNeighborsAlive;

                }
            }
        }

    }

    /// <summary>
    /// Actualizamos el estado de las celulas
    /// </summary>
    public void computeNeighbors()
    {

        Cell[,] next = new Cell[width, height];

        for (int y = 0; y < this.world_cell.GetLength(0); ++y)
        {
            for (int x = 0; x < this.world_cell.GetLength(1); ++x)
            {
                Cell cell = new Cell(this.world_cell[x,y]);
                next[x,y] = this.ruleManager.applyRules(cell);

            }
        }

        copyNewBoard(next);
        searchNeighbors();

    }

    /// <summary>
    /// Copia un nuevo array a la variable existente
    /// </summary>
    /// <param name="next">New Array</param>
    private void copyNewBoard(Cell[,] next)
    {

        for (int y = 0; y < this.world_cell.GetLength(0); ++y)
        {
            for (int x = 0; x < this.world_cell.GetLength(1); ++x)
            {
                this.world_cell[ x,y] = new Cell(next[x, y]);

            }
        }
    }

    /// <summary>
    /// Suaviza el mapa algunos puntos en el centro
    /// </summary>
    public void smoothOutTheMap()
    {
        searchNeighbors();

        Cell[,] next = new Cell[width, height];

        for (int y = 0; y < this.world_cell.GetLength(0); ++y)
        {
            for (int x = 0; x < this.world_cell.GetLength(1); ++x)
            {
                Cell cell = new Cell(this.world_cell[x,y]);
                if (this.world_cell[x,y].countNeighborsAlive == 8 && this.world_cell[x, y].value == CellsType.dead)
                    cell = new Cell(cell.cellInfo.x, cell.cellInfo.y, CellsType.alive);
                else if(this.world_cell[x, y].countNeighborsAlive == 0 && this.world_cell[x, y].value == CellsType.alive)
                    cell = new Cell(cell.cellInfo.x, cell.cellInfo.y, CellsType.dead);


                next[x,y] = cell;

            }
        }

        copyNewBoard(next);
        searchNeighbors();

    }
}