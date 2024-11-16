using Godot;
using System;
using InventoryManager;

public partial class CatchTheBone : Node2D
{
    private Timer spawnTimer;
    private Random random = new Random();
    // Path to your Bone scene file
    private PackedScene boneScene = (PackedScene)GD.Load("res://Scenes/Minigames/CatchTheTreat/Bone.tscn");
    private CTBPlayer player;
    private Timer gameDurationTimer;
    private const int gameTime = 60; //in seconds
    private const int WIN_SCORE = 2000;
    private const int MIN_SPEED = 150;
    private const int MAX_SPEED = 600;
    private const float MIN_SPAWN_TIME = 0.25f; //half a second
    private GameManager.GameManager gameManagerInstance;
	private Inventory inventoryInstance;

    private Vector2[] sizes = new[] { new Vector2(64, 64), new Vector2(96, 96), new Vector2(128, 128) };

    public override void _Ready()
    {
        gameManagerInstance = GameManager.GameManager.Instance;
        inventoryInstance = gameManagerInstance.gameData.Inventory;

        gameDurationTimer = GetNode<Timer>("GameplayTimer");
        gameDurationTimer.OneShot = true;
        gameDurationTimer.WaitTime = gameTime;
        gameDurationTimer.Timeout += gameOver; 

        player = GetNode<CTBPlayer>("Player");
        spawnTimer = new Timer();
        AddChild(spawnTimer);
        spawnTimer.WaitTime = 0.25f;
        spawnTimer.Timeout += SpawnBone;
        spawnTimer.Start();
    }

    private async void gameOver(){
        //if score < 750, lose
        //eif score >= 1000, upgraded item
        //else get item
        var score = player.GetScore();
        if(score < WIN_SCORE * 0.75){
            //lose
            return;
        }
        var item = inventoryInstance.SelectRandomItem(score >= WIN_SCORE);

        if(item.IsFailure) {
			GD.PushError(item.Error);
		}

		var result = inventoryInstance.AddItem(item.Value);
		if(result.IsFailure) {
			GD.PushError(result.Error);
		}
		//Show item get screen first before scene switch
		await gameManagerInstance.SceneChanger(gameManagerInstance.scenePaths["MAIN_SCENE"]);

    }
    
    private void SpawnBone()
    {
        spawnTimer.WaitTime = MIN_SPAWN_TIME + (float)random.NextDouble();
        var boneType = random.Next(0, 3);  // Choose random bone type
        var bone = (Bone)boneScene.Instantiate();  // Use Instantiate() to create an instance of the Bone scene

        // Initialize bone with type (0, 1, or 2 for points and texture)
        

        // Position the bone at a random x-position at the top of the screen
        float xPosition = (float)random.NextDouble() * (GetViewportRect().Size.X-64);
        bone.Position = new Vector2(xPosition, 0);
        bone.SetSpeed(random.Next(MIN_SPEED, MAX_SPEED));
        var meteorChance = random.Next(10);
        if(meteorChance == 1)
            bone.SetSpeed(2000);

        AddChild(bone);
        var boneSize = sizes[random.Next(sizes.Length)];
        bone.CallDeferred("Initialize", boneType, boneSize);
    }
}
