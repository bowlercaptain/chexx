using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class LocalPlayer : MonoBehaviour {
	public SharedPlayer share;
	public string renderBoard;
	public string serverResult;
	public string turnString;


	//gui temps
	string from;
	string to;

	public void acceptNewBoard(Dictionary<Point2, string> chars) {
		Debug.Log("recieved board");
		string[,] board = new string[8, 8];
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				board[x, y] = "?";
			}
		}
		foreach (Point2 pos in chars.Keys) {
			board[pos.x, pos.y] = chars[pos];
		}
		StringBuilder boardBuilder = new StringBuilder();
		for (int y = 0; y < 8; y++) {
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

	void SendMove(string from, string to) {
		Point2 fromPoint = new Point2(int.Parse(from.Substring(0, 1)), int.Parse(from.Substring(2, 1)));
		Point2 toPoint = new Point2(int.Parse(to.Substring(0, 1)), int.Parse(to.Substring(2, 1)));
		share.CmdSendMove(fromPoint, toPoint);
	}

	private void OnGUI() {
		if (!share) { return; }
		GUI.Label(new Rect(400, 0, 400, 400), serverResult + "\n\n" + renderBoard + "\n"+turnString);
		GUI.Label(new Rect(250, 0, 100, 50),"starting point");
		from = GUI.TextField(new Rect(350, 0, 50, 50), from);
		GUI.Label(new Rect(250, 50, 100, 50), "destination");
		to = GUI.TextField(new Rect(350, 50, 50, 50), to);
		if (GUI.Button(new Rect(250, 300, 300, 100), "send lol")) {
			SendMove(from, to);
		}
	}


}
