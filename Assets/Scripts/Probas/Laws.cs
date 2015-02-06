using System;
using UnityEngine;
using UnityEngine;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;

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

    public static double gauss(double moyenne = 0, double stddev = 1)
    {
        return MathNet.Numerics.Distributions.Normal.Sample(moyenne, stddev);
    }

    public static Vector2 gaussCorrelated(double moyenne = 0, double stddev = 1)
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
        
        return (new Vector2((float) vect.At(0), (float) vect.At(1));
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

    /*************************************************************************************************
     * Private used functions                                                                        *
     *************************************************************************************************/
    /* Square root of a matrix using eigenvectors and eigenvalues (assuming it can be applied, undefined if not) */
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

        /* This is debug to see what's actually inside M & M^1/2 */
        /*
        Debug.Log("Old matrix");
        for (int j = 0; j < M.RowCount; j++)
        {
            string message = "";
            for (int k = 0; k < M.ColumnCount; k++)
            {
                message += M.Row(j).At(k).ToString(null, null) + "   ";
            }
            Debug.Log(message);
        }
        Debug.Log("New matrix");
        for (int j = 0; j < sqrtM.RowCount; j++)
        {
            string message = "";
            for (int k = 0; k < sqrtM.ColumnCount; k++)
            {
                message += sqrtM.Row(j).At(k).ToString(null, null) + "   ";
            }
            Debug.Log(message);
        }
        */

        return (Matrix)sqrtM;
    }
}
