using UnityEngine;
using System.Collections;

public class ParameterManager : MonoBehaviour {

    [SerializeField]
    private StateManager stateManager;

    private Parameter currentParameter;

	// Use this for initialization
	void Start () {
        currentParameter = new Parameter();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setParamId(int id)
    {
        currentParameter.id = id;
    }

    public void applyParameter()
    {
        stateManager.setParameter(currentParameter);
    }
}
