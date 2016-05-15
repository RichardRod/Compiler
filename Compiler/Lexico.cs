using System;


namespace Compiler
{
  public class Lexico
  	{
  		//atributos
  		private string fuente;
  		private int indice;
  		private bool continua;
  		private char c;
  		private int estado;
  		private string simbolo;
  		private int tipo;

  		//estados
  		private const int q00 = 0;
  		private const int q01 = 1;
  		private const int q02 = 2;
  		private const int q03 = 3;
  		private const int q04 = 4;
  		private const int q05 = 5;
  		private const int q06 = 6;
  		private const int q07 = 7;
  		private const int q08 = 8;
  		private const int q09 = 9;
  		private const int q10 = 10;
  		private const int q11 = 11;
  		private const int q12 = 12;
  		private const int q13 = 13;
  		private const int q14 = 14;
  		private const int q15 = 15;
  		private const int q16 = 16;
  		private const int q17 = 17;
  		private const int q18 = 18;
  		private const int q19 = 19;
  		private const int q20 = 20;
  		private const int q21 = 21;
  		private const int q22 = 22;
  		private const int q23 = 23;
  		private const int q24 = 24;
  		private const int q25 = 25;
  		private const int q26 = 26;
  		private const int FIN = 27;
  		private const int ERR = 28;

