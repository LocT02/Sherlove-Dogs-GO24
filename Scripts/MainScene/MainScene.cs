using Godot;
using ResultManager;
using System;

public partial class MainScene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private GameManager.GameManager gameInstance;
	private MainSceneUIScript UIScript;
	private PlayerMovement player;
	private LineEdit inputBox;
	private TextureProgressBar healthUI;
	private Label mainScoreUI;
	public override void _Ready()
	{
		// Transition effect here

		InitializeGame();
		gameInstance.setMainScene();
		gameInstance.setUIScript();
		LoadOrInitializeNewGame();

		var uiResult = SetGameUI();
		if (uiResult.IsFailure) {
			throw new InvalidProgramException(uiResult.Error);
		}

		// End Transition Effect
		GD.Print("MainScene Ready");
	}

	private void InitializeGame() {
		gameInstance = GameManager.GameManager.Instance;
		UIScript = GetNode<MainSceneUIScript>("MainSceneUI");
		player = GetNode<PlayerMovement>("Player");
		inputBox = GetNode<LineEdit>("%GuessInputField");
		healthUI = GetNode<TextureProgressBar>("/root/MainSceneNode/MainSceneUI/HealthUI/HealthBar");
		mainScoreUI = GetNode<Label>("/root/MainSceneNode/MainSceneUI/ScoreUI/ScoreBG/ScoreText");
	}

	private void LoadOrInitializeNewGame() {
		if (gameInstance.GameState == "continue") {
			gameInstance.gameData.LoadSaveData();
		} else {
			InitializeNewGameData();
		}
	}

	private void InitializeNewGameData() {
		gameInstance.gameData.Hp = 100;
		gameInstance.gameData.Score = 0;

		var newWordResult = SetNewWord();
		if (newWordResult.IsFailure) {
			throw new InvalidProgramException(newWordResult.Error);
		}
		
	}

	private Result SetNewWord() {
		var gameStartResult = gameInstance.StartGame();
		if (gameStartResult.IsFailure) {
			return Result.Failure($"Unable To Start Game: {gameStartResult.Error}");
		}
		var uiResult = SetGameUI();
		if (uiResult.IsFailure) {
			throw new InvalidProgramException(uiResult.Error);
		}
		return Result.Success();
	}

	public Result SetGameUI() {
		var category = UIScript.UpdateCategoryLabel($"{gameInstance.mainWordGame.Category}");
		var feedback = UIScript.UpdateFeedbackLabel(gameInstance.mainWordGame.CorrectLetters.ToArray());
		var inputFieldContraints = UIScript.SetInputConstraints(gameInstance.mainWordGame.CurrentWord.Length);
		UpdateHPUI(gameInstance.gameData.Hp);
		UpdateScoreUI(gameInstance.gameData.Score);

		var attachItems = UIScript.AttachItemsToButtons();
		


		return (category.IsFailure || feedback.IsFailure || inputFieldContraints.IsFailure || attachItems.IsFailure) 
		? Result.Failure("Unable To Set UI Component") 
		: Result.Success();
	}

	public Result GuessSubmit(string guess) {
		var guessResult = gameInstance.GuessAttempt(guess);

		if (guessResult.IsFailure) {
			// Hp is 0
			gameInstance.EndGame();
			return Result.Success("Game Ending");
		}

		if (guessResult.Value == null) {
			//Correct Guess
			gameInstance.PlaySFX("MainSceneNode", gameInstance.sfxPaths["HAPPY_DOG"]); // Play SFX for correct guess
			// Generate New Word and Apply
			var newWordResult = SetNewWord();

			if (newWordResult.IsFailure) {
				return Result.Failure(newWordResult.Error);
			}

			return Result.Success();
		}

		//Incorrect guess here

		// set new feedback, clear inputfield.
		var feedback = UIScript.UpdateFeedbackLabel(guessResult.Value);

		if (feedback.IsFailure) {
			return feedback;
		}

		return Result.Success();
	}

	public void UpdateHPUI(int hp){
		healthUI.Value = hp;
	}
	public void UpdateScoreUI(int score){
		mainScoreUI.Text = score.ToString();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
