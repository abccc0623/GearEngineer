using Godot;

public partial class CameraFadeOutEvent : Event
{
    private static ShaderMaterial shaderMaterial;
    private static ColorRect colorRect;
    private float radius;
    protected override void Start()
    {
        if (shaderMaterial == null || colorRect == null)
        {
            colorRect = GetNode<ColorRect>("FadeInOut/FadeInOutColorRect");
            shaderMaterial = colorRect.Material as ShaderMaterial;
        }
        
        Vector2 screenSize = node.GetViewport().GetVisibleRect().Size;
        Vector2 screenCenter = screenSize / 2f;
        Vector2 uvCenter = screenCenter / screenSize;
        
        radius = 1.0f;
        shaderMaterial.SetShaderParameter("center", uvCenter);
        shaderMaterial.SetShaderParameter("radius", 1.0f);
        CharacterController.BrakeInput(true);
    }

    protected override bool Update(float delta)
    {
        if (radius < -1.0f)
        {
            return true;
        }
        else
        {
            radius -= delta;
            shaderMaterial.SetShaderParameter("radius", radius);
        }
        return false;
    }

    protected override void End()
    {
       
    }
}
