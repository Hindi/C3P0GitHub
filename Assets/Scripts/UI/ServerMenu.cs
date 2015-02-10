using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerMenu : MonoBehaviour {

    private class ScoreKeeper
    {
        public ScoreKeeper()
        {
            score = 0;
            paramId = 0;
            login = "";
        }

        public ScoreKeeper(int s, int p, string l)
        {
            score = s;
            paramId = p;
            login = l;
        }


        public int CompareTo(ScoreKeeper b)
        {
            return score.CompareTo(b.score);
        }

        int score;
        public int Score { get { return score; } set { score = value; } }
        int paramId;
        public int ParamId { get { return paramId; } set { paramId = value; } }
        string login;
        public string Login { get { return login; } set { login = value; } }
    }

    [SerializeField]
    private GameObject questionCanvas;

    [SerializeField]
    private Canvas adminCanvas;

    [SerializeField]
    private GameObject coursButtons;
    [SerializeField]
    private GameObject sendButtons;
    [SerializeField]
    private GameObject previousButton;
    [SerializeField]
    private GameObject nextButtons;
    [SerializeField]
    private GameObject startGameButton;

    //Answer stats
    [SerializeField]
    private GameObject stats;

    [SerializeField]
    private Text ipLabel;

    private int courseId;

    [SerializeField]
    private Text nextQuestionLabel;

    [SerializeField]
    private UI ui;

    [SerializeField]
    int scoreDisplayCount;

    float lastScoreListUpdateTime;

    [SerializeField]
    float scoreListUpdateCD;

    private bool startGame;
    private bool gameLaunched;

    //TOP SCORE
    [SerializeField]
    private GameObject scoreTopList;
    [SerializeField]
    private Text scoreLoginText;
    [SerializeField]
    private Text scoreParamText;
    [SerializeField]
    private Text scoreScoreText;
    private List<ScoreKeeper> scoreList;

	// Use this for initialization
	void Start () {
        ipLabel.text = "Server ip address : " + C3PONetwork.Instance.getMyIp();
        startGame = false;
        gameLaunched = false;

        scoreList = new List<ScoreKeeper>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if(Time.time - lastScoreListUpdateTime > scoreListUpdateCD)
        {
            updateScoreList();
        }
	}

    public void showStats()
    {
        try
        {
            System.Diagnostics.Process.Start(Application.dataPath + "Resources/html/lastQuestionStat.html");
        }
        catch 
        {

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
        scoreTopList.gameObject.SetActive(false);
        coursButtons.SetActive(false);
        questionCanvas.SetActive(true);
        previousButton.SetActive(true);
        sendButtons.SetActive(true);
        nextButtons.SetActive(true);
        startGameButton.SetActive(false);
        stats.SetActive(false);
        startGame = false;
        loadquestionText();
    }

    private void switchStartGame()
    {
        lastScoreListUpdateTime = Time.time;
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
        stats.SetActive(true);
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
        stats.SetActive(false);
        scoreTopList.gameObject.SetActive(true);
        previousButton.SetActive(false);
        gameLaunched = true;

        string levelName = IdConverter.courseToLevel(courseId);

        if (levelName == "Asteroids")
            EventManager<string>.Raise(EnumEvent.LOADLEVEL, "Asteroids");

        C3PONetworkManager.Instance.loadLevel(levelName);
        C3PONetworkManager.Instance.unlockGame(levelName);
    }

    public void addScore(int score, int paramId, string login)
    {
        ScoreKeeper sk = scoreList.Find(x => x.Login == login);
        if(sk != null)
        {
            if (sk.Score > score)
                return;
            else
            {
                int id = scoreList.FindIndex(x => x.Login == login);
                scoreList[id].Score = score;
                scoreList[id].ParamId = paramId;
            }
        }
        else
        {
            scoreList.Add(new ScoreKeeper(score, paramId, login));
        }
    }

    private void sortAndCutScoreList()
    {
        scoreList.Sort(delegate(ScoreKeeper p1, ScoreKeeper p2)
        {
            return p2.CompareTo(p1);
        });

        if (scoreList.Count > scoreDisplayCount)
            scoreList.RemoveRange(scoreDisplayCount, scoreList.Count - scoreDisplayCount);
    }
    
    private void writeScoreListOnScreen()
    {
        string loginTxt = "";
        string paramTxt = "";
        string scoreTxt = "";
        foreach(ScoreKeeper sk in scoreList)
        {
            loginTxt += sk.Login + "\n";
            paramTxt += sk.ParamId + "\n";
            scoreTxt += sk.Score + "\n";
        }
        scoreLoginText.text = loginTxt;
        scoreParamText.text = paramTxt;
        scoreScoreText.text = scoreTxt;
    }

    private void updateScoreList()
    {
        lastScoreListUpdateTime = Time.time;
        sortAndCutScoreList();
        writeScoreListOnScreen();
    }
}
