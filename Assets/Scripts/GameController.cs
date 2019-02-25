using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
//////////////////////////////

///// classes for minmax AI
public class MNode {
	public int x;
	public int y;
	public int player;
	//public string player;// this is our player

}
public class MovesAndScores {

	public int score;// score
	public MNode point;

	public MovesAndScores(int score, MNode point) {
		this.score = score;
		this.point = point;
	}

}
//// classes for AI ends here

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

	private string playerSide;/// for AI
	private string computerSide;/// for AI
	public bool playerMove;/// for AI
	public float delay;/// for AI
	private int value;/// for AI
	public int lastmove; /// for AI
	private int[] last_moves  = new int[9];


	//MNode[] grid = new MNode[9];
	MNode[,] grid = new MNode[3,3];
	public List<MovesAndScores> rootsChildrenScores;
	//public GameObject ui;


		void Awake()
		{
			SetGameControllerReferenceOnButtons();
			gameOverPanel.SetActive(false);
			moveCount = 0;
			restartButton.SetActive(false);
			playerMove = true;/// for AI
			for (int i = 0; i < 9; i++) {
				last_moves[i] = 0;
			}
		}

/// below update functions for AI only
		void Update () { /// for minmax AI
			if (!isGameOver()) {
	      if (playerMove == false) {
					Debug.Log("min max successfully called");
          callMinimax (0, 1);// first parameter depth second is turn
  				MNode best = returnBestMove ();
					if(isValidMove(best.x, best.y)){
						int valToPutInButtonListIndex = MultiToSingle(best.x, best.y);
						Debug.Log("Value where button list to update " + valToPutInButtonListIndex);
						if (buttonList[valToPutInButtonListIndex].GetComponentInParent<Button>().interactable == true){
							best.player = 2;
							Debug.Log("best X is " + best.x + " best Y is " + best.y);
							grid[best.x,best.y] = best;
							buttonList[valToPutInButtonListIndex].text = GetComputerSide();
							buttonList[valToPutInButtonListIndex].GetComponentInParent<Button>().interactable = false;
							int garbage_values = 0;
							garbage_values = CheckLastMoveWas();
							EndTurn();
						}
					}
	      }
	    }
		}

///// update functions ends

////Some AI functions

int SingleToMulti(int val)
{
	int x = 0, y = 0;
	if (val == 0) {
		x=0;
		y=0;
	}
	else if (val == 1) {
		x=0;
		y=1;
	}
	else if (val == 2) {
		x=0;
		y=2;
	}
	else if (val == 3) {
		x=1;
		y=0;
	}
	else if (val == 4) {
		x=1;
		y=1;
	}
	else if (val == 5) {
		x=1;
		y=2;
	}
	else if (val == 6) {
		x=2;
		y=0;
	}
	else if (val == 7) {
		x=2;
		y=1;
	}
	else if (val == 8) {
		x=2;
		y=2;
	}
	return x*10 + y;// where this function is called after that add line ret_value_x = ret_value/10 and ret_value_y = ret_value%10
}

int MultiToSingle(int x, int y){
	// X is 1 Y is 0 value out was 1
	if(x == 0)
		return y;
	else if (x == 1)
		return 3 + y;
	else if (x == 2)
		return 6 + y;
	else
		return 0;
}

