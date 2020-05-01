using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEditor.Sprites;

public class RoomEditorWindow : EditorWindow
{
    RoomLayout roomLayout;
    string newFileName = "我是文件名";

    bool isExpandCreateButton = false;
    bool isExpandCLoadButton = false;

    string[] roomLayoutFilePath = new[]
{
        "Assets/Resources/ScriptableObject/RoomLayout/NormalRoom",
        "Assets/Resources/ScriptableObject/RoomLayout/BossRoom",
        "Assets/Resources/ScriptableObject/RoomLayout/TreasureRoom",
        "Assets/Resources/ScriptableObject/RoomLayout/ShopRoom",
    };
    string[] roomLayoutAssetPath = new[]
    {
        "Assets/Resources/ScriptableObject/RoomLayout/NormalRoom/N-01-Dip4.asset",
        "Assets/Resources/ScriptableObject/RoomLayout/BossRoom/B-01-DukeOfFlies.asset",
        "Assets/Resources/ScriptableObject/RoomLayout/TreasureRoom/T-01-Fireplace.asset",
        "Assets/Resources/ScriptableObject/RoomLayout/ShopRoom/S-01.asset",
        "Assets/Editor Default Resources/TestRoom/T-Start.asset",
    };

    bool monsterFoldout = false;
    string[] monsterPrefabPath = new[]
    {
        "Assets/Prefabs/Monster/Minions/Dip.prefab",
        "Assets/Prefabs/Monster/Elite",
        "Assets/Prefabs/Monster/Boss/The Duke Of Flies.prefab",
    };

    bool obstaclesFoldout = false;
    string[] obstaclesPrefabPath = new[]
    {
        "Assets/Prefabs/Obstacles/Rock/Rock.prefab",
        "Assets/Prefabs/Obstacles/Spikes/Spikes.prefab",
        "Assets/Prefabs/Obstacles/FirePlace/FirePlace.prefab",
        "Assets/Prefabs/Obstacles/Poop/Poop.prefab",
    };

    bool propFoldout = false;
    string[] propPrefabPath = new[]
    {
        "Assets/Prefabs/Prop/Pickup/Chest/BrownChest.prefab",
        "Assets/Prefabs/Prop/RandomPickup/RandomCoin.prefab",
        "Assets/Prefabs/Prop/RandomItem/TreasureRoom Item.prefab",
        "Assets/Prefabs/Prop/Goods/ItemGoods.prefab",
    };

    bool elseFoldout = false;
    //用于缓存地板精灵和贴图，因为绘制地板需要读写新文件，开销过大
    Sprite floorSprite; Texture2D floorTexture;
    bool IsDrawRewardPosition = true;
    Sprite rewardSprite;


    [MenuItem("Window/房间布局编辑窗口")]
    static void ShowWindow()
    {
        EditorWindow window = GetWindow<RoomEditorWindow>("房间布局编辑器", true);
        window.minSize = window.maxSize = new Vector2(480, 615);
    }

    private void OnEnable()
    {
        rewardSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor Default Resources/RewardSprite.png");
    }

    private void OnGUI()
    {
        //文件操作
        FileOperation();

        //文件选择区域
        GUILayout.BeginVertical("box");
        roomLayout = (RoomLayout)EditorGUILayout.ObjectField("文件", roomLayout, typeof(RoomLayout), false);
        if (roomLayout != null) { GUILayout.Label(AssetDatabase.GetAssetPath(roomLayout)); }

        //文件不为空时绘制编辑区域
        if (roomLayout != null) { DrawEditArea(); }
        GUILayout.EndVertical();

        //文件不为空时绘制预览区域
        if (roomLayout != null) { DrawPreviewArea(); }
    }

