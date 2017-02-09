using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Vector3 spawnTilePlayer1;
    public Vector3 spawnTilePlayer2;

    public InputManager manager;
    public GameObject player;

    private GameObject player1;
    private GameObject player2;


	// Use this for initialization
	void Awake () {
        player1 = SpawnOnGrid(player, spawnTilePlayer1, Quaternion.Euler(0,90,0));
        player1.tag = "Player1";

        player2 = SpawnOnGrid(player, spawnTilePlayer2, Quaternion.Euler(0, -90, 0));
        player2.tag = "Player2";
    }

    GameObject SpawnOnGrid(GameObject obj, Vector3 vec, Quaternion rot)
    {
        vec.x += 0.5f;
        vec.z += 0.5f;

        return Instantiate(obj, vec, rot);
    }
	
}
