using Godot;
using System;
using System.Collections.Generic;

public partial class BaseMap : Node3D

{
    [Export] private  door rightDoor = null;
    [Export] private  door leftDoor = null;
    [Export] private  door upDoor = null;
    [Export] private  door downDoor = null;
    
    public override void _Ready()
    {
        
    }
    public override void _EnterTree()
    {
        
    }

    public override void _Process(double delta)
    {
        
    }


    public virtual void PlayerEnter()
    {
        if(rightDoor != null){rightDoor.Close();}
        if(leftDoor != null){leftDoor.Close();}
        if(upDoor != null){upDoor.Close();}
        if(downDoor != null){downDoor.Close();}
    }

    public virtual void PlayerExited()
    {
        
    }
}
