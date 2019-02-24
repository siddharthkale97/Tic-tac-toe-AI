using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//////////////////////////////
	[System.Serializable]
public class Player
{
	public Image panel;
	public Text text;
	public Button button;
}
	[System.Serializable]
public class PlayerColor
{
	public Color panelColor;
	public Color textColor;
}
/////////////////////////////////////
public class GameController : MonoBehaviour {

	public Text[] buttonList;
	public GameObject gameOverPanel;
	public Text gameOverText;
	private int moveCount;
	private int gameends_flag = 0;
	public GameObject restartButton;
	public GameObject startInfo;
	public Player playerX;
	public Player playerO;
	public PlayerColor activePlayerColor;
	public PlayerColor inactivePlayerColor;
	private string playerSide;

		void Awake()
		{
			SetGameControllerReferenceOnButtons();
			gameOverPanel.SetActive(false);
			moveCount = 0;
			restartButton.SetActive(false);
		}

		void SetGameControllerReferenceOnButtons(){
			for (int i = 0; i < buttonList.Length; i++) {

					buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
			}
		}

		public void SetStartingSide(string startingSide){
			playerSide = startingSide;
			if (playerSide == "X") {
				SetPlayersColors(playerX, playerO);
			}
			else
			{
				SetPlayersColors(playerO, playerX);
			}
			StartGame();
		}

		void StartGame()
		{
			SetBoardInteractable(true);
			SetPlayerButtons(false);
			startInfo.SetActive(false);
		}

		public string GetPlayerSide(){
			return playerSide;
		}

		public void EndTurn(){
			moveCount++;
			if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide) {
					Gameover(playerSide);
			}
			else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide) {
					Gameover(playerSide);
			}
			else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide) {
					Gameover(playerSide);
			}
			else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide) {
					Gameover(playerSide);
			}
			else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide) {
					Gameover(playerSide);
			}
			else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide) {
					Gameover(playerSide);
			}
			else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide) {
					Gameover(playerSide);
			}
			else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide) {
					Gameover(playerSide);
			}
			else if (moveCount >= 9) {
				if (gameends_flag != 1) {
						Gameover("draw");
				}
			}
			else{
					ChangeSides();
			}

		}

		void SetPlayersColors(Player newPlayer, Player oldPlayer){
			newPlayer.panel.color = activePlayerColor.panelColor;
			newPlayer.text.color = activePlayerColor.textColor;
			oldPlayer.panel.color = inactivePlayerColor.panelColor;
			oldPlayer.text.color = inactivePlayerColor.textColor;

		}

		void CheckGameOver(){

		}

		void Gameover(string winningPlayer){
			SetBoardInteractable(false);

			if (winningPlayer == "draw") {
					SetGameOverText("It's a draw");
					SetPlayersColorInactive();
			}
			else{
					SetGameOverText(playerSide + " wins!");
			}

			restartButton.SetActive(true);
			gameends_flag = 1;
		}

		void ChangeSides(){
			playerSide = (playerSide == "X") ? "O" : "X";
			if (playerSide == "X") {
				SetPlayersColors(playerX, playerO);
			}
			else{
				SetPlayersColors(playerO, playerX);
			}

		}

		void SetGameOverText(string value){
			gameOverPanel.SetActive(true);
			gameOverText.text = value;
		}

		public void RestartGame()
		{
			moveCount = 0;
			gameends_flag = 0;
			gameOverPanel.SetActive(false);
			for (int i = 0; i < buttonList.Length; i++) {
					buttonList[i].text = "";
			}
			restartButton.SetActive(false);
			SetPlayerButtons(true);
			SetPlayersColorInactive();
			startInfo.SetActive(true);
		}

		void SetBoardInteractable(bool toggle){
			for (int i = 0; i < buttonList.Length; i++) {
					buttonList[i].GetComponentInParent<Button>().interactable = toggle;
			}
		}

		void SetPlayerButtons(bool toggle){
			playerX.button.interactable = toggle;
			playerO.button.interactable = toggle;
		}

		void SetPlayersColorInactive(){
			playerX.panel.color = inactivePlayerColor.panelColor;
			playerX.text.color = inactivePlayerColor.textColor;
			playerO.panel.color = inactivePlayerColor.panelColor;
			playerO.text.color = inactivePlayerColor.textColor;
		}

}
