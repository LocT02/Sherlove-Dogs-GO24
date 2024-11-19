using System;
using Godot;

public partial class CTBPlayer : PlayerMovement
{
    private int score = 0;

    public override void _Ready()
    {
        this.Speed = 300.0f;
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = new Vector2(Velocity.X, 0);
        MoveAndSlide();
    }

    private void OnBoneCollected(Area2D bone)
    {
        if (bone is Bone boneInstance)
        {
            score = Math.Max(score + boneInstance.Points, 0);
            GD.Print("Score: " + score);

            boneInstance.QueueFree();
        }
    }

    public int GetScore(){
        return score;
    }
}
