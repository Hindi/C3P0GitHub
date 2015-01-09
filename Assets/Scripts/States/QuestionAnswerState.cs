using UnityEngine;
using System.Collections;

public class QuestionAnswerState : State
{
    private Canvas questionMenu;
    private Canvas scoreMenu;
    private QuestionAnswerMenu QAMenu;

    protected bool loaded;
    private GameObject networkManager;
    private C3PONetwork c3poNetwork;
    private C3PONetworkManager c3poNetworkManager;
    private UI ui;

    private bool isThereAQuestion;

    public QuestionAnswerState(StateManager stateManager)
        : base(stateManager)
    {
    }

    public override void onLevelWasLoaded(int lvl)
    {
        EventManager<QuestionManager.QuestionKeeper>.AddListener(EnumEvent.QUESTIONRCV, onQuestionRecieved);
        EventManager<int>.AddListener(EnumEvent.ANSWERSELECT, onAnswerSelected);
        EventManager<QuestionManager.AnswerKeeper>.AddListener(EnumEvent.ANSWERRCV, onAnswerSend); 
        loaded = true;
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        c3poNetwork = networkManager.GetComponent<C3PONetwork>();
        c3poNetworkManager = networkManager.GetComponent<C3PONetworkManager>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        questionMenu = ui.QuestionCanvas;
        scoreMenu = ui.ScoreMenu;
        QAMenu = questionMenu.GetComponent<QuestionAnswerMenu>();
        testQuestion();
    }

    public void onAnswerSelected(int id)
    {
        c3poNetworkManager.sendAnswer(id);
    }

    private void testQuestion()
    {
        ui.updateCurrentCanvas(questionMenu);
        questionMenu.gameObject.SetActive(true);
        QAMenu.setAnswerCount(3);
        QAMenu.setQuestionText("ABWABWABWBABWABWBAWBA");
        QAMenu.setAnswerText(0, "huk");
        QAMenu.setAnswerText(1, "huuuuuuk");
        QAMenu.startQuestion();
    }

    public void onAnswerSend(QuestionManager.AnswerKeeper k)
    {
        ui.updateCurrentCanvas(scoreMenu);
    }

    public void onQuestionRecieved(QuestionManager.QuestionKeeper keeper)
    {
        ui.updateCurrentCanvas(questionMenu);
        questionMenu.gameObject.SetActive(true);
        QAMenu.setAnswerCount(keeper.choicesNb);
        QAMenu.setQuestionText(keeper.question);
        for (int i = 0; i < keeper.choicesNb; ++i)
            QAMenu.setAnswerText(i, "huk");
        QAMenu.startQuestion();
    }

    // Use this for initialization
    public override void start()
    {

    }

    public override void noticeInput(KeyCode key)
    {

    }
}
