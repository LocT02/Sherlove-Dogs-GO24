using System;
using System.Collections.Generic;
using IMainWordGame;
using ResultManager;

namespace MainWordGameWIPNAME
{
    public class MainWordGame : _IMainWordGame {
        // Attributes
        private string CurrentWord;
        public string Category { get; set; } = "";
        public List<char> GuessedLetters { get; set; } = new List<char>();

        public MainWordGame() {
            
        }

        // Gets a word to start game
        public Result GetWord() {
            // Grab a category and word
            if (CurrentWord == "") { // idk some check 
                // set words
                CurrentWord = "";
                Category = "";

                return Result.Success();
            }

            return Result.Failure("Unable to grab category and word");
        }
        
        // Checks guess against current word
        public Result CheckGuess(string guess) {
            // Also probably not a string return type
            guess = guess.ToUpper();

            foreach (char letter in guess) {
                if (!GuessedLetters.Contains(letter)) {
                    GuessedLetters.Add(letter);
                }
            }

            if (guess == CurrentWord) {
                return Result.Success(); //some form of success
            }

            // Add feedback
            var feedback = GenerateFeedback(guess);

            return feedback;
        }

        // Probably not returning a string
        private Result GenerateFeedback(string guess) {
            // not a success
            // return something idk
            return Result.Failure("Not implemented");
        }

        // Reveals a letter from the current word from item use or something
        public Result<char> RevealLetter() {
            
            List<char> unrevealedLetters = new List<char>();

            foreach (char letter in CurrentWord) {
                if (!GuessedLetters.Contains(letter)) {
                    unrevealedLetters.Add(letter);
                }
            }

            return Result.Failure<char>("Not implemented");
        }
    }
}