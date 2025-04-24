using UnityEngine;
using Systems.Inventory;
using System;
using System.Linq;
using System.Collections.Generic;


namespace Cards
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card/Card Data")]
    [Serializable]
    public class CardData : ItemDetails
    {
        public CardRank Rank;
        public CardSuit Suit;
#if UNITY_EDITOR
        void OnValidate()
        {
            if (!string.IsNullOrEmpty(this.name))
            {
                var token = name.Split('_');
                if (token.Length > 1)
                {
                    Suit = ParseSuit(name);
                    Rank = ParseRank(name);
                    int i = Rank == CardRank.Ace ? 1 : (int)Rank;
                    
                    var sprites = Resources.LoadAll<Sprite>($"playingcards/transparent_bg/cards_basic");
                    int index = i + 14 * (int)Suit ;
                    Icon = sprites.FirstOrDefault(x => x.name == $"cards_basic_{index}");

                }
                Name = Rank + " Of " + Suit;
            }
        }
#endif
        public static CardRank ParseRank(string name)
        {
            foreach (var word in name.Split('_', StringSplitOptions.RemoveEmptyEntries))
            {
                if (int.TryParse(word, out int num))
                {
                    if (Enum.IsDefined(typeof(CardRank), num))
                        return (CardRank)num;
                }
                if (Enum.TryParse<CardRank>(word, true, out var rank))
                    return rank;
            }
            Debug.LogWarning($"Cannot parse rank from {name}, defaulting to Two");
            return CardRank.Two;
        }
        public static CardSuit ParseSuit(string name)
        {
            foreach (var word in name.Split('_', StringSplitOptions.RemoveEmptyEntries))
            {
                if (Enum.TryParse<CardSuit>(word, true, out var suit))
                    return suit;
            }
            Debug.LogWarning($"Cannot parse suit from {name}, defaulting to Hearts");

            return CardSuit.Hearts;
        }
    }
    public enum CardSuit
    {
        Hearts = 0,
        Diamonds = 1,
        Clubs = 2,
        Spades = 3,
    }
    public enum CardRank
    {
        LowAce = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
    }
    public class CardDataRankComparer : IComparer<CardData>
    {
        public int Compare(CardData x, CardData y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            int rankComparison = x.Rank.CompareTo(y.Rank);
            return rankComparison != 0 ? rankComparison : x.Suit.CompareTo(y.Suit);
        }
    }

    public class CardDataSuitComparer : IComparer<CardData>
    {
        public int Compare(CardData x, CardData y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            int suitComparison = x.Suit.CompareTo(y.Suit);
            return suitComparison != 0 ? suitComparison : x.Rank.CompareTo(y.Rank);
        }
    }
}

