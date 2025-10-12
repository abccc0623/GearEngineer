using Godot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class CharacterController : Node3D
{
    [Export] public float Speed = 5f;
    private static CharacterController characterController;
    private static bool brakeInput = false;
    private Vector3 _velocity = Vector3.Zero;
    
    private AnimationPlayer animationPlayer;
    private CharacterAnimation characterAnimation;
    
    private float rotationSpeed = 10.0f;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer"); 
        characterAnimation = GetNode<CharacterAnimation>("AnimationTree");
        characterController = this;
    }
    public static Node Player => characterController;
    public static void BrakeInput(bool brake) => brakeInput = brake;
    
    public override void _PhysicsProcess(double delta)
    {
        Vector3 direction = Vector3.Zero;
        if (brakeInput == false)
        {
            if (Input.IsActionPressed("move_Left")) direction.X = -1;
            if (Input.IsActionPressed("move_right"))   direction.X = 1; 
            if (Input.IsActionPressed("move_Up"))   direction.Z = -1; 
            if (Input.IsActionPressed("move_Down"))   direction.Z = 1; 
        }
        direction = direction.Normalized();
        // 이동 속도 적용
        _velocity.X += direction.X * Speed * (float)delta;
        _velocity.Z += direction.Z * Speed * (float)delta;
        if (direction.Z != 0 || direction.X != 0)
        {
            Position = _velocity;
        }
        else
        {
            _velocity = Position;
        }
        
        //이동 중 회전 적용
        if (direction != Vector3.Zero)
        {
            Basis basis = Basis.LookingAt(-direction, Vector3.Up);
            Basis = Basis.Slerp(basis, rotationSpeed * (float)delta);
            characterAnimation.IsRunning = true;
        }
        else
        {
            characterAnimation.IsRunning = false;
        }
    }

 
    private void OnBodyEntered(Node body)
    {
        GD.Print("충돌한 객체: " + body.Name);
        EventManager.Play<CameraFadeOutEvent>();
        EventManager.Play<PlayerTeleportEvent>(new List<object>() { new Vector3(-10,0,0) });
        EventManager.Play<CameraFadeInEvent>();
    }
   
}
