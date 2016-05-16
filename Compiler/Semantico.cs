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

    public void Analiza(Nodo arbol)
    {
      this.arbol = arbol;
      //arbol.ValidaTipos();

      //tablaSimbolos.Muestra();

      Console.WriteLine("Denunciado papu");


    }//fin del metodo Analiza

    public void MuestraErrores()
    {
      if(listaErrores.Count == 0)
        return;




    }//fin del metodo MuestraErrores


    public bool ExistenErrores()
    {
      return listaErrores.Count > 0;
    }//fin del metodo ExistenErrores

  }//fin de la clase Semantico
}