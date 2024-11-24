using Godot;
using System;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	private AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("RESET");
	}

    public override void _Process(double delta)
    {
        testEsc();
    }

    public void resume(){
		GetTree().Paused = false;
		animationPlayer.PlayBackwards("blur");
		
	}
	public void pause(){
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
		resume();
	}
	public void _on_settings_pressed(){
		//settings ig idk
	}
	public void _on_back_pressed(){
		//go back to menu
	}

}
