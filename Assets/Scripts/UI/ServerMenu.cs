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
    private GameObject sendButtons;
    [SerializeField]
    private GameObject nextButtons;
    [SerializeField]
    private GameObject startGameButton;

    [SerializeField]
    private Text ipLabel;

    private int courseId;

    [SerializeField]
    private Text nextQuestionLabel;

    [SerializeField]
    private UI ui;

    private bool startGame;
    private bool gameLaunched;

	// Use this for initialization
	void Start () {
        ipLabel.text = "Server ip address : " + C3PONetwork.Instance.getMyIp();
        startGame = false;
        gameLaunched = false;
	}
	
	// Update is called once per frame
    void Update()
    {
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
        sendButtons.SetActive(true);
        nextButtons.SetActive(true);
        startGameButton.SetActive(false);
        startGame = false;
        loadquestionText();
    }

    private void switchStartGame()
    {
        coursButtons.SetActive(false);
        sendButtons.SetActive(false);
        nextButtons.SetActive(false);
        startGameButton.SetActive(true);
        startGame = true;
    }

    private void loadquestionText()
    {
        if (!QuestionManager.Instance.isQuestionTimeOver())
            nextQuestionLabel.text = QuestionManager.Instance.getQuestionTxt();
    }

    public void preiouvsQuestion()
    {
        QuestionManager.Instance.goToPreviousQuestion();
        if (startGame)
            switchToSendQuestion();
        loadquestionText();
    }

    public void nextquestion()
    {
        if(QuestionManager.Instance.goToNextQuestion())
            loadquestionText();
        else
        {
            switchStartGame();
        }
    }

    public void sendQuestion()
    {
        QuestionManager.Instance.sendQuestion();
        if (QuestionManager.Instance.isQuestionTimeOver())
        {
            switchStartGame();
        }
        loadquestionText();
    }

    public void openAdminPanel()
    {
        adminCanvas.gameObject.GetComponent<AdminMenu>().setPreviousCanvas(ui.getcurrentCanvas());
        ui.updateCurrentCanvas(adminCanvas);
    }

    public void goBackToMainMenu()
    {
        coursButtons.SetActive(true);
        questionCanvas.SetActive(false);
        startGameButton.SetActive(false);
        QuestionManager.Instance.unloadXml();
        nextQuestionLabel.text = "";

        C3PONetworkManager.Instance.saveClientsStats();
        if (gameLaunched)
            C3PONetworkManager.Instance.saveClientsGameStats();
    }

    public void launchGame()
    {
        gameLaunched = true;
        string levelName= "";
        
        switch(courseId)
        {
            case 1:
                levelName = "SpaceInvader";
                break;
            case 2:
                levelName = "Tetris";
                break;
            case 3:
                levelName = "Pong";
                break;
        }
        C3PONetworkManager.Instance.loadLevel(levelName);
        C3PONetworkManager.Instance.unlockGame(levelName);
    }
}
