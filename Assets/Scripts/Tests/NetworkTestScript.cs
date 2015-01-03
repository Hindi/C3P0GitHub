#define C3POStudent
//#define C3POTeacher

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
		if(GUI.Button(new Rect(width/2 - width /6, 400,
												width * 2/6, height * 2/50), "Server Started!"))
		{
		}
		#endif
		
		#if C3POStudent
		if(netm.isConnectedApp)
		{
			if(GUI.Button(new Rect(width/2 - width /6, 200,
													width * 2/6, height * 2/50),
												"I am connected YAY!"))
			{
				netm.sendAnswer(1);
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
