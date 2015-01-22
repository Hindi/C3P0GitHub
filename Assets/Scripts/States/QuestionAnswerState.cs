using UnityEngine;
using System.Collections;

public class QuestionAnswerState : State
{
    private Canvas questionMenu;
    private Canvas scoreMenu;
    private QuestionAnswerMenu QAMenu;

    protected bool loaded;
    private GameObject networkManager;
    private UI ui;

    private bool isThereAQuestion;

    public QuestionAnswerState(StateManager stateManager)
        : base(stateManager)
    {
    }

    public override void onLevelWasLoaded(int lvl)
    {
        EventManager<QuestionManager.QuestionKeeper>.AddListener(EnumEvent.QUESTIONRCV, onQuestionRecieved);
        EventManager<string, bool>.AddListener(EnumEvent.QUESTIONRESULT, onResultRecieved);
        EventManager.AddListener(EnumEvent.DISCONNECTFROMUNITY, onDisconnectedFromUnity);
        EventManager<int>.AddListener(EnumEvent.ANSWERSELECT, onAnswerSelected);
        loaded = true;
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        questionMenu = ui.QuestionCanvas;
        scoreMenu = ui.ScoreMenu;
        QAMenu = questionMenu.GetComponent<QuestionAnswerMenu>();
    }

    public void onDisconnectedFromUnity()
    {
        stateManager_.changeState(StateEnum.CONNECTION);
        EventManager<string>.Raise(EnumEvent.LOADLEVEL, "Connection");
    }

    public void onResultRecieved(string rep, bool result)
    {
        ui.updateCurrentCanvas(scoreMenu);
    }

    public void onAnswerSelected(int id)
    {
        C3PONetworkManager.Instance.sendAnswer(id);
        ui.updateCurrentCanvas(scoreMenu);
    }

    public void onQuestionRecieved(QuestionManager.QuestionKeeper keeper)
    {
        ui.updateCurrentCanvas(questionMenu);
        questionMenu.gameObject.SetActive(true);
        QAMenu.setAnswerCount(keeper.reponses.Count);
        QAMenu.setQuestionText(keeper.question);
        for (int i = 0; i < keeper.reponses.Count; ++i)
            QAMenu.setAnswerText(i, keeper.reponses[i]);
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
