using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainLunarLander : MonoBehaviour {

    [SerializeField]
    List<GameObject> terrains;

    [SerializeField]
    List<GameObject> spawns;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void hideTerrains()
    {
        foreach(GameObject t in terrains)
            t.SetActive(false);
    }

    public void setTerrain(int id)
    {
        hideTerrains();
        terrains[id].SetActive(true);
    }

    public Vector3 getRandomSpawn()
    {
        return spawns[(int)Random.Range(0, spawns.Count)].transform.position;
    }
}
