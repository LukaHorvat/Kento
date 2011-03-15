using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Operator : Token
	{
		private int precedance;
		public int Precedance
		{
			get { return precedance; }
			set { precedance = value; }
		}
	}
}
