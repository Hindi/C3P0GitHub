//#define C3POStudent
#define C3POTeacher

using UnityEngine;
using System.Collections;

public class NetworkTestScript : MonoBehaviour {

	public C3PONetwork net;
	public C3PONetworkManager netm;
	
	private QuestionManager.AnswerKeeper lastAnswer;
	private bool isThereAnAnswer;
	private bool isThereAQuestion;
	
	private string questionBuffer;
	private string rep1Buffer;
	private string rep2Buffer;
	

	// Use this for initialization
	void Start () {
		net = C3PONetwork.Instance;
		netm = C3PONetworkManager.Instance;
		
		#if C3POTeacher
		net.createTeacherServer();
		#endif
		
		#if C3POStudent
		
		#endif
		
		questionBuffer = "";
		rep1Buffer = "";
		rep2Buffer = "";
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape))
			Application.Quit();
			
		#if C3POStudent
		if(!netm.isConnectedApp)
		{
			try
			{
				netm.connectToTeacher("raphael", "jesuisunmotdepasse");
			}
			catch(System.Exception)
			{
			
			}
		}
		#endif
		
	}
	
	void OnGUI()
	{
		float width = Screen.width;
		float height = Screen.height;
		
		#if C3POTeacher
		questionBuffer = GUI.TextArea(new Rect(width/2 - width /6, 100.0f,
												width * 2/6, height * 2/50),
											questionBuffer, 50);
		rep1Buffer = GUI.TextArea(new Rect(width/2 - width /6, 200,
												width * 2/6, height * 2/50),
											rep1Buffer, 50);
		rep2Buffer = GUI.TextArea(new Rect(width/2 - width /6, 300,
												width * 2/6, height * 2/50),
											rep2Buffer, 50);
		if(GUI.Button(new Rect(width/2 - width /6, 400,
												width * 2/6, height * 2/50), "GO"))
		{
			QuestionManager.Instance.sendQuestion(questionBuffer, rep1Buffer, rep2Buffer);
		}
		
		if(isThereAnAnswer)
		{
			GUI.TextArea(new Rect(width/2 - width /6, 600,
												width * 2/6, height * 2/50),
											((lastAnswer.intRep == 1) ? rep1Buffer : rep2Buffer), 50);
		}
		#endif
		
		#if C3POStudent
		if(isThereAQuestion)
		{
			GUI.TextArea(new Rect(width/2 - width /6, 100.0f,
													width * 2/6, height * 2/50),
												netm.questionBuffer.question);
			if(GUI.Button(new Rect(width/2 - width /6, 200,
													width * 2/6, height * 2/50),
												netm.questionBuffer.rep1))
			{
				netm.sendAnswer(1);
			}
			if(GUI.Button(new Rect(width/2 - width /6, 300,
													width * 2/6, height * 2/50),
												netm.questionBuffer.rep2))
			{
				netm.sendAnswer(2);
			}
		}
		#endif
	}
	
	void qReceived()
	{
		isThereAQuestion = true;
	}
	
	void aReceived(QuestionManager.AnswerKeeper a)
	{
		lastAnswer = a;
		isThereAnAnswer = true;
	}
	
	
}
