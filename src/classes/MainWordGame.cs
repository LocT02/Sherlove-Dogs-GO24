using System.Collections.Generic;
using IMainWordGame;
using ResultManager;
using System;
using System.IO;
using Godot;
using GameData;

namespace MainWordGameWIPNAME
{
	public class MainWordGame : _IMainWordGame {

		private string _CurrentWord;
		private string _Category;
		public List<char> GuessedLetters { get; set; } = new List<char>();
		private List<char> _CorrectLetters;

		public MainWordGame() {
			GD.Print("Main Game Ready");
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
			set {
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

			string categoryFilePath = "Globals/Data/categories.json";

			if (!File.Exists(categoryFilePath)) {
				return Result.Failure<Dictionary<string, string>>($"Category Does Not Exist at: {categoryFilePath}");
			}

			var data = GameDataManager.JsonToDictionary(categoryFilePath);

			if (data.IsFailure || data.Value.Count == 0) {
				return Result.Failure<Dictionary<string, string>>("Unable to load Word Data file.");
			}

			var innerDict = data.Value["categories"].As<Godot.Collections.Dictionary>();

			Random rand = new();

			List<string> categories = new();

			foreach (var key in innerDict.Keys) {
				categories.Add(key.ToString());
			}

			string selectedCategory = categories[rand.Next(categories.Count)];

			var words = (Godot.Collections.Array)innerDict[selectedCategory];

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

			return feedback;
		}

		private Result ConvertWordToList() {
			CorrectLetters = new List<char>(new char[CurrentWord.Length]);
			for (int i = 0; i < CurrentWord.Length; i++) {
				if (CurrentWord[i] != ' '){
					CorrectLetters[i] = '_';
				} else{
					CorrectLetters[i] = ' ';
				}
			}
			return Result.Success();
		}

		// Probably not returning a string
		private Result<char[]> GenerateFeedback(string guess)
		{
			char[] feedback = new char[CurrentWord.Length];
			int[] currentWordLetterCount = new int[26];  // For counting occurrences of each letter in the word
			int[] guessLetterCount = new int[26];  // For counting occurrences of each letter in the guess
			int[] correctLetterCount = new int[26];  // To track how many times a letter has been correctly guessed
			bool[] usedInFeedback = new bool[CurrentWord.Length];  // Tracks if a letter is already marked as correct

			// First pass: Find exact matches and count occurrences of each letter
			for (int i = 0; i < guess.Length; i++) {
				char guessedLetter = guess[i];
				currentWordLetterCount[CurrentWord[i] - 'a']++;  // Track letter count in CurrentWord
				guessLetterCount[guessedLetter - 'a']++;  // Track letter count in guess

				if (guessedLetter == CurrentWord[i]) {
					// Guessed letter is correct and in the correct position
					CorrectLetters[i] = guessedLetter;
					feedback[i] = guessedLetter;
					usedInFeedback[i] = true;  // Mark as used for exact match
					correctLetterCount[guessedLetter - 'a']++;  // Increment correct letter count
				} else {
					feedback[i] = '_';  // Mark as wrong initially
				}
			}

			// Second pass: Handle misplaced letters
			for (int i = 0; i < guess.Length; i++) {
				char guessedLetter = guess[i];

				if (feedback[i] == '_')  { // Only process letters that haven't been placed correctly
					// The guessed letter must be in the word and not have been used up already
					if (currentWordLetterCount[guessedLetter - 'a'] > correctLetterCount[guessedLetter - 'a']) {
						// Check if this guessed letter is already placed correctly
						bool isMisplaced = false;

						// Count occurrences of the letter that have already been correctly placed
						for (int j = 0; j < i; j++) {
							if (guess[j] == guessedLetter && usedInFeedback[j]) {
								isMisplaced = true;
								break;
							}
						}

						if (isMisplaced) {
							feedback[i] = '-';  // Guessed letter is in the word but not in the correct position
							correctLetterCount[guessedLetter - 'a']++;  // Increment the correct letter count
						} else {
							feedback[i] = '_';  // Guessed letter is completely incorrect
						}
					} else {
						feedback[i] = '_';  // Guessed letter is completely incorrect
					}
				}
			}

			// Return the result
			return Result.Success(feedback);
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
