using System;
using System.ComponentModel;
using Godot;

public partial class PlayerMovement : CharacterBody2D
{
	public float Speed {get; set;} = 400.0f;
	
	public void GetInput(){
		Vector2 _inputDirection = Input.GetVector("Move_Left", "Move_Right", "ui_up", "ui_down");
		Velocity = _inputDirection * Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Add the gravity.
		if (!IsOnFloor())
		{
			Velocity += GetGravity() * (float)delta;
		}

		GetInput();
		MoveAndSlide();
	}
}
