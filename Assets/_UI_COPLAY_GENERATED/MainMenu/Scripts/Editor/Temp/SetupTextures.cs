using UnityEditor;
using UnityEngine;

public class SetupTextures
{
    public static void Execute()
    {
        string bgPath = "Assets/_UI_COPLAY_GENERATED/MainMenu/Sprites/Background.png";
        TextureImporter bgImporter = AssetImporter.GetAtPath(bgPath) as TextureImporter;
        if (bgImporter != null)
        {
            bgImporter.textureType = TextureImporterType.Sprite;
            bgImporter.SaveAndReimport();
        }

        string btnPath = "Assets/_UI_COPLAY_GENERATED/MainMenu/Sprites/Button.png";
        TextureImporter btnImporter = AssetImporter.GetAtPath(btnPath) as TextureImporter;
        if (btnImporter != null)
        {
            btnImporter.textureType = TextureImporterType.Sprite;
            btnImporter.spriteBorder = new Vector4(30, 30, 30, 30);
            btnImporter.SaveAndReimport();
        }
    }
}