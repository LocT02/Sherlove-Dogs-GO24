using Result;
using InventoryManager;
using MainWordGameWIPNAME;
using IGameManager;
using Godot;
using System;

namespace GameManager {
    public partial class GameManager : Node, IGameInstance {
        // Attributes
        public int HP { get; set; }
        public int Score { get; set; }
        public Inventory Inventory { get; set; }
        public override void _Ready()
        {
            //read save data into references
            //initialize all data structures before entering game
            GD.Print("Successfully started GameManager.");
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
            } else if (scenePath == "" && GetTree().CurrentScene.SceneFilePath == "") {
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

        //Hp check function

        //end game function

        public Result<int> TakeDamage(int damage) {
            // fix logic // sample logic
            if (HP > damage) {
                HP -= damage;
                return Result<int>.Success(HP);
            }

            return Result<int>.Failure("Take Damage Not implemented");
        }


        // change score
        public Result<int> ChangeScore(int increment) {
            // sample logic fix later
            if (Score > 0) {
                Score += increment;
                return Result<int>.Success(Score);
            }

            return Result<int>.Failure("Change Score Not implemented");

        }

    }
}