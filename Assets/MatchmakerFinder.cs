using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatchmakerFinder : MonoBehaviour {

	public bool isSearching;
	Coroutine searchRoutine;

	public void findGame() {
		//actually call startMatchmaking, wait for match found, then get ip/port and pass to
		searchRoutine = StartCoroutine(findGameRoutine());

	}

	public void StopSearch(){
		StopCoroutine(searchRoutine);
		isSearching = false;
		//send stop search to mm
	}

	public IEnumerator findGameRoutine(){
		isSearching = true;

		//start mm search
		yield return new WaitForSeconds(5f);
		//wait for seconds, wait for mm search to be fulfilled
		string ip = "localhost";
		string port = "7777";
		startGame(ip, port);
		isSearching = false;
	}

	public void startGame(string ip, string port) {
		var nm = FindObjectOfType<NetworkManager>();
		nm.networkAddress = "localhost";
		nm.networkPort = 7777;
		nm.StartClient();
	}

}
