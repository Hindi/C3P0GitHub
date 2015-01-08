using UnityEngine;
using System.Collections;

public class QuestionAnswerState : State
{
    private Canvas questionMenu;
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
        loaded = true;
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        c3poNetwork = networkManager.GetComponent<C3PONetwork>();
        c3poNetworkManager = networkManager.GetComponent<C3PONetworkManager>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        questionMenu = ui.getCurrentCanvas();
        QAMenu = questionMenu.GetComponent<QuestionAnswerMenu>();
    }

    public void onQuestionRecieved(QuestionManager.QuestionKeeper keeper)
    {
        questionMenu.gameObject.SetActive(true);
        QAMenu.setAnswerCount(keeper.choicesNb);
        QAMenu.setQuestionText(keeper.question);
        for (int i = 0; i < keeper.choicesNb; ++i)
            QAMenu.setAnswerText(i, "huk");
    }

    // Use this for initialization
    public override void start()
    {

    }

    public override void noticeInput(KeyCode key)
    {

    }
}
