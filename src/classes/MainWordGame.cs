using System;
using System.Collections.Generic;
using IMainWordGame;
using Result;

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
        public Result<Tuple<string, string>> GetWord() {
            // Grab a category and word
            if (CurrentWord == "") { // idk some check 
                // set words
                CurrentWord = "";
                Category = "";

                //return
                Tuple<string, string> stringPair = new Tuple<string, string>(Category, CurrentWord);
                return Result<Tuple<string,string>>.Success(stringPair);
            }

            return Result<Tuple<string,string>>.Failure("Unable to grab category and word");
        }
        
        // Checks guess against current word
        public Result<string> CheckGuess(string guess) {
            // Also probably not a string return type
            guess = guess.ToUpper();

            foreach (char letter in guess) {
                if (!GuessedLetters.Contains(letter)) {
                    GuessedLetters.Add(letter);
                }
            }

            if (guess == CurrentWord) {
                return Result<string>.Success("U Won or something"); //some form of success
            }

            // Add feedback
            Result<string> feedback = GenerateFeedback(guess);

            return feedback;
        }

        // Probably not returning a string
        private Result<string> GenerateFeedback(string guess) {
            // not a success
            // return something idk
            return Result<string>.Failure("Not implemented");
        }

        // Reveals a letter from the current word from item use or something
        public Result<char> RevealLetter() {
            
            List<char> unrevealedLetters = new List<char>();

            foreach (char letter in CurrentWord) {
                if (!GuessedLetters.Contains(letter)) {
                    unrevealedLetters.Add(letter);
                }
            }

            return Result<char>.Failure("Not implemented");
        }
    }
}