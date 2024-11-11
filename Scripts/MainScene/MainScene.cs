using Godot;
using ResultManager;
using System;

public partial class MainScene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private GameManager.GameManager gameInstance;
	private MainSceneUIScript UIScript;
	public override void _Ready()
	{
		// Need some sort of transition effect here
		gameInstance = GameManager.GameManager.Instance;
		if (gameInstance.GameState == "continue") {
			// Import Save Data
			// gameInstance.gameData.LoadSaveData();
		} else {
			// Grabs UI Elements
			UIScript = GetNode<MainSceneUIScript>("MainSceneUI");

			var newWordResult = SetNewWord();

			if (newWordResult.IsFailure) {
				throw new InvalidProgramException(newWordResult.Error);
			}
			
			// End Transition effect
		}
	}

	private Result SetNewWord() {
		var gameStartResult = gameInstance.StartGame();
		if (gameStartResult.IsFailure) {
			GD.Print(gameStartResult.Error);
			return Result.Failure($"Unable To Start Game: {gameStartResult.Error}");
		}

		var uiResult = SetGameUI();
		if (uiResult.IsFailure) {
			GD.Print(uiResult.Error);
			return Result.Failure($"Unable To Start Game: {uiResult.Error}");
		}

		return Result.Success();
	}

	private Result SetGameUI() {
		// set hp and score later
		// Add guessed letters later
		var category = UIScript.UpdateCategoryLabel($"Category: {gameInstance.mainWordGame.Category}");
		var feedback = UIScript.UpdateFeedbackLabel($"Feedback: {gameInstance.mainWordGame.CorrectLetters}");
		var inputFieldContraints = UIScript.SetInputConstraints(gameInstance.mainWordGame.CurrentWord.Length);

		return (category.IsFailure || feedback.IsFailure || inputFieldContraints.IsFailure) 
		? Result.Failure("Unable To Set UI") 
		: Result.Success();
	}

	public Result GuessSubmit(string guess) {
		var guessResult = gameInstance.GuessAttempt(guess);
		
		if (guessResult.IsFailure) {
			// Hp is 0
			// Play effects here?


			gameInstance.EndGame();
			return Result.Success("Game Ending");
		}

		if (guessResult.Value == null) {
			// Correct Guess
			// Play effects here?


			// Generate New Word and Apply
			var newWordResult = SetNewWord();

			if (newWordResult.IsFailure) {
				return Result.Failure(newWordResult.Error);
			}

			return Result.Success();
		}

		// set new feedback, clear inputfield.

		var feedback = UIScript.UpdateFeedbackLabel($"Feedback: {guessResult.Value}");
		UIScript.ClearInputField();

		if (feedback.IsFailure) {
			return feedback;
		}

		return Result.Success();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
