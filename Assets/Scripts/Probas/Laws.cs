using System;
using UnityEngine;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;

/// <summary>
/// Static class used to provide all the probabilistic needs of this project. <br/>
/// The only calls to external random functions should be made from within this class.
/// </summary>
static class Laws
{
    /// <summary>
    /// The time seed can be used for random number generation
    /// </summary>
    private static float currentTime = Time.time;

    /// <summary>
    /// Maximum number 'currentTime' can reach
    /// </summary>
    private static float period = 2*Mathf.PI;
    private static Uniform uniform;

    /// <summary>
    /// Updates the currentTime taking period into account
    /// </summary>
    private static void updateTime()
    {
        currentTime += Time.deltaTime;
        if (currentTime > period)
            currentTime = 0;
    }

    /// <summary>
    /// Set values returned by the discreteUniforme function <br/>
    /// Only one set can be used at a time, because this project assumes it is used as a unique game mechanic
    /// </summary>
    /// <param name="valuesList">The list containing all values</param>
    public static void setUniformValues(double[] valuesList)
    {
        uniform = new Uniform(valuesList);
    }

    /// <summary>
    /// Returns one of the values from the list set by setUniformValues, each value having an equal probability
    /// </summary>
    /// <returns></returns>
    public static double discreteUniforme()
    {
        if (null != uniform)
        {
            return uniform.next();
        }
        else
            return 0;
    }
    
    /// <summary>
    /// Returns a value between min (included) and max (excluded)<br/>
    /// Current implementation only returns int casted as doubles
    /// </summary>
    /// <param name="min">Minimum of the range</param>
    /// <param name="max">Maximum of the range</param>
    /// <returns>A random value between min and max</returns>
    public static double uniforme(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// Returns a randomly generated value according to a normal distribution
    /// </summary>
    /// <param name="moyenne">The mean of the normal distribution</param>
    /// <param name="stddev">The standard deviation of the normal distribution</param>
    /// <returns>A randomly generated value</returns>
    public static double gauss(double moyenne = 0, double stddev = 1)
    {
        return MathNet.Numerics.Distributions.Normal.Sample(moyenne, stddev);
    }

    /// <summary>
    /// Returns two correlated randomly generated values according to a gauss(0,1)
    /// </summary>
    /// <param name="var">The stddev of the correlation</param>
    /// <returns>A vector2 made of two correlated randomly generated values</returns>
    public static Vector2 gaussCorrelated(double stddev = 1)
    {
        double t1 = Laws.gauss();
        double t2 = Laws.gauss();

        Vector vect = new DenseVector(new double[] { t1, t2, });

        Matrix R = new DenseMatrix(2, 2);
        R.SetRow(0, new double[] { 1, 0, });
        R.SetRow(1, new double[] { 0, 1, });
        R = (Matrix)R.Multiply(Math.Pow(stddev, 2));

        Matrix sqrtR = sqrtm(R);

        vect = (Vector) (sqrtR * vect);
        
        return (new Vector2((float) vect.At(0), (float) vect.At(1)));
    }

    /// <summary>
    /// Returns a randomly generated value according to a Sin distribution
    /// </summary>
    /// <returns>A randomly generated value</returns>
    public static float sin()
    {
        updateTime();
        return (2 + Mathf.Abs(8 * Mathf.Sin(currentTime)));
    }
    
    /// <summary>
    /// Returns a randomly generated value according to an exponential distribution
    /// </summary>
    /// <param name="lambda">The parameter of the exponential distribution</param>
    /// <returns>A randomly generated value</returns>
    public static double exponential(double lambda)
    {
        return MathNet.Numerics.Distributions.Exponential.Sample(lambda);
    }

    /// <summary>
    /// Returns a randomly generated value according to a poisson distribution
    /// </summary>
    /// <param name="lambda">The parameter of the exponential distribution</param>
    /// <returns>A randomly generated value</returns>
    public static float poisson(double lambda)
    {
        return MathNet.Numerics.Distributions.Poisson.Sample(lambda);
    }

    /// <summary>
    /// Returns a randomly generated value according to a binomial distribution
    /// </summary>
    /// <param name="param">The parameter of the exponential distribution</param>
    /// <param name="nb">The number of iterations used in the binomial distribution</param>
    /// <returns>A randomly generated number</returns>
    public static float binom(double param, int nb)
    {
        return MathNet.Numerics.Distributions.Binomial.Sample(param, nb);
    }

    /*************************************************************************************************
     * Private used functions                                                                        *
     *************************************************************************************************/
    /* Square root of a matrix using eigenvectors and eigenvalues (assuming it can be applied, undefined if not) */
    /// <summary>
    /// Square root of a matrix using eigenvectors and eigenvalues (assuming it can be applied, undefined if not)
    /// </summary>
    /// <param name="M">A Matrix</param>
    /// <returns>The square root of M if applicable</returns>
    private static Matrix sqrtm(Matrix M)
    {
        MathNet.Numerics.LinearAlgebra.Matrix<double> sqrtM;
        // Calculating M^(1/2);
        Evd<double> eigenVs = M.Evd();

        DenseVector values = new DenseVector(M.RowCount);
        double[] tempValues = new double[M.RowCount];
        int i = 0;
        foreach (MathNet.Numerics.Complex c in eigenVs.EigenValues.ToArray())
        {
            tempValues[i] = c.Real;
            i++;
        }
        values.SetValues(tempValues);
        values.MapInplace(System.Math.Sqrt);

        DiagonalMatrix newValues = new DiagonalMatrix(M.RowCount, M.RowCount, values.ToArray());
        sqrtM = (eigenVs.EigenVectors * newValues) * eigenVs.EigenVectors.Inverse();

        return (Matrix)sqrtM;
    }
}
