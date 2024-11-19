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
		private Dictionary<char, int> letterCounts = new Dictionary<char, int>();

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
				FillHistogram();
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

		private void FillHistogram() {
			foreach (char letter in CurrentWord) {
				if (letterCounts.ContainsKey(letter)) {
					letterCounts[letter]++;
				} else {
					letterCounts[letter] = 1;
				}
			}
		}

		private Result<char[]> GenerateFeedback(string guess) {
			char[] feedback = new char[CurrentWord.Length];

			// First Pass - Correct Positions
			for (int i = 0; i < guess.Length; i++) {
				char guessedLetter = guess[i];

				if (guessedLetter == CurrentWord[i]) {
					// Correct position
					CorrectLetters[i] = guessedLetter; // Mark as correct in CorrectLetters
					feedback[i] = guessedLetter;       // Add to feedback
					letterCounts[guessedLetter]--;     // Decrement count for this letter
				}
			}

			// Second Pass - Misplaced Letters
			for (int i = 0; i < guess.Length; i++) {
				char guessedLetter = guess[i];

				// Skip already correctly guessed letters
				if (feedback[i] != '\0') {
					continue;
				}

				// Check if guessedLetter exists elsewhere in CurrentWord and has remaining occurrences
				if (letterCounts.ContainsKey(guessedLetter) && letterCounts[guessedLetter] > 0) {
					feedback[i] = '-';                 // Mark as misplaced
				} else {
					feedback[i] = '_';                 // Mark as incorrect
				}
			}

			// Final Step - Preserve previously correctly guessed letters
			for (int i = 0; i < CorrectLetters.Count; i++) {
				if (CorrectLetters[i] != '_') {
					feedback[i] = CorrectLetters[i];   // Preserve correct letters from past guesses
				}
			}

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
