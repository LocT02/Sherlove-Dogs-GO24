using GameData;
using Godot;
using System;

public partial class MainScene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private GameManager.GameManager gameInstance;
	public override void _Ready()
	{
		// Need some sort of transition effect here
		gameInstance = GameManager.GameManager.Instance;
		if (gameInstance.GameState == "continue") {
			gameInstance.gameData.LoadSaveData();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
