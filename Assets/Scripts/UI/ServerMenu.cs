using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerMenu : MonoBehaviour {

    private List<QuestionManager.QuestionKeeper> questionList;
    private int questionNb = 0;

	// Use this for initialization
	void Start () {
        loadXml();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadXml()
    {
        TextAsset questionFile;
        questionFile = (TextAsset)UnityEngine.Resources.Load("xml/questions");
        questionList = XmlHelpers.LoadFromTextAsset<QuestionManager.QuestionKeeper>(questionFile);
        questionNb = 0;
    }

    public void sendQuestion()
    {
        if (questionNb < questionList.Count)
        {
            QuestionManager.Instance.sendQuestion(questionList[questionNb]);
        }
    }

    public void launchGame()
    {
        // lancer le RPC qui charge le jeu à la fois chez les clients et sur le serveur
    }
}
