using Godot;
using System;
using GameManager;
using System.Collections.Generic;

public partial class DogDoor : Area2D
{
	// Called when the node enters the scene tree for the first time.

	private bool showInteractionLabel = false;
	private Label dogDoorLabel;
	private GameManager.GameManager gameManager;
	private Random random = new Random();
	private MainSceneUIScript UIScript;
	private Sprite2D sprite;

	private Dictionary<string, Texture2D> imagePaths = new Dictionary<string, Texture2D>{
		{"closed",(Texture2D)GD.Load("res://Assets/Art/Backgrounds/LivingRoom/DoggyDoor Closed.png")},
		{"open", (Texture2D)GD.Load("res://Assets/Art/Backgrounds/LivingRoom/DoggyDoor.png")}
	};
	public override void _Ready()
	{
		dogDoorLabel = GetNode<Label>("Label");
		gameManager = GameManager.GameManager.Instance;
		UIScript = GetNode<MainSceneUIScript>("/root/MainSceneNode/MainSceneUI");
		sprite = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		dogDoorLabel.Visible = showInteractionLabel;
		if(showInteractionLabel && Input.IsActionJustPressed("Interact") && gameManager.allowMinigameEntry){
			gameManager.PlaySFX("MainSceneNode", gameManager.sfxPaths["DOGGY_DOOR"]);
			GD.Print("interacted with dog door");
			randomMinigame();
		}
	}

	public void _on_body_entered(Node body){
		if(body is CharacterBody2D){
			showInteractionLabel = true;
			sprite.Texture = imagePaths["open"];
		}
		
		
	}
	public void _on_body_exited(Node body){
		if(body is CharacterBody2D){
			showInteractionLabel = false;
			sprite.Texture = imagePaths["closed"];
		}
	}
	private async void randomMinigame(){
		gameManager.allowMinigameEntry = false;

		string[] minigamePaths = {
			gameManager.scenePaths["MEMORY_SCENE"],
			gameManager.scenePaths["CATCH_THE_BONE_SCENE"]
		};
		
		await gameManager.SceneChanger(minigamePaths[random.Next(0,minigamePaths.Length)]);
	}
}
