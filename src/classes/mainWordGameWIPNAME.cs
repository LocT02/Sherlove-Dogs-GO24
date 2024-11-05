using Godot;
using System;
using System.Collections.Generic;
using Result;

namespace mainWordGameWIPNAME
{
    public class MainWordGameWIPNAME : Node
    {
        // Attributes
        private string currentWord;
        public List<char> guessedLetters;

        // Gets a word to start game
        public Result<string> getWord()
        {
            return null;
        }
        
        // Checks guess against current word
        public Result<string> checkGuess(string guess)
        {
            // Also probably not a string return type
            guess = guess.ToUpper();

            foreach (char letter in guess)
            {
                if (!guessedLetters.Contains(letter))
                {
                    guessedLetters.Add(letter);
                }
            }

            if (guess == currentWord) 
            {
                return Result.Success("U Won or something"); //some form of success
            }

            // Add feedback
            string feedback = generateFeedback(guess);

            return Result.Success(feedback);
        }

        // Probably not returning a string
        private Result<string> generateFeedback(string guess)
        {
            // not a success
            // return something idk
            return Result.Failure("Not implemented");
        }


        // Reveals a letter from the current word from item use or something
        public Result<char> revealLetter()
        {
            
            List<char> unrevealedLetters = new List<char>();

            foreach (char letter in currentWord)
            {
                if (!guessedLetters.Contains(letter))
                {
                    unrevealedLetter.Add(letter)
                }
            }

            return Result.Failure("Not implemented");
        }
    }
    
    
}