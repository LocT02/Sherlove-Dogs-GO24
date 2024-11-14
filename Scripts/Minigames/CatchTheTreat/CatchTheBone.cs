using Godot;
using System;

public partial class CatchTheBone : Node2D
{
    private Timer spawnTimer;
    private Random random = new Random();
    // Path to your Bone scene file
    private PackedScene boneScene = (PackedScene)GD.Load("res://Scenes/Minigames/CatchTheTreat/Bone.tscn");
    private CTBPlayer player;

    public override void _Ready()
    {
        player = GetNode<CTBPlayer>("Player");
        spawnTimer = new Timer();
        AddChild(spawnTimer);
        spawnTimer.WaitTime = 1.5f;
        spawnTimer.Timeout += SpawnBone;
        spawnTimer.Start();
    }
    public override void _PhysicsProcess(double delta)
    {
        
    }
    private void SpawnBone()
    {
        var boneType = random.Next(0, 3);  // Choose random bone type
        var bone = (Bone)boneScene.Instantiate();  // Use Instantiate() to create an instance of the Bone scene

        // Initialize bone with type (0, 1, or 2 for points and texture)
        

        // Position the bone at a random x-position at the top of the screen
        float xPosition = (float)random.NextDouble() * GetViewportRect().Size.X;
        bone.Position = new Vector2(xPosition, 0);

        AddChild(bone);
        bone.CallDeferred("Initialize", boneType);
    }
}
