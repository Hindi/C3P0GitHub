﻿using UnityEngine;
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

    [SerializeField]
    private Button param1;
    [SerializeField]
    private Button param2;
    [SerializeField]
    private Button param3;

	// Use this for initialization
    void Start()
    {
        EventManager<float>.AddListener(EnumEvent.GOODANSWERRATIO, goodAnswerRatio);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void goodAnswerRatio(float r)
    {
        Debug.Log(r);
        param2.interactable = true;
        param3.interactable = true;
        if (r < 0.33f)
            param2.interactable = false;
        if (r < 0.66f)
            param3.interactable = false;
    }

    public void onParamClick(int id)
    {
        paramManager.setParamId(id);
        paramManager.applyParameter();
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
