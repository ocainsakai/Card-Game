using UnityEngine;
using UnityEditor;
using Systems.Inventory;

namespace Editor
{
    public class CardGeneratorEditor : EditorWindow
    {
        [MenuItem("Tools/Generate Deck")]
        public static void ShowWindow()
        {
            GetWindow<CardGeneratorEditor>("Card Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Card Generator", EditorStyles.boldLabel);

            if (GUILayout.Button("Generate Deck"))
            {
                
                CardGenerator.GenerateDeck();
            }

            if (GUILayout.Button("Print Deck"))
            {
                CardGenerator.PrintDeck();
            }
        }
    }
}
