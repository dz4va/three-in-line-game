using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class SlotsGrid : MonoBehaviour {

	#region Public Members
	
	public event UnityAction<int> OnSlotClick = delegate{};
	
	#endregion

	#region Private Members
	
	[SerializeField]
	private Color 				defaultSlotColor;
	[SerializeField]
	private List<GameObject> 	gridSlots;
	
	#endregion

	#region Public Methods

	public void Initialize() {
		Debug.Log("Initializing Slots Grid...");
		forEachSlot((index, slot) => { 
			Button slotButton = slot.GetComponent<Button>();
			slotButton.onClick.AddListener(() => { OnSlotClick(index); });
		});
	}
	
	public void CleanUp() {
		forEachSlot((index, slot) => { SetColor(index, defaultSlotColor); });
	}

	public void SetColor(int index, Color color) {
		Image fillImage = gridSlots[index].transform.GetChild(0).GetComponent<Image>();
		fillImage.color = color;
	}
	
	public void SetDefaultColor(int index) {
		SetColor(index, defaultSlotColor);
	}

	public void IncreaseShadowForSlot(int index) {
		setShadowForSlot(index, 7);
	}

	public void DescreaseShadowForSlot(int index) {
		setShadowForSlot(index, 3);
	}

	#endregion

	#region Private Methods

	private void setShadowForSlot(int index, float amount) {
		Shadow fillShadow = gridSlots[index].transform.GetChild(0).GetComponent<Shadow>();
		fillShadow.effectDistance = Vector2.down * amount;
	}

	private void forEachSlot(Action<int, GameObject> action) {
		foreach (GameObject obj in gridSlots)
			action(gridSlots.IndexOf(obj), obj);
	}
	
	#endregion

}
