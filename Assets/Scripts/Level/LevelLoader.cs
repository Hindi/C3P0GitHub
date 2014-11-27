using UnityEngine;
using System.Collections;

/**
 * Classe qui gère le chargement de niveau via le réseau.
 * A tester une fois le réseau fonctionnel.
 */ 

public class LevelLoader : MonoBehaviour {

    [SerializeField]
    private GameObject[] keepAliveList;

    private int lastLevelPrefix;
    private bool levelLoaded = false;

	// Use this for initialization
	void Start () {
	    lastLevelPrefix = 0;
        for (int i = 0; i < keepAliveList.Length; ++i )
            DontDestroyOnLoad(keepAliveList[i]);

        EventManager<string>.AddListener(EnumEvent.LOADLEVEL, onLevelLoad);
        EventManager.Raise(EnumEvent.LOADEND);
	}

    public void onLevelLoad(string levelName)
    {
        StartCoroutine(loadLevelCoroutine(levelName));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    [RPC]
    public void loadLevel(string level, int levelPrefix)
    {
        StartCoroutine(cloadLevel(level, levelPrefix));
    }

    private void OnLevelWasLoaded(int iLevel)
    {
        levelLoaded = true;
    }

    private IEnumerator loadLevelCoroutine(string levelName)
    {
        Application.LoadLevel(levelName);
        while (!levelLoaded)
            yield return 1;
        levelLoaded = false;
    }

    private IEnumerator cloadLevel(string levelName, int levelPrefix)
    {
            lastLevelPrefix = levelPrefix;
            
            // There is no reason to send any more data over the network on the default channel,
            // because we are about to load the level, thus all those objects will get deleted anyway
            Network.SetSendingEnabled(0, false);
            
            // We need to stop receiving because the level must be loaded first.
            // Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
            Network.isMessageQueueRunning = false;
            
            // All network views loaded from a level will get a prefix into their NetworkViewID.
            // This will prevent old updates from clients leaking into a newly created scene.
            Network.SetLevelPrefix(levelPrefix);
            Application.LoadLevel(levelName);
            
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            // Allow receiving data again
            Network.isMessageQueueRunning = true;
            // Now the level has been loaded and we can start sending out data to clients
            Network.SetSendingEnabled(0, true);
            
            
            EventManager.Raise(EnumEvent.LOADLEVEL);
    }
}
