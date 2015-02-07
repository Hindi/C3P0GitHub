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
    private const float questionTime = 5;

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

    void reset()
    {
        foreach(Button b in buttons)
            b.GetComponent<AnswerButton>().hideAllIcon();

        answerLabel.text = "";
        answered = false;
        timeBar.gameObject.SetActive(true);
        timeBar.init(questionTime);
        timeBar.updateValue(questionTime);
    }

    public void startQuestion()
    {
        reset();
        startTime = Time.time;
        C3PONetworkManager.Instance.sendRequestScore();
    }

	// Use this for initialization
	void Start () {
        answered = false;
        score = 0;
        timeBar.gameObject.SetActive(true);
        timeBar.init(questionTime);
        EventManager<string, int>.AddListener(EnumEvent.QUESTIONRESULT, onResultRecieved);
        EventManager<int>.AddListener(EnumEvent.SCOREUPDATEQA, onScoreUpdate);

	}

    public void onScoreUpdate(int s)
    {
        score = s;
        scoreLabel.text = score.ToString();
    }

    public void onResultRecieved(string rep, int resultId)
    {
        timeBar.gameObject.SetActive(false);
        lastGoodAnswerId = resultId;
        setQuestionText("");
        if (answered)
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
        if (!answered && id != -1)
        {
            lastAnswerId = id;
            buttons[id - 1].GetComponent<AnswerButton>().setAnswered();
            answerLabel.text = "En attente des réponses des autres étudiants.";
            EventManager<int>.Raise(EnumEvent.ANSWERSELECT, id);
            answered = true;
        }
    }
}
