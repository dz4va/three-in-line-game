using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour {

	#region Public Members
	
	public event UnityAction OnStartButtonClick = delegate{};
	
	#endregion
	
	#region Private Members
	
	[SerializeField]
	private Button 	startButton;
	
	#endregion

	#region Public Methods
	
	public void Initialize() {
		Debug.Log("Initializing Main Menu");
		startButton.onClick.AddListener(() => { OnStartButtonClick(); });
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}
	
	#endregion

}
