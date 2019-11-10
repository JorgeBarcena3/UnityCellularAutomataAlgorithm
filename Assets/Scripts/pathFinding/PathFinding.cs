using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Clase utilizada para encontrar el pathFinding
/// </summary>
public class PathFinding
{

    //Posicion Inicial de nuestro player
    private Nodo posicionIncial;

    //Lista de nodos abiertos
    private List<Nodo> abierta = new List<Nodo>();

    //Limite de nodos
    public int limiteDeNodos = 10000;
    private int nodosActuales = 0;

    /// <summary>
    /// Devuelve la ruta desde una posicion a otra
    /// </summary>
    /// <param name="board"></param>
    /// <param name="currentPos"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    public List<Cell> calcularRuta(Tablero board, Cell currentPos, Cell goal)
    {
        //Añadimos el nodo principal
        abierta.Add(new Nodo(currentPos, null));


        //Nodo actual
        Nodo nodo = null;

        while (abierta.Count > 0 && nodosActuales < limiteDeNodos)
        {
            nodo = abierta[0];
            abierta.RemoveAt(0);

            if (nodo.esMeta(nodo, goal))
            {
                return nodoACelda(nodo);

            }
            else
            {
                List<Nodo> sucesores = nodo.Expandir(board);

                foreach (Nodo s in sucesores)
                {
                    //Si no lo contiene lo añadimos
                    if (!estaEnLaLista(s, abierta))
                    {
                        abierta.Add(s);
                        nodosActuales++;
                    }
                }
            }

        }

        return new List<Cell>();
    }

    /// <summary>
    /// Recoje el camino necesario para llegar a esa celda
    /// </summary>
    /// <param name="abierta"></param>
    /// <returns></returns>
    private List<Cell> nodoACelda(Nodo abierta)
    {
        List<Cell> wayPoints = new List<Cell>();

        Nodo nodo = abierta;
        while (nodo.getPadre() != null)
        {
            wayPoints.Add(nodo.estado);
            nodo = nodo.getPadre();
        }

        return wayPoints;
    }

    /// <summary>
    /// Se determina si el nodo correspondiente esta en la lista de abiertos
    /// </summary>
    /// <param name="nodo"></param>
    /// <param name="abierta"></param>
    /// <returns></returns>
    public bool estaEnLaLista(Nodo nodo, List<Nodo> abierta)
    {
        bool esta = false;

        foreach (Nodo s in abierta)
        {
            if (s.estado.cellInfo.x == nodo.estado.cellInfo.x
                && s.estado.cellInfo.y == nodo.estado.cellInfo.y
                )
                esta = true;
        }
        return esta;
    }

}

