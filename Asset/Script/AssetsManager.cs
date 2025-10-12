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
    public Dictionary<AssetKey, Dictionary<string, RefCounted>> Assets = new();
    public override void _EnterTree()
    {
        LoadAssets<Texture2D>("res://GearEngineer/Asset/Texture/Weapon", AssetKey.WeaponTexture);
        LoadAssets<PackedScene>("res://GearEngineer/Asset/Prefab/Weapon", AssetKey.WeaponObject);
        LoadAssets<PackedScene>("res://GearEngineer/Asset/Prefab/Character", AssetKey.CharacterObject);
    }

    public void LoadAssets<T>(string assetsPath, AssetKey key) where T : RefCounted
    {
        Assets.Add(key, new Dictionary<string, RefCounted>());
        string absPath = ProjectSettings.GlobalizePath(assetsPath);
        if (Directory.Exists(absPath))
        {
            var loader = Directory.GetFiles(absPath);
            for (var i = 0; i < loader.Length; i++)
            {
                if (loader[i].Contains(".import")) continue;
                string name = Path.GetFileNameWithoutExtension(loader[i]);
                var texture2d = GD.Load<T>(loader[i]);
                Assets[key].Add(name, texture2d);
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