using UnityEngine;
using System.Collections;

public class Uniform {

    private double[] _values;
    private int _size;
    private System.Random randGen;

    public Uniform(double[] valuesList, int seed = 0)
    {
        _values = valuesList;
        _size = _values.Length;
        if (seed != 0)
        {
            randGen = new System.Random(seed);
        }
        else
        {
            randGen = new System.Random();
        }
    }

    public double next()
    {
        return (_values[(int) randGen.Next(0,_size)]);
    }

}
