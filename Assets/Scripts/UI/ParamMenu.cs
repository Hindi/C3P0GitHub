using UnityEngine;
using System.Collections;

public class ParamMenu : MonoBehaviour {

    [SerializeField]
    private ParameterManager paramManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onParamClick(int id)
    {
        paramManager.setParamId(id);
        paramManager.applyParameter();
        EventManager.Raise(EnumEvent.RESTARTGAME);  
        EventManager<bool>.Raise(EnumEvent.PAUSEGAME, false);
    }
}
