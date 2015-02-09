using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidShip : MonoBehaviour {

    [SerializeField]
    private List<Transform> asteroidsTargetList;

    [SerializeField]
    private AsteroidNetwork asteroidNetwork;

    // Use this for initialization
    void Start()
    {

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
        Debug.Log("Je suis mourru");
        if (asteroidNetwork.isServer())
        {
            this.gameOver();
            asteroidNetwork.gameOver();
        }
    }

    public void gameOver()
    {

    }
}
