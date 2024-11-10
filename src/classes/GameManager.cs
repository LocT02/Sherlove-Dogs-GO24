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
			StartGame();
		}

		public static Result StartGame() {
			//whatever else we need for assets or variables
			//call GetWord()
			//switch scenes??
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

        public Result GuessAttempt(string guess) {

            if (guess == null) {
                return Result.Failure("Guess is null");
            }

            var result = mainWordGame.CheckGuess(guess);

            if (result.IsSuccess) {
                // Guessed the word correctly
                // Calculate score
                // Add Score
                // Reset MainWordGame
                // Grab New Word

                return Result.Success();
            }

            // wrong guess
            // calculate damage
            // change hp
            if (gameData.Hp <= 0) {
				EndGame();
			}

            return Result.Success();
        }

        public Result EndGame() {
			// Do endgame stuff
			
			return Result.Success();
		}
	}
}
