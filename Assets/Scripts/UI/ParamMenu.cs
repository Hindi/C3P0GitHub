using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParamMenu : MonoBehaviour {

    [SerializeField]
    private ParameterManager paramManager;

    [SerializeField]
    private Text explanationText;

    [SerializeField]
    private GameObject explanationObj;

    [SerializeField]
    private GameObject mobileInput;
    [SerializeField]
    private GameObject pcInput;

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

    public void onQuestionMarkClick(string s)
    {
        explanationObj.SetActive(true);
        if (s != "")
        {
            explanationText.text = s;
        }
    }

    public void onCloseExplanationClick()
    {
        explanationObj.SetActive(false);
    }

    public void onKeyInfoClick(bool b)
    {
        if (Application.isMobilePlatform)
        {
            mobileInput.SetActive(b);
        }
        else
        {
            pcInput.SetActive(b);
        }
    }
}
