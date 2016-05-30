using System;
using System.IO;

namespace Compiler
{
  public class GeneracionCodigo
  {
    //atributos
    private Nodo _arbol;

    //constructor
    public GeneracionCodigo(Nodo arbol)
    {
      _arbol = arbol;
    }//fin del constructor

    public void GenerarCodigo()
    {
      EliminarArchivo();

      Console.WriteLine("Generacion Codigo:\n");

      _arbol.GeneraCodigoEnsamblador();

    }//fin del metodo GeneracionCodigo

    public static void AgregaArchivo(string codigo)
    {
      //variables locales
      StreamWriter archivo = new StreamWriter("Salida.asm", true);
      archivo.WriteLine(codigo);
      archivo.Close();

    }//fin del metodo ArchivoEnsamblador

    public static void EliminarArchivo()
    {
      if (File.Exists("Salida.asm"))
      {
        File.Delete("Salida.asm");
      }
    }//fin del metodo EliminarArchivo

  }//fin de la clase GeneracionCodigo

}//fin del espacio de nombres Compiler