using Godot;
using System.Collections.Generic;

public partial class CharacterController : Node3D
{

    [Export] public float Speed = 5f;
    private Vector3 _velocity = Vector3.Zero;
    private float rotationSpeed = 10.0f;
    private AnimationPlayer animationPlayer;
    private Area3D area3D;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("Knight/AnimationPlayer"); 
        area3D = GetNode<Area3D>("Area3D");
        area3D.Connect("body_entered", new Callable(this, "OnBodyEntered"));
    }
    
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
            animationPlayer.Play("Walking_A");
        }
        else
        {
            animationPlayer.Play("Idle");
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
