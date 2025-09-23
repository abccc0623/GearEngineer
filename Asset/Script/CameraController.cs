using Godot;
using System;

public partial class CameraController : Node3D
{
    [Export] private Node3D target;
    [Export] public Vector3 targetDistance = new Vector3(0, 8.0f, 8.0f);
    [Export] public float SmoothSpeed = 5f;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 desiredPosition = target.GlobalPosition + targetDistance;
        // 보간 이동
        GlobalPosition = GlobalPosition.Lerp(desiredPosition, (float)(SmoothSpeed * delta));

        // 플레이어 바라보기
        //LookAt(target.GlobalPosition, Vector3.Up);
    }
}
