using System;
using Godot;

namespace GearEngineer.GearEngineer.Asset.Script.Animation;

public partial class CharacterAnimation : AnimationTree
{
	[Export]private bool isRun = false;
	[Export]private int isJump = 0;
	private CharacterController characterNode;
	private AnimationNodeStateMachinePlayback stateMachine;
	private AnimationTree animationTree;
	
	public override void _Ready()
	{
		var fbxNode = GetOwner();
		characterNode = fbxNode.GetOwner<CharacterController>();
		animationTree = GetParent().GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
	}

	public override void _Process(double delta)
	{
		isRun = (characterNode.direction != Vector3.Zero);
		isJump = characterNode.jumpStage;
	}
}
