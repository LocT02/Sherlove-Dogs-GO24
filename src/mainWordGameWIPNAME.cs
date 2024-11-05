using Godot;
using System;
using System.Collections.Generic;

namespace mainWordGameWIPNAME
{
    public class MainWordGameWIPNAME : Node
    {
        // Attributes
        private string currentWord;
        public List<char> guessedLetters;

        // Gets a word to start game
        public string getWord()
        {
            return null;
        }
        
        // Checks guess against current word
        public string checkGuess(string guess)
        {
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
                return null; //some form of success
            }

            // Add feedback
            string feedback = generateFeedback(guess);

            return feedback;
        }

        private string generateFeedback(string guess)
        {
            // not a success
            // return something idk
            return null;
        }


        // Reveals a letter from the current word from item use or something
        public char revealLetter()
        {
            
            List<char> unrevealedLetters = new List<char>();

            foreach (char letter in currentWord)
            {
                if (!guessedLetters.Contains(letter))
                {
                    unrevealedLetter.Add(letter)
                }
            }

            return null;
        }
    }
    
    
}