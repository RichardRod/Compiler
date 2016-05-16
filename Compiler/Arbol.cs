using System;
using System.Collections.Generic;

namespace Compiler
{
  //R1 <programa> ::= <Definiciones>
  public class Programa : Nodo
  {
    //atributos
    private Definiciones definiciones;

    public Programa(Stack<ElementoPila> pila)
    {
      simbolo = "<Programa>";

      pila.Pop(); //estado
      definiciones = (Definiciones) pila.Pop().Nodo;
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine("<Programa>");

      if (definiciones != null)
      {
        definiciones.Muestra();
      } //fin de if
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      //base.ValidaTipos();
      Console.WriteLine("Valida en Programa");

      if (definiciones != null)
      {
        definiciones.ValidaTipos();
      }
    }//fin del metodo ValidaTipos
  } //fin de la clase Programa

  //R2 <Definiciones> ::= \e
  //R3 <Definiciones> ::= <Definicion> <Definiciones>
  public class Definiciones : Nodo
  {
    //atributos
    Nodo definiciones;
    Nodo definicion; //defVar defFunc

    public Definiciones(Stack<ElementoPila> pila)
    {
      simbolo = "<Definiciones>";

      pila.Pop();
      definiciones = pila.Pop().Nodo;
      pila.Pop();
      definicion = pila.Pop().Nodo;
    } //fin del constructor

    public override void Muestra()
    {
      if (definicion != null)
      {
        sangria++;
        definicion.MuestraSangria();
        Console.WriteLine("<Definicion>");
        definicion.Muestra();
        sangria--;
      } //fin de if

      if (definiciones != null)
      {
        sangria++;
        MuestraSangria();
        Console.WriteLine("<Definiciones>");
        definiciones.Muestra();
      } //fin de if

    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en Definiciones");

      if (definicion != null)
      {
        definicion.ValidaTipos();
      }//fin de if

      if (definiciones != null)
      {
        definiciones.ValidaTipos();
      }//fin de if
    }//fin del metodo ValidaTipos

  } //fin de la clase Definiciones

  //R4 <Definicion> ::= <DefVar>
  public class Variables : Nodo
  {
    //atributos
    private string tipo;
    private string identificador;
    private List<Nodo> listaVariables;
    private string puntoComa;


    public Variables(Stack<ElementoPila> pila)
    {
      simbolo = "<ListaVar>";

      pila.Pop();
      puntoComa = pila.Pop().Elemento;

      pila.Pop();
      listaVariables = pila.Pop().Nodo.Hijos;

      pila.Pop();
      identificador = pila.Pop().Elemento;

      pila.Pop();
      tipo = pila.Pop().Elemento;
    } //fin del constructor

    public override void Muestra()
    {
      sangria++;
      MuestraSangria();
      Console.WriteLine("<ListaVar>");
      sangria++;
      MuestraSangria();
      Console.WriteLine("<ListaVariables>");
      sangria++;
      MuestraSangria();
      Console.WriteLine("<Tipo> " + tipo);
      sangria++;
      MuestraSangria();
      Console.WriteLine("<Identificador> " + identificador);

      for (int i = 0; i < listaVariables.Count; i++)
      {
        MuestraSangria();
        Console.WriteLine("<Identificador> " + listaVariables[i].Hijos[0].simbolo);
      } //fin de for

      sangria -= 2;
      MuestraSangria();
      Console.WriteLine("<PuntoComa> " + puntoComa);
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en Variables");

      DefVar variable = new DefVar(tipo, identificador);

      if(variable != null)
        variable.ValidaTipos();

      for (int i = 0; i < listaVariables.Count; i++)
      {
        DefVar definicionVariable = new DefVar(tipo, listaVariables[i].Hijos[0].simbolo);

        if(definicionVariable != null)
          definicionVariable.ValidaTipos();

      }//fin de for
    }//fin del metodo Valida Tipos

  } //fin de la clase Variables

  public class DefVar : Nodo
  {
    //atributos
    private string identificador;

