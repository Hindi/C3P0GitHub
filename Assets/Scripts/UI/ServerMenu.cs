using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerMenu : MonoBehaviour {


    [SerializeField]
    private GameObject questionCanvas;

    [SerializeField]
    private GameObject coursButtons;

    [SerializeField]
    private GameObject startGameButton;

    [SerializeField]
    private Text ipLabel;

    private int courseId;

    [SerializeField]
    private Text nextQuestionLabel;

	// Use this for initialization
	void Start () {
        ipLabel.text = "Server ip address : " + C3PONetwork.Instance.getMyIp();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (QuestionManager.Instance.isQuestionTimeOver())
        {
            coursButtons.SetActive(false);
            questionCanvas.SetActive(false);
            startGameButton.SetActive(true);
        }
	}

    public void loadXml(int id)
    {
        courseId = id;
        QuestionManager.Instance.loadXml(id);
        C3PONetworkManager.Instance.loadClientStats(id);
        switchToSendQuestion();
    }

    private void switchToSendQuestion()
    {
        coursButtons.SetActive(false);
        questionCanvas.SetActive(true);
        loadquestionText();
    }

    private void loadquestionText()
    {
        if (!QuestionManager.Instance.isQuestionTimeOver())
            nextQuestionLabel.text = QuestionManager.Instance.getQuestionTxt();
    }

    public void preiouvsQuestion()
    {
        QuestionManager.Instance.goToPreviousQuestion();
        loadquestionText();
    }

    public void nextquestion()
    {
        QuestionManager.Instance.goToNextQuestion();
        loadquestionText();
    }

    public void sendQuestion()
    {
        QuestionManager.Instance.sendQuestion();
        loadquestionText();
    }

    public void launchGame()
    {
        string levelName= "";
        int stateEnum = 0;

        if (courseId == 2 || courseId == 1)
        {
            levelName = "SpaceInvader";
            stateEnum = (int)StateEnum.SPACEINVADER;
        }
        C3PONetworkManager.Instance.loadLevel(levelName, stateEnum);
    }
}
