using UnityEditor;
using UnityEngine;

public class SetupTexturesV3
{
    public static void Execute()
    {
        string[] slicedSprites = {
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/StartButtonClean.png",
            "Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/NormalButtonClean.png"
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