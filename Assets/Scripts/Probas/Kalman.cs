using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra.Factorization;

public class Kalman {

    private MathNet.Numerics.Distributions.Normal distrib = new MathNet.Numerics.Distributions.Normal();
    private MathNet.Numerics.LinearAlgebra.Double.DenseMatrix Q;

    // k-1 Vector
    Vector4 oldEstimation;

    public Kalman(Vector4 initVector)
    {
        Q = new MathNet.Numerics.LinearAlgebra.Double.DenseMatrix(4, 4);

        // Calculating Q^(1/2);
        Evd<double> eigenVs = Q.Evd();
        MathNet.Numerics.LinearAlgebra.Complex.DenseVector values = (MathNet.Numerics.LinearAlgebra.Complex.DenseVector) eigenVs.EigenValues;
        MathNet.Numerics.Complex[] tab = values.ToArray();
    }

    /* Interpole une valeur pour la position, en supposant aucun input du joueur, dans nbFrames frames */
    Vector2 interpolation(int nbFrames)
    {
        // TODO : à remplir
        return new Vector2(0,0);
    }

    /* Prends le vecteur réel et en retourne une version artificiellement bruitée */
    Vector4 addNoise(Vector4 oldVector)
    {
        // TODO : à remplir
        return new Vector4(0, 0, 0, 0);
    }

    /* Retourne le Vector4 tel que mis à jour par Kalman dans cet objet.
     * A priori pas utile, mais on sait jamais pour le debug */
    Vector4 getNoisedVector()
    {
        // TODO : à remplir
        return oldEstimation;
    }

    /* Prends une observation (pos.x, speed.x, pos.y, speed.y) déjà bruitée et y applique le Kalman */
    void addObservation(Vector4 obs)
    {
        // TODO : à remplir
        // Prédiction
        // Mise à jour
    }

    /** Private used functions **/
    private Vector2 interpolationOnce()
    {

        return new Vector2(0, 0);
    }

    private Vector2 tirageGaussien()
    {
        float f1, f2;
        float u1 = 0, u2 = 0;
        u1 = Random.value;
        u2 = Random.value;

        f1 = calcGaussfromUniforme(u1, u2);
        f2 = calcGaussfromUniforme(u2, u1);

        return new Vector2(f1, f2);
    }

    private float calcGaussfromUniforme(float u1, float u2)
    {
        //Debug.Log(-2 * Mathf.Log(u1) * Mathf.Cos(2 * Mathf.PI * u2));
        return Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Cos(2 * Mathf.PI * u2);
    }
}
