using System;
using Godot;

namespace GearEngineer.GearEngineer.Asset.Script.Animation;

public partial class CharacterAnimation : AnimationTree
{
	[Export]private bool isRun = false;
	private CharacterController cc;

	public override void _Ready()
	{
		cc = GetParent<CharacterController>();
	}

	public override void _Process(double delta)
	{
		isRun = (cc.direction != Vector3.Zero);
	}
}
