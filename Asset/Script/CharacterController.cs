using Godot;
using System;
using System.Diagnostics;

public partial class CharacterController : Node3D
{

    [Export] public float Speed = 5f;
    private Vector3 _velocity = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 direction = Vector3.Zero;

        if (Input.IsActionPressed("move_Left")) direction.X = -1;
        if (Input.IsActionPressed("move_right"))   direction.X = 1; 
        if (Input.IsActionPressed("move_Up"))   direction.Z = -1; 
        if (Input.IsActionPressed("move_Down"))   direction.Z = 1; 
        direction.Normalized();
        // 이동 속도 적용
        _velocity.X += direction.X * Speed * (float)delta;
        _velocity.Z += direction.Z * Speed * (float)delta;
        Position = _velocity;
    }
}
