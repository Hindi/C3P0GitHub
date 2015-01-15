using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerMenu : MonoBehaviour {

    private List<QuestionManager.QuestionKeeper> questionList;
    private int questionNb = 0;

    [SerializeField]
    private Button sendQuestionButton;

    [SerializeField]
    private GameObject coursButtons;

    [SerializeField]
    private GameObject startGameButton;

    private bool questionTimeOver = false;
    private int courseId;

	// Use this for initialization
	void Start () {
        questionList = new List<QuestionManager.QuestionKeeper>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (questionTimeOver)
        {
            coursButtons.SetActive(false);
            startGameButton.SetActive(true);
        }
	}

    public void loadXml(int id)
    {
        courseId = id;
        TextAsset questionFile;
        questionFile = (TextAsset)UnityEngine.Resources.Load("xml/cours" + id);
        questionList = XmlHelpers.LoadFromTextAsset<QuestionManager.QuestionKeeper>(questionFile);
        questionNb = 0;

        switchToSendQuestion();
    }

    private void switchToSendQuestion()
    {
        coursButtons.SetActive(false);
        sendQuestionButton.gameObject.SetActive(true);
    }

    public void sendQuestion()
    {
        if (questionNb < questionList.Count)
        {
            QuestionManager.Instance.sendQuestion(questionList[questionNb]);
            questionNb++;
        }
        else
            questionTimeOver = true;
    }

    public void launchGame()
    {
        string levelName= "";
        int stateEnum = 0;
        //if(courseId == 1)
        {
            levelName = "SpaceInvader";
            stateEnum = (int)StateEnum.SPACEINVADER;
        }
        C3PONetworkManager.Instance.loadLevel(levelName, stateEnum);
    }
}
