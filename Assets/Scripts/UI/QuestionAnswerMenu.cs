using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestionAnswerMenu : MonoBehaviour {

    [SerializeField]
    private Text question;

    [SerializeField]
    List<Button> buttons;

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void answer(int id)
    {
        Debug.Log(id);
    }
}
