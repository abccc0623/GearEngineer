using System.Collections.Generic;
using Godot;
using System.IO;

public partial class WeaponSelect : Node
{
    //매니저
    private AssetsManager assetsManager;
    private GameManager gameManager;
    
    private GridContainer itemParent;
    private PackedScene weaponItem;
    private Node3D lookWeapon;
    
    private Godot.Collections.Dictionary<string, WeaponItem> itemUIList;
    private float time = 0f;
    private Vector3 rotation = new Vector3(0f, 0f, 0f);
    private Vector3 position = new Vector3(0.5f, 0.25f, 0f);
    public override void _Process(double delta)
    {
        if (lookWeapon != null && lookWeapon.GetParent() != null)
        {
            time += (float)delta;
            rotation.Y += (float)delta * 10.0f;
            lookWeapon.RotationDegrees = rotation;
            float yOffset =  Mathf.Sin(time * 1.0f) * 0.15f;
            lookWeapon.Position = position + new Vector3(0, yOffset, 0);
        }
    }

    public override void _Ready()
    {
        itemUIList = new Godot.Collections.Dictionary<string, WeaponItem>();
        //아이템을 생성후 넣어줄 부모객체
        itemParent = GetNode<GridContainer>("./NinePatchRect/Content/GridContainer");
        assetsManager = GetNode<AssetsManager>("/root/AssetsManager");
        gameManager = GetNode<GameManager>("/root/GameManager");
        
        //만들어 놓은 웨폰 아이템로드
        string itemPath = "res://GearEngineer/Asset/Prefab/UI/weapon_item.tscn";
        weaponItem = GD.Load<PackedScene>(itemPath);

        LoadItem();
        CallDeferred(nameof(AllReady));
        GetTree().SceneChanged += SceneChanged;
    }

    private void SceneChanged()
    {
        lookWeapon.QueueFree();
        lookWeapon = null;
    }

    private void AllReady()
    {
        var target = itemParent.GetChild<WeaponItem>(0);
        target?.Click();
    }

    void ChangeItemClick(string itemName)
    {
        foreach (var keyValuePair in itemUIList)
        {
            if(keyValuePair.Key == itemName) { continue; }
            keyValuePair.Value.UnClick();
        }

        if (lookWeapon != null)
        {
            lookWeapon.QueueFree();
        }

        var target = assetsManager.Get<PackedScene>(AssetKey.WeaponObject,itemName);
        lookWeapon = target.Instantiate<Node3D>();
        lookWeapon.Name = itemName;
        AddChild(lookWeapon);
    }

    void LoadItem()
    {
        var target =assetsManager.Get(AssetKey.WeaponTexture);
        const string folderPath = "res://GearEngineer/Asset/FactoryMap/";
        string[] paths = new[]
        {
            folderPath+"1_Sword.json",
            folderPath+"2_Axe.json",
        }; 
        
        for (var i = 0; i < paths.Length; i++)
        {
            string name = Path.GetFileNameWithoutExtension(paths[i]);
            var item = weaponItem.Instantiate<WeaponItem>();
            var texture = (Texture2D)target[name];
            item.SetItemName(name,texture);
            item.ClickAction = ChangeItemClick;
            itemUIList.Add(name,item);
            itemParent.AddChild(item);
        }
    }
   

 
    void PlayGame()
    {
        gameManager.Setting(lookWeapon.Name);
        EventManager.Play<CameraFadeOutEvent>();
        EventManager.Play<SceneChangeEvent>( new List<object>(){"res://GearEngineer/Asset/Scene/InGame.tscn"});
        EventManager.Play<CameraFadeInEvent>();
    }
}
