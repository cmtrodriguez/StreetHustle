using UnityEditor;
using UnityEngine;

public class SetupTexturesV4
{
    public static void Execute()
    {
        string[] sprites = {
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/BackgroundClean.png",
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/IconPlay.png",
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/IconBook.png",
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/IconGear.png",
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/IconExit.png"
        };

        foreach (string path in sprites)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
            }
        }
    }
}