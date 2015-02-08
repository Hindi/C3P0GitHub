using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.LinearAlgebra.Double;

/// <summary>
/// Applies a Kalman filter (position + speed) on a 2Dimentional object
/// </summary>
public class Kalman {

    /* Elements utilisés pour garder en mémoire */
    /// <summary>
    /// Vector containing the estimated position/speed of the object after applying the Kalman filter
    /// </summary>
    private Vector estimation;
    /// <summary>
    /// Matrix used to apply the Kalman filter
    /// </summary>
    private Matrix P;

    /* Elements utilisés pour rajouter le bruit */
    /// <summary>
    /// Matrix used as a model for the noise on the object's trajectory
    /// </summary>
    private Matrix Q;
    private Matrix sqrtQ;

    /* Elements utilisés pour le Kalman */
    /// <summary>
    /// Trajectory matrix
    /// </summary>
    private Matrix F;
    /// <summary>
    /// Matrix turning a Position+Speed Vector into a Position Vector
    /// </summary>
    private Matrix H;
    /// <summary>
    /// Matrix used in the Kalman filter operation, doesn't actually need to be saved between iterations
    /// </summary>
    private Matrix K;
    /// <summary>
    /// Matrix used as a model for sensor imprecision
    /// </summary>
    private Matrix R;
    private Matrix sqrtR;
    /// <summary>
    /// The unit matrix
    /// </summary>
    private Matrix I;

    /* Elements utilisés pour la visualisation (debug principalement) */
    private Vector2 posBruit; // Position bruitée : observation
    /// <summary>
    /// Position of the object as modelled by the noise Q, may be used for graphical feedback
    /// </summary>
    public Vector2 PosBruit
    {
        get { return posBruit; }
        private set{}
    }
    private Vector2 posInterp; // Position interprétée : après le filtre de Kalman
    /// <summary>
    /// Contains the position interpreted by the Kalman filter
    /// </summary>
    public Vector2 PosInterp
    {
        get { return posInterp; }
        private set { }
    }
    private Vector2 posImprecise; // Position captée par les capteurs
    /// <summary>
    /// Position of the object after sensor imprecision, may be used for graphical feedback
    /// </summary>
    public Vector2 PosImprecise
    {
        get { return posImprecise; }
        private set { }
    }

    /// <summary>
    /// Creates a Kalman filter for the object
    /// </summary>
    /// <param name="initVector">Initial position of the object</param>
    /// <param name="noiseVariance">Variance used for Q, trajectory noise model</param>
    /// <param name="imprecisionVariance">Variance used for R, sensor imprecision model</param>
    public Kalman(Vector4 initVector, double noiseVariance, double imprecisionVariance)
    {
        // Q
        Q = new DenseMatrix(4, 4);
        Q.SetRow(0, new double[] { 1 / 3.0, 1 / 2.0, 0, 0, });
        Q.SetRow(1, new double[] { 1 / 2.0, 1, 0, 0, });
        Q.SetRow(2, new double[] { 0, 0, 1 / 3.0, 1 / 2.0, });
        Q.SetRow(3, new double[] { 0, 0, 1 / 2.0, 1, });
        Q =(Matrix) Q.Multiply(noiseVariance);

        // sqrtQ
        sqrtQ = sqrtm(Q);

        // estimation
        estimation = new DenseVector(4);
        double[] storage = new double[4];
        storage[0] = initVector.x; storage[1] = initVector.y; storage[2] = initVector.z; storage[3] = initVector.w; 
        estimation.SetValues(storage);
        estimation = addNoise(estimation);

        // public estimation
        posInterp.x = (float)estimation.At(0);
        posInterp.y = (float)estimation.At(2);

        // public noised values
        posBruit.x = (float) estimation.At(0);
        posBruit.y = (float) estimation.At(2);

        // P precision
        P = new DenseMatrix(4, 4);
        P.SetRow(0, new double[] { 10, 0, 0, 0, });
        P.SetRow(1, new double[] { 0, 1, 0, 0, });
        P.SetRow(2, new double[] { 0, 0, 10, 0, });
        P.SetRow(3, new double[] { 0, 0, 0, 1, });

        // F
        F = new DenseMatrix(4, 4);
        F.SetRow(0, new double[] { 1, 1, 0, 0, });
        F.SetRow(1, new double[] { 0, 1, 0, 0, });
        F.SetRow(2, new double[] { 0, 0, 1, 1, });
        F.SetRow(3, new double[] { 0, 0, 0, 1, });

        // H
        H = new DenseMatrix(2, 4);
        H.SetRow(0, new double[] { 1, 0, 0, 0, });
        H.SetRow(1, new double[] { 0, 0, 1, 0, });

        // K
        K = new DenseMatrix(4, 2);

        // R
        R = new DenseMatrix(2, 2);
        R.SetRow(0, new double[] { 1, 0, });
        R.SetRow(1, new double[] { 0, 1, });
        R = (Matrix) R.Multiply(imprecisionVariance);

        sqrtR = sqrtm(R);

        // I, matrice 4x4 identité
        I = new DenseMatrix(4, 4);
        I.SetRow(0, new double[] { 1, 0, 0, 0, });
        I.SetRow(1, new double[] { 0, 1, 0, 0, });
        I.SetRow(2, new double[] { 0, 0, 1, 0, });
        I.SetRow(3, new double[] { 0, 0, 0, 1, });
    }

