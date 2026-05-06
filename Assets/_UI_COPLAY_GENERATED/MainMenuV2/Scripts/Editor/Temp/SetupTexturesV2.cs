using UnityEditor;
using UnityEngine;

public class SetupTexturesV2
{
    public static void Execute()
    {
        string[] simpleSprites = {
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/Background.png",
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/Logo.png"
        };

        foreach (string path in simpleSprites)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
            }
        }

        string[] slicedSprites = {
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/StartButton.png",
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/NormalButton.png"
        };

        foreach (string path in slicedSprites)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteBorder = new Vector4(20, 20, 20, 20);
                importer.SaveAndReimport();
            }
        }
    }
}