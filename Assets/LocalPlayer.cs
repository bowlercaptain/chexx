using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class LocalPlayer : MonoBehaviour {

	private float? currElo;
	public float elo {
		get { if (currElo == null) { currElo = PlayerPrefs.GetFloat("elo", 2000f); } return (float)currElo; }
		set { if (value != currElo) { currElo = value; PlayerPrefs.SetFloat("elo", value); } }
	}
	public SharedPlayer share;
	public MatchmakerFinder matchmakerFinder;
	public string renderBoard;
	public string serverResult;
	public string turnString;
	public string quitReason = "";
	public float inProgressOpponentElo = 2000f;
	const float changeSpeed = 50f;



	//gui temps
	string from;
	string to;

	public void acceptNewBoard(Dictionary<Point2, string> chars) {
		string[,] board = new string[8, 8];
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				board[x, y] = "—";
			}
		}
		foreach (Point2 pos in chars.Keys) {
			board[pos.x, pos.y] = chars[pos];
		}
		StringBuilder boardBuilder = new StringBuilder();
		boardBuilder.Append("　　01234567\n　　　　　　　　　　\n");
		for (int y = 0; y < 8; y++) {
			boardBuilder.Append(y.ToString() + "　");
			for (int x = 0; x < 8; x++) {
				boardBuilder.Append(board[x, y]);
			}
			boardBuilder.Append("\n");
		}
		renderBoard = boardBuilder.ToString();
	}

	public void AcceptServerResponse(string response) {
		serverResult = response;
	}

	public void AcceptTurnChange(bool turnInfo) {
		if (turnInfo) {
			turnString = "It's your turn!";
		} else {
			turnString = "It's not your turn. You can still try to send moves (because you're a cheating hackerman) but it won't work.";
		}
	}

	public void acceptGameResult(bool won) {
		float eloProportion = elo / inProgressOpponentElo;
		if (won) {
			elo += changeSpeed / eloProportion;
		} else {
			elo -= changeSpeed * eloProportion;
		}
	}

	void SendMove(string from, string to) {
		Point2 fromPoint = new Point2(int.Parse(from.Substring(0, 1)), int.Parse(from.Substring(2, 1)));
		Point2 toPoint = new Point2(int.Parse(to.Substring(0, 1)), int.Parse(to.Substring(2, 1)));
		share.CmdSendMove(fromPoint, toPoint);
	}

	private void OnGUI() {
		Rect topLabelRect = new Rect(250, 0, 100, 50);
		Rect goButtonRect = new Rect(250, 300, 300, 100);
		Rect topFieldRect = new Rect(350, 0, 50, 50);
		Rect boardLabelRect = new Rect(400, 0, 400, 500);
		if (share) {
			GUI.Label(boardLabelRect, serverResult + "\n\n" + renderBoard + "\n" + turnString);
			GUI.Label(topLabelRect, "starting point");
			from = GUI.TextField(topFieldRect, from);
			GUI.Label(new Rect(250, 50, 100, 50), "destination");
			to = GUI.TextField(new Rect(350, 50, 50, 50), to);
			if (GUI.Button(goButtonRect, "send move")) {
				SendMove(from, to);
			}
		} else if (quitReason != "") {
			GUI.Label(boardLabelRect, serverResult + "\n\n" + renderBoard + "\n" + quitReason);
			if (GUI.Button(goButtonRect, "leave game")) {
				quitGame();
			}
		} else if (matchmakerFinder.isSearching) {
			GUI.Label(topLabelRect, "Searching for game...");
			if (GUI.Button(goButtonRect, "cancel search")){
				matchmakerFinder.StopSearch();
			}
		} else {
			float elo = this.elo;
			GUI.Label(topLabelRect, "current elo");
			if (float.TryParse(GUI.TextField(topFieldRect, elo.ToString()), out elo)) { this.elo = elo; }
			if (GUI.Button(goButtonRect, "find matchmaker game")) {

				findMatchmakerGame();

			}
		}
	}

	void quitGame() {
		quitReason = "";
		renderBoard = "";
		serverResult = "";
		turnString = "";
		//do something
		//open main menu?
	}

	void findMatchmakerGame() {
		matchmakerFinder = FindObjectOfType<MatchmakerFinder>();
		matchmakerFinder.findGame();
	}

	private void OnApplicationQuit() {
		if (share != null) {//check a game is running... somehow
			acceptGameResult(false);
		}
	}
}
