using System.Collections.Generic;
/// <summary>
/// Room que compone un mapa
/// </summary>
public class Room
{
    /// <summary>
    /// Lista de celdas que componen la habitacion
    /// </summary>
    public List<Cell> celdas { get; private set; }

    /// <summary>
    /// Vertices de cada habitacion
    /// </summary>
    public List<Cell> limitesHabitacion { get; private set; }

    /// <summary>
    /// Habitaciones a las que esta conectado
    /// </summary>
    public List<Room> connectedRooms { get; private set; }

    /// <summary>
    /// Tamaño de la room
    /// </summary>
    public int roomSize { get; private set; }

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public Room()
    {
    }

    /// <summary>
    /// Constructor con parametros
    /// </summary>
    /// <param name="_room">Lista de celdas de la habitacion</param>
    public Room(List<Cell> _room, Tablero _tablero)
    {
        this.celdas = _room;
        this.roomSize = celdas.Count;
        this.connectedRooms = new List<Room>();

        this.limitesHabitacion = new List<Cell>();

        foreach (Cell cell in this.celdas)
        {
            for (int x = cell.cellInfo.x - 1; x <= cell.cellInfo.x + 1; x++)
            {
                for (int y = cell.cellInfo.y - 1; y <= cell.cellInfo.y + 1; y++)
                {
                    if (
                            (x >= 0 && x < _tablero.world_cell.GetLength(0))
                         && (y >= 0 && y < _tablero.world_cell.GetLength(1))
                         && (x == cell.cellInfo.x || y == cell.cellInfo.y)
                    )
                    {
                        if (_tablero[x, y].value == CellsType.alive)
                        {
                            limitesHabitacion.Add(cell);
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// Conecta dos habitaciones
    /// </summary>
    /// <param name="roomA">Habitacion a conectar A</param>
    /// <param name="roomB">Habitacion a conectar B</param>
    public static void conectarHabitaciones(Room roomA, Room roomB)
    {
        roomA.connectedRooms.Add(roomB);
        roomB.connectedRooms.Add(roomA);
    }

    /// <summary>
    /// Determinamos si la habitacion esta conectada con la anterior
    /// </summary>
    /// <param name="otherRoom">Habitacion con la que deberá estar conectado</param>
    /// <returns></returns>
    public bool IsConnected(Room otherRoom)
    {
        return connectedRooms.Contains(otherRoom);
    }


}