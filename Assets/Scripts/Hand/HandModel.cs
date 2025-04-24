using System.Collections.Generic;
using System.Linq;
using Systems.Inventory;
using UnityEngine;

namespace Cards.Hand
{
    public class HandModel : InventoryModel
    {
        List<CardData> cards;
        public HandModel(IEnumerable<CardData> itemDetails, int capacity) : base(itemDetails, capacity)
        {
            cards = new List<CardData>(itemDetails);
        }
        public void SortBySuit()
        {
            cards = cards.OrderBy(x => x.Suit).ThenBy(x => x.Rank).ToList();
            for (int i = 0; i < cards.Count; i++)
            {
                var item = Get(cards[i].Id);
                int index = IndexOf(item);
                Swap(i, index);
            }
        }
        public void SortByRank()
        {
            cards = cards.OrderBy(x => x.Rank).ThenBy(x => x.Suit).ToList();
            for (int i = 0; i < cards.Count; i++)
            {
                var item = Get(cards[i].Id);
                int index = IndexOf(item);
                Swap(i, index);
            }
        }
    }
}