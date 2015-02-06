using System;
using UnityEngine;
using UnityEngine;
using System.Collections.Generic;

static class Laws
{
    private static float currentTime = Time.time;
    private static float period = 2*Mathf.PI;
    private static Uniform uniform;

    private static void updateTime()
    {
        currentTime += Time.deltaTime;
        if (currentTime > period)
            currentTime = 0;
    }

    public static void setUniformValues(double[] valuesList)
    {
        uniform = new Uniform(valuesList);
    }

    public static double discreteUniforme()
    {
        if (null != uniform)
        {
            return uniform.next();
        }
        else
            return 0;
    }

    public static double uniforme(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static double gauss(double moyenne = 0, double variance = 1)
    {
        return MathNet.Numerics.Distributions.Normal.Sample(moyenne, variance);
    }

    public static float sin()
    {
        updateTime();
        return (2 + Mathf.Abs(8 * Mathf.Sin(currentTime)));
    }

    public static double exponential(double lambda)
    {
        return MathNet.Numerics.Distributions.Exponential.Sample(lambda);
    }
}
