using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Encargado de determinar las salas que contiene un tablero
/// </summary>
public class roomsManager
{
    /// <summary>
    /// Lista de habitaciones
    /// </summary>
    private List<Room> rooms;

    /// <summary>
    /// Referencia al tablero donde esta la habitacion
    /// </summary>
    private Tablero tablero;

    /// <summary>
    /// Determina si los pasillos serán estrechos o no
    /// </summary>
    private bool pasillosEstrechos;

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public roomsManager(Tablero _tablero, bool _pasillosEstrechos)
    {
        this.rooms = new List<Room>();
        this.tablero = _tablero;
        this.pasillosEstrechos = _pasillosEstrechos;
    }

    /// <summary>
    /// Determina las rooms que hay en un tablero
    /// </summary>
    /// <param name="_tablero">Tablero de donde se buscaran las habitaciones</param>
    public void checkRooms(Tablero _tablero, bool unirHabitaciones = false)
    {

        setCeldasOutOfRoom(_tablero);

        for (int y = 0; y < _tablero.world_cell.GetLength(0); ++y)
        {
            for (int x = 0; x < _tablero.world_cell.GetLength(1); ++x)
            {
                if (_tablero[x, y].value == CellsType.alive && !_tablero[x, y].cellInfo.isInRoom)
                {
                    Room newRoom = new Room(GetRegionTiles(_tablero, _tablero[x, y]), _tablero);
                    Color RegionColor = new Color((float)Random.Range(0f, 1f), (float)Random.Range(0f, 1f), (float)Random.Range(0f, 1f));
                    foreach (Cell cell in newRoom.celdas)
                    {
                        cell.color = RegionColor;
                        cell.cellInfo.isInRoom = true;

                    }
                    rooms.Add(newRoom);

                }
            }
        }

        if (unirHabitaciones)
            ConnectClosestRooms();
    }

    /// <summary>
    /// Elimina de las celdas de un tablero
    /// </summary>
    /// <param name="tablero">Tablero del cual eliminar las cosas</param>
    private void setCeldasOutOfRoom(Tablero _tablero)
    {

        this.rooms = new List<Room>();

        for (int y = 0; y < _tablero.world_cell.GetLength(0); ++y)
        {
            for (int x = 0; x < _tablero.world_cell.GetLength(1); ++x)
            {
                _tablero[x, y].cellInfo.isInRoom = false;
                _tablero[x, y].color = Color.white;
            }
        }
    }

    /// <summary>
    /// A partir de un celda se obtiene una region
    /// </summary>
    /// <param name="tablero">Mapa donde se buscará la región</param>
    /// <param name="cell">Celda raiz</param>
    /// <returns></returns>
    private List<Cell> GetRegionTiles(Tablero _tablero, Cell _cell)
    {

        List<Cell> room = new List<Cell>();

        Queue<Cell> queue = new Queue<Cell>();
        _cell.cellInfo.isInRoom = true;
        queue.Enqueue(_cell);

        while (queue.Count > 0)
        {
            Cell cell = queue.Dequeue();
            room.Add(cell);
            CellsType cellType = cell.value;

            int radioVecinos = _tablero.radioVecino;

            for (int x = cell.cellInfo.x - radioVecinos; x <= cell.cellInfo.x + radioVecinos; x++)
            {
                for (int y = cell.cellInfo.y - radioVecinos; y <= cell.cellInfo.y + radioVecinos; y++)
                {

                    int NeighborX = x;
                    int NeighborY = y;

                    if (
                        (NeighborX >= 0 && NeighborX < _tablero.world_cell.GetLength(0))
                     && (NeighborY >= 0 && NeighborY < _tablero.world_cell.GetLength(1))
                     && (NeighborX == cell.cellInfo.x || NeighborY == cell.cellInfo.y)
                     )
                    {
                        if (_tablero[NeighborX, NeighborY].value == cellType && !_tablero[NeighborX, NeighborY].cellInfo.isInRoom)
                        {
                            _tablero[NeighborX, NeighborY].cellInfo.isInRoom = true;
                            queue.Enqueue(_tablero[NeighborX, NeighborY]);

                        }

                    }

                }
            }
        }

        return room;
    }

