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
	private AnimatedSprite2D girlSprite;
	private AnimatedSprite2D dogSprite;
	private bool reaction_finished;
	public override void _Ready()
	{
		// Transition effect here

		InitializeGame();
		gameInstance.setMainScene();
		gameInstance.setUIScript();
		LoadOrInitializeNewGame();

		GD.Print($"Current HP: {gameInstance.gameData.Hp}");
		GD.Print($"Current Score: {gameInstance.gameData.Score}");

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
		girlSprite = GetNode<AnimatedSprite2D>("GirlAnimation");
		girlSprite.Play("Talk");
		dogSprite = GetNode<AnimatedSprite2D>("Player/AnimatedSprite2D");
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
			GD.Print(gameStartResult.Error);
			return Result.Failure($"Unable To Start Game: {gameStartResult.Error}");
		}
		var uiResult = SetGameUI();
		if (uiResult.IsFailure) {
			throw new InvalidProgramException(uiResult.Error);
		}
		return Result.Success();
	}

	public Result SetGameUI() {
		var category = UIScript.UpdateCategoryLabel($"Category: {gameInstance.mainWordGame.Category}");
		string feedbackString = string.Join("  ", gameInstance.mainWordGame.CorrectLetters);
		var feedback = UIScript.UpdateFeedbackLabel($"Feedback:  {feedbackString}");
		var inputFieldContraints = UIScript.SetInputConstraints(gameInstance.mainWordGame.CurrentWord.Length);
		Result attachItems = Result.Success();
		UpdateHPUI(gameInstance.gameData.Hp);
		UpdateScoreUI(gameInstance.gameData.Score);

		attachItems = UIScript.AttachItemsToButtons();
		


		return (category.IsFailure || feedback.IsFailure || inputFieldContraints.IsFailure || attachItems.IsFailure) 
		? Result.Failure("Unable To Set UI Component") 
		: Result.Success();
	}

	public Result GuessSubmit(string guess) {
		var guessResult = gameInstance.GuessAttempt(guess);
		GD.Print($"Current HP: {gameInstance.gameData.Hp}");

		if (guessResult.IsFailure) {
			// Hp is 0
			// Play effects here?
			gameInstance.EndGame();
			return Result.Success("Game Ending");
		}

		if (guessResult.Value == null) {
			girlSprite.Play("Happy");
			dogSprite.Play("HappyDog");
			// Correct Guess
			// Play effects here?
			GD.Print($"Current Score: {gameInstance.gameData.Score}");


			// Generate New Word and Apply
			var newWordResult = SetNewWord();

			if (newWordResult.IsFailure) {
				return Result.Failure(newWordResult.Error);
			}

			return Result.Success();
		}
		girlSprite.Play("Angry");
		dogSprite.Play("SadDog");
		// set new feedback, clear inputfield.
		string feedbackString = string.Join("  ", guessResult.Value);
		var feedback = UIScript.UpdateFeedbackLabel($"Feedback:  {feedbackString}");

		if (feedback.IsFailure) {
			return feedback;
		}

		return Result.Success();
	}
	
	private void OnGirlAnimationFinished() {
		girlSprite.Play("Talk");
	}
	
	private void OnDogAnimationFinished() {
		dogSprite.Play("IdleRight");
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
