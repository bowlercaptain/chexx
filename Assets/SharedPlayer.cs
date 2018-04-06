using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class SharedPlayer : NetworkBehaviour {

	int elo;
	string multiplayervalue;
	public GameObject cubePrefab;

	Serv serv;
	LocalPlayer localPlayer;

	public override void OnStartServer() {
		serv = FindObjectOfType<Serv>();
		serv.AddPlayer(this);
	}

	public override void OnStartClient() {
		
		localPlayer = FindObjectOfType<LocalPlayer>();
		localPlayer.share = this;
	}

	[Command]
	public void CmdSendMove(Point2 from, Point2 to) {
		serv.processMove(from, to, this);
	}

	[ClientRpc]
	public void RpcRecieveBoard(Point2[] points, string[] positions) {
		Debug.Log("recieving board");
		var l = new Dictionary<Point2, string>();
		for (int i = 0; i < points.Length; i++) {
			l.Add(points[i], positions[i]);
		}
		localPlayer.acceptNewBoard(l);
	}

	[ClientRpc]
	public void RpcRecieveTurn(bool isTurn){
		localPlayer.AcceptTurnChange(isTurn);
	}

	[ClientRpc]
	public void RpcRecieveTurnResult(string result){
		localPlayer.AcceptServerResponse(result);
	}

	
}
