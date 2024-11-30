using System;
using Godot;

public partial class CTBPlayer : PlayerMovement
{
	private int score = 0;
	private GameManager.GameManager gameManagerInstance;

	public override void _Ready()
	{
		gameManagerInstance = GameManager.GameManager.Instance;
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
			// Play SFX based on bone Type (1 == tomato, else == treat)
			if (boneInstance.boneTypeForSFX == 1){
				gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["TOMATO_CAUGHT"]);
			} else{gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["TREAT_CAUGHT"]);}
			score = Math.Max(score + boneInstance.Points, 0);

			boneInstance.QueueFree();
		}
	}

	public int GetScore(){
		return score;
	}
}
