using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不受Time.timeScale影响的等待时间
/// </summary>
public class WaitForSecondsRealtime : CustomYieldInstruction
{
    private float waitTime;

    public override bool keepWaiting
    {
        get
        {
            return Time.realtimeSinceStartup < waitTime;
        }
    }

    public WaitForSecondsRealtime(float time)
    {
        waitTime = Time.realtimeSinceStartup + time;
    }
}
