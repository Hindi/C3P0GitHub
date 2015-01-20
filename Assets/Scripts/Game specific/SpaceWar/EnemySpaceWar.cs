using UnityEngine;
using System.Collections;

public class EnemySpaceWar : Spaceship {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject target;

    private Vector2 targetPosition;

    float calcGaussfromUinforme(float u1, float u2)
    {
        //Debug.Log(-2 * Mathf.Log(u1) * Mathf.Cos(2 * Mathf.PI * u2));
        return Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Cos(2 * Mathf.PI * u2);
    }

    Vector2 tirageGaussien()
    {
        float f1, f2;
        float u1=0, u2=0;
        u1 = Random.value;
        u2 = Random.value;

        f1 = calcGaussfromUinforme(u1, u2);
        f2 = calcGaussfromUinforme(u2, u1);

        return new Vector2(f1, f2);
    }

	// Use this for initialization
	void Start () {
        Kalman k = new Kalman(new Vector4(0, 0, 0, 0), 1);
	}

    void addNoiseToPlayerPosition()
    {

    }

    void applyKalman()
    {

    }

    void estimateNextShotPoition()
    {
        targetPosition = player.transform.position;
        addNoiseToPlayerPosition();
        applyKalman();
    }

    void updateTargetPosition()
    {
        target.transform.position = targetPosition;
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 tirage = tirageGaussien();
        //Debug.Log(tirage.x + " " + tirage.y);
        estimateNextShotPoition();
        updateTargetPosition();
	}
}