    //constructor
    public DefVar(string tipo, string identificador)
    {
      TipoDato = Tipo.DameTipo(tipo);
      this.identificador = identificador;

    }//fin del constructor

    public string Identificador
    {
      get { return identificador; }
    }

    public override void ValidaTipos()
    {
      Console.WriteLine("Agregando Variable: " + identificador + " Tipo: " + TipoDato);
      tablaSimbolos.Agrega(this);
    }//fin del metodo VaidaTipos

  }//fin de la clase DefVar

  //R5 <Definicion> ::= <DefFunc>
  public class DefFunc : Nodo
  {
    //atributos
    string tipo;
    string identificador;
    string parentesisInicio;
    Nodo parametros;
    string parentesisFin;
    Nodo bloqueFunc;

    public DefFunc(Stack<ElementoPila> pila)
    {
      simbolo = "<DefFunc>";

      pila.Pop();
      bloqueFunc = pila.Pop().Nodo;

      pila.Pop();
      parentesisFin = pila.Pop().Elemento;

      pila.Pop();
      parametros = pila.Pop().Nodo;

      pila.Pop();
      parentesisInicio = pila.Pop().Elemento;

      pila.Pop();
      identificador = pila.Pop().Elemento;

      pila.Pop();
      tipo = pila.Pop().Elemento;
    } //fin del constructor

    public override void Muestra()
    {
      sangria++;
      MuestraSangria();
      Console.WriteLine("<DefinicionFuncion>");
      sangria++;
      MuestraSangria();
      Console.WriteLine("<Tipo> " + tipo);
      sangria++;
      MuestraSangria();
      Console.WriteLine("<Identificador> " + identificador);
      sangria++;
      MuestraSangria();
      Console.WriteLine("<ParentesisInicio> " + parentesisInicio);
      sangria++;
      MuestraSangria();
      Console.WriteLine("<Parametros>");
      if (parametros != null)
      {
        sangria++;
        MuestraSangria();
        parametros.Muestra();
      }
      sangria -= 2;
      MuestraSangria();
      Console.WriteLine("<ParentesisFin> " + parentesisFin);
      if (bloqueFunc != null)
      {
        sangria++;
        MuestraSangria();
        Console.WriteLine("<BloqueFuncion>");
        bloqueFunc.Muestra();
      } //fin de if
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en DefFunc");

      tipoAmbito = "Local";
      TipoDato = Tipo.DameTipo(tipo);
      NombreFuncion = identificador;
      tablaSimbolos.Agrega(this);

      if (parametros != null)
      {
        parametros.ValidaTipos();
      }//fin de if

      if (bloqueFunc != null)
      {
        bloqueFunc.ValidaTipos();
      }//fin de if

    }//fin del metodo ValidaTipos

    public Parametros GetParametros()
    {
      return parametros as Parametros;
    }

  } //fin de la clase DefinicionFuncion

  public class NodoVariableSimple : Nodo
  {
    public NodoVariableSimple(Stack<ElementoPila> pila)
      : base()
    {
      pila.Pop();
      simbolo = pila.Pop().Elemento;
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine(simbolo);
    } //fin del metodo Muestra
  } //fin de la clase NodoVariableSimple

  //R36 <Termino> ::= identificador
  public class Identificador : NodoVariableSimple
  {
    public Identificador(Stack<ElementoPila> pila)
      : base(pila)
    {
    }//fin del constructor

