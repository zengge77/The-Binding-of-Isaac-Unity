using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStateInfo
{
    public bool isDone;
    public SkillStateInfo() { isDone = false; }
}

public delegate IEnumerator SkillDelegate();
