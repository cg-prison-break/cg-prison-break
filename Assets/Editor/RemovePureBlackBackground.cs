using UnityEngine;
using UnityEditor;
using System.IO;

public class RemovePureBlackBackground
{
    [MenuItem("Assets/Remove Pure Black Background")]
    public static void RemovePureBlack()
    {
        Texture2D tex = Selection.activeObject as Texture2D;
        if (tex == null)
        {
            Debug.LogError("Select a Texture2D (PNG).");
            return;
        }

        string path = AssetDatabase.GetAssetPath(tex);

        // Make texture readable
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
        importer.isReadable = true;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.SaveAndReimport();

        Color32[] pixels = tex.GetPixels32();

        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 c = pixels[i];

            // EXACT equivalent of:
            // np.all(rgba[:, :, :3] == [0, 0, 0])
            if (c.r == 0 && c.g == 0 && c.b == 0)
            {
                c.a = 0;
                pixels[i] = c;
            }
        }

        Texture2D outTex = new Texture2D(
            tex.width,
            tex.height,
            TextureFormat.RGBA32,
            false
        );

        outTex.SetPixels32(pixels);
        outTex.Apply();

        string outPath = Path.ChangeExtension(path, null) + "_alpha.png";
        File.WriteAllBytes(outPath, outTex.EncodeToPNG());
        AssetDatabase.ImportAsset(outPath);

        Object.DestroyImmediate(outTex);

        Debug.Log("Pure black background removed:\n" + outPath);
    }
}