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
        MuestraFunciones();
        Console.WriteLine("\n");
        MuestraVariables();
        Console.WriteLine("\n");
      } //fin de if
      else
      {
        Console.WriteLine("Tabla de simbolos vacia");
      } //fin de else
    } //fin del metodo Muestra

    private void MuestraFunciones()
    {
      Console.WriteLine("Funcion:\t\tTipo:");

      foreach (ElementoTabla elemento in tabla)
      {
        if(elemento.EsFuncion())
          Console.WriteLine(elemento.Simbolo + "\t\t" + elemento.Tipo);
      } //fin de foreach
    } //fin del metodo MuestraFunciones

    public Funcion ObtenerFuncion(string simbolo)
    {
      Funcion miFuncion = null;

      foreach (ElementoTabla elemento in tabla)
      {
        if (elemento.EsFuncion())
        {
          if(elemento.Simbolo == simbolo)
            miFuncion = (Funcion) elemento;
        }
      }
      return miFuncion;

    }//fin del metodo ObtenerFuncion

    private void MuestraVariables()
    {
      Console.WriteLine("Variable:\t\tTipo:\t\tAmbito:");

      foreach (ElementoTabla elemento in tabla)
      {
        if(elemento.EsVariable())
          Console.WriteLine(elemento.Simbolo + "\t\t" + elemento.Tipo + "\t\t" + ((Variable)elemento).Ambito);
      }//fin de foreach
    }//fin del metodo MuestraVariables

    public bool VariableLocalDefinida(string variable)
    {
      foreach (ElementoTabla elemento in tabla)
      {
        if (elemento.EsVariable() && elemento.EsVarLocal())
        {
          if (elemento.Simbolo == variable)
            return true;
        } //fin de if
      } //fin de foreach

      return false;
    } //fin del metodo VariableLocalDefinida

    public bool VariableGlobalDefinida(string variable)
    {
      foreach (ElementoTabla elemento in tabla)
      {
        if (elemento.EsVariable() && !elemento.EsVarLocal())
        {
          if(elemento.Simbolo == variable)
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
          if (simbolo == elemento.Simbolo)
            return true;
        }
      } //fin de foreach
      return false;
    } //fin del metodo FuncionDefinida

    public ElementoTabla BuscaIdentificador(string simbolo)
    {

      foreach (ElementoTabla elemento in tabla)
      {
        if (elemento.Simbolo == simbolo)
        {
          if (elemento.EsFuncion())
            return (Funcion)elemento;

        }//fin de if

        /*if (elemento.Simbolo.CompareTo(simbolo) == 0)
        {
          if (elemento.EsVariable())
          {
            if (elemento.EsVarLocal())
              return varLocal = (Variable) elemento;
            else
              return varGlobal = (Variable) elemento;
          }
          else
            return funcion = (Funcion) elemento;
        } //fin de if*/
      } //fin de foreach

      return null;
    }

    public Variable ObtenerVariable(string simbolo)
    {
      Variable miVar = null;

      foreach (ElementoTabla elemento in tabla)
      {
        if (elemento.EsVariable()  )
        {
          if(elemento.Simbolo == simbolo)
            miVar = (Variable)elemento;
        }
      }

      return miVar;
    }

    public void AgregarError(String error)
    {
      Console.WriteLine("Agregando Error: " + error);
      listaErrores.Add(error);
    } //fin del metodo AgregarError

    public void Agrega(DefVar defVar)
    {
      bool correcto = true;

      Variable variable = new Variable(defVar.TipoDato, defVar.Identificador, defVar.Ambito);

      if (VariableLocalDefinida(variable.Simbolo) || VariableGlobalDefinida(variable.Simbolo))
      {
        AgregarError("La Variable: \"" + variable.Simbolo + "\" ya fue definida");
        correcto = false;
      }

      if (correcto)
        Agrega(variable);
    } //fin del metodo Agrega

    public void Agrega(DefFunc defFunc)
    {
      bool correcto = true;

      Funcion funcion = new Funcion(defFunc.TipoDato, defFunc.NombreFuncion, defFunc.GetParametros());

      Console.WriteLine("Agrega Funcion: " + defFunc.GetParametros());

      if (FuncionDefinida(funcion.Simbolo))
      {
        AgregarError("La Funcion: \"" + funcion.Simbolo + "\" ya fue definida.");
        correcto = false;
      } //fin de if

      if (correcto)
        Agrega(funcion);
    } //fin del metodo Agrega

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
      Tipo = tipo;
      Simbolo = simbolo;
      this.ambito = ambito;
      if (ambito == "Local")
        local = true;
      else
        local = false;
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
    } //fin del constructor

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
          Console.WriteLine("Entreeee: " + parametros.Hijos.Count);
          foreach (Nodo parametro in parametros.Hijos)
          {
            Console.WriteLine("Cadena: " + parametrosCadena);
            parametrosCadena += (parametro.Hijos[0].simbolo + " "); //tipo del parametro
            parametrosCadena += parametro.Hijos[1].simbolo + " "; //identificador del parametro
          } //fin de foreach

          //parametrosCadena = parametrosCadena.Substring(0, parametrosCadena.Length - 2);
        } //fin de if
        else
        {
          Console.WriteLine("Es nulo");
        }

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

