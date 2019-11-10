using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tipos de celda que puede haber en nuestro mapa
/// </summary>
public enum CellsType
{
    dead = 0,
    alive = 1
}

/// <summary>
/// Celda de juego
/// </summary>
public class Cell
{

    /// <summary>
    /// Informacion de la celda en un tablero
    /// </summary>
    public CellInfo cellInfo { get; set; }

    /// <summary>
    /// Valor del contenido de la celda
    /// </summary>
    public CellsType value { get; set; }

    /// <summary>
    /// Cuenta de vecinos vivos
    /// </summary>
    public int countNeighborsAlive { get; set; }

    /// <summary>
    /// Probabilidad de iniciar como suelo
    /// </summary>
    public float probability_alive { get; set; }

    /// <summary>
    /// Color de representacion de la célula
    /// </summary>
    public Color color { get; set; }

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public Cell()
    {
    }

    /// <summary>
    /// Constructor para cuando inicializamos una celda en un mundo
    /// </summary>
    public Cell(int x, int y, float _probability_alive = 0.4f)
    {
        this.probability_alive = _probability_alive;
        float prob = UnityEngine.Random.Range(0f, 1f);
        this.value = (prob < probability_alive) ? CellsType.dead : CellsType.alive;
        this.cellInfo = new CellInfo(x, y);
        this.color = this.value == CellsType.dead ? Color.black : Color.white;

    }

    /// <summary>
    /// Constructor de copia
    /// </summary>
    /// <param name="oldCell">Celda antigua</param>
    public Cell(Cell oldCell)
    {
        this.value = oldCell.value;
        this.cellInfo = new CellInfo(oldCell.cellInfo.x, oldCell.cellInfo.y);
        this.probability_alive = oldCell.probability_alive;
        this.countNeighborsAlive = oldCell.countNeighborsAlive;
        this.color = oldCell.color;

    }

    /// <summary>
    /// Constructor para cuando inicializamos una celda en un mundo
    /// </summary>
    public Cell(int x, int y, CellsType _value, float _probability_alive = 0.4f)
    {
        this.value = _value;
        this.cellInfo = new CellInfo(x, y);
        this.probability_alive = _probability_alive;
        this.color = this.value == CellsType.dead ? Color.black : Color.white;


    }

    /// <summary>
    /// Devuelve los vecinos que son walkables
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public List<Cell> getVecinosWalkables(Tablero board)
    {
        int radioVecinos = board.radioVecino;

        List<Cell> celdasWalkables = new List<Cell>();

        //Cogemos todos los vecinos
        for (int y = radioVecinos; y >= -radioVecinos; --y)
        {
            for (int x = radioVecinos; x >= -radioVecinos; --x)
            {
                int NeighborX = cellInfo.x + x;
                int NeighborY = cellInfo.y + y;

                if (
                    (NeighborX >= 0 && NeighborX < board.world_cell.GetLength(0))
                 && (NeighborY >= 0 && NeighborY < board.world_cell.GetLength(1))
                 && (Math.Abs(x) + Math.Abs(y) != 0)
                 && (NeighborX == cellInfo.x || NeighborY == cellInfo.y)
                 )
                {

                    celdasWalkables.Add(board[NeighborX, NeighborY]);

                }
            }
        }

        return celdasWalkables;
    }

    /// <summary>
    /// Devuelve los vecinos que son walkables
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public List<Cell> getVecinos(Tablero board)
    {
        int radioVecinos = board.radioVecino;

        List<Cell> celdasWalkables = new List<Cell>();

        //Cogemos todos los vecinos
        for (int y = radioVecinos; y >= -radioVecinos; --y)
        {
            for (int x = radioVecinos; x >= -radioVecinos; --x)
            {
                int NeighborX = cellInfo.x + x;
                int NeighborY = cellInfo.y + y;

                if (
                    (NeighborX >= 0 && NeighborX < board.world_cell.GetLength(0))
                 && (NeighborY >= 0 && NeighborY < board.world_cell.GetLength(1))
                 && (Math.Abs(x) + Math.Abs(y) != 0)
                 )
                {

                    celdasWalkables.Add(board[NeighborX, NeighborY]);

                }
            }
        }

        return celdasWalkables;
    }



}
