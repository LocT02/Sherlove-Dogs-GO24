using Godot;
using System;
using  GameManager;

public partial class DogBed : Area2D
{
	// Called when the node enters the scene tree for the first time.

	private bool showInteractionLabel = false;
	private Label dobBedLabel;
	public override void _Ready()
	{
		dobBedLabel = GetNode<Label>("Label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		dobBedLabel.Visible = showInteractionLabel;
		if(showInteractionLabel && Input.IsActionJustPressed("Interact")){
			GD.Print("interacted with dog bed");
			//
		}
	}

	public void _on_body_entered(Node body){
		if(body is CharacterBody2D){
			showInteractionLabel = true;
		}
			
	}
	public void _on_body_exited(Node body){
		if(body is CharacterBody2D){
			showInteractionLabel = false;
		}
	}
}
