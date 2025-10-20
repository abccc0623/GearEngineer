using Godot;
using System;

public partial class CameraController : Camera3D
{
    [Export] public Node3D target;
    [Export] public float SmoothSpeed = 2f;
    
    Vector3 targetDistance = new Vector3(0,8.0f,8.5f);
    Vector3 targetRotation = new Vector3(-45.0f, 0, 0);
    
    Vector3 topLookTargetDistance = new Vector3(0,12.0f,0);
    Vector3 topLookTargetRotation = new Vector3(-90.0f, 0, 0);

    [Export]public int lookType = 0;
    
    public override void _PhysicsProcess(double delta)
    {
        switch (lookType)
        {
            case 0:
                DefaultLook(delta);
                break;
            case 1:
                TopLook(delta);
                break;
        }
    }

    void DefaultLook(double delta)
    {
        Vector3 desiredPosition = target.GlobalPosition + targetDistance;
        RotationDegrees = targetRotation;
        GlobalPosition = GlobalPosition.Lerp(desiredPosition, (float)(SmoothSpeed * delta));
    }

    void TopLook(double delta)
    {
        Vector3 desiredPosition = target.GlobalPosition + topLookTargetDistance;
        RotationDegrees = topLookTargetRotation;
        GlobalPosition = GlobalPosition.Lerp(desiredPosition, (float)(SmoothSpeed * delta));
    }
    
}
