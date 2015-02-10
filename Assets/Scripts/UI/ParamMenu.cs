using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script associated to the objects managing parameters for a game
/// </summary>
public class ParamMenu : MonoBehaviour {

    /// <summary>
    /// The parameter manager to notify when the user selects a new parameter
    /// </summary>
    [SerializeField]
    private ParameterManager paramManager;

    /// <summary>
    /// The explanation text in case we want to change it (most of the time we won't)
    /// </summary>
    [SerializeField]
    private Text explanationText;

    /// <summary>
    /// The panel containing the explanation text so we can activate/deactivate it
    /// </summary>
    [SerializeField]
    private GameObject explanationObj;

    /// <summary>
    /// The panel containing mobile inputs so we can activate/deactivate it
    /// </summary>
    [SerializeField]
    private GameObject mobileInput;
    /// <summary>
    /// The panel containing pc inputs so we can activate/deactivate it
    /// </summary>
    [SerializeField]
    private GameObject pcInput;

    /// <summary>
    /// The panel containing the first parameter so we can activate/deactivate it
    /// </summary>
    [SerializeField]
    private Button param1;
    /// <summary>
    /// The panel containing the second parameter so we can activate/deactivate it
    /// </summary>
    [SerializeField]
    private Button param2;
    /// <summary>
    /// The panel containing the third parameter so we can activate/deactivate it
    /// </summary>
    [SerializeField]
    private Button param3;

    /// <summary>
    /// The good answer/total questions ratio so we can choose which parameters are allowed to be picked
    /// </summary>
    private float answerRatio;
    public float AnswerRatio
    {
        get
        {
            return answerRatio;
        }
        set
        {
            answerRatio = value;
        }
    }

	// Use this for initialization
    void Start()
    {
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnEnable()
    {
        if(C3PONetwork.Instance.IsConnectedToTeacher)
            activateButtons(QuestionManager.Instance.AnswerRatio);
    }

    /// <summary>
    /// Activates the correct parameters depending on the good answer/total questions ratio
    /// </summary>
    /// <param name="r"></param>
    public void activateButtons(float r)
    {
        param2.interactable = true;
        param3.interactable = true;
        if (r < 0.33f)
        {
            param2.interactable = false;
        }
        if (r < 0.66f)
            param3.interactable = false;
    }

    /// <summary>
    /// Called when the player chooses a parameter
    /// </summary>
    /// <param name="id">0 to 2</param>
    public void onParamClick(int id)
    {
        paramManager.setParamId(id);
        paramManager.applyParameter();
        EventManager<bool>.Raise(EnumEvent.PAUSEGAME, false);
    }

    /// <summary>
    /// Called when the player clicks on the explanation button
    /// </summary>
    /// <param name="s">Optional explanation text if we want to make sure explanation is never empty</param>
    public void onQuestionMarkClick(string s)
    {
        GetComponent<GraphicRaycaster>().enabled = false;
        explanationObj.SetActive(true);
        if (s != "")
        {
            explanationText.text = s;
        }
    }

    /// <summary>
    /// Called when the player closes the explanation
    /// </summary>
    public void onCloseExplanationClick()
    {
        GetComponent<GraphicRaycaster>().enabled = true;
        explanationObj.SetActive(false);
    }

    /// <summary>
    /// Called when the player opens/closes the input explanation
    /// </summary>
    /// <param name="b">True if opening, false if closing</param>
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
