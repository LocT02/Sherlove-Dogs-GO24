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
        private List<char> _CorrectLetters;

        public MainWordGame() {
            
        }

        public string CurrentWord {
            get { return _CurrentWord; }
            set {
                if ( value == null) {
                    throw new InvalidOperationException("Error: value cannot be null");
                }
                _CurrentWord = value.ToUpper();
                ConvertWordToList();
            }
        }

        public string Category {
            get { return _Category; }
            set {
                if ( value == null) {
                    throw new InvalidOperationException("Error: value cannot be null");
                }
                _Category = value.ToUpper();
            }
        }

        public List<char> CorrectLetters {
            get { return _CorrectLetters; }
            private set {
                _CorrectLetters = value;
            }
        }

        public Result ResetMainWordGame() {
            CurrentWord = "";
            Category = "";
            GuessedLetters.Clear();
            return Result.Success();
        }

        // Gets a word to start game
        public Result<Dictionary<string, string>> GetNewWord() {

            var data = GameDataManager.JsonToDictionary("res://Globals/Data/categories.json");

            if (data.IsFailure || data.Value.Count == 0) {
                return Result.Failure<Dictionary<string, string>>("Unable to load Word Data file.");
            }

            Random rand = new();

            List<string> categories = new();

            foreach (var key in data.Value.Keys) {
                categories.Add(key.ToString());
            }

            string selectedCategory = categories[rand.Next(categories.Count)];

            var words = (Godot.Collections.Array)data.Value[selectedCategory];

            string selectedWord = (string)words[rand.Next(words.Count)];

            return Result.Success(new Dictionary<string, string>{
                {"GeneratedCategory", selectedCategory},
                {"GeneratedWord", selectedWord}
                });
        }
        
        // Checks guess against current word
        public Result<char[]> CheckGuess(string guess) {
            // Also probably not a string return type
            guess = guess.ToUpper();

            foreach (char letter in guess) {
                if (!GuessedLetters.Contains(letter)) {
                    GuessedLetters.Add(letter);
                }
            }

            if (guess == CurrentWord) {
                // Returns null no feedback needed
                return Result.Success<char[]>(null);
            }

            var feedback = GenerateFeedback(guess);

            return Result.Success(feedback.Value);
        }

        private Result ConvertWordToList() {
            CorrectLetters = new List<char>(new char[CurrentWord.Length]);
            for (int i = 0; i < CurrentWord.Length; i++) {
                CorrectLetters[i] = '_';
            }
            return Result.Success();
        }

        // Probably not returning a string
        private Result<char[]> GenerateFeedback(string guess) {
            char[] feedback = new char[CurrentWord.Length];

            for (int i = 0; i < guess.Length; i++) {
                char guessedLetter = guess[i];

                if (guessedLetter == CurrentWord[i]) {
                    // Guessed Letter is in correct position
                    CorrectLetters[i] = guessedLetter;
                    feedback[i] = guessedLetter;
                } else if (CurrentWord.Contains(guessedLetter) && CorrectLetters[i] == '_') {
                    // Guessed Letter is in the word but not in correct position
                    feedback[i] = '-';
                } else {
                    // Wrong Letter
                    feedback[i] = '_';
                }
            }

            // Adds already correct letters to feedback from previous guesses
            for (int i = 0; i < CorrectLetters.Count; i++) {
                if (CorrectLetters[i] != '_') {
                    feedback[i] = CorrectLetters[i];
                }
            }

            return Result.Success(feedback);
        }

        public Result<int> CalculatePoints() {
            return Result.Success(0);
        }

        public Result<int> CalculateDamage() {
            return Result.Success(0);
        }

        // Reveals letters 
        public Result<char[]> RevealLetters(List<int> indicesToReveal) {

            if (indicesToReveal.Count == 0 || indicesToReveal == null) {
                return Result.Failure<char[]>("No Indices To Reveal");
            }
            
            for (int i = 0; i < indicesToReveal.Count; i++) {
                int index = indicesToReveal[i];
                if (index >= 0 && index < CurrentWord.Length) {
                    CorrectLetters[index] = CurrentWord[index];
                }
            }

            char[] feedback = GenerateFeedback(new string(CorrectLetters.ToArray())).Value;
            return Result.Success(feedback);
        }
    }
}