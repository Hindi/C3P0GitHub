using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidShip : MonoBehaviour {

    [SerializeField]
    private List<Transform> asteroidsTargetList;

    [SerializeField]
    private AsteroidNetwork asteroidNetwork;

    private int hp;

    // Use this for initialization
    void Start()
    {
        hp = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 getTarget()
    {
        return asteroidsTargetList[(int)Random.Range(0, asteroidsTargetList.Count)].position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (asteroidNetwork.isServer())
        {
            this.hit();
        }
        // Bouger l'écran
    }


    public void hit()
    {
        hp--;
        if (hp <= 0)
        {
            asteroidNetwork.gameOver();
            hp = 3;
        }
        // Changer le sprite pour fissurer l'écran
    }
}