    public override void Muestra()
    {
      Console.Write("<Identificador> ");
      base.Muestra();
    }//fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en Identificador");


    }//fin del metodo ValidaTipos

  } //fin de la clase Identificador

  //R37 <Termino> ::= entero
  public class Entero : NodoVariableSimple
  {
    public Entero(Stack<ElementoPila> pila)
      : base(pila)
    {
    }

    public override void Muestra()
    {
      Console.Write("<Entero> ");
      base.Muestra();
    }//fin del metodo Muestra

    public override void ValidaTipos()
    {
      TipoDato = 'i';
      Console.WriteLine("Valida en Entero");
    }//fin del metodo ValidaTipos

  } //fin de la clase Entero

  public class Real : NodoVariableSimple
  {
    public Real(Stack<ElementoPila> pila)
      : base(pila)
    {
    }

    public override void Muestra()
    {
      Console.Write("<Real> ");
      base.Muestra();
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      TipoDato = 'f';
      Console.WriteLine("Valida en Real");
    }//fin del metodo ValidaTipos

  } //fin de la clase Real

  public class Cadena : NodoVariableSimple
  {
    //constructor
    public Cadena(Stack<ElementoPila> pila)
      : base(pila)
    {
    } //fin del constructor

    public override void Muestra()
    {
      Console.Write("<Cadena> ");
      base.Muestra();
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      TipoDato = 's';
      Console.WriteLine("Valida en Cadena");
    }
  } //fin de la clase Cadena


  public class Variables2 : Nodo
  {
    public Variables2(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<ListaVar>";

      pila.Pop();
      List<Nodo> listaVariables = pila.Pop().Nodo.Hijos;
      pila.Pop();
      String identificador = pila.Pop().Elemento;
      pila.Pop();
      pila.Pop();

      Nodo variable = new Nodo("<DefVar>");
      variable.AñadirHijo(new Nodo(identificador));

      AñadirHijo(variable);

      for (int i = listaVariables.Count - 1; i >= 0; i--)
        AñadirHijo(listaVariables[i]);
    }
  }

  public class ExpresionOperadoresBinarios : Nodo
  {
    //atributos
    private Nodo expresionDerecha;
    private String operador;
    private Nodo expresionIzquierda;

    public ExpresionOperadoresBinarios(Stack<ElementoPila> pila)
    {
      simbolo = "<Expresion>";

      pila.Pop();
      expresionDerecha = pila.Pop().Nodo;
      pila.Pop();
      operador = pila.Pop().Elemento;
      pila.Pop();
      expresionIzquierda = pila.Pop().Nodo;

      Nodo expresion = new Nodo(operador);
      expresion.AñadirHijo(expresionIzquierda);
      expresion.AñadirHijo(expresionDerecha);

      AñadirHijo(expresion);
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine("<ExpresionIzquierda>");
      if (expresionIzquierda != null)
      {
        sangria++;
        MuestraSangria();
        expresionIzquierda.Muestra();
      } //fin de if
      Console.WriteLine("<Operador> " + operador);
      Console.WriteLine("<ExpresionDerecha>");
      if (expresionDerecha != null)
      {
        sangria++;
        MuestraSangria();
        expresionDerecha.Muestra();
      }
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en ExpresionesOperadoresBinarios");

      if (expresionIzquierda != null)
      {
        expresionIzquierda.ValidaTipos();
      }

      if (expresionDerecha != null)
      {
        expresionDerecha.ValidaTipos();
      }


    }//fin del metodo Valida
  } //fin de la clase ExpresionOperadoresBinarios


  public class OperadorAdicion : ExpresionOperadoresBinarios
  {
    //constructor
    public OperadorAdicion(Stack<ElementoPila> pila)
      : base(pila)
    {
    } //fin del constructor

    public override void Muestra()
    {
      base.Muestra();
      Nodo simboloIzquierda = hijos[0].Hijos[0].Hijos[0];
      Nodo simboloDerecha = hijos[0].Hijos[1].Hijos[0];
      sangria++;
      MuestraSangria();
      simboloIzquierda.Muestra();
      sangria++;
      MuestraSangria();
      simboloDerecha.Muestra();
    } //fin del metodo Muestra
  } //fin de la clase OperadorAdicion


  public class OperadorMultiplicacion : ExpresionOperadoresBinarios
  {
    public OperadorMultiplicacion(Stack<ElementoPila> pila)
      : base(pila)
    {
    }

    public override void Muestra()
    {
      base.Muestra();
      Nodo simboloIzquierda = hijos[0].Hijos[0].Hijos[0];
      Nodo simboloDerecha = hijos[0].Hijos[1].Hijos[0];
      sangria++;
      MuestraSangria();
      simboloIzquierda.Muestra();
      sangria++;
      MuestraSangria();
      simboloDerecha.Muestra();
    } //fin del metodo Muestra
  } //fin de la clase OperadorMultiplicacion

  public class OperadorRelacional : ExpresionOperadoresBinarios
  {
    public OperadorRelacional(Stack<ElementoPila> pila)
      : base(pila)
    {
    }

    public override void Muestra()
    {
      base.Muestra();
      Nodo simboloIzquierda = hijos[0].Hijos[0].Hijos[0];
      Nodo simboloDerecha = hijos[0].Hijos[1].Hijos[0];
      sangria++;
      MuestraSangria();
      simboloIzquierda.Muestra();
      sangria++;
      MuestraSangria();
      simboloDerecha.Muestra();
    } //fin del metodo Muestra
  } //fin de la clase OperadorRelacional

  public class OperadorOr : ExpresionOperadoresBinarios
  {
    public OperadorOr(Stack<ElementoPila> pila)
      : base(pila)
    {
    }

    public override void Muestra()
    {
      base.Muestra();
      Nodo simboloIzquierda = hijos[0].Hijos[0].Hijos[0];
      Nodo simboloDerecha = hijos[0].Hijos[1].Hijos[0];
      sangria++;
      MuestraSangria();
      simboloIzquierda.Muestra();
      sangria++;
      MuestraSangria();
      simboloDerecha.Muestra();
    } //fin del metodo Muestra
  } //fin de la clase OperadorOR

  public class OperadorAnd : ExpresionOperadoresBinarios
  {
    public OperadorAnd(Stack<ElementoPila> pila)
      : base(pila)
    {
    }

    public override void Muestra()
    {
      base.Muestra();
      Nodo simboloIzquierda = hijos[0].Hijos[0].Hijos[0];
      Nodo simboloDerecha = hijos[0].Hijos[1].Hijos[0];
      sangria++;
      MuestraSangria();
      simboloIzquierda.Muestra();
      sangria++;
      MuestraSangria();
      simboloDerecha.Muestra();
    } //fin del metodo Muestra
  } //fin de la clase OperadorAnd

  public class OperadorIgualdad : ExpresionOperadoresBinarios
  {
    public OperadorIgualdad(Stack<ElementoPila> pila)
      : base(pila)
    {
    }

    public override void Muestra()
    {
      base.Muestra();
      Nodo simboloIzquierda = hijos[0].Hijos[0].Hijos[0];
      Nodo simboloDerecha = hijos[0].Hijos[1].Hijos[0];
      sangria++;
      MuestraSangria();
      simboloIzquierda.Muestra();
      sangria++;
      MuestraSangria();
      simboloDerecha.Muestra();
    } //fin del metodo Muestra
  } //fin de la clase OperadorIgualdad

  public class OperadorAdicion2 : Nodo
  {
    public OperadorAdicion2(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<Expresion>";

      pila.Pop();
      Nodo expresion = pila.Pop().Nodo;
      pila.Pop();
      String operador = pila.Pop().Elemento;

      AñadirHijo(new Nodo(operador));
      AñadirHijo(expresion);
    }

    public override void Muestra()
    {
      Console.WriteLine("MuestraOPAdicion2");
    }
  }

  public class OperadorNot : Nodo
  {
    public OperadorNot(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<Expresion>";

      pila.Pop();
      Nodo expresion = pila.Pop().Nodo;
      pila.Pop();
      pila.Pop();

      AñadirHijo(new Nodo("!"));
      AñadirHijo(expresion);
    }
  }

  public class ExpresionEntreParentesis : Nodo
  {
    public ExpresionEntreParentesis(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<Expresion>";

      pila.Pop();
      pila.Pop();
      pila.Pop();
      Nodo expresion = pila.Pop().Nodo;
      pila.Pop();
      pila.Pop();

      AñadirHijo(new Nodo("("));
      AñadirHijo(expresion);
      AñadirHijo(new Nodo(")"));
    }

    public override void Muestra()
    {
      Console.WriteLine("Muestra en expresion entre parentesis");
    }
  }

  public class Sentencias : Nodo
  {
    //atributos
    Nodo sentencias;
    Nodo sentencia;

    public Sentencias(Stack<ElementoPila> pila)
    {
      simbolo = "<Sentencias>";

      pila.Pop();
      sentencias = pila.Pop().Nodo;

      pila.Pop();
      sentencia = pila.Pop().Nodo;

      AñadirHijo(sentencia);
      foreach (Nodo hijo in sentencias.Hijos)
        AñadirHijo(hijo);
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine(simbolo);
      //Console.WriteLine ("RAQUELUNO: " + sentencias);
      if (sentencias != null)
      {
        sentencias.Muestra();
      }

      //Console.WriteLine ("RAQUELDOS: " + sentencia);
      if (sentencia != null)
      {
        sentencia.Muestra();
      } //fin de if
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en Sentencias");

      if (sentencias != null)
      {
        sentencias.ValidaTipos();
      }

      if (sentencia != null)
      {
        sentencia.ValidaTipos();
      }
    }
  } //fin de la clase Sentencias

  public class SentenciaAsignacion : Nodo
  {
    //atributos
    Nodo expresion;
    String identificador;
    String signoIgual;
    String puntoComa;

    public SentenciaAsignacion(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<Sentencia>";

      pila.Pop();
      puntoComa = pila.Pop().Elemento;

      pila.Pop();
      expresion = pila.Pop().Nodo;

      pila.Pop();
      signoIgual = pila.Pop().Elemento;

      pila.Pop();
      identificador = pila.Pop().Elemento;
    } //fin del constructor

    public override void Muestra()
    {
      sangria++;
      MuestraSangria();
      Console.WriteLine(simbolo);
      sangria++;
      MuestraSangria();
      Console.WriteLine("<Identificador>" + identificador);
      sangria++;
      MuestraSangria();
      Console.WriteLine("<Asignacion> " + signoIgual);
      sangria++;
      MuestraSangria();

      Console.WriteLine("<Expresion>");
      if (expresion != null)
      {
        sangria++;
        MuestraSangria();
        expresion.Muestra();
        //Console.WriteLine(expresion);
      } //fin de if

      sangria++;
      MuestraSangria();
      Console.WriteLine("<PuntoComa> " + puntoComa);
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en SentenciaAsignacion");

      if (expresion != null)
      {
        expresion.ValidaTipos();
      }
    }//fin del metodo ValidaTipos
  } //fin de la clase SentenciaAsignacion

  public class SentenciaLlamadaFuncion : Nodo
  {
    private String puntoComa;
    private Nodo llamadaFuncion;

    //constructor
    public SentenciaLlamadaFuncion(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<Sentencia>";

      pila.Pop();
      puntoComa = pila.Pop().Elemento;
      pila.Pop();
      llamadaFuncion = pila.Pop().Nodo;
      AñadirHijo(llamadaFuncion);
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine("<LlamadaFuncion>");
      if (llamadaFuncion != null)
      {
        llamadaFuncion.Muestra();
      } //fin de if

      Console.WriteLine("<PuntoComa> " + puntoComa);
    } //fin del metodo Muestra
  } //fin de la clase SentenciaLlamadaFuncion

  public class SentenciaValorRegresa : Nodo
  {
    //atributos
    private String retorno;
    private String puntoComa;
    //public Nodo valorRegresa;

    public SentenciaValorRegresa(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<Sentencia>";

      pila.Pop();
      puntoComa = pila.Pop().Elemento;

      pila.Pop();
      nodoSiguiente = (pila.Pop().Nodo);

      pila.Pop();
      retorno = pila.Pop().Elemento;

      AñadirHijo(new Nodo("return"));
      AñadirHijo(nodoSiguiente);
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine("<Regresa> " + retorno);
      //Console.WriteLine ("MAGIA: " + valorRegresa.Hijos[0].Hijos[0].Hijos[0].Hijos[0].simbolo);
      Console.WriteLine(nodoSiguiente);
      if (nodoSiguiente != null)
      {
        //sig.Muestra();
        //Console.WriteLine ("vas");
        nodoSiguiente.Hijos[0].Hijos[0].Muestra();
      }
      Console.WriteLine("<PuntoComa> " + puntoComa);
    } //fin del metodo Muestra
  } //fin de la clase SentenciaValorRegresa

  public class SentenciaIf : Nodo
  {
    //atributos
    private string condicionalIf;
    private string parentesisInicio;
    private string parentesisFin;
    Nodo otros;
    Nodo bloque;
    Nodo expresion;

    //constructor
    public SentenciaIf(Stack<ElementoPila> pila)
    {
      simbolo = "<Sentencia>";

      pila.Pop();
      otros = pila.Pop().Nodo;

      pila.Pop();
      bloque = pila.Pop().Nodo;

      pila.Pop();
      parentesisFin = pila.Pop().Elemento;

      pila.Pop();
      expresion = pila.Pop().Nodo;

      pila.Pop();
      parentesisInicio = pila.Pop().Elemento;

      pila.Pop();
      condicionalIf = pila.Pop().Elemento;

      AñadirHijo(new Nodo("if"));
      AñadirHijo(new Nodo("("));
      AñadirHijo(expresion);
      AñadirHijo(new Nodo(")"));
      AñadirHijo(bloque);
      AñadirHijo(otros);
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine("<Condicional> " + condicionalIf);
      Console.WriteLine("<ParentesisInicio>" + parentesisInicio);
      if (expresion != null)
      {
        expresion.Muestra();
      }

      Console.WriteLine("<ParentesisFin> " + parentesisFin);

      if (bloque != null)
      {
        bloque.Muestra();
      } //fin de if

      if (otros != null)
      {
        otros.Muestra();
      } //fin de if
    } //fin del metodo muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en SentenciasIf");
    }//fin del metodo ValidaTipos

  } //fin de la clase SentenciaIf

  public class SentenciaWhile : Nodo
  {
    private String repetitivaWhile;
    private String parentesisInicio;
    private Nodo expresion;
    private String parentesisFin;
    private Nodo bloque;

    public SentenciaWhile(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<Sentencia>";

      pila.Pop();
      bloque = pila.Pop().Nodo;

      pila.Pop();
      parentesisFin = pila.Pop().Elemento;

      pila.Pop();
      expresion = pila.Pop().Nodo;

      pila.Pop();
      parentesisInicio = pila.Pop().Elemento;

      pila.Pop();
      repetitivaWhile = pila.Pop().Elemento;
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine("<RepetitivaWhile> " + repetitivaWhile);
      Console.WriteLine("<ParentesisInicio> " + parentesisInicio);
      if (expresion != null)
      {
        expresion.Muestra();
      }
      Console.WriteLine("<ParentesisFin> " + parentesisFin);
      if (bloque != null)
      {
        bloque.Muestra();
      }
    } //fin del metodo Muestra
  } //fin de la clase SentenciaWhile

  public class Bloque : Nodo
  {
    //atributos
    private String llaveInicio;
    Nodo sentencias;
    private String llaveFin;

    public Bloque(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<Bloque>";

      pila.Pop();
      llaveFin = pila.Pop().Elemento;

      pila.Pop();
      sentencias = pila.Pop().Nodo;

      pila.Pop();
      llaveInicio = pila.Pop().Elemento;

      AñadirHijo(new Nodo("{"));
      AñadirHijo(sentencias);
      AñadirHijo(new Nodo("}"));
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine(simbolo);
      Console.WriteLine("<LlaveInicio> " + llaveInicio);

      if (sentencias != null)
      {
        sentencias.Muestra();
      }

      Console.WriteLine("<LlaveFin>" + llaveFin);
    } //fin del metodo Muestra
  } //fin de la clase Bloque

  public class Otro : Nodo
  {
    //atributos
    private string condicionalElse;
    Nodo bloque;

    public Otro(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<Otro>";

      pila.Pop();
      bloque = pila.Pop().Nodo;

      pila.Pop();
      condicionalElse = pila.Pop().Elemento;

      AñadirHijo(new Nodo("else"));
      AñadirHijo(bloque);
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine(simbolo);

      Console.WriteLine("<CondionalElse> " + condicionalElse);

      if (bloque != null)
      {
        bloque.Muestra();
      } //fin de if
    } //fin del metodo Muestra
  } //fin de la clase Otro


  public class Parametros : Nodo
  {
    //atributos
    private string tipo;
    private string identificador;
    private List<Nodo> listaParametros;

    public Parametros(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<ListaParam>";

      pila.Pop();
      listaParametros = pila.Pop().Nodo.Hijos;
      pila.Pop();
      identificador = pila.Pop().Elemento;
      pila.Pop();
      tipo = pila.Pop().Elemento;
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine("<ListaParametros>");
      sangria++;
      MuestraSangria();
      Console.WriteLine("<Tipo> " + tipo);
      sangria++;
      MuestraSangria();
      Console.WriteLine("<Identificador> " + identificador);

      foreach (Nodo parametro in listaParametros)
      {
        MuestraSangria();
        Console.WriteLine("<Tipo> " + parametro.Hijos[0].simbolo);
        MuestraSangria();
        Console.WriteLine("<Identificador> " + parametro.Hijos[1].simbolo);
      } //fin de foreach
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en Parametros");
    }
  }

  public class Parametros2 : Nodo
  {
    public Parametros2(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<ListaParam>";

      pila.Pop();
      List<Nodo> listaVariables = pila.Pop().Nodo.Hijos;
      pila.Pop();
      String identificador = pila.Pop().Elemento;
      pila.Pop();
      String tipo = pila.Pop().Elemento;
      pila.Pop();
      pila.Pop();

      Nodo parametro = new Nodo("<Parametro>");
      parametro.AñadirHijo(new Nodo(tipo));
      parametro.AñadirHijo(new Nodo(identificador));

      AñadirHijo(parametro);

      for (int i = listaVariables.Count - 1; i >= 0; i--)
        AñadirHijo(listaVariables[i]);
    }
  }

  public class BloqueFunc : Nodo
  {
    private string llaveInicio;
    Nodo defLocales;
    private string llaveFin;

    public BloqueFunc(Stack<ElementoPila> pila)
    {
      simbolo = "<BloqueFunc>";

      pila.Pop();
      llaveFin = pila.Pop().Elemento;

      pila.Pop();
      defLocales = pila.Pop().Nodo;

      pila.Pop();
      llaveInicio = pila.Pop().Elemento;

      AñadirHijo(new Nodo("{"));
      AñadirHijo(defLocales);
      AñadirHijo(new Nodo("}"));
    }

    public override void Muestra()
    {
      sangria++;
      MuestraSangria();
      Console.WriteLine("<LlaveInicio> " + llaveInicio);

      if (defLocales != null)
      {
        sangria++;
        MuestraSangria();
        defLocales.Muestra();
      } //fin de if

      sangria--;
      MuestraSangria();
      Console.WriteLine("<LlaveFin> " + llaveFin);
    }//fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en BloqueFunc");

      if (defLocales != null)
      {
        defLocales.ValidaTipos();
      }
    }//fin del metodo ValidaTipos
  } //fin de la clase BloqueFuncion

  public class LlamadaFuncion : Nodo
  {
    private String identificador;
    private String parentesisInicio;
    Nodo argumentos;
    private String parentesisFin;


    public LlamadaFuncion(Stack<ElementoPila> pila)
    {
      simbolo = "<LlamadaFunc>";

      pila.Pop();
      parentesisFin = pila.Pop().Elemento;

      pila.Pop();
      argumentos = pila.Pop().Nodo;

      pila.Pop();
      parentesisInicio = pila.Pop().Elemento;

      pila.Pop();
      identificador = pila.Pop().Elemento;

      AñadirHijo(new Nodo(identificador));
      AñadirHijo(new Nodo("("));
      AñadirHijo(argumentos);
      AñadirHijo(new Nodo(")"));
    }//fin del constructor

    public override void Muestra()
    {
      Console.WriteLine("<Identificador> " + identificador);
      Console.WriteLine("<ParentesisInicio> " + parentesisInicio);
      if (argumentos != null)
      {
        argumentos.Muestra();
      }

      Console.WriteLine("<ParentesisFin> " + parentesisFin);
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en LlamadaFuncion");

      if (argumentos != null)
      {
        argumentos.ValidaTipos();
      }//fin de if
    }//fin del metodo ValidaTipos

  } //fin de la clase LlamadaFuncion

  public class ListaArgumentos : Nodo
  {
    //atributos
    private Nodo listaArgumentos;
    private Nodo expresion;

    public ListaArgumentos(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<ListaArgumentos>";

      pila.Pop();
      listaArgumentos = pila.Pop().Nodo;

      pila.Pop();
      expresion = pila.Pop().Nodo;

      AñadirHijo(expresion);
      foreach (Nodo nodo in listaArgumentos.Hijos)
        AñadirHijo(nodo);
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine(simbolo);
      //Console.WriteLine(hijos.Count); //cantidad de parametros

      hijos[0].Hijos[0].Muestra();

      if (listaArgumentos != null)
      {
        listaArgumentos.Muestra();
      }

      if (expresion != null)
      {
        expresion.Muestra();
      }
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en ListaArgumentos");

      if (listaArgumentos != null)
      {
        listaArgumentos.ValidaTipos();
      }

      if (expresion != null)
      {
        expresion.ValidaTipos();
      }
    }//fin del metodo ValidaTipos

  } //fin de la clase ListaArgumentos

  public class ListaArgumentos2 : Nodo
  {
    Nodo listaArgumentos;
    Nodo expresion;
    String coma;

    public ListaArgumentos2(Stack<ElementoPila> pila)
    {
      simbolo = "<ListaArgumentos>";

      pila.Pop();
      listaArgumentos = pila.Pop().Nodo;

      pila.Pop();
      expresion = pila.Pop().Nodo;

      pila.Pop();
      coma = pila.Pop().Elemento;

      AñadirHijo(expresion);
      foreach (Nodo nodo in listaArgumentos.Hijos)
        AñadirHijo(nodo);
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine("<Coma> " + coma);
      expresion.Hijos[0].Muestra();

      if (expresion != null)
      {
        expresion.Muestra();
      }

      if (listaArgumentos != null)
      {
        listaArgumentos.Muestra();
      }
    } //fin del metodo Muestra
  } //fin de la clase


  public class DefinicionesLocales : Nodo
  {
    //atributos
    Nodo defLocales;
    Nodo defLocal;

    public DefinicionesLocales(Stack<ElementoPila> pila)
      : base()
    {
      simbolo = "<DefLocales>";

      pila.Pop();
      defLocales = pila.Pop().Nodo;

      pila.Pop();
      defLocal = pila.Pop().Nodo;

      Console.WriteLine("MAGIA: " + defLocal); //<Sentencia>
    } //fin del constructor

    public override void Muestra()
    {
      sangria++;
      MuestraSangria();
      Console.WriteLine(simbolo);

      if (defLocal != null)
      {
        sangria++;
        MuestraSangria();
        defLocal.Muestra();
      } //fin de if

      if (defLocales != null)
      {
        sangria++;
        MuestraSangria();
        defLocales.Muestra();
      } //fin de if
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en DefinicionesLocales");

      if (defLocal != null)
      {
        defLocal.ValidaTipos();
      }//fin de if

      if (defLocales != null)
      {
        defLocales.ValidaTipos();
      }//fin de if
    }//fin del metodo ValidaTipos

  } //fin de la clase DefinicionesLocales
}

