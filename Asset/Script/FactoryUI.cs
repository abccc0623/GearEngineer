using Godot;
using System;
using System.Collections.Generic;

public partial class FactoryUI : Control
{
    List<Button> buttons = new List<Button>();
    public override void _Ready()
    {
        buttons.Add(GetChild<Button>(0));
        buttons.Add(GetChild<Button>(1));
        buttons.Add(GetChild<Button>(2));
        buttons.Add(GetChild<Button>(3));
    }

    public void Open()
    {
        var tween = this.GetTree().CreateTween();
        for (var i = 0; i < buttons.Count; i++)
        {
            Vector2 movePosition = Vector2.Zero;
            switch (i)
            {
                case 0: movePosition = new Vector2(-50, 0); break;
                case 1: movePosition = new Vector2(0, -50); break;
                case 2: movePosition = new Vector2(50, 0); break;
                case 3: movePosition = new Vector2(0, 50); break;
            }
        
            buttons[i].Scale = new Vector2(0.0f, 0.0f);
            tween.Parallel().TweenProperty(buttons[i], "scale", new Vector2(1.0f, 1.0f), 0.2f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
            tween.TweenProperty(buttons[i], "position", movePosition, 0.2f).SetTrans(Tween.TransitionType.Sine);
        }
        this.Position = GetViewport().GetMousePosition();
    }
}
