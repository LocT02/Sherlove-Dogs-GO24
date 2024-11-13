using System;
using System.ComponentModel;
using Godot;

public partial class PlayerMovement : CharacterBody2D
{
	public float Speed {get; set;} = 400.0f;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{

		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += Gravity * (float)delta;
		}

		velocity.X = 0;
		if (Input.IsActionPressed("Move_Left")){
			velocity.X = -Speed;
		}
		else if (Input.IsActionPressed("Move_Right")){
			velocity.X = Speed;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
