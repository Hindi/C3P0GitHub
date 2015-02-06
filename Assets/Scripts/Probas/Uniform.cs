using UnityEngine;
using System.Collections;

/// <summary>
/// A class used to store a list of values and generate random numbers from that list
/// </summary>
public class Uniform {

    private double[] _values;
    private int _size;
    private System.Random randGen;

    /// <summary>
    /// Creates the instance using the list of values that will be returned and a seed for random generation
    /// </summary>
    /// <param name="valuesList">List of values that will be returned</param>
    /// <param name="seed">Seed for random generation</param>
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

    /// <summary>
    /// Returns one of the values in the list randomly
    /// </summary>
    /// <returns>One of the values of the list</returns>
    public double next()
    {
        return (_values[(int) randGen.Next(0,_size)]);
    }

}
