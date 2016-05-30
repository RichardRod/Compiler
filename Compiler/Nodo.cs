using System;
using System.Collections.Generic;

namespace Compiler
{
  public class Nodo
  {
    //atributos
    public static int sangria;
    public string simbolo;
    public Nodo nodoSiguiente;
    protected List<Nodo> hijos = new List<Nodo>();
    public static List<Variable> listaExpresion = new List<Variable>();

    //atributos semantico
    public static TablaSimbolos tablaSimbolos;
    public static string tipoAmbito = "Global";
    private char tipoDato;
    private string nombreFuncion;
    protected static string funcionActual;

    protected static int contadorRegistro = 1;

    public char TipoDato
    {
      get { return tipoDato; }
      set { tipoDato = value; }
    }

    public string NombreFuncion
    {
      get { return nombreFuncion; }
      set { nombreFuncion = value; }
    }

    public List<Nodo> Hijos
    {
      get { return hijos; }
    }

    //constructor
    public Nodo()
    {
    }

    public Nodo(string texto)
    {
      this.simbolo = texto;
      tipoDato = 'v';
    }

    public virtual void Muestra()
    {
    }

    public virtual void MuestraSangria()
    {
      //for (int i = 0; i < sangria; i++)
      //Console.Write (" ");
    }

    public void AñadirHijo(Nodo hijo)
    {
      hijos.Add(hijo);
    }

    public virtual void ValidaTipos()
    {
      tipoDato = tipoDato = 'v';

      foreach (Nodo nodo in hijos)
      {
        nodo.ValidaTipos();
      }//fin de foreach

    } //fin del metodo ValidaTipos

    public virtual char ObtenerTipo()
    {
      return ' ';
    }

    public virtual void GeneraCodigoEnsamblador()
    {

    }

  } //fin de la clase Nodo
}

