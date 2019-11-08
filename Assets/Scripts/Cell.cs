using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// Contador de las id de las celdas
    /// </summary>
    public static int idCount = 0;

    /// <summary>
    /// Id unico de cada celda
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Informacion de la celda en un tablero
    /// </summary>
    public CellInfo cellInfo { get; set; }

    /// <summary>
    /// Valor del contenido de la celda
    /// </summary>
    public CellsType value { get; set; }

    /// <summary>
    /// Lista de vecinos de la celda
    /// </summary>
    public List<Cell> neighbors { get; private set; }

    /// <summary>
    /// Cuenta de vecinos vivos
    /// </summary>
    public int countNeighborsAlive { get; private set; }

    /// <summary>
    /// Probabilidad de iniciar como suelo
    /// </summary>
    public float probability_alive { get; set; }

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public Cell()
    {
        this.Id = Cell.idCount++;

    }

    /// <summary>
    /// Constructor para cuando inicializamos una celda en un mundo
    /// </summary>
    public Cell(Tablero _world, int x, int y, float _probability_alive = 0.4f)
    {
        this.Id = Cell.idCount++;
        this.probability_alive = _probability_alive;
        float prob = UnityEngine.Random.Range(0f, 1f);
        this.value = (prob < probability_alive) ? CellsType.dead : CellsType.alive;
        this.cellInfo = new CellInfo(_world, x, y);

    }

    /// <summary>
    /// Constructor para cuando inicializamos una celda en un mundo
    /// </summary>
    public Cell(Tablero _world, int x, int y, CellsType _value, float _probability_alive = 0.4f)
    {
        this.Id = Cell.idCount++;
        this.value = _value;
        this.cellInfo = new CellInfo(_world, x, y);
        this.probability_alive = _probability_alive;


    }

    /// <summary>
    /// Computa los vecinos de una celda
    /// </summary>
    /// <param name="_world">Mundo donde buscar los vecinos</param>
    /// <returns>Vecinos de la celda</returns>
    public void setNeighbors()
    {
        int radioVecinos = this.cellInfo.world.radioVecino;

        this.neighbors = new List<Cell>();
        this.countNeighborsAlive = 0;

        //Cogemos todos los vecinos
        for (int y = radioVecinos; y >= -radioVecinos; --y)
        {
            for (int x = radioVecinos; x >= -radioVecinos; --x)
            {
                int NeighborX = this.cellInfo.x + x;
                int NeighborY = this.cellInfo.y + y;

                if (
                    (NeighborX >= 0 && NeighborX < this.cellInfo.world.world_cell.GetLength(0))
                 && (NeighborY >= 0 && NeighborY < this.cellInfo.world.world_cell.GetLength(1))

                 )
                {
                    if ((x + y == 0))
                    {
                        //NO HAGO NADA
                    }
                    else
                    {
                        this.neighbors.Add(this.cellInfo.world.world_cell[NeighborX, NeighborY]);
                        if (this.neighbors.Last().value == CellsType.alive)
                            countNeighborsAlive++;
                    }
                }
            }
        }

    }

}
