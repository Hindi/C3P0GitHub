using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.LinearAlgebra.Double;

public class Kalman {

    /* Elements utilisés pour garder en mémoire */
    private Vector estimation;
    private Matrix P;

    /* Elements utilisés pour rajouter le bruit */
    private MathNet.Numerics.Distributions.Normal distrib = new MathNet.Numerics.Distributions.Normal();
    private Matrix Q;
    private Matrix sqrtQ;

    /* Elements utilisés pour le Kalman */
    private Matrix F;
    private Matrix H;
    private Matrix K;
    private Matrix R;

    /* Elements utilisés pour la visualisation (debug principalement) */
    public Vector2 posBruit; // Position bruitée : observation
    public Vector2 posInterp; // Position interprétée : après le filtre de Kalman

    public Kalman(Vector4 initVector, double noiseVariance)
    {
        // estimation
        estimation = new DenseVector(4);
        double[] storage = new double[4];
        storage[0] = initVector.w; storage[1] = initVector.x; storage[2] = initVector.y; storage[3] = initVector.z; 
        estimation.SetValues(storage);
        estimation = addNoise(estimation);

        // public estimation
        posInterp.x = initVector.w;
        posInterp.y = initVector.y;

        // public noised values
        posInterp.x = (float) estimation.At(0);
        posInterp.y = (float) estimation.At(1);

        // P precision
        P = new DenseMatrix(4, 4);
        F.SetRow(0, new double[] { 10, 0, 0, 0, });
        F.SetRow(1, new double[] { 0, 10, 0, 0, });
        F.SetRow(2, new double[] { 0, 0, 10, 0, });
        F.SetRow(3, new double[] { 0, 0, 0, 10, });

        // Q
        Q = new DenseMatrix(4, 4);
        Q.SetRow(0, new double[] {1/3.0, 1/2.0, 0, 0,});
        Q.SetRow(1, new double[] {1/2.0, 1, 0, 0,});
        Q.SetRow(2, new double[] { 0, 0, 1/3.0, 1/2.0, });
        Q.SetRow(3, new double[] { 0, 0, 1/2.0, 1, });
        Q.Multiply(noiseVariance);

        // sqrtQ
        sqrtQ = sqrtm(Q);

        // F
        F = new DenseMatrix(4, 4);
        F.SetRow(0, new double[] { 1, 1, 0, 0, });
        F.SetRow(1, new double[] { 0, 1, 0, 0, });
        F.SetRow(2, new double[] { 0, 0, 1, 1, });
        F.SetRow(3, new double[] { 0, 0, 0, 1, });

        // H
        F = new DenseMatrix(2, 4);
        F.SetRow(0, new double[] { 1, 0, 0, 0, });
        F.SetRow(1, new double[] { 0, 0, 1, 0, });

        // K
        K = new DenseMatrix(2, 2);

        // R
        R = new DenseMatrix(2, 2);
        R.SetRow(0, new double[] { 0, 0, });
        R.SetRow(1, new double[] { 0, 0, });

    }

    /* Interpole une valeur pour la position, en supposant aucun input du joueur, dans nbFrames frames */
    Vector2 interpolation(int nbFrames)
    {
        // TODO : à remplir
        return new Vector2(0,0);
    }

    /* Retourne le Vector4 tel que mis à jour par Kalman dans cet objet.
     * A priori pas utile, mais on sait jamais pour le debug */
    Vector4 getNoisedVector()
    {
        // TODO : à remplir
        return new Vector4(0, 0, 0, 0);
    }

    /* Prends une observation (pos.x, speed.x, pos.y, speed.y) déjà bruitée et y applique le Kalman */
    void addObservation(Vector4 obs)
    {
        // TODO : à remplir
        // Prédiction
        // Mise à jour
    }

    /*************************************************************************************************
     * Private used functions                                                                        *
     *************************************************************************************************/
    private Vector2 interpolationOnce()
    {

        return new Vector2(0, 0);
    }

    /* Square root of a matrix using eigenvectors and eigenvalues (assuming it can be applied, undefined if not) */
    private Matrix sqrtm(Matrix M)
    {
        MathNet.Numerics.LinearAlgebra.Matrix<double> sqrtM;
        // Calculating M^(1/2);
        Evd<double> eigenVs = M.Evd();

        DenseVector values = new DenseVector(4);
        double[] tempValues = new double[4];
        int i = 0;
        foreach (MathNet.Numerics.Complex c in eigenVs.EigenValues.ToArray())
        {
            tempValues[i] = c.Real;
            i++;
        }
        values.SetValues(tempValues);
        values.MapInplace(System.Math.Sqrt);

        DiagonalMatrix newValues = new DiagonalMatrix(4, 4, values.ToArray());
        sqrtM = (eigenVs.EigenVectors * newValues) * eigenVs.EigenVectors.Inverse();

        /* This is debug to see what's actually inside M^1/2 */
        /*
        for (int j = 0; j < sqrtM.RowCount; j++)
        {
            string message = "";
            for (int k = 0; k < sqrtM.ColumnCount; k++)
            {
                message += sqrtM.Row(k).At(j).ToString(null, null) + "   ";
            }
            Debug.Log(message);
        }
        */

        return (Matrix) sqrtM;
    }

    /* Prends le vecteur réel et en retourne une version artificiellement bruitée */
    private Vector addNoise(Vector oldVector)
    {
        double t1 = distrib.Sample();
        double t2 = distrib.Sample();
        double t3 = distrib.Sample();
        double t4 = distrib.Sample();

        Vector noise = new DenseVector(new double[] { t1, t2, t3, t4, });

        return ((Vector)(oldVector + noise));
    }
}
