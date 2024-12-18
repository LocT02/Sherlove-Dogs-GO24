using ResultManager;
using MainWordGameWIPNAME;
using IGameManager;
using Godot;
using System;
using GameData;
using System.Threading.Tasks;
using SceneTransitionManager;
using System.Collections.Generic;
using System.Numerics;
using GodotPlugins.Game;
using System.IO;

namespace GameManager
{
	public partial class GameManager : Node, IGameInstance {

		public static GameManager Instance { get; private set; }
		public GameDataManager gameData { get; private set; }
		public MainWordGame mainWordGame { get; private set; }
		public readonly Dictionary<string, string> scenePaths = new Dictionary<string, string> {
			{"MAIN_SCENE", "res://Scenes/MainScene/main_scene.tscn"},
			{"MEMORY_SCENE", "res://Scenes/Minigames/Memory/memory.tscn"},
			{"TRANSITION_SCENE", "res://Scenes/scene_transition.tscn"},
			{"CATCH_THE_BONE_SCENE", "res://Scenes/Minigames/CatchTheTreat/catch_the_treat.tscn"},
			{"DDR_SCENE", "res://Scenes/Minigames/DDR/ddr.tscn"},
			{"GAME_OVER", "res://Scenes/GameOver/game_over.tscn"},
			{"MAIN_MENU", "res://Scenes/MainMenu/main_menu.tscn"}
		};
		public readonly Dictionary<string, string> sfxPaths = new Dictionary<string, string> {
			{"BUTTON_CLICKED", "res://Assets/Audio/SFX/Button Clicked.wav"},
			{"BUTTON_HOVER", "res://Assets/Audio/SFX/Button_Hover.wav"},
			{"DOGGY_DOOR", "res://Assets/Audio/SFX/Doggy Door.wav"},
			{"HAPPY_DOG", "res://Assets/Audio/SFX/Happy Dog.wav"},
			{"SAD_DOG", "res://Assets/Audio/SFX/Dog_Sad.wav"},
			{"MEMORY_CORRECT", "res://Assets/Audio/SFX/Memory Correct.wav"},
			{"MEMORY_INCORRECT", "res://Assets/Audio/SFX/Memory Incorrect.wav"},
			{"MINIGAME_LOSS", "res://Assets/Audio/SFX/Minigame Loss.wav"},
			{"TOMATO_CAUGHT", "res://Assets/Audio/SFX/Tomato Caught.wav"},
			{"TREAT_CAUGHT", "res://Assets/Audio/SFX/Treat Caught.wav"},
			{"MINIGAME_WIN", "res://Assets/Audio/SFX/Win.wav"}
		};

		private SceneTransition sceneTransition;
		public MainSceneUIScript UIScript;
		private Node miniGameNode;
		private MainScene mainScene;
		public string GameState { get; set; } = "newgame";
		public bool allowMinigameEntry { get; set; } = true;
		public bool allowMinigameStart { get; set; } = false;
		private AudioStreamPlayer AudioPlayer;

		public override void _Ready()
		{
			//read save data into references
			//initialize all data structures before entering game
			Instance = this;
			mainWordGame = new MainWordGame();
			gameData = new GameDataManager();
			var sceneTranstionPacked = ResourceLoader.Load<PackedScene>(scenePaths["TRANSITION_SCENE"]);
			sceneTransition = (SceneTransition)sceneTranstionPacked.Instantiate();
			sceneTransition.Layer = 999;
			CallDeferred("AddSceneTransitionToRoot");

			GD.Print("GameManager Ready");
		}
		public void setMainScene(){
			mainScene = GetNode<MainScene>("/root/MainSceneNode");
		}
		public void setUIScript(){
			UIScript = GetNode<MainSceneUIScript>("/root/MainSceneNode/MainSceneUI");
		}
		public Result StartGame() {
			mainWordGame.ResetMainWordGame();
			GD.Print("Starting Game");

			allowMinigameEntry = true;

			var newGame = mainWordGame.GetNewWord();

			if (newGame.IsFailure || newGame.Value.Count != 2) {
				return Result.Failure(newGame.Error);
			}
			mainWordGame.Category = newGame.Value["GeneratedCategory"];
			mainWordGame.CurrentWord = newGame.Value["GeneratedWord"];
			GD.Print($"Successfully started game with the word {newGame.Value["GeneratedWord"]}");
			return Result.Success();
		}

