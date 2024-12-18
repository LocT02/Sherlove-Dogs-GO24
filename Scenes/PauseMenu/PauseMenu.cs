using GameData;
using GameManager;
using Godot;
using System;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	private AnimationPlayer animationPlayer;
	private GameManager.GameManager gameManagerInstance;
	private Label gameSaveLabel;
	private TextureButton[] textureButtons;
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		gameSaveLabel = GetNode<Label>("PanelContainer/VBoxContainer/Control2/Save/Label");
		textureButtons = new TextureButton[]{
			GetNode<TextureButton>("PanelContainer/VBoxContainer/Control/Resume"),
			GetNode<TextureButton>("PanelContainer/VBoxContainer/Control2/Save"),
			GetNode<TextureButton>("PanelContainer/VBoxContainer/Control3/Back")
		};
		animationPlayer.Play("RESET");
		gameManagerInstance = GameManager.GameManager.Instance;
		setTextureButtonMouseFilter(MouseFilterEnum.Ignore);
	}

	public override void _Process(double delta)
	{
		testEsc();
	}

	public void resume(){
		setTextureButtonMouseFilter(MouseFilterEnum.Ignore);
		foreach(TextureButton btn in textureButtons){
			btn.Disabled = true;
		}
		GetTree().Paused = false;
		animationPlayer.PlayBackwards("blur");
		
	}
	public void pause(){
		setTextureButtonMouseFilter(MouseFilterEnum.Stop);
		foreach(TextureButton btn in textureButtons){
			btn.Disabled = false;
		}
		GetTree().Paused = true;
		animationPlayer.Play("blur");
	}
	public void testEsc(){
		if(Input.IsActionJustPressed("Pause") && GetTree().Paused == false){
			pause();
		} else if (Input.IsActionJustPressed("Pause") && GetTree().Paused == true){
			resume();
		}
	}

	public void _on_resume_pressed(){
		gameSaveLabel.Text = "Save Game";
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_CLICKED"]);
		resume();
	}
	public void _on_resume_mouse_entered(){
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_HOVER"]);
	}
	public void _on_savegame_pressed(){
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_CLICKED"]);
		gameManagerInstance.gameData.SaveGame();
		gameSaveLabel.Text = "Game Saved";
	}
	public void _on_save_mouse_entered(){
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_HOVER"]);
	}
	public async void _on_back_pressed(){
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_CLICKED"]);
		resume();
		await gameManagerInstance.SceneChanger(gameManagerInstance.scenePaths["MAIN_MENU"]);
	}
	public void _on_back_mouse_entered(){
		gameManagerInstance.PlaySFX("MainSceneNode", gameManagerInstance.sfxPaths["BUTTON_HOVER"]);
	}
	private void setTextureButtonMouseFilter(MouseFilterEnum filter){
		foreach (TextureButton button in textureButtons){
			button.MouseFilter = filter;
			button.ZIndex = (filter == MouseFilterEnum.Ignore) ? -2 : 2;
		}
	}

}
