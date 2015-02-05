using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Classe qui gère le chargement de niveau via le réseau.
 * A tester une fois le réseau fonctionnel.
 */

/// <summary>Class that load unity scenes.</summary>
public class LevelLoader : MonoBehaviour {

    /// <summary>The list of the objects that needs to stay alive on scene change.</summary>
    [SerializeField]
    private GameObject[] keepAliveList;

    /// <summary>A reference to the stateManager.</summary>
    [SerializeField]
    private StateManager stateManager;


    /// <summary>Monobehaviour Start function.</summary>
	void Start () {
        for (int i = 0; i < keepAliveList.Length; ++i )
            DontDestroyOnLoad(keepAliveList[i]);
	}

    /// <summary>Callback called when the LOADLEVEL event is triggered.</summary>
    /// <param name="levelName">The name of the level.</param>
    public void onLevelLoad(string levelName)
    {
        loadLevel(levelName);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>Starts the coroutine that loads a scene. Also notify the StateManager to load the corresponding state.</summary>
    /// <param name="levelName">The name of the level.</param>
    public void loadLevel(string levelName)
    {
        StartCoroutine(loadLevelCoroutine(levelName));
        stateManager.changeState(IdConverter.levelToState(levelName));
    }

    /// <summary>Coroutine that loads a Unity scene.</summary>
    /// <param name="levelName">The name of the level.</param>
    private IEnumerator loadLevelCoroutine(string levelName)
    {
        Application.LoadLevel(levelName);
		while(Application.isLoadingLevel)
			yield return 1;
    }
}