		private async Task<Result> PlayFadeTransition(Func<Task<Result>> action, bool leaveminigame = false) {
			AddSceneTransitionToRoot();

			if (leaveminigame) {
				var fadeInResult = await sceneTransition.StartFadeInSlow();
				if (fadeInResult.IsFailure) {
					return fadeInResult;
				}
			} else {
				var fadeInResult = await sceneTransition.StartFadeInFast();
				if (fadeInResult.IsFailure) {
					return fadeInResult;
				}
			}

			var actionResult = await action();
			if (actionResult.IsFailure) {
				return actionResult;
			}

			return Result.Success();
		}

		private void AddSceneTransitionToRoot()
		{
			// Add SceneTransition to the root if it's not already added
			if (sceneTransition.GetParent() == null)
			{
				GetTree().Root.AddChild(sceneTransition);
			}
		}

		private Result LoadAndAddScene(string scenePath) {
			GD.Print($"Adding Scene {scenePath}");
			Godot.Vector2 MinigamePosition = new (2200,-1018);
			try {
				miniGameNode = ResourceLoader.Load<PackedScene>(scenePath).Instantiate();

				if (miniGameNode is Node2D node2DScene) {
					node2DScene.Position = MinigamePosition;
				} else if (miniGameNode is Control control) {
					control.Position = MinigamePosition;
				}

				GetTree().CurrentScene.AddChild(miniGameNode);
				return Result.Success();
			} catch (Exception e) {
				return Result.Failure(e.Message);
			}
		}

		private Result LoadAndSwitchScene(string scenePath) {
			var scene = ResourceLoader.Load<PackedScene>(scenePath);
			Error attemptSceneChange = GetTree().ChangeSceneToPacked(scene);

			return attemptSceneChange == Error.Ok
			? Result.Success()
			: Result.Failure($"Scene Change to {scenePath} failed.");
		}

		public async Task<Result> SceneChanger(string changeToScenePath) {
			List<string> miniGameNames = new() {
				"Memory_Minigame",
				"CatchTheBone",
				"DDR"
			};
			bool isMinigame = false;

			foreach (string minigame in miniGameNames) {
				if (GetTree().CurrentScene.HasNode(minigame)) {
					isMinigame = true;
				}
			}

			if (changeToScenePath == "res://Scenes/MainScene/main_scene.tscn" && isMinigame) {
				// Remove minigame 
				var result =  await PlayFadeTransition(async () => {
					await Task.Run(() => miniGameNode?.QueueFree());
					miniGameNode = null;
					mainScene.SetGameUI();
					return Result.Success();
				}, true);
				CameraAndFade();
				return result;
			} else if (changeToScenePath.Contains("Minigames")) {
				// Add the new scene as a child to the current scene
				var result = await PlayFadeTransition(() => Task.FromResult(LoadAndAddScene(changeToScenePath)));
				CameraAndFade();
				return result;
			}

			var mainSceneResult = await PlayFadeTransition(() => Task.FromResult(LoadAndSwitchScene(changeToScenePath)));
			
			if (mainSceneResult.IsSuccess) {
				sceneTransition.ReverseFadeInFast();
				return mainSceneResult;
			}

			return Result.Failure("SceneChanger Failed To Operate");
		}

		private void CameraAndFade() {
			UIScript.ChangeCamera();
			sceneTransition.ReverseFadeInFast();
		}

		public Result<char[]> GuessAttempt(string guess) {
			// Returns a list of characters:
			// Apple is the word
			// Guess is Apart
			// Will return : ['A','P','_','_','_']
			// '_' = wrong letter || '-' = wrong position
			// mainWordGame contains GuessedLetters for letters to display what they guessed.

			var result = mainWordGame.CheckGuess(guess);

			// null means no feedback = correct guess
			if (result.Value == null) {
				gameData.ChangeScore(1000);
				mainScene.UpdateScoreUI(gameData.Score);
				// Returns to main Game Scene, do effects there, then call startGame again from main Game Scene?
				return Result.Success<char[]>(null);
			}

			if (gameData.ChangeHp(-10).Value <= 0) {
				return Result.Failure<char[]>("Hp0");
			}
			mainScene.UpdateHPUI(gameData.Hp);
			// Returns feedback
			return result;
		}

		public Result EndGame() {
			// called from Main Game Scene
			LoadAndSwitchScene("res://Scenes/GameOver/game_over.tscn");
			return Result.Success();
		}

		public Result MidGameSave() {
			// When the user wants to save Mid Game
			return Result.Success();
		}

		public void PlaySFX(string sceneRoot, string soundPath){
			AudioPlayer = GetNode<AudioStreamPlayer>($"/root/{sceneRoot}/SFXPlayer");
			AudioPlayer.Stream = GD.Load<AudioStream>(soundPath);
			AudioPlayer.Play();
		}
	}
}
