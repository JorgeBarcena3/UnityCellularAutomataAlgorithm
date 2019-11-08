using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Radio por el que se calcularan los vecinos
    /// </summary>
    public string ruleGeneration { get; private set; }

    /// <summary>
    /// Reglas para las celulas vivas
    /// </summary>
    List<int> survive_rules { get; set; }

    /// <summary>
    /// Reglas para las celulas que deben nacer
    /// </summary>
    List<int> born_rules { get; set; }

    /// <summary>
    /// Probabilidades de empezar vivo
    /// </summary>
    private float chanceToLive { get; set; }

    /// <summary>
    /// Constructor para crear el tablero
    /// </summary>
    /// <param name="tamanioX"></param>
    /// <param name="tamanioY"></param>
    public Tablero(int tamanioX, int tamanioY, int _radioVecino, string _reglaDeGeneracion, float probabilidades_de_ser_suelo_inicial)
    {
        this.ruleGeneration = _reglaDeGeneracion;
        this.width = tamanioX;
        this.height = tamanioY;
        this.world_cell = new Cell[width, height];
        this.radioVecino = _radioVecino;
        this.chanceToLive = probabilidades_de_ser_suelo_inicial;
        survive_rules = getRules('S'); //Las matamos
        born_rules = getRules('B'); //Las crecemos
    }

    /// <summary>
    /// Llena un array con cells aleatorias
    /// </summary>
    public void createRandomWorld()
    {
        for (int i = 0; i < this.world_cell.GetLength(0); i++)
        {
            for (int j = 0; j < this.world_cell.GetLength(1); j++)
            {
                this.world_cell[i, j] = new Cell(this, i, j, chanceToLive);
            }
        }

        searchNeighbors();

    }

    /// <summary>
    /// Una vez completado el tablero buscamos y almacenamos los vecinos
    /// </summary>
    private void searchNeighbors()
    {


        for (int i = 0; i < this.world_cell.GetLength(0); i++)
        {
            for (int j = 0; j < this.world_cell.GetLength(1); j++)
            {
                this.world_cell[i, j].setNeighbors();
            }
        }
    }

    /// <summary>
    /// Actualizamos el estado de las celulas
    /// </summary>
    public void computeNeighbors()
    {

        Cell[,] next = new Cell[width, height];

        for (int i = 0; i < this.world_cell.GetLength(0); i++)
        {
            for (int j = 0; j < this.world_cell.GetLength(1); j++)
            {
                Cell cell = this.world_cell[i, j];
                next[i, j] = applyRules(survive_rules, born_rules, cell);

            }
        }

        this.world_cell = next;

        searchNeighbors();

    }

    /// <summary>
    /// Aplicamos las reglas de supervivencia
    /// </summary>
    /// <param name="survive_rules">Reglas de supervivencia (Muerte)</param>
    /// <param name="born_rules">Reglas de supervivencia (Vida)</param>
    /// <param name="cell">Celda actual</param>
    /// <returns></returns>
    private Cell applyRules(List<int> survive_rules, List<int> born_rules, Cell cell)
    {
        Cell next_cell = cell;

        if (cell.value == CellsType.dead)
        {
            if (born_rules.Contains(cell.countNeighborsAlive))
            {
                next_cell = new Cell(this, cell.cellInfo.x, cell.cellInfo.y, CellsType.alive);

            }
        }
        else if (cell.value == CellsType.alive)
        {
            if (survive_rules.Contains(cell.countNeighborsAlive))
            {
                next_cell = new Cell(this, cell.cellInfo.x, cell.cellInfo.y, CellsType.alive);
            }
            else
            {
                next_cell = new Cell(this, cell.cellInfo.x, cell.cellInfo.y, CellsType.dead);
            }

        }


        return next_cell;
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


}