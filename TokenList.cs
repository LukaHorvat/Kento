using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class TokenList
	{
		bool empty = true;
		TokenListNode first, last;
		public TokenListNode First
		{
			get { return first; }
			set { first = value; }
		}
		public TokenListNode Last
		{
			get { return last; }
			set { last = value; }
		}

		public TokenList ()
		{
		}
		public TokenList ( TokenList List )
		{
			for ( var node = List.First ; node != null ; node = node.Next )
			{
				Add( node.Value );
			}
		}
		public void Add ( Token Token )
		{
			if ( empty )
			{
				last = new TokenListNode( Token );
				first = last;
				empty = false;
			} else
			{
				last.Next = new TokenListNode( Token );
				var temp = last;
				last = last.Next;
				last.Previous = temp;
			}
		}
		public LinkedList<Token> ToLinkedList ()
		{
			var newList = new LinkedList<Token>();
			for ( var node = first ; node != null ; node = node.Next )
			{
				newList.AddLast( node.Value );
			}
			return newList;
		}
		public void FromLinkedList ( LinkedList<Token> List )
		{
			for ( var node = List.First ; node != null ; node = node.Next )
			{
				Add( node.Value );
			}
		}
		/// <summary>
		/// Insert a new node between 2 other ones
		/// </summary>
		/// <param name="Node"></param>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		public void Insert ( TokenListNode Node, TokenListNode Left, TokenListNode Right )
		{
			if ( Left != null )
				Left.Next = Node;
			else
			{
				first = Node;
			}
			Node.Next = Right;
			Node.Previous = Left;
			if ( Right != null )
				Right.Previous = Node;
			else
			{
				last = Node;
			}
		}
	}
}
