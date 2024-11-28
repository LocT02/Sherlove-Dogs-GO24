using System;
using System.Collections.Generic;
using System.Linq;
using IItemsTypes;
using ResultManager;

namespace ItemTypes {

    public class ItemA : IItem {

        public string Name { get; } = "Bark of Love"; // Sample Name
        public bool Upgraded { get; set; } = false;
        public string[] ImgFilePath { get; set; } = new string[]{"res://Assets/Art/Items/Bark of Love 1.svg","res://Assets/Art/Items/Bark of Love 2.svg"};

        public Result<char[]> ApplyItem() {
            // Reveal half vowels, upgraded reveal all vowels
            List<int> indicesToReveal = new();
            var gameInstance = GameManager.GameManager.Instance.mainWordGame;
            
            for (int i = 0; i < gameInstance.CurrentWord.Length; i++) {
                if ("AEIOU".Contains(gameInstance.CurrentWord[i])) {
                    indicesToReveal.Add(i);
                }
            }

            if (indicesToReveal.Count == 0) {
                return Result.Failure<char[]>("No vowels found to reveal.");
            }

            if (!Upgraded) {
                Random rand = new();
                int vowelsToReveal = indicesToReveal.Count / 2;

                vowelsToReveal = Math.Max(1, vowelsToReveal);

                List<int> vowelsToRandomlyReveal = new();
                while (vowelsToRandomlyReveal.Count < vowelsToReveal) {
                    int randomIndex = rand.Next(indicesToReveal.Count);
                    if (!vowelsToRandomlyReveal.Contains(indicesToReveal[randomIndex])) {
                        vowelsToRandomlyReveal.Add(indicesToReveal[randomIndex]);
                    }
                }
                
                indicesToReveal = vowelsToRandomlyReveal;
            }

            return gameInstance.RevealLetters(indicesToReveal);
        }
    }

    public class ItemB : IItem {

        public string Name { get; } = "A Bone for Two"; 
        public bool Upgraded { get; set; } = false;
        public string[] ImgFilePath { get; set; } = new string[]{"res://Assets/Art/Items/Bone for Two 1.svg","res://Assets/Art/Items/Bone for Two 2.svg"};
        
        public Result<char[]> ApplyItem() {
            // Reveal first and last letters
            List<int> indicesToReveal = new();
            var gameInstance = GameManager.GameManager.Instance.mainWordGame;

            if (gameInstance.CorrectLetters.All(c => c != '_')) {
            // If the word is fully revealed, return a failure or handle accordingly
                return Result.Failure<char[]>("All letters are already revealed.");
            }

            // First and Last if unrevealed
            if (!Upgraded) {
                //Grabs first letter
                if (gameInstance.CorrectLetters[0] == '_') {
                    indicesToReveal.Add(0);
                }
                //Grabs last letter
                if (gameInstance.CorrectLetters[gameInstance.CurrentWord.Length - 1] == '_') {
                    indicesToReveal.Add(gameInstance.CurrentWord.Length - 1);
                }

            } else {
                // Grabs first non revealed letter
                for (int i = 0; i < gameInstance.CorrectLetters.Count; i++)
                {
                    if (gameInstance.CorrectLetters[i] == '_')
                    {
                        indicesToReveal.Add(i);
                        break;
                    }
                }
                // Grabs last non revealed letter
                for (int i = gameInstance.CorrectLetters.Count - 1; i >= 0; i--)
                {
                    if (gameInstance.CorrectLetters[i] == '_')
                    {
                        if (!indicesToReveal.Contains(i)) {
                            indicesToReveal.Add(i);
                        }
                        break;
                    }
                }
            }

            return gameInstance.RevealLetters(indicesToReveal);
        }
    }

    public class ItemC : IItem {

        public string Name { get; } = "Neighbor's Frisbee";
        public bool Upgraded { get; set; } = false;
        public string[] ImgFilePath { get; set; } = new string[]{"res://Assets/Art/Items/Neighbors Frisbee 1.svg","res://Assets/Art/Items/Neighbors Frisbee 2.svg"};

        public Result<char[]> ApplyItem() {
            // Randomly reveal up to 2 letters, if upgraded up to 4 if there is enough letters
            Random rand = new();
            var gameInstance = GameManager.GameManager.Instance.mainWordGame;
            List<int> indicesToReveal = new();
            List<int> unrevealedIndices = new();
            int numLetters = 2;

            if (Upgraded) {
                numLetters = 4;
            }

            for (int i = 0; i < gameInstance.CorrectLetters.Count; i++) {
                if (gameInstance.CorrectLetters[i] == '_') {
                    unrevealedIndices.Add(i);
                }
            }

            if (unrevealedIndices.Count == 0) {
                return Result.Failure<char[]>("No letters left to reveal.");
            }

            if (unrevealedIndices.Count < 2) {
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