using Godot;
using System;
using System.Collections.Generic;
using Result;

namespace MainWordGameWIPNAME
{
    public class MainWordGame
    {
        // Attributes
        private string CurrentWord;
        public List<char> GuessedLetters;

        // Gets a word to start game
        public Result<string> GetWord()
        {
            return null;
        }
        
        // Checks guess against current word
        public Result<string> CheckGuess(string guess)
        {
            // Also probably not a string return type
            guess = guess.ToUpper();

            foreach (char letter in guess)
            {
                if (!GuessedLetters.Contains(letter))
                {
                    GuessedLetters.Add(letter);
                }
            }

            if (guess == CurrentWord) 
            {
                return Result.Success("U Won or something"); //some form of success
            }

            // Add feedback
            Result<string> feedback = GenerateFeedback(guess);

            return Result.Success(feedback);
        }

        // Probably not returning a string
        private Result<string> GenerateFeedback(string guess)
        {
            // not a success
            // return something idk
            return Result.Failure("Not implemented");
        }


        // Reveals a letter from the current word from item use or something
        public Result<char> RevealLetter()
        {
            
            List<char> unrevealedLetters = new List<char>();

            foreach (char letter in CurrentWord)
            {
                if (!GuessedLetters.Contains(letter))
                {
                    unrevealedLetters.Add(letter);
                }
            }

            return Result.Failure("Not implemented");
        }
    }
    
    
}