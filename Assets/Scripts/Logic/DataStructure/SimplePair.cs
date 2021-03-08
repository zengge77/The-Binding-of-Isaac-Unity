using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自定义泛型数据对
public class SimplePair<T1, T2>
{
    public T1 value1;
    public T2 value2;
    public SimplePair(T1 value1, T2 value2)
    {
        this.value1 = value1;
        this.value2 = value2;
    }
}

//要可视化操作必须使用具体类型
[System.Serializable]
public class SimplePairWithGameObjectInt : SimplePair<GameObject, int>
{
    public SimplePairWithGameObjectInt(GameObject value1, int value2) : base(value1, value2) { }
}

[System.Serializable]
public class SimplePairWithGameObjectVector2 : SimplePair<int, int>
{
    public SimplePairWithGameObjectVector2(int value1, int value2) : base(value1, value2) { }
}
