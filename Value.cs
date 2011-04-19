using System;
using System.Collections.Generic;

namespace Kento
{
	public abstract class Value : Token, IComparable<Value>
	{
		public virtual bool Static { get; set; }

		#region IComparable<Value> Members

		public virtual int CompareTo ( Value Other )
		{
			return -1;
		}

		#endregion

		public virtual Value Evaluate ()
		{
			return this;
		}

		public virtual Array ToArray ()
		{
			return new Array( this );
		}

		public virtual List<Token> Tokenize ()
		{
			return new List<Token>( new[] { (Token)this } );
		}

		public virtual Value Clone ()
		{
			return this;
		}

		public virtual void Destroy ()
		{

		}
	}

	internal class NoValue : Value
	{
		private static NoValue noValue = new NoValue();

		public static NoValue Value
		{
			get { return noValue; }
			set { noValue = value; }
		}
	}

	internal class Nothing : Value
	{
		private static Nothing val = new Nothing();

		public static Nothing Value
		{
			get { return val; }
			set { val = value; }
		}
	}
}