using Godot;
using System;
using  GameManager;

public partial class DogBed : Area2D
{
	// Called when the node enters the scene tree for the first time.

	private bool showInteractionLabel = false;
	private Label dobBedLabel;
	private LineEdit inputBox;
	private PlayerMovement player;
	public bool interactable = true;
	public override void _Ready()
	{
		dobBedLabel = GetNode<Label>("Label");
		inputBox = GetNode<LineEdit>("%GuessInputField");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		dobBedLabel.Visible = showInteractionLabel;
		if(showInteractionLabel && Input.IsActionJustPressed("Interact") && interactable){
			GD.Print("interacted with dog bed");
			inputBox.Editable = true;
			player.controllable = false;
			interactable = false;
			inputBox.GrabFocus();
			inputBox.GrabClickFocus();
		}
	}

	public void _on_body_entered(Node body){
		if(body is CharacterBody2D){
			showInteractionLabel = true;
			player = (PlayerMovement)body;
		}
			
	}
	public void _on_body_exited(Node body){
		if(body is CharacterBody2D){
			showInteractionLabel = false;
		}
	}
}