  		private int[,] tablaEstados =
  		{
  		//   (_)                                                                                          ( )
  		//  (a-z) (0-9) (.)  (") (+,-) (*,/) (<,>) (|)  (&)  (!)  (=)  (;)  (,)  (()  ())  ({)  (})  ($)  (\t) (otro)
  		//  (A-Z)                                                                                         (\n)
  			{q24,  q18, ERR, q21, q01,  q02,  q25, q03, q05, q07, q08, q11, q12, q13, q14, q15, q16, q17, FIN,  ERR}, //q00
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q01
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q02
  			{ERR,  ERR, ERR, ERR, ERR,  ERR,  ERR, q04, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR,  ERR}, //q03
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q04
  			{ERR,  ERR, ERR, ERR, ERR,  ERR,  ERR, ERR, q06, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR,  ERR}, //q05
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q06
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, q10, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q07
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, q09, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q08
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q09
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q10
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q11
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q12
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q13
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q14
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q15
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q16
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q17
  			{FIN,  q18, q19, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q18
  			{ERR,  q20, ERR, ERR, ERR,  ERR,  ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR, ERR,  ERR}, //q19
  			{FIN,  q20, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q20
  			{q22,  q22, q22, q22, q22,  q22,  q22, q22, q22, q22, q22, q22, q22, q22, q22, q22, q22, ERR, q22,  q22}, //q21
  			{q22,  q22, q22, q23, q22,  q22,  q22, q22, q22, q22, q22, q22, q22, q22, q22, q22, q22, ERR, q22,  q22}, //q22
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q23
  			{q24,  q24, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q24
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, q26, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}, //q25
  			{FIN,  FIN, FIN, FIN, FIN,  FIN,  FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN, FIN,  FIN}  //q26
  		};


  		//constructor sin parametros
  		public Lexico ()
  		{
  			indice = 0;
  		}//fin del constructor sin parametros

  		//constructor parametrizado
  		public Lexico(string fuente)
  		{
  			indice = 0;
  			simbolo = "";
  			this.fuente = fuente;
  		}//fin del constructor parametrizado

  		public string Simbolo
  		{
  			get { return simbolo; }
  		}

  		public int Tipo
  		{
  			get { return tipo; }
  		}

  		public string TipoACadena(int tipo)
  		{
  			string cadena = "";

  			switch (tipo)
  			{
  			case TipoSimbolo.ERROR:
  				cadena = "Error";
  				break;

  			case TipoSimbolo.IDENTIFICADOR:
  				cadena = "Identificador";
  				break;

  			case TipoSimbolo.ENTERO:
  				cadena = "Entero";
  				break;

  			case TipoSimbolo.REAL:
  				cadena = "Real";
  				break;

  			case TipoSimbolo.CADENA:
  				cadena = "Cadena";
  				break;

  			case TipoSimbolo.TIPO:
  				cadena = "Tipo";
  				break;

  			case TipoSimbolo.OP_SUMA:
  				cadena = "Operador Suma";
  				break;

  			case TipoSimbolo.OP_MULTIPLICACION:
  				cadena = "Operador Multiplicacion";
  				break;

  			case TipoSimbolo.OP_RELACIONAL:
  				cadena = "Operador Relacional";
  				break;

  			case TipoSimbolo.OP_OR:
  				cadena = "Operador OR";
  				break;

  			case TipoSimbolo.OP_AND:
  				cadena = "Operador AND";
  				break;

  			case TipoSimbolo.OP_NOT:
  				cadena = "Operador NOT";
  				break;

  			case TipoSimbolo.OP_IGUALDAD:
  				cadena = "Operador Igualdad";
  				break;

  			case TipoSimbolo.PUNTO_COMA:
  				cadena = "Punto y Coma";
  				break;

  			case TipoSimbolo.COMA:
  				cadena = "Coma";
  				break;

  			case TipoSimbolo.PARENTESIS_INICIO:
  				cadena = "Parentesis Inicio";
  				break;

  			case TipoSimbolo.PARENTESIS_FIN:
  				cadena = "Parentesis Fin";
  				break;

  			case TipoSimbolo.LLAVE_INICIO:
  				cadena = "Llave Inicio";
  				break;

  			case TipoSimbolo.LLAVE_FIN:
  				cadena = "Llave Fin";
  				break;

  			case TipoSimbolo.IGUAL:
  				cadena = "Operador Asignacion";
  				break;

  			case TipoSimbolo.IF:
  				cadena = "Palabra Reservada if";
  				break;

  			case TipoSimbolo.WHILE:
  				cadena = "Palabra Reservada while";
  				break;

  			case TipoSimbolo.RETURN:
  				cadena = "Palabra Reservada return";
  				break;

  			case TipoSimbolo.ELSE:
  				cadena = "Palabra Reservada else";
  				break;

  			case TipoSimbolo.PESOS:
  				cadena = "Fin de la entrada";
  				break;

  			}//fin de switch

  			return cadena;
  		}//fin del metodo TipoACadena

  		public void Entrada(string fuente)
  		{
  			indice = 0;
  			simbolo = "";
  			this.fuente = fuente;
  		}//fin del metodo Entrada

  		public void SiguienteSimbolo()
  		{
  			int estadoAnterior, entrada;
  			continua = true;
  			simbolo = "";

  			//Revisamos si es una palabra reservada de control de flujo (if, else, while, return)
  			tipo = EsPalabraReservada(ref simbolo);
  			if (tipo != -1)
  				return;

  			//Revisamos si es una palabra reservada de tipo (int, float o void)
  			tipo = EsTipo(ref simbolo);
  			if (tipo != -1)
  				return;

  			estado = estadoAnterior = q00;
  			while(continua)
  			{
  				c = SiguienteCaracter();

  				if (EsLetra(c))
  					entrada = 0;
  				else if (EsDigito(c))
  					entrada = 1;
  				else if (c == '.')
  					entrada = 2;
  				else if (c == '"')
  					entrada = 3;
  				else if (c == '+' || c == '-')
  					entrada = 4;
  				else if (c == '*' || c == '/')
  					entrada = 5;
  				else if (c == '<' || c == '>')
  					entrada = 6;
  				else if (c == '|')
  					entrada = 7;
  				else if (c == '&')
  					entrada = 8;
  				else if (c == '!')
  					entrada = 9;
  				else if (c == '=')
  					entrada = 10;
  				else if (c == ';')
  					entrada = 11;
  				else if (c == ',')
  					entrada = 12;
  				else if (c == '(')
  					entrada = 13;
  				else if (c == ')')
  					entrada = 14;
  				else if (c == '{')
  					entrada = 15;
  				else if (c == '}')
  					entrada = 16;
  				else if (c == '$')
  					entrada = 17;
  				else if (EsEspacio(c))
  					entrada = 18;
  				else
  					entrada = 19;
  				estadoAnterior = estado;
  				estado = CalculaEstado(entrada);
  				simbolo += c;
  				if (estado == FIN)
  				{
  					switch (estadoAnterior)
  					{
  					case q00:
  						break;
  					case q01:
  						tipo = TipoSimbolo.OP_SUMA;
  						break;
  					case q02:
  						tipo = TipoSimbolo.OP_MULTIPLICACION;
  						break;
  					case q04:
  						tipo = TipoSimbolo.OP_OR;
  						break;
  					case q06:
  						tipo = TipoSimbolo.OP_AND;
  						break;
  					case q07:
  						tipo = TipoSimbolo.OP_NOT;
  						break;
  					case q08:
  						tipo = TipoSimbolo.IGUAL;
  						break;
  					case q09: case q10:
  						tipo = TipoSimbolo.OP_RELACIONAL;
  						break;
  					case q11:
  						tipo = TipoSimbolo.PUNTO_COMA;
  						break;
  					case q12:
  						tipo = TipoSimbolo.COMA;
  						break;
  					case q13:
  						tipo = TipoSimbolo.PARENTESIS_INICIO;
  						break;
  					case q14:
  						tipo = TipoSimbolo.PARENTESIS_FIN;
  						break;
  					case q15:
  						tipo = TipoSimbolo.LLAVE_INICIO;
  						break;
  					case q16:
  						tipo = TipoSimbolo.LLAVE_FIN;
  						break;
  					case q17:
  						tipo = TipoSimbolo.PESOS;
  						break;
  					case q18:
  						tipo = TipoSimbolo.ENTERO;
  						break;
  					case q20:
  						tipo = TipoSimbolo.REAL;
  						break;
  					case q23:
  						tipo = TipoSimbolo.CADENA;
  						break;
  					case q24:
  						tipo = TipoSimbolo.IDENTIFICADOR;
  						break;
  					case q25: case q26:
  						tipo = TipoSimbolo.OP_RELACIONAL;
  						break;
  					}
  					if (EsEspacio(c))
  					{
  						simbolo = simbolo.Remove(simbolo.Length - 1);
  						break;
  					}
  					Retroceso();
  					break;
  				}
  				else if (estado == ERR)
  				{
  					tipo = TipoSimbolo.ERROR;
  					if (estadoAnterior == q24 || estadoAnterior == q19 || estadoAnterior == q03 || estadoAnterior == q05 || estadoAnterior == q22)
  						Retroceso();
  					break;
  				}
  			}

  			//Si es un sólo blanco
  			if (estado == FIN && estadoAnterior == q00)
  				SiguienteSimbolo();
  		}//fin del metodo SiguienteSimbolo

  		private int CalculaEstado(int entrada)
  		{
  			return tablaEstados[estado, entrada];
  		}

  		private char SiguienteCaracter()
  		{
  			if (Terminado ())
  				return '$';
  			return fuente [indice++];

  		}//fin del metodo SiguienteCaracter

  		private void SiguienteEstado(int estado)
  		{
  			this.estado = estado;
  			simbolo += c;
  		}//fin del metodo SiguienteEstado

  		private void Aceptacion(int estado)
  		{
  			SiguienteEstado (estado);
  			continua = false;
  		}//fin del metodo Aceptacion

  		private bool Terminado()
  		{
  			return indice >= fuente.Length;
  		}//fin del metodo Terminado

  		private bool EsLetra(char c)
  		{
  			return Char.IsLetter (c) || c == '_';
  		}//fin del metodo EsLetra

  		private bool EsDigito(char c)
  		{
  			return Char.IsDigit (c);
  		}//fin del metodo EsDigito

  		private bool EsEspacio(char c)
  		{
  			return Char.IsWhiteSpace (c);
  		}//fin del metodo EsEspacio

  		private int EsTipo(ref String tipo)
  		{
  			if (indice + 3 <= fuente.Length && fuente.Substring(indice, 3) == "int")
  			{
  				tipo = "int";
  				indice += 3;
  				return TipoSimbolo.TIPO;
  			}
  			else if (indice + 5 <= fuente.Length && fuente.Substring(indice, 5) == "float")
  			{
  				tipo = "float";
  				indice += 5;
  				return TipoSimbolo.TIPO;
  			}
  			else if (indice + 4 <= fuente.Length && fuente.Substring(indice, 4) == "void")
  			{
  				tipo = "void";
  				indice += 4;
  				return TipoSimbolo.TIPO;
  			}
  			return -1;
  		}

  		private int EsPalabraReservada(ref String palabraReservada)
  		{
  			if (indice + 2 <= fuente.Length && fuente.Substring(indice, 2) == "if")
  			{
  				palabraReservada = "if";
  				indice += 2;
  				return TipoSimbolo.IF;
  			}
  			else if (indice + 4 <= fuente.Length && fuente.Substring(indice, 4) == "else")
  			{
  				palabraReservada = "else";
  				indice += 4;
  				return TipoSimbolo.ELSE;
  			}
  			else if (indice + 5 <= fuente.Length && fuente.Substring(indice, 5) == "while")
  			{
  				palabraReservada = "while";
  				indice += 5;
  				return TipoSimbolo.WHILE;
  			}
  			else if (indice + 6 <= fuente.Length && fuente.Substring(indice, 6) == "return")
  			{
  				palabraReservada = "return";
  				indice += 6;
  				return TipoSimbolo.RETURN;
  			}
  			return -1;
  		}

  		private void Retroceso()
  		{
  			simbolo = simbolo.Remove(simbolo.Length - 1);
  			if (c != '$')
  				indice--;
  			continua = false;
  		}

  	}//fin de la clase Lexico

  	public class TipoSimbolo
  	{
  		public const int ERROR = -1;
  		public const int IDENTIFICADOR = 0;
  		public const int ENTERO = 1;
  		public const int REAL = 2;
  		public const int CADENA = 3;
  		public const int TIPO = 4;
  		public const int OP_SUMA = 5;
  		public const int OP_MULTIPLICACION = 6;
  		public const int OP_RELACIONAL = 7;
  		public const int OP_OR = 8;
  		public const int OP_AND = 9;
  		public const int OP_NOT = 10;
  		public const int OP_IGUALDAD = 11;
  		public const int PUNTO_COMA = 12;
  		public const int COMA = 13;
  		public const int PARENTESIS_INICIO = 14;
  		public const int PARENTESIS_FIN = 15;
  		public const int LLAVE_INICIO = 16;
  		public const int LLAVE_FIN = 17;
  		public const int IGUAL = 18;
  		public const int IF = 19;
  		public const int WHILE = 20;
  		public const int RETURN = 21;
  		public const int ELSE = 22;
  		public const int PESOS = 23;

  	}//fin de la clase TipoSimbolo
}