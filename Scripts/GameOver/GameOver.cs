using Godot;
using System;

public partial class GameOver : Control
{
	private GameManager.GameManager gameManagerInstance;
	private Label _scoreText; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManagerInstance = GameManager.GameManager.Instance;
		_scoreText = GetNode<Label>("BG/ScoreText");
		_scoreText.Text = gameManagerInstance.gameData.Score.ToString();
	}
	private async void _on_retry_btn_pressed(){
		await gameManagerInstance.SceneChanger(gameManagerInstance.scenePaths["MAIN_SCENE"]);
	}
	private async void _on_main_menu_btn_pressed(){
		await gameManagerInstance.SceneChanger(gameManagerInstance.scenePaths["MAIN_MENU"]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
