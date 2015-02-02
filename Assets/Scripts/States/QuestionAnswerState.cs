using UnityEngine;
using System.Collections;

public class QuestionAnswerState : State
{
    private Canvas questionMenu;
    private Canvas scoreMenu;
    private QuestionAnswerMenu QAMenu;

    protected bool loaded;
    private UI ui;

    private bool isThereAQuestion;

    /// <summary>Constructor.</summary>
    public QuestionAnswerState(StateManager stateManager)
        : base(stateManager)
    {
        EventManager<QuestionManager.QuestionKeeper>.AddListener(EnumEvent.QUESTIONRCV, onQuestionRecieved);
        EventManager.AddListener(EnumEvent.DISCONNECTFROMUNITY, onDisconnectedFromUnity);
        EventManager<int>.AddListener(EnumEvent.ANSWERSELECT, onAnswerSelected);
    }

    /// <summary>Called when the lobby scene from Unity is loaded.</summary>
    /// <param name="lvl">Id of the level loaded.</param>
    /// <returns>void</returns>
    public override void onLevelWasLoaded(int lvl)
    {
        loaded = true;
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        questionMenu = ui.QuestionCanvas;
        scoreMenu = ui.ScoreMenu;
        QAMenu = questionMenu.GetComponent<QuestionAnswerMenu>();
    }

    /// <summary>Called when the client is disconnected from the server.</summary>
    /// <returns>void</returns>
    public void onDisconnectedFromUnity()
    {
        if(loaded)
            EventManager<string>.Raise(EnumEvent.LOADLEVEL, "Connection");
    }

    /// <summary>Called when the client selects an answer.</summary>
    /// <param name="id">The id of the selected answer.</param>
    /// <returns>void</returns>
    public void onAnswerSelected(int id)
    {
        C3PONetworkManager.Instance.sendAnswer(id);
    }

    /// <summary>Called when a new question is recieved.</summary>
    /// <param name="keeper">The questionKeeper object with all of the datas related to the new question.</param>
    /// <returns>void</returns>
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

    /// <summary>Called on start.</summary>
    /// <returns>void</returns>
    public override void start()
    {

    }

    /// <summary>Called when leaving this state.</summary>
    /// <returns>void</returns>
    public override void end()
    {

    }

    /// <summary>Called each frame.</summary>
    /// <returns>void</returns>
    public override void update()
    {
    }


    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key)
    {

    }

    /// <summary>Recieves all the necessary inputs (keyboard, gamepad and mouse).</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
    public override void noticeInput(EnumInput key, Touch[] inputs)
    {

    }
}
