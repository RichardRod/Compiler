using System;
using System.Collections.Generic;

namespace Compiler
{
  public class TablaSimbolos
  	{
  		//atributos
  		protected List<ElementoTabla> tabla = new List<ElementoTabla>();
  		public Variable varLocal;
  		public Variable varGlobal;
  		public Funcion funcion;
  		public List<String> listaErrores;

  		public TablaSimbolos (List<String> listaErrores)
  		{
  			this.listaErrores = listaErrores;
  		}

  		public void Agrega(ElementoTabla elemento)
  		{
  			tabla.Add (elemento);

  		}//fin del constructor

  		public void Muestra()
  		{
  		  Console.WriteLine("Muestra en Tabla de simbolos");
  		}//fin del metodo Muestra

  		public bool VarGlobalDefinida(string variable)
  		{
  			foreach (ElementoTabla elemento in tabla) {
  				if (elemento.EsVariable () && !elemento.EsVarLocal ()) {
  					if (elemento.Simbolo.CompareTo (variable) == 0)
  						return true;
  				}
  			}//fin de foreach

  			return false;
  		}//fin de la funcion VarGlobalDefinida

  		public bool FuncionDefinida(string simbolo)
  		{
  			foreach (ElementoTabla elemento in tabla) {
  				if (elemento.EsFuncion ()) {
  					if (elemento.Simbolo.CompareTo (funcion) == 0)
  						return true;
  				}
  			}//fin de foreach
  			return false;
  		}

  		public bool VarLocalDefinida(string variable, string funcion)
  		{
  			foreach (ElementoTabla elemento in tabla) {
  				if (elemento.EsVariable () && elemento.EsVarLocal ()) {
  					if (((Variable)elemento).Ambito.CompareTo (funcion) == 0 && elemento.Simbolo.CompareTo (variable) == 0)
  						return true;
  				}
  			}//fin de foreach


  			return false;
  		}

  		public void BuscaIdentificador(string simbolo)
  		{
  			varGlobal = null;
  			varLocal = null;
  			funcion = null;

  			foreach (ElementoTabla elemento in tabla) {
  				if (elemento.Simbolo.CompareTo (simbolo) == 0) {
  					if (elemento.EsVariable ()) {
  						if (elemento.EsVarLocal ())
  							varLocal = (Variable)elemento;
  						else
  							varGlobal = (Variable)elemento;
  					} else
  						funcion = (Funcion)elemento;
  				}//fin de if
  			}//fin de foreach

  		}

  		public void BuscaFuncion(string simbolo)
  		{
  			varGlobal = null;
  			varLocal = null;
  			funcion = null;

  			foreach (ElementoTabla elemento in tabla) {
  				if (elemento.Simbolo.CompareTo (simbolo) == 0 && elemento.EsFuncion ()) {
  					funcion = (Funcion)elemento;
  					return;
  				}
  			}
  		}

  		public void Agrega(Variables defVar)
  		{

  		}

  		public void Agrega(DefFunc defFunc)
  		{

  		}

  		public void Agrega(Parametros parametros)
  		{

  		}


  	}//fin de la clase TablaSimbolos

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

  		public virtual void Muestra() {}

  	}//fin de la clase ElementoTabla

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

  		}//fin del constructor

  		public string Ambito
  		{
  			get { return ambito; }
  		}

  		public override bool EsVarLocal ()
  		{
  			return local;
  		}

  		public override bool EsVariable ()
  		{
  			return true;
  		}

  		public override void Muestra ()
  		{
  			Console.WriteLine ("Variable: " + Simbolo + " Tipo: " + Tipo);

  			if (local)
  				Console.WriteLine ("Local");
  			else
  				Console.WriteLine ("Global");
  		}
  	}//fin de la clase Variable

  	public class Funcion : ElementoTabla
  	{
  		//atributos
  		private string parametros;

  		public Funcion(char tipo, string simbolo, string parametros)
  		{
  			this.Simbolo = simbolo;
  			this.Tipo = tipo;
  			this.parametros = parametros;
  		}

  		public override bool EsFuncion ()
  		{
  			return true;
  		}

  		public override void Muestra ()
  		{
  			Console.WriteLine ("Funcion: " + Simbolo + " Tipo: " + Tipo + " Parametros: " + parametros);
  		}

  	}//fin de la clase Funcion
} //fin del espacio de nombres Compilador

