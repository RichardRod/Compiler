using System;
using System.Collections.Generic;

namespace Compiler
{
  public class Semantico
  {
    //atributos
    private TablaSimbolos tablaSimbolos;
    protected List<string> listaErrores;
    protected Nodo arbol;

    //constructor
    public Semantico()
    {
      listaErrores = new List<string>();
      Nodo.tablaSimbolos = tablaSimbolos = new TablaSimbolos(listaErrores);
    }//fin del constructor

    public void Analiza(Nodo arbol)
    {
      Console.WriteLine("Resultado Analisis Semantico\n");

      this.arbol = arbol;
      arbol.ValidaTipos();

      tablaSimbolos.Muestra();
      MuestraErrores();

    } //fin del metodo Analiza

    public void MuestraErrores()
    {
      if (ExistenErrores())
      {
        Console.WriteLine("Errores Semanticos");
        foreach (string error in listaErrores)
        {
          Console.WriteLine(error);
        } //fin de foreach
      } //fin de if
      else
      {
        Console.WriteLine("Sin errores Semanticos");
        //proceder a la generacion de codigo

      } //fin de else
    } //fin del metodo MuestraErrores

    public bool ExistenErrores()
    {
      return listaErrores.Count > 0;
    } //fin del metodo ExistenErrores

  } //fin de la clase Semantico

  public class Tipo
  {
      public static char DameTipo(string simbolo)
      {
        char tipo = ' ';

        if (simbolo.Equals("int"))
          tipo =  'i';
        if (simbolo.Equals("float"))
          tipo =  'f';
        if (simbolo.Equals("string"))
          tipo = 's';
        if (simbolo.Equals("void"))
          tipo = 'v';

        return tipo;
      }//fin del metodo DameTipo

    }//fin de la clase Tipo
}