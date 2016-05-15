using System;

namespace Compiler
{
  public class ElementoPila
  	{
  		protected int id;
  		protected String elemento;
  		protected int tipo;
  		protected int subTipo;
  		protected Nodo nodo;

  		public virtual int Id
  		{
  			get { return id; }
  		}
  		public virtual String Elemento
  		{
  			get { return elemento; }
  		}
  		public virtual int Tipo
  		{
  			get { return tipo; }
  		}
  		public virtual int SubTipo
  		{
  			get { return subTipo; }
  		}
  		public virtual Nodo Nodo
  		{
  			get { return nodo; }
  			set { nodo = value; }
  		}

  		public virtual bool EsEstado()
  		{
  			return false;
  		}

  		public virtual bool EsTerminal()
  		{
  			return false;
  		}

  		public virtual bool EsNoTerminal()
  		{
  			return false;
  		}
  	}//fin de la clase ElementoPila

  	class Terminal : ElementoPila
  	{
  		override public int Id
  		{
  			get { return id; }
  		}

  		override public String Elemento
  		{
  			get { return elemento; }
  		}

  		public Terminal(int id)
  		{
  			this.id = id;
  			elemento = "";
  		}

  		public Terminal(int id, String elemento)
  		{
  			this.id = id;
  			this.elemento = elemento;
  		}

  		override public bool EsTerminal()
  		{
  			return true;
  		}
  	}//fin de la clase Terminal

  	class NoTerminal : ElementoPila
  	{
  		override public int Id
  		{
  			get { return id; }
  		}

  		override public String Elemento
  		{
  			get { return elemento; }
  		}

  		public NoTerminal(int id, String elemento)
  		{
  			this.id = id;
  			this.elemento = elemento;
  		}

  		override public bool EsNoTerminal()
  		{
  			return true;
  		}
  	}//fin de la clase NoTerminal

  	class Estado : ElementoPila
  	{
  		override public int Id
  		{
  			get { return id; }
  		}

  		override public String Elemento
  		{
  			get { return id.ToString(); }
  		}

  		public Estado(int id)
  		{
  			this.id = id;
  		}

  		override public bool EsEstado()
  		{
  			return true;
  		}
  	}//fin de la clase Estado
}