    void FileOperation()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("打开文件"))
        {
            isExpandCLoadButton = !isExpandCLoadButton; isExpandCreateButton = false;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("创建文件"))
        {
            isExpandCreateButton = !isExpandCreateButton; isExpandCLoadButton = false;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("保存修改"))
        {
            if (roomLayout != null)
            {
                EditorUtility.SetDirty(roomLayout);
                AssetDatabase.SaveAssets();
            }
        }
        GUILayout.EndHorizontal();

        if (isExpandCLoadButton)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("普通")) { SelectObject(roomLayoutAssetPath[0]); isExpandCLoadButton = false; }
            if (GUILayout.Button("BOSS")) { SelectObject(roomLayoutAssetPath[1]); isExpandCLoadButton = false; }
            if (GUILayout.Button("宝藏")) { SelectObject(roomLayoutAssetPath[2]); isExpandCLoadButton = false; }
            if (GUILayout.Button("商店")) { SelectObject(roomLayoutAssetPath[3]); isExpandCLoadButton = false; }
            if (GUILayout.Button("测试")) { SelectObject(roomLayoutAssetPath[4]); isExpandCLoadButton = false; }
            GUILayout.EndHorizontal();
        }
        if (isExpandCreateButton)
        {
            GUILayout.BeginVertical();
            newFileName = EditorGUILayout.TextField(newFileName);
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("普通")) { CreateRoomLayoutFile(roomLayoutFilePath[0]); isExpandCreateButton = false; }
            if (GUILayout.Button("BOSS")) { CreateRoomLayoutFile(roomLayoutFilePath[1]); isExpandCreateButton = false; }
            if (GUILayout.Button("宝藏")) { CreateRoomLayoutFile(roomLayoutFilePath[2]); isExpandCreateButton = false; }
            if (GUILayout.Button("宝藏")) { CreateRoomLayoutFile(roomLayoutFilePath[3]); isExpandCreateButton = false; }
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(7);
    }

    void DrawEditArea()
    {
        GUILayout.Space(10);
        GUILayout.BeginVertical("box");
        EditObstacles();
        GUILayout.Space(5);
        EditMonster();
        GUILayout.Space(5);
        EditorProp();
        GUILayout.Space(5);
        EditElse();
        GUILayout.EndVertical();
    }
    void EditObstacles()
    {
        //折叠开关
        obstaclesFoldout = EditorGUILayout.Foldout(obstaclesFoldout, "障碍物列表");
        if (obstaclesFoldout)
        {
            //快速选择文件夹
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("岩石")) { SelectObject(obstaclesPrefabPath[0]); }
            if (GUILayout.Button("尖刺")) { SelectObject(obstaclesPrefabPath[1]); }
            if (GUILayout.Button("火堆")) { SelectObject(obstaclesPrefabPath[2]); }
            if (GUILayout.Button("屎堆")) { SelectObject(obstaclesPrefabPath[3]); }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            //根据文件绘制怪物和怪物坐标的编辑区域
            EditObjectList(roomLayout.obstaclesPreafab, roomLayout.obstaclesCoordinate, roomLayout.RemoveObstacles, roomLayout.AddObstacles);
        }
    }
    void EditMonster()
    {
        monsterFoldout = EditorGUILayout.Foldout(monsterFoldout, "怪物列表");
        if (monsterFoldout)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("普通")) { SelectObject(monsterPrefabPath[0]); }
            if (GUILayout.Button("精英")) { SelectObject(monsterPrefabPath[1]); }
            if (GUILayout.Button("Boss")) { SelectObject(monsterPrefabPath[2]); }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            EditObjectList(roomLayout.monsterPrefab, roomLayout.monsterCoordinate, roomLayout.RemoveMonster, roomLayout.AddMonster);
        }
    }
    void EditorProp()
    {
        propFoldout = EditorGUILayout.Foldout(propFoldout, "道具列表");
        if (propFoldout)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("箱子")) { SelectObject(propPrefabPath[0]); }
            if (GUILayout.Button("随机拾取物")) { SelectObject(propPrefabPath[1]); }
            if (GUILayout.Button("随机道具")) { SelectObject(propPrefabPath[2]); }
            if (GUILayout.Button("随机商品")) { SelectObject(propPrefabPath[3]); }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            EditObjectList(roomLayout.propPreafab, roomLayout.propCoordinate, roomLayout.RemoveProp, roomLayout.AddProp);
        }
    }
    void EditElse()
    {
        elseFoldout = EditorGUILayout.Foldout(elseFoldout, "其他");
        if (elseFoldout)
        {
            roomLayout.floor = (Sprite)EditorGUILayout.ObjectField("地板", roomLayout.floor, typeof(Sprite), false);
            GUILayout.BeginHorizontal();
            roomLayout.isGenerateReward = GUILayout.Toggle(roomLayout.isGenerateReward, "生成奖励品:");
            IsDrawRewardPosition = GUILayout.Toggle(IsDrawRewardPosition, "绘制奖励品:");
            GUILayout.Space(10);
            GUILayout.Label("X");
            int x = EditorGUILayout.IntSlider((int)roomLayout.RewardPosition.x, 1, 25);
            GUILayout.Label("Y");
            int y = EditorGUILayout.IntSlider((int)roomLayout.RewardPosition.y, 1, 13);
            roomLayout.RewardPosition = new Vector2(x, y);
            GUILayout.EndHorizontal();
        }
    }

    void DrawPreviewArea()
    {
        //绘制地板，当地板精灵不一样或为空时制作新的地板贴图，为空时创建空白贴图
        if (roomLayout.floor != null)
        {
            if (floorSprite == null || floorSprite != roomLayout.floor)
            {
                floorSprite = roomLayout.floor;
                floorTexture = GetFloorTexture(roomLayout.floor);
            }
            GUILayout.Box(floorTexture);
        }
        else { GUILayout.Box(new Texture2D(466, 310)); }

        //绘制房间内的物体
        if (Event.current.type == EventType.Repaint)
        {
            //绘制障碍物
            DrawObjectList(roomLayout.obstaclesPreafab, roomLayout.obstaclesCoordinate);
            //绘制怪物
            DrawObjectList(roomLayout.monsterPrefab, roomLayout.monsterCoordinate);
            //绘制道具
            DrawObjectList(roomLayout.propPreafab, roomLayout.propCoordinate);
            //绘制奖励位置
            if (IsDrawRewardPosition)
            {
                Vector2 coordinate = roomLayout.RewardPosition;
                DrawSprite(rewardSprite, coordinate);
            }
        }
    }

    /// <summary>
    /// 根据路径选择Asset下的文件或文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private UnityEngine.Object SelectObject(string path)
    {
        UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(path);
        if (obj == null) { Debug.Log("文件不存在： " + path); return null; }
        Selection.activeObject = obj;
        return obj;
    }

    /// <summary>
    /// 创建新的布局文件并选择该文件
    /// </summary>
    /// <param name="path"></param>
    private void CreateRoomLayoutFile(string path)
    {
        RoomLayout go = ScriptableObject.CreateInstance<RoomLayout>();
        string newPath = Path.Combine(path, newFileName + ".asset");
        AssetDatabase.CreateAsset(go, newPath);
        roomLayout = SelectObject(newPath) as RoomLayout;
    }

    /// <summary>
    /// 根据预制体和坐标绘制编辑行列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefabs"></param>
    /// <param name="coordinates"></param>
    /// <param name="remove"></param>
    /// <param name="add"></param>
    void EditObjectList<T>(List<T> prefabs, List<Vector2> coordinates, Action<int> remove, Action add) where T : UnityEngine.Object
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            GUILayout.BeginHorizontal();
            prefabs[i] = (T)EditorGUILayout.ObjectField(prefabs[i], typeof(T), false);
            GUILayout.Space(30);
            GUILayout.Label("X");
            int x = EditorGUILayout.IntSlider((int)coordinates[i].x, 1, 25);
            GUILayout.Space(10);
            GUILayout.Label("Y");
            int y = EditorGUILayout.IntSlider((int)coordinates[i].y, 1, 13);
            coordinates[i] = new Vector2(x, y);
            GUILayout.Space(10);
            if (GUILayout.Button("移除")) { remove(i); }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("添加", GUILayout.MaxWidth(75))) { add(); }
    }

    /// <summary>
    /// 根据预制体和坐标绘制预览图像
    /// </summary>
    /// <param name="prefabs"></param>
    /// <param name="coordinates"></param>
    void DrawObjectList(List<GameObject> prefabs, List<Vector2> coordinates)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i] != null)
            {
                Sprite sprite = prefabs[i].GetComponent<SpriteRenderer>().sprite;
                Vector2 coordinate = coordinates[i];
                DrawSprite(sprite, coordinate);
            }
        }
    }
    void DrawObjectList<T>(List<T> prefabs, List<Vector2> coordinates) where T : MonoBehaviour
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i] != null)
            {
                Sprite sprite = prefabs[i].GetComponent<SpriteRenderer>().sprite;
                Vector2 coordinate = coordinates[i];
                DrawSprite(sprite, coordinate);
            }
        }
    }

    /// <summary>
    /// 绘制单个精灵；需获取精灵的贴图，再根据精灵的Rect在贴图里截取区域绘制
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="coordinate"></param>
    void DrawSprite(Sprite sprite, Vector2 coordinate)
    {
        if (sprite == null) { Debug.Log("图片为空"); return; }
        //计算绘制的起点
        Vector2 outset = GUILayoutUtility.GetLastRect().position;
        Vector2 offset = new Vector2(55, 55);
        float widthPixels = 13.85f;
        float heightPixels = 14.57f;
        //绘制的位置=起点+偏移量+坐标-精灵的一半大小
        //起点(地板左上角,即该box的起点); 偏移量(去掉墙壁区域); 坐标(坐标*像素大小); 精灵的一半大小(精灵绘制起点位于左上角，减去精灵大小的一半使得显示时：精灵的中心等于前面计算的位置)
        Vector2 pos = outset + offset + new Vector2(widthPixels * coordinate.x, heightPixels * coordinate.y) - new Vector2(sprite.rect.width / 2, sprite.rect.height / 2);

        //设置绘制的位置和大小
        Rect displayArea = sprite.rect;
        float spriteW = displayArea.width;
        float spriteH = displayArea.height;
        Rect newRect = new Rect(pos, new Vector2(spriteW, spriteH));

        //因为4个参数大小要求为0-1之间，所以除以原贴图，得到比例
        var tex = sprite.texture;
        displayArea.xMin /= tex.width;
        displayArea.xMax /= tex.width;
        displayArea.yMin /= tex.height;
        displayArea.yMax /= tex.height;

        //三个参数分别为:绘制的位置和大小,原贴图，原贴图截取的区域
        GUI.DrawTextureWithTexCoords(newRect, tex, displayArea);
    }

    /// <summary>
    /// 制作参数的3个翻转贴图，并合为一张贴图返回
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    Texture2D GetFloorTexture(Sprite sprite)
    {
        //将传入的精灵制作为贴图，注意：需要精灵原贴图设置 高级：可读写
        var rect = sprite.rect;
        int width = (int)rect.width;
        int height = (int)rect.height;
        var texture = new Texture2D(width, height);
        var data = sprite.texture.GetPixels((int)rect.x, (int)rect.y, width, height);
        texture.SetPixels(data);
        texture.Apply(true);

        //制作3张翻转贴图
        var upRightTexture = new Texture2D(width, height);
        for (int i = 0; i < width; i++)
        {
            upRightTexture.SetPixels(i, 0, 1, height, texture.GetPixels(width - i - 1, 0, 1, height));
        }

        var downLeftTexture = new Texture2D(width, height);
        for (int i = 0; i < height; i++)
        {
            downLeftTexture.SetPixels(0, i, width, 1, texture.GetPixels(0, height - i - 1, width, 1));
        }

        var downRightTexture = new Texture2D(width, height);
        for (int i = 0; i < width; i++)
        {
            downRightTexture.SetPixels(i, 0, 1, height, downLeftTexture.GetPixels(width - i - 1, 0, 1, height));
        }

        //制作4合1的贴图并返回
        var newTexture = new Texture2D(width * 2, height * 2);
        newTexture.SetPixels(0, height, width, height, texture.GetPixels());
        newTexture.SetPixels(width, height, width, height, upRightTexture.GetPixels());
        newTexture.SetPixels(0, 0, width, height, downLeftTexture.GetPixels());
        newTexture.SetPixels(width, 0, width, height, downRightTexture.GetPixels());
        newTexture.Apply();
        return newTexture;
    }
}
