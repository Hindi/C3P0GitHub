using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainLunarLander : MonoBehaviour {

    [SerializeField]
    List<GameObject> terrains;

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
        Debug.Log(id);
        hideTerrains();
        terrains[id].SetActive(true);
    }
}
