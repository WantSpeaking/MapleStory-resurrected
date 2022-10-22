using System.Collections.Generic;




namespace ms
{
	// Class that represents the monster card collection of an individual character
	public class MonsterBook
	{
		public MonsterBook()
		{
			cover = 0;
		}

		public void set_cover(int cov)
		{
			cover = cov;
		}
		public void add_card(short card, sbyte level)
		{
			cards[card] = level;
		}

		public int TotalCards => cards.Count;
		private int cover;
		private SortedDictionary<short, sbyte> cards = new SortedDictionary<short, sbyte>();
	}
}


