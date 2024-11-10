using System.Collections.Generic;
using IMainWordGame;
using ResultManager;
using GameData;
using System;

namespace MainWordGameWIPNAME
{
    public class MainWordGame : _IMainWordGame {

        private string _CurrentWord;
        private string _Category;
        public List<char> GuessedLetters { get; set; } = new List<char>();

        public MainWordGame() {
            
        }

        public string CurrentWord {
            get { return _CurrentWord; }
            set {
                if ( value == null) {
                    throw new InvalidOperationException("Error: value cannot be null");
                }
                _CurrentWord = value;
            }
        }

        public string Category {
            get { return _Category; }
            set {
                if ( value == null) {
                    throw new InvalidOperationException("Error: value cannot be null");
                }
                _Category = value;
            }
        }

        // Gets a word to start game
        public Result GetNewWord() {

            var data = GameDataManager.JsonToDictionary("res://Globals/Data/categories.json");

            if (data.IsFailure || data.Value.Count == 0) {
                return Result.Failure("Unable to load Word Data file.");
            }

            Random rand = new();

            List<string> categories = new();

            foreach (var key in data.Value.Keys) {
                categories.Add(key.ToString());
            }

            string selectedCategory = categories[rand.Next(categories.Count)];

            var words = (Godot.Collections.Array)data.Value[selectedCategory];

            string selectedWord = (string)words[rand.Next(words.Count)];

            return Result.Success(new {Category = selectedCategory, CurrentWord = selectedWord});
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
                return Result.Success();
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

        public Result<int> CalculateScore() {
            return Result.Success(1);
        }

        public Result<int> CalculateDamage() {
            return Result.Success(1);
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