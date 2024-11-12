using Godot;

public partial class MainMenu : Control
{
	private GameManager.GameManager gameInstance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		gameInstance = GameManager.GameManager.Instance;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
	public void OnPlayPressed()
	{
		// Put this initializing gamemanager somewhere above like _ready or something
		gameInstance.SceneChanger("res://Scenes/MainScene/main_scene.tscn");
	}
	public void OnSettingsPressed()
	{
		//Todo
	}
	public void OnContinueSavePressed() {
		gameInstance.GameState = "continue";
		gameInstance.SceneChanger("res://Scenes/MainScene/main_scene.tscn");
	}
	public void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
