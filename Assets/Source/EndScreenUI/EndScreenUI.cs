using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EndScreenUI : MonoBehaviour {

	#region Public Members
	
	public event UnityAction OnRestartButtonClick = delegate{};
	
	#endregion

	#region Private Members
	
	[SerializeField]
	private Text 	player1Avatar;
	[SerializeField]
	private Text 	player2Avatar;
	[SerializeField]
	private Button 	restartButton;
	
	#endregion

	#region Public Methods
	
	public void Initialize() {
		Debug.Log("Initializing End Screen");
		restartButton.onClick.AddListener(() => { OnRestartButtonClick(); });
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void SetWinnerPlayer1() {
		player1Avatar.gameObject.SetActive(true);
		player2Avatar.gameObject.SetActive(false);
	}

	public void SetWinnerPlayer2() {
		player1Avatar.gameObject.SetActive(false);
		player2Avatar.gameObject.SetActive(true);
	}
	
	#endregion
	
}
