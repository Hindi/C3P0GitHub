using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidShip : MonoBehaviour {

    [SerializeField]
    private List<Transform> asteroidsTargetList;

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

    [RPC]
    void voteForTarget(int id)
    {

    }
}
