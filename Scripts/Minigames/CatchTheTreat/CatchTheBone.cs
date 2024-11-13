using Godot;
using System;

public partial class CatchTheBone : Node2D
{
    private Random random = new Random();
    private PlayerMovement player;
    private Timer spawnTimer;
    private int score = 0;

    public override void _Ready()
    {
        player = GetNode<PlayerMovement>("Player");
        
        // Set up timer for bone spawning
        spawnTimer = new Timer();
        AddChild(spawnTimer);
        spawnTimer.WaitTime = 1.5f;
        spawnTimer.Timeout += SpawnBone;
        spawnTimer.Start();
    }

    private void SpawnBone()
    {
        var boneType = random.Next(0, 3);  // Random bone type: 0, 1, or 2
        var bone = new Bone();
        bone.Initialize(boneType);

        // Set random x position and add bone at the top of the screen
        float xPosition = (float)random.NextDouble() * GetViewportRect().Size.X;
        bone.Position = new Vector2(xPosition, 0);
        //bone.BodyEntered += OnBoneCollected;
        AddChild(bone);
    }

    private void OnBoneCollected(Node body)
    {
        GD.Print(body.GetClass());
        if (body is Bone)
        {
            var bone = body as Bone;
            score += bone.Points;
            GD.Print("Score: " + score);  // For debugging; replace with actual UI update

            bone.QueueFree();
        }
        GD.Print("Score: " + score);
    }

    // Optional: Method to retrieve score (e.g., for UI or Game Over screen)
    public int GetScore()
    {
        return score;
    }
}
