using System;
using System.Collections.Generic;
using System.IO;

namespace Compiler
{
  public class Sintactico
  {
    //atributos
    private Lexico lexico;
    private String entrada;
    private int fila, columna, accion;
    private Stack<ElementoPila> pila;
    private NoTerminal nt;
    private Nodo nodo;
    private Nodo arbol;
    private bool aceptacion = false;

    int[] idReglas = new int[52];
    int[] lonReglas = new int[52];
    String[] strReglas = new string[52];
    private int[,] tabla = new int[95, 46];

    private Semantico _semantico;

    public Sintactico(String entrada)
    {
      this.entrada = entrada;
      lexico = new Lexico(entrada);
      pila = new Stack<ElementoPila>();
      arbol = null;
      CargarArchivos();

      _semantico = new Semantico();
    } //fin del constructor

    public String Entrada
    {
      get { return entrada; }
      set
      {
        entrada = value;
        lexico = new Lexico(entrada);
        pila.Clear();
      }
    }

    public void AnalisisSintactico()
    {
      pila.Push(new Terminal(TipoSimbolo.PESOS, "$"));
      pila.Push(new Estado(0));
      lexico.SiguienteSimbolo();

      Console.WriteLine("Elementos en pila\t\tSimbolo\tAccion");

      while (true)
      {
        SiguienteAccion();
        MostrarPila();

        if (accion > 0)
        {
          pila.Push(new Terminal(lexico.Tipo, lexico.Simbolo));
          pila.Push(new Estado(accion));
          lexico.SiguienteSimbolo();
        } //fin de if
        else if (accion < 0)
        {
          if (accion == -1)
          {
            aceptacion = true;
            pila.Pop();
            arbol = pila.Pop().Nodo; //<Programa>
            Console.WriteLine("Aceptacion");
            Console.WriteLine("\n\n");

            if (aceptacion)
            {
              arbol.Muestra();
              Console.WriteLine("\n\n");
              _semantico.Analiza(arbol);
            }//fin de if

            break;
          } //fin de if

          int regla = -(accion + 2);

          switch (regla + 1)
          {
            case 1: //<programa> ::= <Definiciones>
              nodo = new Programa(pila);
              break;
            case 2: //<Definiciones> ::= \e
              nodo = new Nodo("<Definiciones>");
              break;
            case 3: //<Definiciones> ::= <Definicion> <Definiciones>
              nodo = new Definiciones(pila);
              break;
            case 4: //<Definicion> ::= <DefVar>
              pila.Pop();
              nodo = pila.Pop().Nodo;
              break;
            case 5: //<Definicion> ::= <DefFunc>
              pila.Pop();
              nodo = pila.Pop().Nodo;
              break;
            case 6: //<DefVar> ::= tipo identificador <ListaVar> ;
              nodo = new Variables(pila);
              break;
            case 7: //<ListaVar> ::= \e
              nodo = new Nodo("<ListaVar>");
              break;
            case 8: //<ListaVar> ::= , identificador <ListaVar>
              nodo = new Variables2(pila);
              break;
            case 9: //<DefFunc> ::= tipo identificador ( <Parametros> ) <BloqFunc>
              nodo = new DefFunc(pila);
              break;
            case 10: //<Parametros> ::= \e
              nodo = new Nodo("<ListaParam>");
              break;
            case 11: //<Parametros> ::= tipo identificador <ListaParam>
              nodo = new Parametros(pila);
              break;
            case 12: //<ListaParam> ::= \e
              nodo = new Nodo("<ListaParam>");
              break;
            case 13: //<ListaParam> ::= , tipo identificador <ListaParam>
              nodo = new Parametros2(pila);
              break;
            case 14: //<BloqFunc> ::= { <DefLocales> }
              nodo = new BloqueFunc(pila);
              break;
            case 15: //<DefLocales> ::= \e
              nodo = new Nodo("<DefLocales>");
              break;
            case 16: //<DefLocales> ::= <DefLocal> <DefLocales>
              nodo = new DefinicionesLocales(pila);
              break;
            case 17: //<DefLocal> ::= <DefVar>
              pila.Pop();
              nodo = pila.Pop().Nodo;
              break;
            case 18: //<DefLocal> ::= <Sentencia>
              pila.Pop();
              nodo = pila.Pop().Nodo;
              break;
            case 19: //<Sentencias> ::= \e
              nodo = new Nodo("<Sentencias>");
              break;
            case 20: //<Sentencias> ::= <Sentencia> <Sentencias>
              nodo = new Sentencias(pila);
              break;
            case 21: //<Sentencia> ::= identificador = <Expresion> ;
              nodo = new SentenciaAsignacion(pila);
              break;
            case 22: //<Sentencia> ::= if ( <Expresion> ) <SentenciaBloque> <Otro>
              nodo = new SentenciaIf(pila);
              break;
            case 23: //<Sentencia> ::= while ( <Expresion> ) <Bloque>
              nodo = new SentenciaWhile(pila);
              break;
            case 24: //<Sentencia> ::= return <ValorRegresa> ;
              nodo = new SentenciaValorRegresa(pila);
              break;
            case 25: //<Sentencia> ::= <LlamadaFunc> ;
              nodo = new SentenciaLlamadaFuncion(pila);
              break;
            case 26: //<Otro> ::= \e
              nodo = new Nodo("<Otro>");
              break;
            case 27: //<Otro> ::= else <SentenciaBloque>
              nodo = new Otro(pila);
              break;
            case 28: //<Bloque> ::= { <Sentencias> }
              nodo = new Bloque(pila);
              break;
            case 29: //<ValorRegresa> ::= \e
              nodo = new Nodo("<ValorRegresa>");
              break;
            case 30: //<ValorRegresa> ::= <Expresion>
              pila.Pop();
              Nodo valorRegresar = new Nodo("<ValorRegresa>");
              valorRegresar.AñadirHijo(pila.Pop().Nodo);
              nodo = valorRegresar;
              break;
            case 31: //<Argumentos> ::= \e
              nodo = new Nodo("<ListaArgumentos>");
              break;
            case 32: //<Argumentos> ::= <Expresion> <ListaArgumentos>
              nodo = new ListaArgumentos(pila);
              break;
            case 33: //<ListaArgumentos> ::= \e
              nodo = new Nodo("<ListaArgumentos>");
              break;
            case 34: //<ListaArgumentos> ::= , <Expresion> <ListaArgumentos>
              nodo = new ListaArgumentos2(pila);
              break;
            case 35: //<Termino> ::= <LlamadaFunc>
              pila.Pop();
              nodo = pila.Pop().Nodo;
              break;
            case 36: //<Termino> ::= identificador
              nodo = new Identificador(pila);
              break;
            case 37: //<Termino> ::= entero
              nodo = new Entero(pila);
              break;
            case 38: //<Termino> ::= real
              nodo = new Real(pila);
              break;
            case 39: //<Termino> ::= cadena
              nodo = new Cadena(pila);
              break;
            case 40: //<LlamadaFunc> ::= identificador ( <Argumentos> )
              nodo = new LlamadaFuncion(pila);
              break;
            case 41: //<SentenciaBloque> ::= <Sentencia>
              pila.Pop();
              nodo = pila.Pop().Nodo;
              break;
            case 42: //<SentenciaBloque> ::= <Bloque>
              pila.Pop();
              nodo = pila.Pop().Nodo;
              break;
            case 43: //<Expresion> ::= ( <Expresion> )
              nodo = new ExpresionEntreParentesis(pila);
              break;
            case 44: //<Expresion> ::= opSuma <Expresion>
              nodo = new OperadorAdicion2(pila);
              break;
            case 45: //<Expresion> ::= opNot <Expresion>
              nodo = new OperadorNot(pila);
              break;
            case 46: //<Expresion> ::= <Expresion> opMul <Expresion>
              nodo = new OperadorMultiplicacion(pila);
              break;
            case 47: //<Expresion> ::= <Expresion> opSuma <Expresion>
              nodo = new OperadorAdicion(pila);
              break;
            case 48: //<Expresion> ::= <Expresion> opRelac <Expresion>
              nodo = new OperadorRelacional(pila);
              break;
            case 49: //<Expresion> ::= <Expresion> opIgualdad <Expresion>
              nodo = new OperadorIgualdad(pila);
              break;
            case 50: //<Expresion> ::= <Expresion> opAnd <Expresion>
              nodo = new OperadorAnd(pila);
              break;
            case 51: //<Expresion> ::= <Expresion> opOr <Expresion>
              nodo = new OperadorOr(pila);
              break;
            case 52: //<Expresion> ::= <Termino>
              pila.Pop();
              Nodo expresion = new Nodo("<Expresion>");
              expresion.AñadirHijo(pila.Pop().Nodo);
              nodo = expresion;
              break;
            default:
              for (int i = 0; i < lonReglas[regla]*2; i++)
                pila.Pop();
              break;
          }

          fila = pila.Peek().Id;
          columna = idReglas[regla];
          accion = tabla[fila, columna];

          nt = new NoTerminal(idReglas[regla], strReglas[regla]);
          nt.Nodo = nodo;

          pila.Push(nt);
          pila.Push(new Estado(accion));
        } //fin de else if
        if (accion == 0)
          Console.WriteLine("Error");
      } //fin de while
    } //fin del metodo AnalisisSintactico

