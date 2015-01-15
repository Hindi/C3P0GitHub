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

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadXml(int id)
    {
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
    }

    public void launchGame()
    {
        // lancer le RPC qui charge le jeu à la fois chez les clients et sur le serveur
    }
}
