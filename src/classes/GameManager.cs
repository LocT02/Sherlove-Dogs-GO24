using Result;
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

		public static Result<string> StartGame() {
			//Instantiate inventory, mainwordgame (possibly move this into game manager?),
			//whatever else we need for assets or variables
			//call GetWord()
			//switch scenes??
			return Result<string>.Success("Successfully started game with word: currentWord");
			
		}

		public Result<Error> SceneChanger(string scenePath) {

			var scene = ResourceLoader.Load<PackedScene>(scenePath);

			if (scenePath == "res://Scenes/MainScene/main_scene.tscn" && GetTree().CurrentScene.SceneFilePath.Contains("Minigames")) {
				// Current scene is mini-game, remove it to return to main game
				GetTree().CurrentScene.QueueFree();
				return Result<Error>.Success(Error.Ok,"Successfully Changed Scene");
			} else if (scenePath.Contains("Minigames")) {
				// Add the new scene as a child to the current scene
				try {
					GetTree().CurrentScene.AddChild(scene.Instantiate());
					return Result<Error>.Success(Error.Ok,"Successfully added child scene.");
				} catch (Exception) {
					return Result<Error>.Failure("Failed to add child scene", Error.Failed);
				}
			}

			Error attemptSceneChange = GetTree().ChangeSceneToPacked(scene);

			return attemptSceneChange == Error.Ok
			? Result<Error>.Success(attemptSceneChange,"Successfully Changed Scene")
			: Result<Error>.Failure($"Scene Change to {scenePath} failed.", attemptSceneChange);
		}

		public Result<int> EndGame() {
			// Do endgame stuff
			
			return Result<int>.Success(1, "Ended Game");
		}
	}
}
