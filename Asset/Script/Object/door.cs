using Godot;
using System;

public partial class door : Node
{
    MeshInstance3D doorMesh;
    AnimationPlayer animationPlayer;
    
    public void Open()
    {
        animationPlayer.Play("Open");
    }

    public void Close()
    {
        animationPlayer.Play("Close");
    }
    
    
    public override void _Ready()
    {
        doorMesh = GetNode<MeshInstance3D>("wall_doorway/wall_doorway_door");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Open();
    }
}
