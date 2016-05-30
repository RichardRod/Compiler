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
    } //fin del metodo ValidaTipos

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera codigo Programa");

      GeneracionCodigo.AgregaArchivo("section .text");
      GeneracionCodigo.AgregaArchivo("\t\tglobal main");
      GeneracionCodigo.AgregaArchivo("\t\textern printf");
      GeneracionCodigo.AgregaArchivo("main:");


      if (definiciones != null)
      {
        definiciones.GeneraCodigoEnsamblador();
      }
    }
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
      } //fin de if

      if (definiciones != null)
      {
        definiciones.ValidaTipos();
      } //fin de if
    } //fin del metodo ValidaTipos

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera codigo Definiciones");

      if (definicion != null)
      {
        definicion.GeneraCodigoEnsamblador();
      }

      if (definiciones != null)
      {
        definiciones.GeneraCodigoEnsamblador();
      }
    }
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

      if (variable != null)
        variable.ValidaTipos();

      for (int i = 0; i < listaVariables.Count; i++)
      {
        DefVar definicionVariable = new DefVar(tipo, listaVariables[i].Hijos[0].simbolo);

        if (definicionVariable != null)
          definicionVariable.ValidaTipos();
      } //fin de for
    } //fin del metodo Valida Tipos

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera codigo DefVar");
    }
  } //fin de la clase Variables

  public class DefVar : Nodo
  {
    //atributos
    private string identificador;
    private string ambito;

    //constructor
    public DefVar(string tipo, string identificador)
    {
      TipoDato = Tipo.DameTipo(tipo);
      this.identificador = identificador;
      ambito = tipoAmbito;
    } //fin del constructor

    public string Identificador
    {
      get { return identificador; }
    }

    public string Ambito
    {
      get { return ambito; }
    }

    public override void ValidaTipos()
    {
      Console.WriteLine("Agregando Variable: " + identificador + " Tipo: " + TipoDato + " Ambito: " + tipoAmbito);
      tablaSimbolos.Agrega(this);
    } //fin del metodo VaidaTipos
  } //fin de la clase DefVar

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

      funcionActual = identificador;

      tipoAmbito = "Local";
      TipoDato = Tipo.DameTipo(tipo);
      NombreFuncion = identificador;
      tablaSimbolos.Agrega(this);

      if (parametros != null)
      {
        parametros.ValidaTipos();
      } //fin de if

      if (bloqueFunc != null)
      {
        bloqueFunc.ValidaTipos();
      } //fin de if
    } //fin del metodo ValidaTipos

    public Parametros GetParametros()
    {
      return parametros as Parametros;
    }

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera codigo DefFunc");

      //parametros

      if (bloqueFunc != null)
      {
        bloqueFunc.GeneraCodigoEnsamblador();
      }
    } //fin del metodo GeneraCodigoEnsamblador
  } //fin de la clase DefinicionFuncion

  public class NodoVariableSimple : Nodo
  {
    public NodoVariableSimple(Stack<ElementoPila> pila)
    {
      pila.Pop();
      simbolo = pila.Pop().Elemento;
    } //fin del constructor

    public override void Muestra()
    {
      Console.WriteLine(simbolo);
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en Nodo Variable simple");
      Console.WriteLine("Obteniendo Tipo: " + simbolo);

      Variable miVariable = tablaSimbolos.ObtenerVariable(simbolo);
      char tipoVariable = ' ';

      if (miVariable != null)
      {
        Console.WriteLine("Variable existente: ");
        Console.WriteLine("Tipo: " + miVariable.Tipo);
        tipoVariable = miVariable.Tipo;

        listaExpresion.Add(miVariable);
      }
      else
      {
        tablaSimbolos.AgregarError("Variable indefinida: \"" + simbolo + "\"");
      }
    }

    public override char ObtenerTipo()
    {
      //Console.WriteLine("Obteniendo Tipo: " + simbolo);

      Variable miVariable = tablaSimbolos.ObtenerVariable(simbolo);
      char tipoVariable = ' ';

      if (miVariable != null)
      {
        Console.WriteLine("Variable existente: ");
        Console.WriteLine("Tipo: " + miVariable.Tipo);
        tipoVariable = miVariable.Tipo;
      }
      else
      {
        tablaSimbolos.AgregarError("Variable indefinida: \"" + simbolo + "\"");
      }

      return tipoVariable;
    }
  } //fin de la clase NodoVariableSimple

  //R36 <Termino> ::= identificador
  public class Identificador : NodoVariableSimple
  {
    public Identificador(Stack<ElementoPila> pila)
      : base(pila)
    {
    } //fin del constructor

    public override void Muestra()
    {
      Console.Write("<Identificador> ");
      base.Muestra();
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en Identificador");
      base.ValidaTipos();
      //Console.WriteLine("VALIDA que este declarado: " + );
    } //fin del metodo ValidaTipos

    public override char ObtenerTipo()
    {
      Console.WriteLine("Obteniendo en: Identificador");
      return base.ObtenerTipo();
    } //fin del metodo ObtenerTipo
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
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      TipoDato = 'i';
      Console.WriteLine("Valida en Entero");
    } //fin del metodo ValidaTipos
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
    } //fin del metodo ValidaTipos
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
        expresionIzquierda.ObtenerTipo();
      }

      if (expresionDerecha != null)
      {
        expresionDerecha.ValidaTipos();
        expresionDerecha.ObtenerTipo();
      }

      //valida expresion
      List<Variable> listaAuxiliar = new List<Variable>(listaExpresion);
      foreach (Variable variable in listaExpresion)
      {
        Variable aux = variable;
        Console.WriteLine(variable.Simbolo);
      } //fin de foreach

      Console.WriteLine("_____________________");

      listaAuxiliar.Reverse();
      for (int i = 0; i < listaAuxiliar.Count; i++)
      {
        if (listaAuxiliar[i].Tipo != listaExpresion[i].Tipo)
        {
          tablaSimbolos.AgregarError("No puede formarse la expresion Tipo Requerido: \"" + listaAuxiliar[i].Tipo + "\"");
          break;
        }
        //Console.WriteLine(listaAuxiliar[i].Simbolo + " - " + listaExpresion[i].Simbolo);
      }
    } //fin del metodo Valida

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera codigo ExpresionOperadoresBinarios");

      switch (operador)
      {
        case "+":
          GeneracionCodigo.AgregaArchivo("\t\tadd eax, ebx");
          GeneracionCodigo.AgregaArchivo("\t\tpush eax");
          GeneracionCodigo.AgregaArchivo("\t\tpush message");
          GeneracionCodigo.AgregaArchivo("\t\tcall printf");
          GeneracionCodigo.AgregaArchivo("\t\tadd esp, 8");
          GeneracionCodigo.AgregaArchivo("\t\tret");
          GeneracionCodigo.AgregaArchivo("");
          GeneracionCodigo.AgregaArchivo("message db \"Resultado: %d\", 10, 0");
          break;

        case "-":
          GeneracionCodigo.AgregaArchivo("\t\tsub eax, ebx");
          GeneracionCodigo.AgregaArchivo("\t\tpush eax");
          GeneracionCodigo.AgregaArchivo("\t\tpush message");
          GeneracionCodigo.AgregaArchivo("\t\tcall printf");
          GeneracionCodigo.AgregaArchivo("\t\tadd esp, 8");
          GeneracionCodigo.AgregaArchivo("\t\tret");
          GeneracionCodigo.AgregaArchivo("");
          GeneracionCodigo.AgregaArchivo("message db \"Resultado: %d\", 10, 0");
          break;

        case "*":
          GeneracionCodigo.AgregaArchivo("\t\tmul eax");
          GeneracionCodigo.AgregaArchivo("\t\tpush eax");
          GeneracionCodigo.AgregaArchivo("\t\tpush message");
          GeneracionCodigo.AgregaArchivo("\t\tcall printf");
          GeneracionCodigo.AgregaArchivo("\t\tadd esp, 8");
          GeneracionCodigo.AgregaArchivo("\t\tret");
          GeneracionCodigo.AgregaArchivo("");
          GeneracionCodigo.AgregaArchivo("message db \"Resultado: %d\", 10, 0");
          break;

          case "/":
          GeneracionCodigo.AgregaArchivo("\t\txor edx, edx");
          GeneracionCodigo.AgregaArchivo("\t\tdiv ebx");
          GeneracionCodigo.AgregaArchivo("\t\tpush edx");
          GeneracionCodigo.AgregaArchivo("\t\tpush eax");
          GeneracionCodigo.AgregaArchivo("\t\tpush message");
          GeneracionCodigo.AgregaArchivo("\t\tcall printf");
          GeneracionCodigo.AgregaArchivo("\t\tadd esp, 12");
          GeneracionCodigo.AgregaArchivo("\t\tret");
          GeneracionCodigo.AgregaArchivo("");
          GeneracionCodigo.AgregaArchivo("message db \"Resultado: %d\", 10, 0");

          break;
      } //fin de switch
    } //fin del metodo GeneracionCodigo
  } //fin de la clase ExpresionOperadoresBinarios


  public class OperadorAdicion : ExpresionOperadoresBinarios
  {
    //atributos
    private Nodo simboloIzquierda;
    private Nodo simboloDerecha;

    //constructor
    public OperadorAdicion(Stack<ElementoPila> pila)
      : base(pila)
    {
    } //fin del constructor

    public override void Muestra()
    {
      base.Muestra();

      simboloIzquierda = hijos[0].Hijos[0].Hijos[0];
      simboloDerecha = hijos[0].Hijos[1].Hijos[0];
      sangria++;

      MuestraSangria();
      simboloIzquierda.Muestra();
      sangria++;

      MuestraSangria();
      simboloDerecha.Muestra();
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en Operador Adicion: " + simboloIzquierda + " " + simboloDerecha);

      base.ValidaTipos();

      if (simboloIzquierda != null)
      {
        Console.WriteLine("Entre izquierda");
        simboloIzquierda.ValidaTipos();
      }

      if (simboloDerecha != null)
      {
        Console.WriteLine("Entre derecha");
        simboloDerecha.ValidaTipos();
      }
    } //fin del metodo ValidaTipos

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera codigo OperadorAdicion");

      base.GeneraCodigoEnsamblador();

      if (simboloIzquierda != null)
      {
        simboloIzquierda.GeneraCodigoEnsamblador();
      }

      if (simboloDerecha != null)
      {
        simboloDerecha.GeneraCodigoEnsamblador();
      }
    }
  } //fin de la clase OperadorAdicion


  public class OperadorMultiplicacion : ExpresionOperadoresBinarios
  {
    //atributos
    private Nodo simboloIzquierda;
    private Nodo simboloDerecha;

    public OperadorMultiplicacion(Stack<ElementoPila> pila)
      : base(pila)
    {
    }

    public override void Muestra()
    {
      base.Muestra();
      simboloIzquierda = hijos[0].Hijos[0].Hijos[0];
      simboloDerecha = hijos[0].Hijos[1].Hijos[0];
      sangria++;
      MuestraSangria();
      simboloIzquierda.Muestra();
      sangria++;
      MuestraSangria();
      simboloDerecha.Muestra();
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      base.ValidaTipos();

      if (simboloIzquierda != null)
      {
        simboloIzquierda.ValidaTipos();
      }

      if (simboloDerecha != null)
      {
        simboloDerecha.ValidaTipos();
      }
    } //fin del metodo ValidaTipos
  } //fin de la clase OperadorMultiplicacion

  public class OperadorRelacional : ExpresionOperadoresBinarios
  {
    private Nodo simboloIzquierda;
    private Nodo simboloDerecha;

    public OperadorRelacional(Stack<ElementoPila> pila)
      : base(pila)
    {
    }

    public override void Muestra()
    {
      base.Muestra();
      simboloIzquierda = hijos[0].Hijos[0].Hijos[0];
      simboloDerecha = hijos[0].Hijos[1].Hijos[0];
      sangria++;
      MuestraSangria();
      simboloIzquierda.Muestra();
      sangria++;
      MuestraSangria();
      simboloDerecha.Muestra();
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      base.ValidaTipos();

      if (simboloIzquierda != null)
      {
        simboloIzquierda.ValidaTipos();
      }

      if (simboloDerecha != null)
      {
        simboloDerecha.ValidaTipos();
      }
    }
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
      if (sentencias != null)
      {
        sentencias.Muestra();
      }

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

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera codigo Sentencias");

      if (sentencias != null)
      {
        sentencias.GeneraCodigoEnsamblador();
      }

      if (sentencia != null)
      {
        sentencia.GeneraCodigoEnsamblador();
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

    private string valor;

    public SentenciaAsignacion(Stack<ElementoPila> pila)
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
      Console.WriteLine("<Identificador> " + identificador);

      sangria++;
      MuestraSangria();
      Console.WriteLine("<Asignacion> " + signoIgual);

      sangria++;
      MuestraSangria();

      Console.WriteLine("<Expresion>");

      if (expresion.Hijos[0] != null)
      {
        valor = expresion.Hijos[0].simbolo;
        expresion.Hijos[0].Muestra();
      }


      if (expresion != null)
      {
        sangria++;
        MuestraSangria();
        expresion.Muestra();
      } //fin de if

      sangria++;
      MuestraSangria();
      Console.WriteLine("<PuntoComa> " + puntoComa);
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en SentenciaAsignacion");

      Variable miVariable = tablaSimbolos.ObtenerVariable(identificador);

      if (miVariable == null)
      {
        tablaSimbolos.AgregarError("Variable indefinida: \"" + identificador + "\"");
      } //fin de if

      if (expresion != null)
      {
        expresion.ValidaTipos();
      }
    } //fin del metodo ValidaTipos

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera codigo en sentenciaAsignacion");

      if (contadorRegistro == 1)
      {
        GeneracionCodigo.AgregaArchivo("\t\tmov eax, " + valor);
      }
      else if (contadorRegistro == 2)
      {
        GeneracionCodigo.AgregaArchivo("\t\tmov ebx, " + valor);
      }

      contadorRegistro++;

      if (expresion != null)
      {
        expresion.GeneraCodigoEnsamblador();
      }
    }
  } //fin de la clase SentenciaAsignacion

  public class SentenciaLlamadaFuncion : Nodo
  {
    private String puntoComa;
    private Nodo llamadaFuncion;

    //constructor
    public SentenciaLlamadaFuncion(Stack<ElementoPila> pila)
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
      Console.WriteLine("<SentenciaLlamadaFuncion>");
      if (llamadaFuncion != null)
      {
        llamadaFuncion.Muestra();
      } //fin de if

      Console.WriteLine("<PuntoComa> " + puntoComa);
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en SentenciaLlamadaFuncion: " + llamadaFuncion);

      //buscar si la funcion ya fue definida: caso contrario marcar error
      if (llamadaFuncion != null)
      {
        llamadaFuncion.ValidaTipos();
      }
    } //fin del metodo ValidaTipos
  } //fin de la clase SentenciaLlamadaFuncion

  public class SentenciaValorRegresa : Nodo
  {
    //atributos
    private String retorno;
    private String puntoComa;
    private Nodo elementoRegresa;
    //public Nodo valorRegresa;

    public SentenciaValorRegresa(Stack<ElementoPila> pila)
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
      //Console.WriteLine(nodoSiguiente);
      if (nodoSiguiente != null)
      {
        //sig.Muestra();
        //Console.WriteLine ("vas");
        elementoRegresa = nodoSiguiente.Hijos[0].Hijos[0];
        elementoRegresa.Muestra();
      }
      Console.WriteLine("<PuntoComa> " + puntoComa);
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en ValorRegresa");
      Console.WriteLine("FuncionActual: " + funcionActual);

      Funcion miFuncion = (Funcion) (tablaSimbolos.BuscaIdentificador(funcionActual));

      if (miFuncion != null)
      {
        Console.WriteLine("Datos Funcion: " + elementoRegresa.simbolo);

        Console.WriteLine("Identificador: " + miFuncion.Simbolo + " Tipo: " + miFuncion.Tipo);

        Variable miVariable = (tablaSimbolos.ObtenerVariable(elementoRegresa.simbolo));

        if (miVariable != null)
        {
          char valorRetorno = miVariable.Tipo;
          char tipoFuncion = miFuncion.Tipo;

          if (valorRetorno != tipoFuncion)
          {
            tablaSimbolos.AgregarError("Error: Tipo de Retorno Incompatible, se esperaba: " + tipoFuncion +
                                       " Recibido: " + valorRetorno + " Funcion: \"" + miFuncion.Simbolo + "\"");
          }
        }
      }

      if (nodoSiguiente != null)
      {
        nodoSiguiente.ValidaTipos();
      }
    } //fin del metodo ValidaTipos
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
    } //fin del metodo ValidaTipos
  } //fin de la clase SentenciaIf

  public class SentenciaWhile : Nodo
  {
    private String repetitivaWhile;
    private String parentesisInicio;
    private Nodo expresion;
    private String parentesisFin;
    private Nodo bloque;

    public SentenciaWhile(Stack<ElementoPila> pila)
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

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera Codigo Sentencia While");




    }
  } //fin de la clase SentenciaWhile

  public class Bloque : Nodo
  {
    //atributos
    private String llaveInicio;
    Nodo sentencias;
    private String llaveFin;

    public Bloque(Stack<ElementoPila> pila)
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

      DefVar variable = new DefVar(tipo, identificador);

      if (variable != null)
        variable.ValidaTipos();

      foreach (Nodo parametro in listaParametros)
      {
        string tipoParametro = parametro.Hijos[0].simbolo;
        string identificadorParametro = parametro.Hijos[1].simbolo;

        DefVar variableParametro = new DefVar(tipoParametro, identificadorParametro);
        if (variableParametro != null)
          variableParametro.ValidaTipos();
      } //fin de foreach
    } //fin del metodo ValidaTipos
  } //fin de la clase ValidaTipos

  public class Parametros2 : Nodo
  {
    public Parametros2(Stack<ElementoPila> pila)
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

    public override void Muestra()
    {
      Console.WriteLine("Muestra en parametros2");
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
    } //fin del metodo Muestra

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en BloqueFunc");

      if (defLocales != null)
      {
        defLocales.ValidaTipos();
      }
    } //fin del metodo ValidaTipos

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera Codigo BloqueFunc");

      if (defLocales != null)
      {
        defLocales.GeneraCodigoEnsamblador();
      }
    }
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
    } //fin del constructor

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
      Console.WriteLine("Valida en LlamadaFuncion: ");

      bool existeFuncion = tablaSimbolos.FuncionDefinida(identificador);

      if (!existeFuncion)
        tablaSimbolos.AgregarError("No existe la funcion: \"" + identificador + "\".");

      if (argumentos != null)
      {
        argumentos.ValidaTipos();
      } //fin de if

      Funcion mifuncion = tablaSimbolos.ObtenerFuncion(identificador);

      if (mifuncion != null)
      {
        Console.WriteLine("CUANTO: " + mifuncion.ParametrosCadena);
      }
    } //fin del metodo ValidaTipos
  } //fin de la clase LlamadaFuncion

  public class ListaArgumentos : Nodo
  {
    //atributos
    private Nodo listaArgumentos;
    private Nodo expresion;

    public ListaArgumentos(Stack<ElementoPila> pila)
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
      Console.WriteLine("Cantidad: " + hijos.Count); //cantidad de parametros

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
      Console.WriteLine("Valida en ListaArgumentos: " + listaArgumentos + " " + expresion);

      if (listaArgumentos != null)
      {
        listaArgumentos.ValidaTipos();
      }

      if (expresion != null)
      {
        expresion.ValidaTipos();
      }
    } //fin del metodo ValidaTipos
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

    public override void ValidaTipos()
    {
      Console.WriteLine("Valida en ListaDeArgumentos2: " + listaArgumentos + " : " + expresion);

      if (expresion != null)
      {
        expresion.ValidaTipos();
      }

      if (listaArgumentos != null)
      {
        listaArgumentos.ValidaTipos();
      }
    } //fin del metodo ValidaTipos
  } //fin de la clase


  public class DefinicionesLocales : Nodo
  {
    //atributos
    Nodo defLocales;
    Nodo defLocal;

    public DefinicionesLocales(Stack<ElementoPila> pila)
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

      tipoAmbito = "Local";

      if (defLocal != null)
      {
        defLocal.ValidaTipos();
      } //fin de if

      if (defLocales != null)
      {
        defLocales.ValidaTipos();
      } //fin de if

      tipoAmbito = "Global";
    } //fin del metodo ValidaTipos

    public override void GeneraCodigoEnsamblador()
    {
      Console.WriteLine("Genera codigo DefLocales");

      if (defLocal != null)
      {
        defLocal.GeneraCodigoEnsamblador();
      }

      if (defLocales != null)
      {
        defLocales.GeneraCodigoEnsamblador();
      }
    } //fin del metodo GeneraCodigoEnsamblador
  } //fin de la clase DefinicionesLocales
}