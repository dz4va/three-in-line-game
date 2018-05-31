using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum GameState 			{ MainMenu, InGame, EndScreen }
enum GameTurn 			{ Player1, Player2 }
enum GameResult 		{ StillPlaying, Player1Won, Player2Won }
public enum SlotState 	{ Empty, Player1, Player2 }

public class Game : MonoBehaviour {

	#region Private Members
	
	[SerializeField]
	private MainMenu 	mainMenu;
	[SerializeField]
	private GameUI		gameUI;
	[SerializeField]
	private EndScreenUI endScreenUI;

	private GameState 		gameState;
	private GameTurn 		gameTurn;
	private SlotState[] 	slotStates;
	private int 			gameTurnsCount;

	private bool movingSlotTurn;
	private int  movingSlotIndex;
	private int[] movingSlotEmptyNeighbors;
	
	#endregion

	#region MB Methods
	
	void Start() {
		initialize();
	}
	
	#endregion

	#region Private Methods
	
	private void initialize() {
		Debug.Log("Initializing Game");

		gameState = GameState.MainMenu;

		mainMenu.Initialize();
		addMainMenuEventHandlers();
		mainMenu.Show();

		gameUI.Initialize();
		addGameUIEventHandlers();
		gameUI.Hide();

		endScreenUI.Initialize();
		addEndScreenUIEventHandlers();
		endScreenUI.Hide();
	}

	private void initializeGameData() {
		slotStates = new SlotState[9] { 
			SlotState.Empty, SlotState.Empty, SlotState.Empty,
			SlotState.Empty, SlotState.Empty, SlotState.Empty,
			SlotState.Empty, SlotState.Empty, SlotState.Empty
		};

		gameTurnsCount = 0;
	}

	private void addMainMenuEventHandlers() {
		mainMenu.OnStartButtonClick += onMenuStartButtonClick;
	}

	private void addGameUIEventHandlers() {
		gameUI.AddListenerOnSlotClick(onGridSlotClick);
	}

	private void addEndScreenUIEventHandlers() {
		endScreenUI.OnRestartButtonClick += onRestartButtonClick;
	}

	private void onMenuStartButtonClick() {
		gameState = GameState.InGame;

		initializeGameData();
		mainMenu.Hide();
		gameUI.Show();
		gameUI.CleanUp();

		gameTurn = Random.Range(0, 100) < 50 ? GameTurn.Player1 : GameTurn.Player2;

		updateUIWithPlayerTurn();
	}

	private void onRestartButtonClick() {
		gameState = GameState.MainMenu;
		mainMenu.Show();
		gameUI.Hide();
		endScreenUI.Hide();
	}

	private void onGridSlotClick(int slotIndex) {
		processSlotClick(slotIndex);
	}

	private void switchTurn() {
		if (gameTurn == GameTurn.Player1)
			gameTurn = GameTurn.Player2;
		else
			gameTurn = GameTurn.Player1;
	}

	private void processSlotClick(int slotIndex) {
		bool needToSwitchTurn = false;

		switch(gameTurn) {
			case GameTurn.Player1:
				if (gameTurnsCount < 6) {
					slotStates[slotIndex] = SlotState.Player1;
					gameUI.OccupySlotWithPlayer1Color(slotIndex);
					needToSwitchTurn = true;
				} else {
					if (slotStates[slotIndex] == SlotState.Player1 && !movingSlotTurn) {
						setMovingSlotStateForIfHaveEmptyNeighbors(slotIndex);
					} else if (slotStates[slotIndex] == SlotState.Empty && movingSlotTurn) {
						moveSavedSlotToTheDestinationIfHaveEmptyNeighbor(slotIndex, SlotState.Player1);
						needToSwitchTurn = true;
					}
				}
				break;
			case GameTurn.Player2:
				if (gameTurnsCount < 6) {
					slotStates[slotIndex] = SlotState.Player2;
					gameUI.OccupySlotWithPlayer2Color(slotIndex);
					needToSwitchTurn = true;
				} else {
					if (slotStates[slotIndex] == SlotState.Player2 && !movingSlotTurn) {
						setMovingSlotStateForIfHaveEmptyNeighbors(slotIndex);
					} else if (slotStates[slotIndex] == SlotState.Empty && movingSlotTurn) {
						moveSavedSlotToTheDestinationIfHaveEmptyNeighbor(slotIndex, SlotState.Player2);
						needToSwitchTurn = true;
					}
				}
				break;
		}

		if (needToSwitchTurn) {
			switchTurn();
			updateUIWithPlayerTurn();
			gameTurnsCount++;
		}

		GameResult gameResult = getGameResultForTurn();

		if (gameResult != GameResult.StillPlaying) {
			gameUI.FadeBothAvatars();
			toggleEndScreenWithResult(gameResult);
		}
	}

	private void updateUIWithPlayerTurn() {
		switch(gameTurn) {
			case GameTurn.Player1:
				gameUI.TogglePlayer1Turn();
				break;
			case GameTurn.Player2:
				gameUI.TogglePlayer2Turn();
				break;
		}
	}

