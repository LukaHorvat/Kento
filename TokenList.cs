using System.Collections.Generic;

namespace Kento
{
	internal class TokenList
	{
		private bool empty = true;
		private TokenListNode first, last;

		public TokenList() {}

		public TokenList(TokenList List)
		{
			for (TokenListNode node = List.First; node != null; node = node.Next)
			{
				Add(node.Value);
			}
		}

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

		public void Add(Token Token)
		{
			if (empty)
			{
				last = new TokenListNode(Token);
				first = last;
				empty = false;
			}
			else
			{
				last.Next = new TokenListNode(Token);
				TokenListNode temp = last;
				last = last.Next;
				last.Previous = temp;
			}
		}

		public LinkedList<Token> ToLinkedList()
		{
			var newList = new LinkedList<Token>();
			for (TokenListNode node = first; node != null; node = node.Next)
			{
				newList.AddLast(node.Value);
			}
			return newList;
		}

		public void FromLinkedList(LinkedList<Token> List)
		{
			for (LinkedListNode<Token> node = List.First; node != null; node = node.Next)
			{
				Add(node.Value);
			}
		}

		/// <summary>
		/// Insert a new node between 2 other ones
		/// </summary>
		/// <param name="Node"></param>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		public void Insert(TokenListNode Node, TokenListNode Left, TokenListNode Right)
		{
			if (Left != null)
				Left.Next = Node;
			else
			{
				first = Node;
			}
			Node.Next = Right;
			Node.Previous = Left;
			if (Right != null)
				Right.Previous = Node;
			else
			{
				last = Node;
			}
		}
	}
}