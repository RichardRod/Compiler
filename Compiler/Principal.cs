using System;
using System.IO;

namespace Compiler
{
  class Principal
  {
    static void Main(string[] args)
    {
      Principal principal = new Principal();
      string fuente = principal.LeerArchivo();

      Sintactico sintactico = new Sintactico(fuente);
      sintactico.AnalisisSintactico();
    }

    private string LeerArchivo()
    {
      String linea = "";
      string contenido = "";
      string nombreArchivo = "archivoFuente.txt";
      using (var lector = new StreamReader(nombreArchivo))
      {
        while ((linea = lector.ReadLine()) != null)
        {
          contenido += linea;
        } //fin de while
      } //fin de using

      return contenido;
    } //fin del metodo LeerArchivo
  }
}


