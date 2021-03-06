﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Serv : MonoBehaviour {
	Piece[,] board = new Piece[8, 8];
	int turn = 0;
	public List<SharedPlayer> players = new List<SharedPlayer>();
	private Point2 boardSize = new Point2(8, 8);

	private void Start() {//call from onserverstart or something
		BeginMatch();
		GetComponent<NetworkManager>().StartServer();
	}
	public void BeginMatch() {
		board = new Piece[8, 8];
		board[0, 0] = new Rook(0);
		board[7, 0] = new Rook(0);
		board[0, 7] = new Rook(1);
		board[7, 7] = new Rook(1);
		board[1, 0] = new Knight(0);
		board[6, 0] = new Knight(0);
		board[1, 7] = new Knight(1);
		board[6, 7] = new Knight(1);
		board[2, 0] = new Bishop(0);
		board[5, 0] = new Bishop(0);
		board[2, 7] = new Bishop(1);
		board[5, 7] = new Bishop(1);
		board[3, 0] = new King(0);
		board[4, 0] = new Queen(0);
		board[3, 7] = new King(1);
		board[4, 7] = new Queen(1);
		for(int i = 0; i < 8; i++){
			board[i, 1] = new Pawn(0);
			board[i, 6] = new Pawn(1);
		}
	}

	private void OnGUI() {
		if(GUI.Button(new Rect(300,300,300,300),"show boards")){
			showBoard(0);
			showBoard(1);
			players[0].RpcRecieveTurnResult("u get hekt");
		}
	}

	public void AddPlayer(SharedPlayer player) {
		players.Add(player);
		Invoke("ShowBoardToNewPlayer", .00001f);
	}

	public void ShowBoardToNewPlayer(){
		int playernum = players.Count - 1;
		showBoard(playernum);
		players[playernum].RpcRecieveTurn(playernum == turn);
		if(players.Count == 2){
			InvokeRepeating("checkForPlayerQuits", 5f, 5f);
		}
	}

	public Dictionary<Vector2, string> seenSquares(int owner) {
		Dictionary<Vector2, string> result = new Dictionary<Vector2, string>();
		for (int x = 0; x < board.GetLength(0); x++) {
			for (int y = 0; y < board.GetLength(1); y++) {
				Piece piece = board[x, y];
				if (piece != null && piece.owner == owner) {
					if (!result.ContainsKey(new Point2(x, y))) {
						result.Add(new Point2(x, y), piece.getCharacter());
					}

					foreach (Point2 targetSquare in piece.getAllTargetablePositions(new Point2(x, y), new Point2(board.GetLength(0), board.GetLength(1)))) {
						if(!boardSize.inBounds(targetSquare)){
							Debug.Log(piece);
							Debug.Log(targetSquare.x);
						}
						if (!result.ContainsKey(targetSquare)) {
							//targetSquare is guaranteed to be inside board (hopefully)
							Piece seenPiece = board[targetSquare.x, targetSquare.y];
							if (seenPiece != null) {
								result.Add(targetSquare, seenPiece.getCharacter());
							} else {
								result.Add(targetSquare,  ((targetSquare.x + targetSquare.y)%2==0 ? "　" : "　"));
							}
						}
					}
				}
			}
		}
		return result;
	}

	public void showBoard(int playerNum) {
		List<Point2> points = new List<Point2>();
		List<string> strings = new List<string>();
		var squares = seenSquares(playerNum);
		foreach (Point2 point in squares.Keys) {
			points.Add(point);
			strings.Add(squares[point]);
		}
		players[playerNum].RpcRecieveBoard(points.ToArray(), strings.ToArray());
	}

	public void processMove(Point2 from, Point2 to, SharedPlayer player) {
		player.RpcRecieveTurnResult(getMoveResult(from, to, player));
	}

	public string getMoveResult(Point2 from, Point2 to, SharedPlayer player) {
		if (player != players[turn]) {
			return "Not your turn, cheater!";
		}
		if (!(from.x < board.GetLength(0) && from.x >= 0 && from.y < board.GetLength(1) && from.y >= 0)) {
			return "out of bounds from point.";
		}
		if (!(to.x < board.GetLength(0) && to.x >= 0 && to.y < board.GetLength(1) && to.y >= 0)) {
			return "out of bounds to point.";
		}
		if (board[from.x, from.y] == null) {
			return "there's no piece there.";
		}
		if(board[from.x, from.y].owner != turn){
			return "that's not your piece!";
		}
		if (board[from.x, from.y].getAttackablePositions(from, boardSize).Contains(to) && board[to.x, to.y] != null && board[to.x, to.y].owner != turn) {
			//Destroy(board[to.x, to.y]);
			if(board[to.x, to.y].GetType()==typeof(King)){
				board[to.x, to.y] = board[from.x, from.y];
				board[from.x, from.y] = null;
				checkPromotePawn(to);
				players[(turn + 1) % 2].RpcRecieveGameResult(false);
				players[(turn + 1) % 2].RpcRecieveTurnResult("You lose!");
				players[turn].RpcRecieveGameResult(true);
				return "You win!";
			}
			board[to.x, to.y] = board[from.x, from.y];
			board[from.x, from.y] = null;
			checkPromotePawn(to);
			nextTurn();//sketchu
			players[(turn + 1) % 2].RpcRecieveTurnResult("Your piece got taken!");
			return "you took a piece!";
		}
		if (board[from.x, from.y].getMovablePositions(from, boardSize).Contains(to) && board[to.x, to.y] == null) {
			board[to.x, to.y] = board[from.x, from.y];
			board[from.x, from.y] = null;
			checkPromotePawn(to);
			nextTurn();
			return "move successful.";
		}
		return "That piece can't move there.";
	}

	public void checkPromotePawn(Point2 spot){
		if (board[spot.x, spot.y].GetType() == typeof(Pawn)){
			if(spot.y == 0 || spot.y == boardSize.y-1){//this assumes pawns only move forward
				//capture ownership, destroy pawn
				board[spot.x, spot.y] = new Queen(board[spot.x, spot.y].owner);//this assumes the piece is not null
			}
		}
	}

	public void nextTurn() {
		turn = (turn + 1) % 2;
		for (int playernum = 0; playernum < players.Count; playernum++) {
			showBoard(playernum);
			players[playernum].RpcRecieveTurn(playernum == turn);
		}

	}

	public void checkForPlayerQuits(){
		for (int i = 0; i < players.Count; i++) {
			if (players[i]==null){
				var otherPlayer = players[(i + 1) % 2];
				otherPlayer.RpcRecieveQuitReason("Your opponent ragequit. You win. :3");
				otherPlayer.RpcRecieveGameResult(true);
				endGame();
			}
		}
	}

	void endGame(){
		Application.Quit();
	}
}
