using System;
using Godot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class CharacterController : CharacterBody3D
{
	private static CharacterController characterController;
	private CharacterBody3D body;
	private static bool brakeInput = false;
	
	[Export] public float Speed = 3f;
	[Export] public float JumpTime = 0.0f;
	public float Gravity = -9.81f;
	
	private float rotationSpeed = 10.0f;
	public Vector3 direction; //방향
	public int jumpStage = -1;
	
	[Export]private bool jump = false;
	[Export] private float jumpSpeed = 3;
	[Export] private float jumpDownSpeed = 10;
	[Export] private float jumpUpSpeed = 10;
	[Export] private float maxjumpHeight = 3;
	[Export] private float startJumpPosition;
	private bool isjumpUP = false;
	
	public override void _Ready()
	{
		characterController = this;
	}
	public static Node Player => characterController;
	public static void BrakeInput(bool brake) => brakeInput = brake;
	
	public override void _PhysicsProcess(double delta)
	{
		direction = Vector3.Zero;

		if (Input.IsActionPressed("move_Left")) direction.X = -1;
		if (Input.IsActionPressed("move_right")) direction.X = 1;
		if (Input.IsActionPressed("move_Up")) direction.Z = -1;
		if (Input.IsActionPressed("move_Down")) direction.Z = 1;
		direction = direction.Normalized();

		if (direction != Vector3.Zero) direction *= Speed;
		
		var targetDirection = direction;
		if (targetDirection != Vector3.Zero)
		{
			targetDirection.X *= -1;
			targetDirection.Z *= -1;
		    Basis targetBasis = Basis.LookingAt(targetDirection, Vector3.Up);
		    Basis = Basis.Slerp(targetBasis, rotationSpeed * (float)delta);
		}
		JumpCheck(delta);
		Velocity = direction;
		MoveAndSlide();
	}

	void JumpCheck(double delta)
	{
		if (jump == false)
		{
			if (Input.IsActionJustPressed("move_jump"))
			{
				startJumpPosition = GlobalPosition.Y;
				jump = true;
				isjumpUP = true;
				jumpSpeed = 3;
				jumpStage = 1;
			}

			if (IsOnFloor() == false)
			{
				jumpSpeed -= (float)delta * jumpDownSpeed; 
				direction.Y = jumpSpeed;
			}
		}
		else
		{
			if (jumpSpeed <= (maxjumpHeight + startJumpPosition) && isjumpUP == true)
			{
				//올라가는 로직
				jumpSpeed += (float)delta * jumpUpSpeed; 
				if (jumpSpeed >= (maxjumpHeight + startJumpPosition)) isjumpUP = false; 
			
			}
			else if(IsOnFloor() == false && isjumpUP == false)
			{
				//내려가는 로직
				jumpSpeed -= (float)delta * jumpDownSpeed; 
			}
			
			if (IsOnFloor() && isjumpUP == false)
			{
				jump = false;
				jumpStage = 2;
			}
			direction.Y = jumpSpeed;
		}

	}
}
