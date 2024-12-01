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
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_CLICKED"]);
		HowToOverlay.Visible = false;
		gameManagerInstance.allowMinigameStart = true;
	}
	private void OnHowToExitButtonHover(){
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_HOVER"]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
