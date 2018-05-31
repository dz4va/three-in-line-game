using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GameUI : MonoBehaviour {
	
	#region Private Members
	
	[Header("Grid & Connections")]
	[SerializeField]
	private SlotsGrid 		slotsGrid;
	[SerializeField]
	private GridConnections gridConnections;

	[Header("Player Avatars & Colors")]
	[SerializeField]
	private Text			player1AvatarText;
	[SerializeField]
	private Color			player1Color;
	[SerializeField]
	private Color			player1DisabledColor;
	[SerializeField]
	private Text			player2AvatarText;
	[SerializeField]
	private Color			player2Color;
	[SerializeField]
	private Color			player2DisabledColor;
	
	#endregion

	#region Public Methods
	
	public void Initialize() {
		Debug.Log("Initializing Game UI");
		slotsGrid.Initialize();
	}

	public void AddListenerOnSlotClick(UnityAction<int> action) {
		slotsGrid.OnSlotClick += action;
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void OccupySlotWithPlayer1Color(int slotIndex) {
		slotsGrid.SetColor(slotIndex, player1Color);
	}

	public void OccupySlotWithPlayer2Color(int slotIndex) {
		slotsGrid.SetColor(slotIndex, player2Color);
	}

	public void SetSlotWithIndexEmpty(int slotIndex) {
		slotsGrid.SetDefaultColor(slotIndex);
	}

	public bool ConnectionExists(int firstSlotIndex, int secondSlotIndex) {
		return gridConnections.CheckForConnection(firstSlotIndex, secondSlotIndex);
	}

	public void ColorizeConnections(int slotIndex, int[] emptyNeighbors) { 
		foreach (int connectionIndex in emptyNeighbors) {
			gridConnections.Colorize(slotIndex, connectionIndex);
		}
	}

	public void DecolorizeConnections(int slotIndex, int[] emptyNeighbors) {
		foreach (int connectionIndex in emptyNeighbors) {
			gridConnections.Decolorize(slotIndex, connectionIndex);
		}
	}

	public void IncreaseShadowForSlot(int slotIndex) {
		slotsGrid.IncreaseShadowForSlot(slotIndex);
	}

	public void DecreaseShadowForSlot(int slotIndex) {
		slotsGrid.DescreaseShadowForSlot(slotIndex);
	}

	public void TogglePlayer1Turn() {
		player1AvatarText.color = player1Color;
		player2AvatarText.color = player2DisabledColor;
	}

	public void TogglePlayer2Turn() {
		player2AvatarText.color = player2Color;
		player1AvatarText.color = player1DisabledColor;
	}

	public void FadeBothAvatars() {
		player1AvatarText.color = player1DisabledColor;
		player2AvatarText.color = player2DisabledColor;
	}

	public void CleanUp() {
		slotsGrid.CleanUp();
		gridConnections.CleanUp();
		player1AvatarText.color = player1Color;
		player2AvatarText.color = player2Color;
	}
	
	#endregion
	
}
