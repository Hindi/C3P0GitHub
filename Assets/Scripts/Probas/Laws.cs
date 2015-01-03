using System;
using UnityEngine;
using System.Collections.Generic;

static class Laws
{
    private static float currentTime = Time.time;
    private static float period = 2*Mathf.PI;

    private static void updateTime()
    {
        currentTime += Time.deltaTime;
        if (currentTime > period)
            currentTime = 0;
    }


    public static float uniforme()
    {
        updateTime();
        if (currentTime > 0 && currentTime < Mathf.PI / 2)
            return 3;
        else if (currentTime > Mathf.PI / 2 && currentTime < 1.5f * Mathf.PI)
            return 10;
        else
            return 6;
    }

    public static float sin()
    {
        updateTime();
        return (2 + Mathf.Abs(8 * Mathf.Sin(currentTime)));
    }
}
