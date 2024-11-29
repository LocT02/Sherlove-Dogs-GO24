using GameData;
using Godot;
using ResultManager;

public partial class MainMenu : Control
{
	private GameManager.GameManager gameInstance;
	private TextureButton ContinueButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		gameInstance = GameManager.GameManager.Instance;
		ContinueButton = GetNode<TextureButton>("MarginContainer/VBoxContainer/Continue");

		CheckLoad();
		GD.Print("Main Menu Ready");
	}

	private void CheckLoad() {
		var save = GameDataManager.JsonToDictionary("res://Globals/Data/categories.json");
		if (save.Value != null || save.Value.Count > 0) {
			ContinueButton.Disabled = false;
			var color = ContinueButton.SelfModulate;
			color.A = 255;
			ContinueButton.SelfModulate = color;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
	public async void OnPlayPressed()
	{
		// Put this initializing gamemanager somewhere above like _ready or something
		GD.Print("play pressed");
		await gameInstance.SceneChanger("res://Scenes/MainScene/main_scene.tscn");
	}
	public async void OnSettingsPressed()
	{
		gameInstance.GameState = "continue";
		await gameInstance.SceneChanger("res://Scenes/MainScene/main_scene.tscn");
	}
	public async void OnContinueSavePressed() {
		gameInstance.GameState = "continue";
		await gameInstance.SceneChanger("res://Scenes/MainScene/main_scene.tscn");
	}
	public void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
