using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject buttons;
	public GameObject optionLv;
	public Button btnPlay;
	public Button btnCaste;
	public Button btnLeaderBoard;
	public Button btnOption;
	public Button btnExit;

	public Button btnBack;


	// Use this for initialization
	void Start () {
		btnPlay.onClick.AddListener (() => { ButtonPlay_Onclick();	});
		btnBack.onClick.AddListener (() => { Button_Back_Onclick();	});
	
	}
	
	void ButtonPlay_Onclick()
	{
		buttons.SetActive (false);
		optionLv.SetActive (true);
	}
	void Button_Back_Onclick()
	{
		buttons.SetActive (true);
		optionLv.SetActive (false);
	}
}
