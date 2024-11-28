using Godot;
using System;

public partial class HowToUi : Control
{
	private GameManager.GameManager gameManagerInstance;
	private CanvasLayer HowToOverlay;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManagerInstance = GameManager.GameManager.Instance;
		HowToOverlay = GetNode<CanvasLayer>("HowToOverlay");
		HowToOverlay.Visible = true;
		gameManagerInstance.allowMinigameStart = false;
	}

	private void OnHowToExitButtonPressed(){
		HowToOverlay.Visible = false;
		gameManagerInstance.allowMinigameStart = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
