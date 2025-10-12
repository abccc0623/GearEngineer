using Godot;
using System;

public partial class GameManager : Node
{
    private AssetsManager assetsManager;
    private string playWeaponName = "1_Sword";

    public override void _Ready()
    {
        assetsManager = GetNode<AssetsManager>("../AssetsManager");
        GetTree().SceneChanged += SceneChanged;
    }

    private void SceneChanged()
    {
        var currentScene = GetTree().CurrentScene; // Node
        if (currentScene.Name == "InGame")
        {
            var target = assetsManager.Get<PackedScene>(AssetKey.CharacterObject, playWeaponName);
            var character = (Node3D)target.Instantiate();
            CameraController cc = new CameraController();
            cc.target = character;
            
            currentScene.AddChild(character);
            currentScene.AddChild(cc);
        }
    }

    public void Setting(string weaponName)
    {
        playWeaponName = weaponName;
    }

    public void PlayGame()
    {
        
        
    }
}
