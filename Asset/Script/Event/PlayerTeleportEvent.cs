using System;
using Godot;

public partial class PlayerTeleportEvent : Event
{
    private float time = 0;
    protected override void Start()
    {
        var movePosition = Vector3.Zero;
        if (parameterList[0] is Vector3 pos) movePosition = pos; 
        var node = GetNode<CharacterController>("/root/Node3D/Player");
        node.Position = movePosition;
    }

    protected override bool Update(float delta)
    {
        time += delta;
        if (time > 1)
        {
            
            return true;
        }
        return false;
    }

    protected override void End()
    {
        
    }
}