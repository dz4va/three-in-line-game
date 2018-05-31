using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GridConnections : MonoBehaviour {

	#region Private Members

	[System.Serializable]
	private class Connection {
		public int 		FirstSlot;
		public int 		SecondSlot;
		public Image 	ConnectionImage;
	}
	
	[SerializeField]
	private Color 				defaultColor;
	[SerializeField]
	private Color				selectedColor;
	[SerializeField]
	private List<Connection> 	connections;
	
	#endregion

	#region Public Methods

	public bool CheckForConnection(int firstIndex, int secondIndex) {
		bool connectionExists = false;
		forEachConnection((connection) => { 
			if (connectionIndicesMatch(connection, firstIndex, secondIndex))
				connectionExists = true;
		});
		return connectionExists;
	}

	public void Colorize(int firstIndex, int secondIndex) {
		forEachConnection((connection) => { 
			if (connectionIndicesMatch(connection, firstIndex, secondIndex))
				connection.ConnectionImage.color = selectedColor;
		});
	}

	public void Decolorize(int firstIndex, int secondIndex) {
		forEachConnection((connection) => { 
			if (connectionIndicesMatch(connection, firstIndex, secondIndex))
				connection.ConnectionImage.color = defaultColor;
		});
	}
	
	public void CleanUp() {
		forEachConnection((connection) => { setColorForConnection(connection, defaultColor); });
	}
	
	#endregion

	#region Private Methods

	private bool connectionIndicesMatch(Connection connection, int firstIndex, int secondIndex) {
		return (connection.FirstSlot == firstIndex + 1 && connection.SecondSlot == secondIndex + 1) ||
				(connection.FirstSlot == secondIndex + 1 && connection.SecondSlot == firstIndex + 1);
	}

	private void setColorForConnection(Connection connection, Color color) {
		connection.ConnectionImage.color = color;
	}
	
	private void forEachConnection(Action<Connection> action) {
		foreach(Connection connection in connections)
			action(connection);
	}
	
	#endregion

}
