using UnityEngine;
using System.Collections;

public class MarioSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject goomba;
    [SerializeField]
    private GameObject mushroom;

    private float lastSpawnTime;

    [SerializeField]
    private float spawnCD;

    private int paramId;
    public int ParamId
    {
        get { return paramId; }
        set { paramId = value; }
    }

	// Use this for initialization
	void Start () {
        lastSpawnTime = Time.time;
	}

    private double getDeltaPos()
    {
        double ret;
        switch(paramId)
        {
            case 0:
                ret = Laws.uniforme(-4, 4);
                break;
            case 1:
                ret = Laws.gauss(0, 4);
                break;
            case 2:
                ret = 0;
                break;
            default:
                ret = 0;
                break;
        }

        if (ret > 4)
            ret = 4;

        return ret;
    }

    private void spawnSomething()
    {
        float pickRdm = Random.value;
        float dirRdm = Random.value;
        GameObject curObj;
        Vector3 spawnPos = new Vector3(transform.position.x + (float)getDeltaPos(), transform.position.y, transform.position.z);

        if(pickRdm > 0.5f)
        {
            curObj = ((GameObject)Instantiate(goomba, spawnPos, Quaternion.identity));
            if (dirRdm > 0.5f)
            {
                GoombaBehaviour goom = curObj.transform.GetChild(0).GetComponent<GoombaBehaviour>();
                goom.revertSpeed();
            }
        }
        else
        {
            curObj = ((GameObject)Instantiate(mushroom, spawnPos, Quaternion.identity));
            if (dirRdm > 0.5f)
            {
                MushroomBehaviour shroom = curObj.GetComponent<MushroomBehaviour>();
                shroom.changeDir(Vector3.left);
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.time - lastSpawnTime > spawnCD)
        {
            lastSpawnTime = Time.time;
            spawnSomething();
        }
	}
}
