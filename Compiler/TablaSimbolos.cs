using System;
using System.Collections.Generic;

namespace Compiler
{
  public class TablaSimbolos
  {
    //atributos
    protected List<ElementoTabla> tabla;
    public Variable varLocal;
    public Variable varGlobal;
    public Funcion funcion;
    public List<String> listaErrores;

    public TablaSimbolos(List<String> listaErrores)
    {
      this.listaErrores = listaErrores;
      tabla = new List<ElementoTabla>();
    }

    public void Agrega(ElementoTabla elemento)
    {
      Console.WriteLine("Agregando Elemento Tabla");
      tabla.Add(elemento);
    }

    public void Muestra()
    {
      Console.WriteLine("Tabla de simbolos");

      if (tabla.Count != 0)
      {
        Console.WriteLine("Funcion:\t\tTipo:");
        foreach (ElementoTabla elemento in tabla)
        {
          Console.WriteLine(elemento.Simbolo + "\t\t" + elemento.Tipo);
        }
      }
      else
      {
        Console.WriteLine("Tabla de simbolos vacia");
      }
    } //fin del metodo Muestra

    public bool VarGlobalDefinida(string variable)
    {
      foreach (ElementoTabla elemento in tabla)
      {
        if (elemento.EsVariable() && !elemento.EsVarLocal())
        {
          if (elemento.Simbolo.CompareTo(variable) == 0)
            return true;
        }
      } //fin de foreach

      return false;
    } //fin de la funcion VarGlobalDefinida

    public bool FuncionDefinida(string simbolo)
    {
      foreach (ElementoTabla elemento in tabla)
      {
        if (elemento.EsFuncion())
        {
          if (elemento.Simbolo.CompareTo(funcion) == 0)
            return true;
        }
      } //fin de foreach
      return false;
    }

    public bool VarLocalDefinida(string variable, string funcion)
    {
      foreach (ElementoTabla elemento in tabla)
      {
        if (elemento.EsVariable() && elemento.EsVarLocal())
        {
          if (((Variable) elemento).Ambito.CompareTo(funcion) == 0 && elemento.Simbolo.CompareTo(variable) == 0)
            return true;
        }
      } //fin de foreach


      return false;
    }

    public void BuscaIdentificador(string simbolo)
    {
      varGlobal = null;
      varLocal = null;
      funcion = null;

      foreach (ElementoTabla elemento in tabla)
      {
        if (elemento.Simbolo.CompareTo(simbolo) == 0)
        {
          if (elemento.EsVariable())
          {
            if (elemento.EsVarLocal())
              varLocal = (Variable) elemento;
            else
              varGlobal = (Variable) elemento;
          }
          else
            funcion = (Funcion) elemento;
        } //fin de if
      } //fin de foreach
    }

    public bool BuscaFuncion(string identificador)
    {
      varGlobal = null;
      varLocal = null;
      funcion = null;

      var identificadoresEncontrados = tabla.FindAll(e => ((e.Simbolo == identificador) && e.EsFuncion()));

      if (identificadoresEncontrados.Count == 1)
        funcion = (Funcion) identificadoresEncontrados[0];

      return identificadoresEncontrados.Count != 1 ? false : true;

    }//fin del metodo BuscaFuncion

    public void AgregarError(String error)
    {
      Console.WriteLine("Agregando Error: " + error);
      listaErrores.Add(error);
    }//fin del metodo AgregarError

    public void Agrega(Variables defVar)
    {
    }

    public void Agrega(DefFunc defFunc)
    {
      bool correcto = true;

      Funcion funcion = new Funcion(defFunc.TipoDato, defFunc.NombreFuncion, defFunc.GetParametros());

      //Console.WriteLine(funcion.Tipo + " " + funcion.Simbolo);

      if (BuscaFuncion(funcion.Simbolo))
      {
        AgregarError("La Funcion: " + funcion.Simbolo + " ya fue definida.");
        correcto = false;
      }

      if(correcto)
        Agrega(funcion);

    }//fin del metodo Agrega

    public void Agrega(Parametros parametros)
    {
    }
  } //fin de la clase TablaSimbolos

  public class ElementoTabla
  {
    //atributos
    private string simbolo;
    private char tipo;

    public string Simbolo
    {
      get { return simbolo; }
      set { simbolo = value; }
    }

    public char Tipo
    {
      get { return tipo; }
      set { tipo = value; }
    }

    public virtual bool EsVariable()
    {
      return false;
    }

    public virtual bool EsVarLocal()
    {
      return false;
    }

    public virtual bool EsFuncion()
    {
      return false;
    }

    public virtual void Muestra()
    {
    }
  } //fin de la clase ElementoTabla

  public class Variable : ElementoTabla
  {
    //atributos
    protected bool local;
    private string ambito;

    //constructor
    public Variable(char tipo, string simbolo, string ambito)
    {
      this.Tipo = tipo;
      this.Simbolo = simbolo;
      this.ambito = ambito;
      this.local = (this.Ambito.CompareTo("") != 0);
    } //fin del constructor

    public string Ambito
    {
      get { return ambito; }
    }

    public override bool EsVarLocal()
    {
      return local;
    }

    public override bool EsVariable()
    {
      return true;
    }

    public override void Muestra()
    {
      Console.WriteLine("Variable: " + Simbolo + " Tipo: " + Tipo);

      if (local)
        Console.WriteLine("Local");
      else
        Console.WriteLine("Global");
    }
  } //fin de la clase Variable

  public class Funcion : ElementoTabla
  {
    //atributos
    private Parametros parametros;

    //constructor
    public Funcion(char tipo, string simbolo, Parametros parametros)
    {
      Simbolo = simbolo;
      Tipo = tipo;
      this.parametros = parametros;
    }//fin del constructor

    public Parametros Parametros
    {
      get { return parametros; }
    }

    public String ParametrosCadena
    {
      get
      {
        String parametrosCadena = "";
        if (parametros != null)
        {
          foreach (Nodo parametro in parametros.Hijos)
          {
            parametrosCadena += (parametro.Hijos[0].simbolo + " "); //tipo del parametro
            parametrosCadena += parametro.Hijos[1].simbolo + " "; //identificador del parametro
          }//fin de foreach

          parametrosCadena = parametrosCadena.Substring(0, parametrosCadena.Length - 2);
        }//fin de if

        return parametrosCadena;
      }
    }

    public override bool EsFuncion()
    {
      return true;
    }

    public override void Muestra()
    {
      Console.WriteLine("Funcion: " + Simbolo + " Tipo: " + Tipo + " Parametros: " + parametros);
    }
  } //fin de la clase Funcion
} //fin del espacio de nombres Compilador

