using Godot;
using System;

public partial class InGameSetting : Node
{
    public override void _Ready()
    {
        GD.Print("Applying Zelda-style project settings...");

        // -----------------------------
        // 1️⃣ 윈도우 해상도
        // -----------------------------
        ProjectSettings.SetSetting("display/window/size/width", 1920);
        ProjectSettings.SetSetting("display/window/size/height", 1080);

        // Stretch 모드와 Aspect
        ProjectSettings.SetSetting("display/window/stretch/mode", "viewport");
        ProjectSettings.SetSetting("display/window/stretch/aspect", "keep");

        // -----------------------------
        // 2️⃣ 안티에일리어싱
        // -----------------------------
        ProjectSettings.SetSetting("rendering/anti_aliasing/quality/msaa_3d", 4); // 4x MSAA
        ProjectSettings.SetSetting("rendering/anti_aliasing/quality/use_taa", true); // Temporal AA
        ProjectSettings.SetSetting("rendering/anti_aliasing/quality/use_fxaa", true); // FXAA

        // -----------------------------
        // 3️⃣ 텍스처 필터링 기본값
        // -----------------------------
        ProjectSettings.SetSetting("rendering/quality/filters/default_filter", true);   // Filter 켜기
        ProjectSettings.SetSetting("rendering/quality/filters/default_mipmaps", true); // Mipmaps 켜기

        // -----------------------------
        // 4️⃣ Tonemap & Exposure
        // -----------------------------
        ProjectSettings.SetSetting("rendering/quality/tonemap/mode", "ACES");
        ProjectSettings.SetSetting("rendering/quality/tonemap/exposure", 1.1f);
        ProjectSettings.SetSetting("rendering/quality/tonemap/saturation", 1.3f);
        ProjectSettings.SetSetting("rendering/quality/tonemap/contrast", 1.1f);
        ProjectSettings.SetSetting("rendering/quality/tonemap/white", 2.0f);

        // -----------------------------
        // 5️⃣ Debanding
        // -----------------------------
        ProjectSettings.SetSetting("rendering/quality/filters/use_debanding", true);

        GD.Print("Zelda-style project settings applied successfully!");
    }
}
