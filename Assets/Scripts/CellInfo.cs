public class CellInfo
{
    /// <summary>
    /// Tablero al que pertenece
    /// </summary>
    public Tablero world;

    /// <summary>
    /// Posicion X que ocupa
    /// </summary>
    public int x;

    /// <summary>
    /// Posicion Y que ocupa
    /// </summary>
    public int y;

    /// <summary>
    /// Casilla a la que pertenece
    /// </summary>
    public Cell cell { get { return world.world_cell[x, y]; } }

    /// <summary>
    /// Constructor que almacena la info de la casilla
    /// </summary>
    /// <param name="world">Tamblero</param>
    /// <param name="x">Posicion X</param>
    /// <param name="y">Posicion Y</param>
    public CellInfo(Tablero _world, int x, int y)
    {
        this.world = _world;
        this.x = x;
        this.y = y;
    }
}