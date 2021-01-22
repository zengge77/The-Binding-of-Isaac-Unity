using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureAssetProcessor : AssetPostprocessor
{
    [MenuItem("Assets/将文件夹下图片的过滤模式改为Point")]
    public static void SetAllTextureType()
    {
        //获取鼠标点击图片目录
        var arr = Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets);
        string folder = AssetDatabase.GetAssetPath(arr[0]);
        Debug.Log("Reimport Path:" + folder);

        //针对目录下的所有文件进行遍历 取出.png和.jpg文件进行处理 可自行拓展
        DirectoryInfo direction = new DirectoryInfo(folder);
        FileInfo[] pngFiles = direction.GetFiles("*.png", SearchOption.AllDirectories);
        FileInfo[] jpgfiles = direction.GetFiles("*.jpg", SearchOption.AllDirectories);

        try
        {
            SetTexture(pngFiles);
            SetTexture(jpgfiles);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    static void SetTexture(FileInfo[] fileInfo)
    {
        for (int i = 0; i < fileInfo.Length; i++)
        {
            //这里第一次写时有一个接口可直接调用，但是第二次写时找不到了 所以用了切割字符
            string fullpath = fileInfo[i].FullName.Replace("\\", "/");
            string path = fullpath.Replace(Application.dataPath, "Assets");
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            EditorUtility.DisplayProgressBar("批量处理图片", fileInfo[i].Name, i / (float)fileInfo.Length);
            SetTextureFormat(textureImporter);
        }
    }

    //设置图片格式
    static void SetTextureFormat(TextureImporter textureImporter)
    {
        //FilterMode更改为Point模式，保留像素风格
        if (textureImporter.filterMode != FilterMode.Point)
        {
            textureImporter.filterMode = FilterMode.Point;

            //保存
            EditorUtility.SetDirty(textureImporter);
            textureImporter.SaveAndReimport();
        }

        #region 其他设置
        //textureImporter.mipmapEnabled = false;
        //textureImporter.isReadable = false;
        //textureImporter.textureType = TextureImporterType.Sprite;
        //根据路径获得文件夹目录，设置图集的packagingTag
        //string AtlasName = new DirectoryInfo(Path.GetDirectoryName(textureImporter.assetPath)).Name;
        //textureImporter.spritePackingTag = AtlasName;
        //textureImporter.wrapMode = TextureWrapMode.Clamp;
        //textureImporter.npotScale = TextureImporterNPOTScale.None;
        //textureImporter.textureFormat = TextureImporterFormat.Automatic;
        //textureImporter.textureCompression = TextureImporterCompression.Compressed;
        #endregion

        #region 移动端单独设置
        ////Android端单独设置
        //TextureImporterPlatformSettings setting_android = new TextureImporterPlatformSettings();
        //setting_android.overridden = true;
        //setting_android.name = "Android";
        ////根据是否有透明度，选择RGBA还是RGB
        //if (textureImporter.DoesSourceTextureHaveAlpha())
        //    setting_android.format = TextureImporterFormat.ETC2_RGBA8;
        //else
        //    setting_android.format = TextureImporterFormat.ETC2_RGB4;

        //textureImporter.SetPlatformTextureSettings(setting_android);

        ////IOS端单独设置
        //TextureImporterPlatformSettings setting_iphone = new TextureImporterPlatformSettings();
        //setting_iphone.overridden = true;
        //setting_iphone.name = "iOS";
        ////根据是否有透明度，选择RGBA还是RGB
        //if (textureImporter.DoesSourceTextureHaveAlpha())
        //    setting_android.format = TextureImporterFormat.ASTC_RGBA_6x6;
        //else
        //    setting_android.format = TextureImporterFormat.ASTC_RGB_6x6;
        //textureImporter.SetPlatformTextureSettings(setting_iphone);
        #endregion
    }

}
