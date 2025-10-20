using Godot;
using System;

public partial class Passageway : BaseMap
{
    private CameraController cc;
    
    public void EnterEvent(Node3D body)
    {
        if (body.GetType() == typeof(CharacterController))
        {
            if (cc == null)
            {
                cc = GetNode<CameraController>("/root/InGame/mainCamera");
            }
            cc.lookType = 1;
        }
    }

    public void ExitedEvent(Node3D body)
    {
        cc.lookType = 0;
    }
}
