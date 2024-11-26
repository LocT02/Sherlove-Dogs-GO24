using Godot;
using System;
using InventoryManager;
using System.Linq;

public partial class CatchTheBone : Node2D
{
    private Timer spawnTimer;
    private Random random = new Random();
    // Path to your Bone scene file
    private PackedScene boneScene = (PackedScene)GD.Load("res://Scenes/Minigames/CatchTheTreat/Bone.tscn");
    private CTBPlayer player;
    private Timer gameDurationTimer;
    private const int GAME_TIME = 1; //in seconds
    private const int WIN_SCORE = -50;
    //Bone Parameters
    private const int MIN_SPEED = 100;
    private const int MAX_SPEED = 750;
    private const float MIN_SPAWN_TIME = 0.05f; //half a second
    private GameManager.GameManager gameManagerInstance;
	private Inventory inventoryInstance;
    private RichTextLabel timerLabel, scoreLabel, winLoseLabel;

    private Vector2[] sizes = new[] { new Vector2(64, 64), new Vector2(96, 96), new Vector2(128, 128) };

    public override void _Ready()
    {
        gameManagerInstance = GameManager.GameManager.Instance;
        inventoryInstance = gameManagerInstance.gameData.Inventory;

        timerLabel = GetNode<RichTextLabel>("TimerContainer/RichTextLabel");
        scoreLabel = GetNode<RichTextLabel>("ScoreContainer/RichTextLabel");
        winLoseLabel = GetNode<RichTextLabel>("WinLoseLabel");

        gameDurationTimer = GetNode<Timer>("GameplayTimer");
        gameDurationTimer.OneShot = true;
        gameDurationTimer.WaitTime = GAME_TIME;
        gameDurationTimer.Timeout += gameOver; 
        gameDurationTimer.Start();

        player = GetNode<CTBPlayer>("Player");
        spawnTimer = new Timer();
        AddChild(spawnTimer);
        spawnTimer.WaitTime = 1f;
        spawnTimer.Timeout += SpawnBone;
        spawnTimer.Start();
    }

    public override void _Process(double delta)
    {
        timerLabel.Text = $"[center]{gameDurationTimer.TimeLeft.ToString("00")}[/center]";
        scoreLabel.Text = $"[center]Score:{player.GetScore()}[/center]";
    }

    private async void gameOver(){

        //if score < 750, lose
        //eif score >= 1000, upgraded item
        //else get item
        spawnTimer.Paused = true;
        DeleteBones();
        var score = player.GetScore();
        if(score < WIN_SCORE * 0.75){
            winLoseLabel.Text = $"[center][color=red]YOU LOST[/color][/center]";
            await gameManagerInstance.SceneChanger(gameManagerInstance.scenePaths["MAIN_SCENE"]);
            return;
        }
        winLoseLabel.Text = $"[center][color=green]YOU WON[/color][/center]";
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
        float xPosition = (float)random.NextDouble() * (GetViewportRect().Size.X * 0.88f - GetViewportRect().Size.X * 0.18f) + GetViewportRect().Size.X * 0.18f; //343, 1701
        bone.Position = new Vector2(xPosition, -150);
        double bias = 0.65; // Adjust this value between 0 (min) and 1 (max), closer to 1 skews towards MAX_SPEED
        double randomFactor = Math.Pow(random.NextDouble(), 1 - bias); 
        int weightedSpeed = MIN_SPEED + (int)(randomFactor * (MAX_SPEED - MIN_SPEED));
        bone.SetSpeed(weightedSpeed);
        var meteorChance = random.Next(10);
        if(meteorChance == 1)
            bone.SetSpeed(2000);

        AddChild(bone);
        var boneSize = sizes[random.Next(sizes.Length)];
        bone.CallDeferred("Initialize", boneType, boneSize);
    }
    private void DeleteBones(){
        foreach(Node node in GetChildren().Where(child => child is Bone)){
            node.QueueFree();
        }
    }
}
