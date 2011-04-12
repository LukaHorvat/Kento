using System;

namespace Kento
{
	internal class Number : Literal
	{
		private double value;

		public Number(double Value)
		{
			value = Value;
		}

		public double Val
		{
			get { return value; }
			set { this.value = value; }
		}

		public override string ToString()
		{
			return value.ToString();
		}

		public override Value Clone()
		{
			return new Number(value);
		}

		public override int CompareTo(Value Other)
		{
			if (Other is Number) return (int) Math.Ceiling(value - (Other as Number).value);
			return 1;
		}
	}
}