using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestionAnswerMenu : MonoBehaviour {

    [SerializeField]
    private Text question;

    [SerializeField]
    List<Button> buttons;

    [SerializeField]
    private Text scoreLabel;

    [SerializeField]
    private Text answerLabel;

    [SerializeField]
    private Text resultBoolLabel;

    [SerializeField]
    private ProgressBar timeBar;

    private int score;

    private float startTime;
    private const float questionTime = 10;

    public void setQuestionText(string q)
    {
        question.text = q;
    }

    public void setAnswerCount(int n)
    {
        for (int i = 0; i < buttons.Count; ++i)
        {
            if (i < n)
                buttons[i].gameObject.SetActive(true);
            else
                buttons[i].gameObject.SetActive(false);
        }
    }

    public void setAnswerText(int id, string text)
    {
        buttons[id].GetComponentInChildren<Text>().text = text;
    }

    public void startQuestion()
    {
        startTime = Time.time;
        C3PONetworkManager.Instance.sendRequestScore();
    }

	// Use this for initialization
	void Start () {
        score = 0;
        timeBar.init(questionTime, new Vector2(200, 50), new Vector2(150, 20));
        EventManager<string, bool>.AddListener(EnumEvent.QUESTIONRESULT, onResultRecieved);
        EventManager<int>.AddListener(EnumEvent.SCOREUPDATE, onScoreUpdate);

	}

    public void onScoreUpdate(int s)
    {
        score = s;
        Debug.Log(score);
        scoreLabel.text = score.ToString();
    }

    public void onResultRecieved(string rep, bool result)
    {
        if (result)
            resultBoolLabel.text ="Vrais :";
        else
            resultBoolLabel.text = "Faux :";
        
        answerLabel.text = rep;
    }
	
	// Update is called once per frame
	void Update () {
        timeBar.updateValue(questionTime - (Time.time - startTime));
        if(Time.time - startTime > questionTime)
            answer(-1);
	}

    public void answer(int id)
    {
        resultBoolLabel.text = "";
        answerLabel.text = "En attente des réponses des autres étudiants.";
        EventManager<int>.Raise(EnumEvent.ANSWERSELECT, id);
    }
}
