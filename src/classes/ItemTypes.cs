using System;
using System.Collections.Generic;
using IItemsTypes;
using ResultManager;

namespace ItemTypes {

    public class ItemA : IItem {

        public string Name { get; }= "Bark of Love"; // Sample Name

        public Result<char[]> ApplyItem() {
            // Reveal all vowels
            List<int> indicesToReveal = new();
            var gameInstance = GameManager.GameManager.Instance.mainWordGame;
            
            for (int i = 0; i < gameInstance.CurrentWord.Length; i++) {
                if ("AEIOU".Contains(gameInstance.CurrentWord[i])) {
                    indicesToReveal.Add(i);
                }
            }

            return gameInstance.RevealLetters(indicesToReveal);
        }
    }

    public class ItemB : IItem {

        public string Name { get; } = "A Bone for Two"; // Sample Name
        
        public Result<char[]> ApplyItem() {
            // Reveal first and last letters
            List<int> indicesToReveal = new();
            var gameInstance = GameManager.GameManager.Instance.mainWordGame;

            // First and Last if unrevealed
            if (gameInstance.CorrectLetters[0] == '_') {
                indicesToReveal.Add(0);
            }

            if (gameInstance.CorrectLetters[gameInstance.CurrentWord.Length - 1] == '_') {
                indicesToReveal.Add(gameInstance.CurrentWord.Length - 1);
            }

            /* First and Last Not Already Revealed indices

            for (int i = 0; i < gameInstance.CorrectLetters.Count; i++)
            {
                if (gameInstance.CorrectLetters[i] == '_')
                {
                    indicesToReveal.Add(i);
                    break;
                }
            }

            for (int i = gameInstance.CorrectLetters.Count - 1; i >= 0; i--)
            {
                if (gameInstance.CorrectLetters[i] == '_')
                {
                    indicesToReveal.Add(i);
                    break;
                }
            }

            */

            return gameInstance.RevealLetters(indicesToReveal);
        }
    }

    public class ItemC : IItem {

        public string Name { get; } = "Pot Luck";
        public Result<char[]> ApplyItem() {
            // Randomly reveal up to 3 letters, reveals the remaining letters if unrevealed is 3 or less.
            Random rand = new();
            var gameInstance = GameManager.GameManager.Instance.mainWordGame;
            List<int> indicesToReveal = new();
            List<int> unrevealedIndices = new();
            int numLetters = 3;

            for (int i = 0; i < gameInstance.CorrectLetters.Count; i++) {
                if (gameInstance.CorrectLetters[i] == '_') {
                    unrevealedIndices.Add(i);
                }
            }

            if (unrevealedIndices.Count < 3) {
                numLetters = unrevealedIndices.Count;
            }

            for (int i = 0; i < numLetters; i++) {
                int randomIndex = rand.Next(unrevealedIndices.Count);
                indicesToReveal.Add(unrevealedIndices[randomIndex]);
                unrevealedIndices.RemoveAt(randomIndex);
            }
            
            return gameInstance.RevealLetters(indicesToReveal);
        }
    }
}