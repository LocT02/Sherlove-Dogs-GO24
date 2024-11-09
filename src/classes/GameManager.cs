using Result;
using MainWordGameWIPNAME;
using IGameManager;
using Godot;
using System;
using GameData;

namespace GameManager {
    public partial class GameManager : Node, IGameInstance {


        public override void _Ready()
        {
            //read save data into references
            //initialize all data structures before entering game
            GameData.GameData gameData = new GameData.GameData();
            MainWordGame mainWordGame = new MainWordGame();
            GD.Print("Successfully started GameManager.");
            GameManager.StartGame();
        }

        public static Result<string> StartGame() {
            //Instantiate inventory, mainwordgame (possibly move this into game manager?),
            //whatever else we need for assets or variables
            //call GetWord()
            //switch scenes??
            return Result<string>.Success("Successfully started game with word: currentWord");
            
        }

        public Result<Error> SceneChanger(string scenePath, bool deleteScene) {
            var scene = ResourceLoader.Load<PackedScene>(scenePath);

            if (deleteScene) {
                // Instantly delete the current scene and switch
                Error attemptSceneChange = GetTree().ChangeSceneToPacked(scene);
                return attemptSceneChange == Error.Ok
                ? Result<Error>.Success(attemptSceneChange,"Successfully Changed Scene")
                : Result<Error>.Failure($"Scene Change to {scenePath} failed.", attemptSceneChange);
            } else if (scenePath == "res://Scenes/MainScene/main_scene.tscn" && GetTree().CurrentScene.SceneFilePath == "") {
                // Current scene is mini-game, remove it to return to main game
                GetTree().CurrentScene.QueueFree();
                return Result<Error>.Success(Error.Ok,"Successfully Changed Scene");
            } else {
                // Add the new scene as a child to the current scene
                try {
                    GetTree().CurrentScene.AddChild(scene.Instantiate());
                    return Result<Error>.Success(Error.Ok,"Successfully added child scene.");
                } catch (Exception e) {
                    return Result<Error>.Failure("Failed to add child scene", Error.Failed);
                }
            }
        }

        public Result<int> EndGame() {
            // Do endgame stuff
            
            return Result<int>.Success(1, "Ended Game");
        }
    }
}