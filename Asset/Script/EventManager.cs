using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class EventManager : Node
{
    private Queue<Event> EventQueue;
    private static EventManager instance;
    
    public override void _Ready()
    {
        instance = this;
        EventQueue = new Queue<Event>();
        CreateCanvasLayer();
    }
    public override void _Process(double delta)
    {
        if(EventQueue.Count > 0)
        {
            var call = instance.EventQueue.First();
            call.InnerStart(this);
            call.InnerUpdate((float)delta);
            if(call.InnerEnd()) instance.EventQueue.Dequeue();
        }
    }

    public static void Play<T>() where T: Event, new()
    {
        instance.EventQueue.Enqueue(new T());
    }
    
    public static void Play<T>(List<object> p) where T: Event, new()
    {
        var newT = new T();
        newT.SetParameter(p);
        instance.EventQueue.Enqueue(newT);
    }


    void CreateCanvasLayer()
    {
        CanvasLayer canvasLayer = new CanvasLayer();
        ColorRect colorRect = new ColorRect();
        
        canvasLayer.AddChild(colorRect);
        this.AddChild(canvasLayer);
        canvasLayer.Layer = 10;
        canvasLayer.Name = "FadeInOut";
        colorRect.Name = "FadeInOutColorRect";
        
        colorRect.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        colorRect.SetOffsetsPreset(Control.LayoutPreset.FullRect);
        colorRect.Visible = true;
        colorRect.Material = GD.Load<ShaderMaterial>("res://GearEngineer/Asset/Shader/FadeOut.tres");
    }
}
