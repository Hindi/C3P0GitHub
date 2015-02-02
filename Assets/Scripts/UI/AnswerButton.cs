using UnityEngine;
using System.Collections;

/// <summary>This class display icon on the answer button depending on the result.</summary>
public class AnswerButton : MonoBehaviour {

    /// <summary>Icon displayed for the good answer.</summary>
    [SerializeField]
    private GameObject rightIcon;
    /// <summary>Icon displayed if he client answered a wrong answer.</summary>
    [SerializeField]
    private GameObject wrongIcon;
    /// <summary>Icon displayed when the client just answered and is waiting for the result.</summary>
    [SerializeField]
    private GameObject answeredIcon;

    /// <summary>Hide the three icons.</summary>
    /// <returns>void</returns>
    public void hideAllIcon()
    {
        rightIcon.SetActive(false);
        wrongIcon.SetActive(false);
        answeredIcon.SetActive(false);
    }

    /// <summary>Display the right icon.</summary>
    /// <returns>void</returns>
    public void setRight()
    {
        rightIcon.SetActive(true);
        wrongIcon.SetActive(false);
        answeredIcon.SetActive(false);
    }

    /// <summary>Display the wrong icon.</summary>
    /// <returns>void</returns>
    public void setWrong()
    {
        rightIcon.SetActive(false);
        wrongIcon.SetActive(true);
        answeredIcon.SetActive(false);
    }

    /// <summary>Display the answered icon.</summary>
    /// <returns>void</returns>
    public void setAnswered()
    {
        rightIcon.SetActive(false);
        wrongIcon.SetActive(false);
        answeredIcon.SetActive(true);
    }
}
