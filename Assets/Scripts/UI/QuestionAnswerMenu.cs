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
    private ProgressBar timeBar;

    private int lastAnswerId;
    private int lastGoodAnswerId;

    private int score;

    bool answered;
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
        buttons[lastAnswerId].GetComponent<AnswerButton>().hideAllIcon();
        buttons[lastGoodAnswerId].GetComponent<AnswerButton>().hideAllIcon();
        answerLabel.text = "";
        startTime = Time.time;
        C3PONetworkManager.Instance.sendRequestScore();
    }

	// Use this for initialization
	void Start () {
        answered = false;
        score = 0;
       // timeBar.init(questionTime, new Vector2(200, 50), new Vector2(150, 20));
        EventManager<string, int>.AddListener(EnumEvent.QUESTIONRESULT, onResultRecieved);
        EventManager<int>.AddListener(EnumEvent.SCOREUPDATE, onScoreUpdate);

	}

    public void onScoreUpdate(int s)
    {
        score = s;
        scoreLabel.text = score.ToString();
    }

    public void onResultRecieved(string rep, int resultId)
    {
        lastGoodAnswerId = resultId;
        setQuestionText("");
        buttons[lastAnswerId - 1].GetComponent<AnswerButton>().setWrong();
        buttons[resultId - 1].GetComponent<AnswerButton>().setRight();

        answerLabel.text = rep;
    }
	
	// Update is called once per frame
	void Update () {
        timeBar.updateValue(questionTime - (Time.time - startTime));
        if(Time.time - startTime > questionTime && !answered)
            answer(-1);
	}

    public void answer(int id)
    {
        if (!answered)
        {
            lastAnswerId = id;
            buttons[id - 1].GetComponent<AnswerButton>().setAnswered();
            answerLabel.text = "En attente des réponses des autres étudiants.";
            EventManager<int>.Raise(EnumEvent.ANSWERSELECT, id);
            answered = true;
        }
    }
}
