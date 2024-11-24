using System;
using System.ComponentModel;
using Godot;

public partial class MainScenePlayer : PlayerMovement
{
	private AnimatedSprite2D dog_animation;
	private LineEdit inputBox;
	
	public override void _Ready() 
	{
		dog_animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        inputBox = GetNode<LineEdit>("%GuessInputField");
		controllable = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		if(!controllable) return;
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += Gravity * (float)delta;
		}

		velocity.X = 0;
		if (Input.IsActionPressed("Move_Left")){
			velocity.X = -Speed;
			dog_animation.Play("WalkLeft");
		}
		else if (Input.IsActionPressed("Move_Right")){
			velocity.X = Speed;
			dog_animation.Play("WalkRight");
		}
		
		if(Input.IsActionJustReleased("Move_Left")) {
			dog_animation.Play("IdleLeft");
		}
		else if (Input.IsActionJustReleased("Move_Right")) {
			dog_animation.Play("IdleRight");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
    private void _on_input_event(){
		controllable = true;
		inputBox.Editable = false;
	}

}
