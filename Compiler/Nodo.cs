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

		public List<Nodo> Hijos {
			get {
				return hijos;
			}
		}

		//constructor
		public Nodo (){}

		public Nodo(string texto)
		{
			this.simbolo = texto;

		}

		public virtual void Muestra() {}
		public virtual void MuestraSangria()
		{
			//for (int i = 0; i < sangria; i++)
				//Console.Write (" ");
		}

		public void AñadirHijo(Nodo hijo)
		{
			hijos.Add (hijo);

		}

		public virtual void ValidaTipos()
		{
			if (nodoSiguiente != null) {
				nodoSiguiente.ValidaTipos ();
			}

		}//fin del metodo ValidaTipos

	}//fin de la clase Nodo
}

