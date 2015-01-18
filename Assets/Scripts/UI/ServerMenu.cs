using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerMenu : MonoBehaviour {


    [SerializeField]
    private Button sendQuestionButton;

    [SerializeField]
    private GameObject coursButtons;

    [SerializeField]
    private GameObject startGameButton;

    [SerializeField]
    private Text ipLabel;

    private int courseId;

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
            sendQuestionButton.gameObject.SetActive(false);
            startGameButton.SetActive(true);
        }
	}

    public void loadXml(int id)
    {
        courseId = id;
        QuestionManager.Instance.loadXml(id);
        switchToSendQuestion();
    }

    private void switchToSendQuestion()
    {
        coursButtons.SetActive(false);
        sendQuestionButton.gameObject.SetActive(true);
    }

    public void sendQuestion()
    {
        QuestionManager.Instance.sendQuestion();
    }

    public void launchGame()
    {
        string levelName= "";
        int stateEnum = 0;
        Debug.Log(courseId);
        if(courseId == 2)
        {
            levelName = "SpaceInvader";
            stateEnum = (int)StateEnum.SPACEINVADER;
        }
        C3PONetworkManager.Instance.loadLevel(levelName, stateEnum);
    }
}
