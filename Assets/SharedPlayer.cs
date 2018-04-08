using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class SharedPlayer : NetworkBehaviour {

	Serv serv;
	LocalPlayer localPlayer;

	public override void OnStartServer() {
		serv = FindObjectOfType<Serv>();
		serv.AddPlayer(this);
	}

	public override void OnStartLocalPlayer() {
		localPlayer = FindObjectOfType<LocalPlayer>();
		localPlayer.share = this;
	}


	[Command]
	public void CmdSendMove(Point2 from, Point2 to) {

		serv.processMove(from, to, this);
	}

	[Command]
	public void CmdIAmQuitting() {
		Debug.Log("player is quitting");
	}

	private void OnApplicationQuit() {
		CmdIAmQuitting();
	}

	[ClientRpc]
	public void RpcRecieveBoard(Point2[] points, string[] positions) {
		if (!isLocalPlayer) { return; }
		Debug.Log("recieving board");
		var l = new Dictionary<Point2, string>();
		for (int i = 0; i < points.Length; i++) {
			l.Add(points[i], positions[i]);
		}
		localPlayer.acceptNewBoard(l);
	}

	[ClientRpc]
	public void RpcRecieveTurn(bool isTurn) {
		if (!isLocalPlayer) { return; }
		localPlayer.AcceptTurnChange(isTurn);
	}

	[ClientRpc]
	public void RpcRecieveTurnResult(string result) {
		if (!isLocalPlayer) { return; }
		localPlayer.AcceptServerResponse(result);
	}

	[ClientRpc]
	public void RpcRecieveQuitReason(string reason){
		if (!isLocalPlayer) { return; }
		localPlayer.quitReason = reason;
	}

	public void RpcRecieveGameResult(bool won){
		if(!isLocalPlayer){ return; }
		localPlayer.acceptGameResult(won);
	}
}
