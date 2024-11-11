using ResultManager;
using MainWordGameWIPNAME;
using IGameManager;
using Godot;
using System;
using GameData;

namespace GameManager
{
	public partial class GameManager : Node, IGameInstance {

		public static GameManager Instance { get; private set; }
		public GameDataManager gameData { get; private set; }
		public MainWordGame mainWordGame { get; private set; }
		public string GameState { get; set; } = "newgame";

		public override void _Ready()
		{
			//read save data into references
			//initialize all data structures before entering game
			Instance = this;
			gameData = new GameDataManager();
			mainWordGame = new MainWordGame();
			GD.Print("Successfully started GameManager.");
		}

		public Result StartGame() {
			mainWordGame.ResetMainWordGame();
			var newGame = mainWordGame.GetNewWord();
			mainWordGame.Category = newGame.Value["GeneratedCategory"];
			mainWordGame.CurrentWord = newGame.Value["GeneratedWord"];
			return Result.Success();
		}

		public Result SceneChanger(string scenePath) {

			var scene = ResourceLoader.Load<PackedScene>(scenePath);

			if (scenePath == "res://Scenes/MainScene/main_scene.tscn" && GetTree().CurrentScene.SceneFilePath.Contains("Minigames")) {
				// Current scene is mini-game, remove it to return to main game
				GetTree().CurrentScene.QueueFree();
				return Result.Success();
			} else if (scenePath.Contains("Minigames")) {
				// Add the new scene as a child to the current scene
				try {
					GetTree().CurrentScene.AddChild(scene.Instantiate());
					return Result.Success();
				} catch (Exception e) {
					return Result.Failure(e.Message);
				}
			}

			Error attemptSceneChange = GetTree().ChangeSceneToPacked(scene);

			return attemptSceneChange == Error.Ok
			? Result.Success()
			: Result.Failure($"Scene Change to {scenePath} failed.");
		}

		public Result<char[]> GuessAttempt(string guess) {
			// Returns a list of characters:
			// Apple is the word
			// Guess is Apart
			// Will return : ['A','P','_','_','_']
			// '_' = wrong letter || '-' = wrong position
			// mainWordGame contains GuessedLetters for letters to display what they guessed.

			if (guess == null) {
				return Result.Failure<char[]>("Guess is null");
			}

			var result = mainWordGame.CheckGuess(guess);

			// null means no feedback = correct guess
			if (result.Value == null) {
				gameData.ChangeScore(1000);
				// Returns to main Game Scene, do effects there, then call startGame again from main Game Scene?
				return Result.Success<char[]>(null);
			}

			if (gameData.ChangeHp(-10).Value <= 0) {
				return Result.Failure<char[]>("Player Died Time to Get Disowned.");
			}

			// Returns feedback
			return Result.Success(result.Value);
		}

		public Result EndGame() {
			// called from Main Game Scene
			// Save high score
			
			return Result.Success();
		}

		public Result MidGameSave() {
			// When the user wants to save Mid Game
			return Result.Success();
		}
	}
}
