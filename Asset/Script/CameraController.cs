using Godot;
using System;

public partial class CameraController : Camera3D
{
    [Export] public Node3D target;
    [Export] public float SmoothSpeed = 5f;
     public Vector3 targetDistance;

     public override void _Ready()
     {
    
     }

     public override void _EnterTree()
    {
        targetDistance = new Vector3(0,8.0f,8.5f);
        RotationDegrees = new Vector3(-45.0f, 0, 0);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 desiredPosition = target.GlobalPosition + targetDistance;
        // 보간 이동
        GlobalPosition = GlobalPosition.Lerp(desiredPosition, (float)(SmoothSpeed * delta));

        // 플레이어 바라보기
        //LookAt(target.GlobalPosition, Vector3.Up);
    }
}
