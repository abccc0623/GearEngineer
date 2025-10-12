using System;
using System.IO;
using Godot;
using Godot.Collections;

public partial class WeaponItem : Node
{
    private string itemName = "None";
    private TextureRect textureRect;
    private TextureRect itemicon;
    private NinePatchRect ninePatchRect;
    private Button button;
    private Texture2D texture;
    public Action<string> ClickAction;
    
    
    public override void _Ready()
    {
        textureRect = GetNode<TextureRect>("./TextureRect");
        ninePatchRect = GetNode<NinePatchRect>("./NinePatchRect");
        button = GetNode<Button>("./NinePatchRect/Button");
        itemicon = GetNode<TextureRect>("./NinePatchRect/icon");
        textureRect.Hide();

        button.Pressed += Click;
        itemicon.Texture = texture;
    }

    public void SetItemName(string itemName,Texture2D texture)
    {
        this.itemName = itemName;
        this.texture = texture;
    }
    public void Click()
    {
        textureRect.Show();
        ClickAction?.Invoke(itemName);
    }

    public void UnClick() => textureRect.Hide();
    
}