using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class FilesBatchProcessUnit
{
    /// <summary>
    /// Asset文件如scriptableobject的批处理
    /// </summary>
    /// <typeparam name="T">要处理的文件类型</typeparam>
    /// <param name="action">该文件类型的具体处理方法</param>
    public static void AssetFilesProcess<T>(Action<T> action)
    {
        //获取文件
        var files = Selection.GetFiltered<T>(SelectionMode.DeepAssets);
        Debug.Log("已找到文件个数：" + files.Length);

        //处理
        try
        {
            for (int i = 0; i < files.Length; i++)
            {
                UnityEngine.Object asset = files[i] as UnityEngine.Object;
                EditorUtility.DisplayProgressBar("处理中", asset.name, i / (float)files.Length);
                action(files[i]);
                EditorUtility.SetDirty(asset);
            }
            AssetDatabase.SaveAssets();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            Debug.Log("操作结束");
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// prefab上组件的批处理
    /// </summary>
    public static void PrefabFilesProcess<T>(Action<T> action) where T : Component
    {
        //获取所有文件并筛选
        var files = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
        List<T> prefabs = new List<T>();
        foreach (var item in files)
        {
            if (item is GameObject)
            {
                T component = (item as GameObject).GetComponent(typeof(T)) as T;
                if (component != null)
                {
                    prefabs.Add(component);
                }
            }
        }
        Debug.Log("已找到文件个数：" + prefabs.Count);

        //处理
        try
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                EditorUtility.DisplayProgressBar("处理中", prefabs[i].gameObject.name, i / (float)files.Length);
                action(prefabs[i]);
                EditorUtility.SetDirty(prefabs[i]);
            }
            AssetDatabase.SaveAssets();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            Debug.Log("操作结束");
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Assets/文件操作/复制数据")]
    public static void Copy()
    {
        //PrefabFilesProcess((RandomGameObjectTable file) =>
        //{
        //    file.table.Clear();
        //    int count = file.gameObjects.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        file.table.Add(new SimplePairWithGameObjectInt(file.gameObjects[i], file.probability[i]));
        //    }
        //});
    }

    [MenuItem("Assets/文件操作/回赋数据")]
    public static void GetGameItem()
    {
        //PrefabFilesProcess((RandomGameObjectTable file) =>
        //{
        //    file.goList.Clear();
        //    int count = file.list.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        file.goList.Add(file.list[i]);
        //    }
        //});
    }

    [MenuItem("Assets/文件操作/保存")]
    public static void Save()
    {
        PrefabFilesProcess((RandomGameObjectTable file) => { });
    }
}