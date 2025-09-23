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
}
