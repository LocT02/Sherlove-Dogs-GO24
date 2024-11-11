using Godot;
using ResultManager;
using System;
using System.Dynamic;

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
		}

		// Grabs UI Elements
		UIScript = GetNode<MainSceneUIScript>("MainSceneUI");

		var gameStartResult = gameInstance.StartGame();
		if (gameStartResult.IsFailure) {
			GD.Print(gameStartResult.Error);
			throw new InvalidProgramException($"Unable To Start Game: {gameStartResult.Error}");
		}

		var uiResult = SetGameUI();
		if (uiResult.IsFailure) {
			GD.Print(uiResult.Error);
			throw new InvalidProgramException($"Unable To Start Game: {uiResult.Error}");
		}
		
		// End Transition effect
	}

	private Result SetGameUI() {
		// set hp and score later
		var category = UIScript.UpdateCategoryLabel($"Category: {gameInstance.mainWordGame.Category}");
		var feedback = UIScript.UpdateFeedbackLabel($"Feedback: {gameInstance.mainWordGame.CorrectLetters}");

		return (category.IsFailure || feedback.IsFailure) ? Result.Failure("Unable To Set UI") : Result.Success();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