    /// <summary>
    /// Conectamos las habitaciones mas cercanas
    /// </summary>
    /// <param name="allRooms">Lista de habitaciones</param>
    private void ConnectClosestRooms()
    {
        int mejorDistancia = 0;

        Cell mejorCeldaA = new Cell();
        Cell mejorCeldaB = new Cell();
        Room mejorHabitacionA = new Room();
        Room mejorHabitacionB = new Room();

        bool conexionEncontrada = false;

        Room habitacionPrincipal = rooms.OrderByDescending(i => i.roomSize).Take(1).FirstOrDefault();

        if (habitacionPrincipal != null)
        {
            foreach (Room roomA in rooms)
            {
                conexionEncontrada = false;

                if (roomA != habitacionPrincipal)
                {

                    for (int celdaAIndex = 0; celdaAIndex < roomA.limitesHabitacion.Count; celdaAIndex++)
                    {
                        for (int celdaBIndex = 0; celdaBIndex < habitacionPrincipal.limitesHabitacion.Count; celdaBIndex++)
                        {
                            Cell celdaA = roomA.limitesHabitacion[celdaAIndex];
                            Cell celdaB = habitacionPrincipal.limitesHabitacion[celdaBIndex];
                            int distanciaManhattanEntreHabitaciones = (int)(Mathf.Pow(celdaA.cellInfo.x - celdaB.cellInfo.x, 2) + Mathf.Pow(celdaA.cellInfo.y - celdaB.cellInfo.y, 2));

                            if (distanciaManhattanEntreHabitaciones < mejorDistancia || !conexionEncontrada)
                            {
                                mejorDistancia = distanciaManhattanEntreHabitaciones;
                                conexionEncontrada = true;
                                mejorCeldaA = celdaA;
                                mejorCeldaB = celdaB;
                                mejorHabitacionA = roomA;
                                mejorHabitacionB = habitacionPrincipal;
                            }
                        }
                    }
                }


                if (conexionEncontrada)
                {
                    crearPasillo(mejorHabitacionA, mejorHabitacionB, mejorCeldaA, mejorCeldaB);
                }
            }
        }
    }

    /// <summary>
    /// Crea un pasillo uniendo esas habitaciones
    /// </summary>
    /// <param name="roomA"></param>
    /// <param name="roomB"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    void crearPasillo(Room roomA, Room roomB, Cell startPos, Cell endPos)
    {
        Room.conectarHabitaciones(roomA, roomB);

        List<Cell> lineaEntreHabitaciones = obtenerlineaEntreHabitaciones(startPos, endPos, roomA, roomB);
        lineaEntreHabitaciones.Insert(0, startPos);

        foreach (Cell c in lineaEntreHabitaciones)
        {
            crearPasillo(c);
        }

    }

    /// <summary>
    /// Crea el pasillo en el tablero
    /// </summary>
    /// <param name="c"></param>
    /// <param name="conVecinos"></param>
    void crearPasillo(Cell c)
    {

        int drawX = c.cellInfo.x;
        int drawY = c.cellInfo.y;

        tablero[drawX, drawY].value = CellsType.alive;
        tablero[drawX, drawY].color = Color.red;

        if (!this.pasillosEstrechos)
        {
            List<Cell> vecinos = c.getVecinos(tablero);

            foreach (Cell cell in vecinos)
            {
                cell.value = CellsType.alive;
                cell.color = Color.red;
            }
        }

    }

    /// <summary>
    /// Mediante Pathfinding encuentra un camino de habitacion a habitacion
    /// </summary>
    /// <param name="desde"></param>
    /// <param name="hasta"></param>
    /// <param name="roomDesde"></param>
    /// <param name="roomHasta"></param>
    /// <returns></returns>
    List<Cell> obtenerlineaEntreHabitaciones(Cell desde, Cell hasta, Room roomDesde, Room roomHasta)
    {

        PathFinding pf = new PathFinding();
        return pf.calcularRuta(tablero, desde, hasta);

    }

}