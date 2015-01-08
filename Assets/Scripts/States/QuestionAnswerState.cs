using UnityEngine;
using System.Collections;

public class QuestionAnswerState : State
{
    protected bool loaded;
    private GameObject networkManager;
    private C3PONetwork c3poNetwork;
    private C3PONetworkManager c3poNetworkManager;

    private bool isThereAQuestion;

    public QuestionAnswerState(StateManager stateManager)
        : base(stateManager)
    {
    }

    public override void onLevelWasLoaded(int lvl)
    {
        loaded = true;
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        c3poNetwork = networkManager.GetComponent<C3PONetwork>();
        c3poNetworkManager = networkManager.GetComponent<C3PONetworkManager>();
    }

    // Use this for initialization
    public override void start()
    {

    }

    public override void noticeInput(KeyCode key)
    {

    }

    public void OnGUI()
    {
        float width = Screen.width;
        float height = Screen.height;
        if (isThereAQuestion)
        {
            /*GUI.TextArea(new Rect(width / 2 - width / 6, 100.0f,
                                                    width * 2 / 6, height * 2 / 50),
                                                c3poNetworkManager.questionBuffer.question);
            if (GUI.Button(new Rect(width / 2 - width / 6, 200,
                                                    width * 2 / 6, height * 2 / 50),
                                                c3poNetworkManager.questionBuffer.rep1))
            {
                c3poNetworkManager.sendAnswer(1);
            }
            if (GUI.Button(new Rect(width / 2 - width / 6, 300,
                                                    width * 2 / 6, height * 2 / 50),
                                                c3poNetworkManager.questionBuffer.rep2))
            {
                c3poNetworkManager.sendAnswer(2);
            }*/
        }
    }
}