    /* Interpole une valeur pour la position, en supposant aucun input du joueur, dans nbFrames frames */
    /// <summary>
    /// Tries to predict the object's position in nbFrames units of time.<br/>
    /// Currently does a pretty poor job at it.
    /// </summary>
    /// <param name="nbFrames">The number of units of time ahead we want to know the object's position</param>
    /// <returns>A position vector</returns>
    public Vector2 interpolation(int nbFrames)
    {
        Vector temp = new DenseVector(estimation.ToArray());
        for (int i = 0; i < nbFrames; i++)
        {
            temp =(Vector) (F * temp);
        }
        return new Vector2((float)temp.At(0),(float)temp.At(2));
    }

    /* Prends une observation (pos.x, speed.x, pos.y, speed.y) non bruitée, la bruite et estime les nouveaux paramètres */
    /// <summary>
    /// Takes a sensor-perfect observation as entry for a Kalman filter iteration, adds realistic sensor imperfection and updates the internal state accordingly
    /// </summary>
    /// <param name="obs">A noised sensor-perfect position+speed observation</param>
    public void addObservation(Vector4 obs)
    {
        Vector tempVector = new DenseVector(4);
        double[] storage = new double[4];
        storage[0] = obs.x; storage[1] = obs.y; storage[2] = obs.z; storage[3] = obs.w;
        tempVector.SetValues(storage);
        //tempVector = addNoise(tempVector);        
        
        // public noised values
        posBruit.x = (float)tempVector.At(0);
        posBruit.y = (float)tempVector.At(2);

        /****************** Prédiction ******************/
        // n(k|k-1) = F * n(k-1|k-1)
        Vector nkminus1 = (Vector) (F * estimation);

        // P(k|k-1) = F * P(k-1|k-1) * F^T + Q
        Matrix Pkminus1 = (Matrix) (F * P * F.Transpose() + Q);

        // Mise à jour
        // K = P(k|k-1) * H^T * (R + H * P(k|k-1) * H^T)^-1
        Matrix S = (Matrix)(R + H * Pkminus1 * H.Transpose());
        K = (Matrix)(Pkminus1 * H.Transpose() * S.Inverse());

        // P(k|k) = (I - KH) * P(k| k-1)
        P =(Matrix)((I - K * H) * Pkminus1);

        // n(k|k) = n(k|k-1) + K(y - H*n(k|k-1))
        Vector y = (Vector) (H * tempVector);
        y = addImprecision(y);
        posImprecise = new Vector2((float)y.At(0), (float)y.At(1));
        estimation = (Vector)(nkminus1 + K * (y - (H * nkminus1)));
        posInterp.x = (float)estimation.At(0);
        posInterp.y = (float)estimation.At(2);
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
    private Matrix sqrtm(Matrix M)
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

        return (Matrix) sqrtM;
    }

    /* Prends le vecteur réel et en retourne une version artificiellement bruitée */
    /// <summary>
    /// Adds artificial noise to a Position+Speed Vector according to Q
    /// </summary>
    /// <param name="oldVector">Position+Speed Vector</param>
    /// <returns>Noised Vector</returns>
    private Vector addNoise(Vector oldVector)
    {
        double t1 = Laws.gauss();
        double t2 = Laws.gauss();
        double t3 = Laws.gauss();
        double t4 = Laws.gauss();

        Vector noise = new DenseVector(new double[] { t1, t2, t3, t4, });

        return ((Vector)(oldVector + sqrtQ*noise));
    }

    /// <summary>
    /// Adds artificial sensor imprecision to a Position Vector according to R
    /// </summary>
    /// <param name="oldVector">Position Vector</param>
    /// <returns>Noised Vector</returns>
    private Vector addImprecision(Vector oldVector)
    {
        double t1 = Laws.gauss();
        double t2 = Laws.gauss();

        Vector imprecision = new DenseVector(new double[] { t1, t2, });

        return ((Vector)(oldVector + sqrtR * imprecision));
    }
}