////// some AI funcions ends

		int CheckLastMoveWas()
		{
				int movesLastWas = 0;
				for (int i=0; i<9; i++) {
					if (buttonList[i].GetComponentInParent<Button>().interactable == false && last_moves[i] == 0)
					{
						last_moves[i] = 1;
						movesLastWas = i;
						break;
					}
				}
				return movesLastWas;
		}

		void SetGameControllerReferenceOnButtons(){
			for (int i = 0; i < buttonList.Length; i++) {
					buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
			}
		}

		public void SetStartingSide(string startingSide){
			playerSide = startingSide;
			if (playerSide == "X") {
				computerSide = "O"; /// for AI
				SetPlayersColors(playerX, playerO);
			}
			else
			{
				computerSide = "X";/// for AI
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

		public string GetComputerSide(){/// for AI
			return computerSide;
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
			/// for AI next 8 if/ else if
			else if (buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide) {
					Gameover(computerSide);
			}
			else if (buttonList[3].text == computerSide && buttonList[4].text == computerSide && buttonList[5].text == computerSide) {
					Gameover(computerSide);
			}
			else if (buttonList[6].text == computerSide && buttonList[7].text == computerSide && buttonList[8].text == computerSide) {
					Gameover(computerSide);
			}
			else if (buttonList[0].text == computerSide && buttonList[3].text == computerSide && buttonList[6].text == computerSide) {
					Gameover(computerSide);
			}
			else if (buttonList[1].text == computerSide && buttonList[4].text == computerSide && buttonList[7].text == computerSide) {
					Gameover(computerSide);
			}
			else if (buttonList[2].text == computerSide && buttonList[5].text == computerSide && buttonList[8].text == computerSide) {
					Gameover(computerSide);
			}
			else if (buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide) {
					Gameover(computerSide);
			}
			else if (buttonList[2].text == computerSide && buttonList[4].text == computerSide && buttonList[6].text == computerSide) {
					Gameover(computerSide);
			}

			else if (moveCount >= 9) {
				if (gameends_flag != 1) {
						Gameover("draw");
				}
			}
			else{
					ChangeSides();
					delay = 10;/// for AI
			}

		}

		void SetPlayersColors(Player newPlayer, Player oldPlayer){
			newPlayer.panel.color = activePlayerColor.panelColor;
			newPlayer.text.color = activePlayerColor.textColor;
			oldPlayer.panel.color = inactivePlayerColor.panelColor;
			oldPlayer.text.color = inactivePlayerColor.textColor;

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
			playerMove = (playerMove == true) ? false : true;/// for AI
			//playerSide = (playerSide == "X") ? "O" : "X";
			if (playerMove == true){ /// for AI
			//if (playerSide == "X") {
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
			playerMove = true;///for AI
			delay = 30;///for AI
			for (int i = 0 ; i< 3; i++) {
				for (int j = 0 ; j< 3; j++) {
					grid[i, j] = null;
				}
			}

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


		/// for minmax algo AI following computerSide

	  // set this function as on click for every Grid Space
	  public void PlayerTurnUpdate(){/// my function to get the player moves updated
	    MNode node = new MNode ();
			lastmove = CheckLastMoveWas();
			int last_move_converter;
			last_move_converter = SingleToMulti(lastmove);
			node.x = last_move_converter/10;//logic = get last move and pass as node.x
			node.y = last_move_converter%10;
	    node.player = 1;
	    grid[node.x, node.y] = node; //insert this MNode into the grid
	  }

		public bool hasOWon(){
			//check to see if the positions you're about to check actually exist in the grid
			if (grid [0, 0] != null && grid [1, 1] != null && grid [2, 2] != null) {
				//check to see if there is a diagonal win for the O player
				if (grid [0, 0].player == grid [1, 1].player && grid [0, 0].player == grid [2, 2].player && grid [0, 0].player == 1)
					return true;
			}
			if (grid [0, 2] != null && grid [1, 1] != null && grid [2, 0] != null) {
				//diagonal win
				if(grid [0, 2].player == grid [1, 1].player && grid [0, 2].player == grid [2, 0].player && grid [0, 2].player == 1)
					return true;
			}
			//Column Wins
			for (int i = 0; i < 3; ++i) {
				if(grid[i,0] != null && grid[i,1] != null && grid[i,2] != null) {
					if(grid[i,0].player == grid[i,1].player && grid[i,0].player == grid[i,2].player && grid[i,0].player == 1)
						return true;
				}
				if(grid[0,i] != null && grid[1,i] != null && grid[2,i] != null) {
					if(grid[0,i].player == grid[1,i].player && grid[0,i].player == grid[2,i].player && grid[0,i].player == 1)
						return true;
				}
			}
			return false; //there are no winning solutions on the board for O
		}

		public bool hasXWon() {
			if (grid [0, 0] != null && grid [1, 1] != null && grid [2, 2] != null) {
				if (grid [0, 0].player == grid [1, 1].player && grid [0, 0].player == grid [2, 2].player && grid [0, 0].player == 2)
					return true;
			}
			if (grid [0, 2] != null && grid [1, 1] != null && grid [2, 0] != null) {
				if(grid [0, 2].player == grid [1, 1].player && grid [0, 2].player == grid [2, 0].player && grid [0, 2].player == 2)
					return true;
			}

			for (int i = 0; i < 3; ++i) {
				if(grid[i,0] != null && grid[i,1] != null && grid[i,2] != null) {
					if(grid[i,0].player == grid[i,1].player && grid[i,0].player == grid[i,2].player && grid[i,0].player == 2)
						return true;
				}
				if(grid[0,i] != null && grid[1,i] != null && grid[2,i] != null) {
					if(grid[0,i].player == grid[1,i].player && grid[0,i].player == grid[2,i].player && grid[0,i].player == 2)
						return true;
				}
			}
			return false; //there are no winning solutions on the board for X
		}

		//game is over is someone has won, or board is full
		public bool isGameOver() {
			//Text text = ui.GetComponent<Text>();

			if (getMoves ().Capacity == 0) {
				//text.text = "It's a draw!";
				return true;
			}
			if (hasOWon ()) {
				//text.text = "You Won!";
				return true;
			}
			if (hasXWon ()) {
				//text.text = "You lost!";
				return true;
			}
			return false;
		}

		//returns a list of MNodes, each MNode being a position that is empty and available
		List<MNode> getMoves() {
			List<MNode> result = new List<MNode>();
			for(int row = 0; row < 3; row++) {
				for(int col = 0; col < 3; col++) {
					if(grid[row,col] == null) {
						MNode newNode = new MNode();
						newNode.x = row;
						newNode.y = col;
						result.Add(newNode); //if the space is empty, add it to the list of results
					}
				}
			}
			return result;
		}

		//gets the new best move and returns it as an MNode
		public MNode returnBestMove() {
			int MAX = -100000;
			int best = -1;

			//iterates through rootsChildrenScores to get the best move
			for (int i = 0; i < rootsChildrenScores.Count; i++) {
				//also makes sure that the position in the grid is not occupied
				if (MAX < rootsChildrenScores[i].score && isValidMove(rootsChildrenScores[i].point.x, rootsChildrenScores[i].point.y)) {
					MAX = rootsChildrenScores[i].score;
					best = i;
				}
			}
			if(best > -1)
				return rootsChildrenScores[best].point;
			MNode blank = new MNode();
			blank.x = 0;
			blank.y = 0;
			return blank;
		}

		//returns true if the location is not currently occupied, returns false otherwise
		public bool isValidMove(int x, int y) {
			if (grid [x, y] == null)
				return true;
			return false;
		}

		//returns the minimum element of the list passed to it
		public int returnMin(List<int> list) {
			int min = 100000;
			int index = -1;
			for (int i = 0; i < list.Count; ++i) {
				if (list[i] < min) {
					min = list[i];
					index = i;
				}
			}
			return list[index];
		}

		//returns the maximum element of the list passed to it
		public int returnMax(List<int> list) {
			int max = -100000;
			int index = -1;
			for (int i = 0; i < list.Count; ++i) {
				if (list[i] > max) {
					max = list[i];
					index = i;
				}
			}
			return list[index];
		}

		//calls the minimax function with a given depth and turn
		public void callMinimax(int depth, int turn){
			rootsChildrenScores = new List<MovesAndScores>();
			minimax(depth, turn);
		}

		public int minimax(int depth, int turn) {

			if (hasXWon()) return +1; //+1 for a player win
			if (hasOWon()) return -1; //-1 for a computer win

			List<MNode> pointsAvailable = getMoves();
			if (pointsAvailable.Capacity == 0) return 0;

			List<int> scores = new List<int>();

			for (int i = 0; i < pointsAvailable.Count; i++) {
				MNode point = pointsAvailable[i];

				//Select the highest from the minimax call on X's turn
				if (turn == 1) {
					MNode x = new MNode();
					x.x = point.x;
					x.y = point.y;
					x.player = 2;
					grid[point.x,point.y] = x;

					int currentScore = minimax(depth + 1, 2);
					scores.Add(currentScore);

					if (depth == 0) {
						MovesAndScores m = new MovesAndScores(currentScore, point);
						m.point = point;
						m.score = currentScore;
						rootsChildrenScores.Add(m);
					}

				}
				//Select the lowest from the minimax call on O's turn
				else if (turn == 2) {
					MNode o = new MNode();
					o.x = point.x;
					o.y = point.y;
					o.player = 1;
					grid[point.x,point.y] = o;
					int currentScore = minimax(depth+1,1);
					scores.Add(currentScore);
				}
				grid[point.x, point.y] = null; //reset the point
			}
			return turn == 1 ? returnMax(scores) : returnMin(scores);
		}
}