    private void MostrarPila()
    {
      ElementoPila[] items = pila.ToArray();
      String elementosPila = "";
      for (int i = items.Length - 1; i >= 0; i--)
        elementosPila += items[i].Elemento;

      Console.WriteLine(elementosPila + " | " + lexico.Simbolo + " | " + accion);
    } //fin del metodo MostrarPila

    private void SiguienteAccion()
    {
      fila = pila.Peek().Id;
      columna = lexico.Tipo;
      accion = tabla[fila, columna];
    } //fin del metodo SiguienteAccion

    private void CargarArchivos()
    {
      string nombreArchivo = "compilador.lr";
      using (var lector = new StreamReader(nombreArchivo))
      {
        String linea = "";
        int contadorLinea = 0;

        while ((linea = lector.ReadLine()) != null)
        {
          String[] arreglo = linea.Split(null);

          for (int i = 0; i < arreglo.Length && contadorLinea < 53; i++)
          {
            if (contadorLinea > 0)
            {
              if (i == 0)
                idReglas[contadorLinea - 1] = Convert.ToInt32(arreglo[i]);
              if (i == 1)
                lonReglas[contadorLinea - 1] = Convert.ToInt32(arreglo[i]);
              if (i == 2)
                strReglas[contadorLinea - 1] = arreglo[i];
            } //fin de if
          } //fin de for

          for (int i = 0; i < arreglo.Length; i++)
          {
            if (contadorLinea > 53)
              tabla[contadorLinea - 54, i] = Convert.ToInt32(arreglo[i]);
          } //fin de for
          contadorLinea++;
        } //fin de while
      } //fin de using
    } //fin del metodo CargarArchivos
  } //fin de la clase Sintactico
}