	private GameResult getGameResultForTurn() {
		GameResult result = GameResult.StillPlaying;

		if (checkWinSlotState(SlotState.Player1))	return GameResult.Player1Won;
		
		if (checkWinSlotState(SlotState.Player2))	return GameResult.Player2Won;

		return result;
	}

	private bool checkWinSlotState(SlotState state) {
		return slotStates[0] == slotStates[1] && slotStates[1] == slotStates[2] && slotStates[2] == state ||
			slotStates[3] == slotStates[4] && slotStates[4] == slotStates[5] && slotStates[5] == state ||
			slotStates[6] == slotStates[7] && slotStates[7] == slotStates[8] && slotStates[8] == state ||
			slotStates[0] == slotStates[3] && slotStates[3] == slotStates[6] && slotStates[6] == state ||
			slotStates[1] == slotStates[4] && slotStates[4] == slotStates[7] && slotStates[7] == state ||
			slotStates[2] == slotStates[5] && slotStates[5] == slotStates[8] && slotStates[8] == state ||
			slotStates[0] == slotStates[4] && slotStates[4] == slotStates[8] && slotStates[8] == state ||
			slotStates[2] == slotStates[4] && slotStates[4] == slotStates[6] && slotStates[6] == state;
	}

	private int[] getEmptyNeighborIndicesForSlot(int slotIndex) {
		int[] rowColumnForm = indexToRowColumn(slotIndex);

		int[] neighborIndices = new int[8] {
			rowColumnToIndex(rowColumnForm[0] - 1, rowColumnForm[1]),			// Neighbor Up
			rowColumnToIndex(rowColumnForm[0] - 1, rowColumnForm[1] + 1),		// Neighbor Up Right Diagonal
			rowColumnToIndex(rowColumnForm[0], rowColumnForm[1] + 1),			// Neighbor Right
			rowColumnToIndex(rowColumnForm[0] + 1, rowColumnForm[1] + 1),		// Neighbor Down Right Diagonal
			rowColumnToIndex(rowColumnForm[0] + 1, rowColumnForm[1]),			// Neighbor Down
			rowColumnToIndex(rowColumnForm[0] + 1, rowColumnForm[1] - 1),		// Neighbor Down Left Diagonal
			rowColumnToIndex(rowColumnForm[0], rowColumnForm[1] - 1),			// Neighbor Left
			rowColumnToIndex(rowColumnForm[0] - 1, rowColumnForm[1] - 1),		// Neighbor Up Left Diagonal
		};

		List<int> emptyAndValidNeighborIndices = new List<int>();

		foreach(int index in neighborIndices) {
			if (index >= 0 && index < 9 && slotStates[index] == SlotState.Empty) {
				if (gameUI.ConnectionExists(slotIndex, index)) {
					Debug.Log("Neighbor Index: " + index);
					emptyAndValidNeighborIndices.Add(index);
				}
			}
		}

		return emptyAndValidNeighborIndices.ToArray();
	}

	private int[] indexToRowColumn(int slotIndex) {
		return new int[2] { (int)(slotIndex / 3), slotIndex % 3 };	// We know the width for this game grid
	}

	private int rowColumnToIndex(int row, int column) {
		return row * 3 + column;
	}

	private bool indexIsInEmptyNeighbors(int slotIndex) {
		foreach(int index in movingSlotEmptyNeighbors)
			if (slotIndex == index) return true;
		return false;
	}

	private void setMovingSlotStateForIfHaveEmptyNeighbors(int slotIndex) {
		movingSlotEmptyNeighbors = getEmptyNeighborIndicesForSlot(slotIndex);
		if (movingSlotEmptyNeighbors.Length != 0) {
			movingSlotTurn = true;
			movingSlotIndex = slotIndex;
			gameUI.ColorizeConnections(slotIndex, movingSlotEmptyNeighbors);
			gameUI.IncreaseShadowForSlot(slotIndex);
		}
	}

	private bool moveSavedSlotToTheDestinationIfHaveEmptyNeighbor(int destinationIndex, SlotState stateToSet) {
		bool succeeded = false;
		if (indexIsInEmptyNeighbors(destinationIndex)) {
			movingSlotTurn = false;
			slotStates[destinationIndex] = stateToSet;
			slotStates[movingSlotIndex] = SlotState.Empty;
			if (stateToSet == SlotState.Player1)
				gameUI.OccupySlotWithPlayer1Color(destinationIndex);
			else
				gameUI.OccupySlotWithPlayer2Color(destinationIndex);
			gameUI.SetSlotWithIndexEmpty(movingSlotIndex);
			gameUI.DecolorizeConnections(movingSlotIndex, movingSlotEmptyNeighbors);
			gameUI.DecreaseShadowForSlot(movingSlotIndex);
			succeeded = true;
		}
		return succeeded;
	}

	private void toggleEndScreenWithResult(GameResult result) {
		gameState = GameState.EndScreen;
		StartCoroutine(waitAndShowEndScreen(result));
	}

	private IEnumerator waitAndShowEndScreen(GameResult result) {
		yield return new WaitForSeconds(3);
		if (result == GameResult.Player1Won)
			endScreenUI.SetWinnerPlayer1();
		else
			endScreenUI.SetWinnerPlayer2();
		gameUI.Hide();
		endScreenUI.Show();
	} 

	#endregion

}
