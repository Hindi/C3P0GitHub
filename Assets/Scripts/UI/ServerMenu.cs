using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerMenu : MonoBehaviour {


    [SerializeField]
    private GameObject questionCanvas;

    [SerializeField]
    private Canvas adminCanvas;

    [SerializeField]
    private GameObject coursButtons;

    [SerializeField]
    private GameObject startGameButton;

    [SerializeField]
    private Text ipLabel;

    private int courseId;

    [SerializeField]
    private Text nextQuestionLabel;

    [SerializeField]
    private UI ui;

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

    public void openAdminPanel()
    {
        adminCanvas.gameObject.GetComponent<AdminMenu>().setPreviousCanvas(ui.getcurrentCanvas());
        ui.updateCurrentCanvas(adminCanvas);
    }

    public void launchGame()
    {
        string levelName= "";
        int stateEnum = 0;
        
        switch(courseId)
        {
            case 1:
                levelName = "SpaceInvader";
                stateEnum = (int)StateEnum.SPACEINVADER;
                break;
            case 2:
                levelName = "Tetris";
                stateEnum = (int)StateEnum.TETRIS;
                break;
            case 3:
                levelName = "Pong";
                stateEnum = (int)StateEnum.PONG;
                break;
        }
        if (courseId == 1)
        {
        }
        C3PONetworkManager.Instance.loadLevel(levelName, stateEnum);
    }
}
