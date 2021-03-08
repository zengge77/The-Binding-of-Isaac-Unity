using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class FilesBatchProcessUnit
{
    /// <summary>
    /// 文件的批处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    public static void FilesBatchProcess<T>(Action<T> action) where T : class
    {
        var files = new List<T>();

        //判断T类型并获取文件
        if (typeof(T).IsSubclassOf(typeof(ScriptableObject)))
        {
            files.AddRange(Selection.GetFiltered<T>(SelectionMode.DeepAssets));
        }
        else if (typeof(T).IsSubclassOf(typeof(Component)))
        {
            var allFiles = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
            foreach (var item in allFiles)
            {
                if (item is GameObject)
                {
                    T component = (item as GameObject).GetComponent(typeof(T)) as T;
                    if (component != null)
                    {
                        files.Add(component);
                    }
                }
            }
        }
        else { Debug.Log("未知类型数据!"); }
        Debug.Log("已找到文件个数：" + files.Count);

        //处理
        try
        {
            for (int i = 0; i < files.Count; i++)
            {
                UnityEngine.Object asset = files[i] as UnityEngine.Object;
                EditorUtility.DisplayProgressBar("处理中", asset.name, i / (float)files.Count);
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

    //[MenuItem("Assets/文件操作/复制数据")]
    public static void Copy()
    {
        FilesBatchProcess((RoomLayout file) =>
        {

        });
    }

    //[MenuItem("Assets/文件操作/回赋数据")]
    public static void Backtrack()
    {
        FilesBatchProcess((RoomLayout file) =>
        {

        });
    }

    //[MenuItem("Assets/文件操作/保存")]
    public static void Save()
    {
        FilesBatchProcess((RandomGameObjectTable file) => { });
    }
}