using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manager de las reglas del algoritmo del cellular atomata
/// </summary>
public class RuleManager
{
    /// <summary>
    /// Radio por el que se calcularan los vecinos
    /// </summary>
    public string ruleGeneration { get; private set; }

    /// <summary>
    /// Survive char
    /// </summary>
    private char S_prefix { get; set; }

    /// <summary>
    /// Born char
    /// </summary>
    private char B_prefix { get; set; }

    /// <summary>
    /// Reglas para las celulas vivas
    /// </summary>
    List<int> survive_rules { get; set; }

    /// <summary>
    /// Reglas para las celulas que deben nacer
    /// </summary>
    List<int> born_rules { get; set; }

    public RuleManager(string _ruleGeneration, char _S_prefix, char _B_prefix)
    {

        this.ruleGeneration = _ruleGeneration;
        this.S_prefix = _S_prefix;
        this.B_prefix = _B_prefix;

        survive_rules = getRules(S_prefix); //Las matamos
        born_rules = getRules(B_prefix); //Las crecemos
    }

    /// <summary>
    /// Obtenemos las reglas segun determinemos en los parametros
    /// </summary>
    /// <param name="index">Prefijo de cada zona</param>
    /// <param name="separator">Separador de reglas</param>
    /// <returns></returns>
    private List<int> getRules(char index, char separator = '/')
    {
        List<string> rules = this.ruleGeneration.Split(separator).OfType<string>().ToList();

        string surviveRules = rules.Where(m => m.Contains(index)).FirstOrDefault();

        if (surviveRules == null)
        {
            if (index == 'B')
            {
                surviveRules = rules[1];
            }
            else
            {
                surviveRules = rules[0];

            }
        }

        List<int> surviveRulesList = new List<int>();

        if (surviveRules != null)
        {
            for (int i = 0; i < surviveRules.Length; ++i)
            {
                if ((surviveRules[i] != index))
                {
                    surviveRulesList.Add(int.Parse(surviveRules[i].ToString()));
                }
            }

        }

        return surviveRulesList;
    }

    /// <summary>
    /// Aplicamos las reglas de supervivencia
    /// </summary>
    /// <param name="parent_board">Tablero de referencia</param>
    /// <param name="cell">Celda actual</param>
    /// <returns></returns>
    public Cell applyRules(Cell cell)
    {
        Cell next_cell = new Cell (cell);
        next_cell.color = next_cell.value == CellsType.dead ? Color.black : Color.white;

        if (next_cell.value == CellsType.dead)
        {
            if (born_rules.Contains(next_cell.countNeighborsAlive))
            {
                next_cell = new Cell(next_cell.cellInfo.x, next_cell.cellInfo.y, CellsType.alive);
                next_cell.color = Color.blue;

            }
        }
        else if (next_cell.value == CellsType.alive)
        {
            if (survive_rules.Contains(next_cell.countNeighborsAlive))
            {
                next_cell = new Cell(next_cell.cellInfo.x, next_cell.cellInfo.y, CellsType.alive);
            }
            else
            {
                next_cell = new Cell(next_cell.cellInfo.x, next_cell.cellInfo.y, CellsType.dead);
                next_cell.color = Color.red;

            }

        }


        return next_cell;
    }
}