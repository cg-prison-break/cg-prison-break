using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveRenderTextureToFile
{
    [MenuItem("Assets/Save RenderTexture to PNG")]
    public static void SaveRTToFile()
    {
        RenderTexture rt = Selection.activeObject as RenderTexture;

        if (rt == null)
        {
            Debug.LogError("No RenderTexture selected.");
            return;
        }

        // Backup active RenderTexture
        RenderTexture previousRT = RenderTexture.active;

        // Make RT active
        RenderTexture.active = rt;

        // Create Texture2D with alpha
        Texture2D tex = new Texture2D(
            rt.width,
            rt.height,
            TextureFormat.RGBA32,
            false,
            false
        );

        // Read pixels from RenderTexture
        tex.ReadPixels(
            new Rect(0, 0, rt.width, rt.height),
            0,
            0,
            false
        );
        tex.Apply();

        // ----- GAMMA / LINEAR FIX -----
        // Ensures lighting looks identical to Game View
        if (QualitySettings.activeColorSpace == ColorSpace.Linear)
        {
            Color[] pixels = tex.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].r = Mathf.LinearToGammaSpace(pixels[i].r);
                pixels[i].g = Mathf.LinearToGammaSpace(pixels[i].g);
                pixels[i].b = Mathf.LinearToGammaSpace(pixels[i].b);
            }
            tex.SetPixels(pixels);
            tex.Apply();
        }
        // --------------------------------

        // Restore previous RenderTexture
        RenderTexture.active = previousRT;

        // Encode to PNG
        byte[] png = tex.EncodeToPNG();
        Object.DestroyImmediate(tex);

        // Build path
        string assetPath = AssetDatabase.GetAssetPath(rt);
        string pngPath = Path.ChangeExtension(assetPath, ".png");

        File.WriteAllBytes(pngPath, png);
        AssetDatabase.ImportAsset(pngPath, ImportAssetOptions.ForceUpdate);

        Debug.Log($"RenderTexture exported correctly to:\n{pngPath}");
    }

    [MenuItem("Assets/Save RenderTexture to PNG", true)]
    public static bool SaveRTToFileValidation()
    {
        return Selection.activeObject is RenderTexture;
    }
}
