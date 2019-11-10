using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nodo utilizado para el calculo del pathfinding
/// </summary>
public class Nodo
{
    /// <summary>
    /// Estado de la celda del nodo
    /// </summary>
    public Cell estado;

    /// <summary>
    /// Nodo padre
    /// </summary>
    public Nodo nodoPadre;

    /// <summary>
    /// Constructor con parametros
    /// </summary>
    /// <param name="_estado"></param>
    /// <param name="_padre"></param>
    public Nodo(Cell _estado, Nodo _padre)
    {

        estado = _estado;
        nodoPadre = _padre;

    }

    /// <summary>
    /// Expandimos la lista, con todos los vecinos walkables por el usuario
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public List<Nodo> Expandir(Tablero board)
    {

        List<Nodo> result = new List<Nodo>();
        List<Cell> vecinos = estado.getVecinosWalkables(board);

        for (int i = 0; i < vecinos.Count; i++)
        {
            Cell vecino = vecinos[i];

            Nodo nuevo = new Nodo(vecino, this);
            result.Add(nuevo);


        }

        return result;
    }

    /// <summary>
    /// Devuelve el nodo padre
    /// </summary>
    /// <returns></returns>
    public Nodo getPadre()
    {
        return nodoPadre;
    }

    /// <summary>
    /// Determina si el nodo ha llegado a la meta
    /// </summary>
    /// <param name="nodo">Nodo a comprobar</param>
    /// <param name="to">Celda meta</param>
    /// <returns></returns>
    public bool esMeta(Nodo nodo, Cell to)
    {
        return nodo.estado.cellInfo.x == to.cellInfo.x && nodo.estado.cellInfo.y == to.cellInfo.y;
    }
}