using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Informacion de cada celda
/// </summary>
public class CellInfo
{
 
    /// <summary>
    /// Posicion X que ocupa
    /// </summary>
    public int x;

    /// <summary>
    /// Posicion Y que ocupa
    /// </summary>
    public int y;


    /// <summary>
    /// Constructor que almacena la info de la casilla
    /// </summary>
    /// <param name="_x">Posicion X</param>
    /// <param name="_y">Posicion Y</param>
    public CellInfo(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }
}