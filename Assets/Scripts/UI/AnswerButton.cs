using UnityEngine;
using System.Collections;

public class AnswerButton : MonoBehaviour {

    [SerializeField]
    private GameObject rightIcon;
    [SerializeField]
    private GameObject wrongIcon;
    [SerializeField]
    private GameObject answeredIcon;

	// Use this for initialization
	void Start () {
	
	}
	
    public void setRight()
    {
        rightIcon.SetActive(true);
        wrongIcon.SetActive(false);
        answeredIcon.SetActive(false);
    }

    public void setWrong()
    {
        rightIcon.SetActive(false);
        wrongIcon.SetActive(true);
        answeredIcon.SetActive(false);
    }

    public void setAnswered()
    {
        rightIcon.SetActive(false);
        wrongIcon.SetActive(false);
        answeredIcon.SetActive(true);
    }
}
