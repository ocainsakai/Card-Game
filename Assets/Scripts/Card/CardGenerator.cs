using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Cards;

namespace Systems.Inventory
{
    public static class CardGenerator
    {
        public static string folderPath = "Assets/GameData/Deck"; 
        public static List<CardData> generatedDeck;

      
        public static void GenerateDeck()
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath); 
            }

            generatedDeck = new List<CardData>();

            foreach (CardSuit suit in System.Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardRank rank in System.Enum.GetValues(typeof(CardRank)))
                {
                    
                    CardData newCard = ScriptableObject.CreateInstance<CardData>();
                    newCard.Suit = suit;
                    newCard.Rank = rank;

                    
                    string cardFilePath = Path.Combine(folderPath, $"{suit}_{rank}.asset");

                    AssetDatabase.CreateAsset(newCard, cardFilePath);
                    AssetDatabase.SaveAssets();

                    generatedDeck.Add(newCard);
                }
            }

            Debug.Log("Deck generated and saved.");
        }

  
        public static void PrintDeck()
        {
            foreach (CardData card in generatedDeck)
            {
                Debug.Log($"{card.Rank} of {card.Suit}");
            }
        }
    }
}
