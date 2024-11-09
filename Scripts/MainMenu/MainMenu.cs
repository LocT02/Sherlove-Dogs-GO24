using Godot;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	// public override void _Ready()
	// {
	// }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
	public void OnPlayPressed()
	{
		// Put this initializing gamemanager somewhere above like _ready or something
		GameManager.GameManager gameManager = new GameManager.GameManager();
		gameManager.SceneChanger("res://Scenes/MainScene/main_scene.tscn", false);
	}
	public void OnSettingsPressed()
	{
		//Todo
	}
	public void OnQuitPressed()
	{
		GetTree().Quit();
	}
}