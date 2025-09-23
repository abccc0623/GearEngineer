using Godot;
using System;

public partial class CameraZoomEvent : Event
{
    private static CameraController cc = null;
    private float time = 0.0f;
    private float startPositionY;
    private int count = 0;
    
    protected override void Start()
    {
        if (cc == null)
        {
            cc = GetNode<CameraController>("/root/Node3D/Camera3D");
        }
        startPositionY = cc.targetDistance.Y;
    }

    protected override bool Update(float delta)
    {
        time += delta;
        if (time > 1.0f)
        {
            cc.targetDistance += new Vector3(0, 1, 0);
            count++;
            time = 0.0f;
        }

        if (count > 10)  return true;
        
        return false;
    }

    protected override void End()
    {
        cc.targetDistance = new Vector3(cc.GlobalPosition.X, startPositionY, cc.GlobalPosition.Z);
    }
}
