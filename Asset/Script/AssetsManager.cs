using System.IO;
using Godot;
using Godot.Collections;

public enum AssetKey
{
    WeaponTexture,
    WeaponObject,
    CharacterObject
}

public partial class AssetsManager : Node
{
    const string sword = "1_Sword";
    const string axe = "2_Axe";

    private const string weaponTexturePath = "res://GearEngineer/Asset/Texture/Weapon/";
    private const string weaponObjectPath = "res://GearEngineer/Asset/Prefab/Weapon/";
    private const string characterObjectPath = "res://GearEngineer/Asset/Prefab/Character/";
    
    public Dictionary<AssetKey, Dictionary<string, RefCounted>> Assets = new();
    
    
    
    public override void _EnterTree()
    {
        string[] weaponTexturePaths = new []
        {
            weaponTexturePath + sword + ".png",
            weaponTexturePath + axe + ".png",
        };
        
        string[] weaponObjectPaths = new []
        {
            weaponObjectPath + sword + ".tscn",
            weaponObjectPath + axe+ ".tscn",
        };
        
        string[] characterObjectPaths = new []
        {
            characterObjectPath + sword+ ".tscn",
            characterObjectPath + axe+ ".tscn",
        };

        LoadAssets<Texture2D>(weaponTexturePaths, AssetKey.WeaponTexture);
        LoadAssets<PackedScene>(weaponObjectPaths, AssetKey.WeaponObject);
        LoadAssets<PackedScene>(characterObjectPaths, AssetKey.CharacterObject);
    }

    public void LoadAssets<T>(string[] assetPaths, AssetKey key) where T : RefCounted
    {
        Assets.Add(key, new Dictionary<string, RefCounted>());
        foreach (var path in assetPaths)
        {
            var resource = GD.Load<T>(path);
            string name = Path.GetFileNameWithoutExtension(path);
            Assets[key].Add(name, resource);
            if (resource == null)
            {
                GD.Print(path);
            }
        }
    }
    public Dictionary<string, RefCounted> Get(AssetKey key)
    {
        if (Assets.TryGetValue(key, out var value))
        {
            return value;
        }
        else
        {
            GD.PrintErr("key not found");
            return null;
        }
    }

    public T Get<T>(AssetKey key, string fileName) where T : RefCounted
    {
        if (Assets.TryGetValue(key, out var value))
        {
            return (T)value[fileName];
        }
        else
        {
            GD.PrintErr("key not found");
        }
        return null;
    }
    
    
}