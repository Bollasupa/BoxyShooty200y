using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Vector3 spawnTilePlayer1;
    public Vector3 spawnTilePlayer2;

    public InputManager manager;
    public GameObject player;

    private GameObject player1;
    private GameObject player2;
    
    public List<int> rounds;

    public Text scoreText1;
    public Text scoreText2;
    public Text winText;

    private int score1 = 0;
    private int score2 = 0;

    // Use this for initialization
    void Awake()
    {

        StartGame();

    }

    private void StartGame()
    {
        player1 = SpawnOnGrid(player, spawnTilePlayer1, Quaternion.Euler(0, 90, 0));
        player1.tag = "Player1";
        player1.GetComponent<Renderer>().material = Resources.Load("Blue") as Material;
        manager.controllerA = player1.GetComponent<PlayerController>();

        player2 = SpawnOnGrid(player, spawnTilePlayer2, Quaternion.Euler(0, -90, 0));
        player2.tag = "Player2";
        player2.GetComponent<Renderer>().material = Resources.Load("Green") as Material;
        manager.controllerB = player2.GetComponent<PlayerController>();


    }

    private void Start()
    {

        IEnumerator freeze = FreezeInput(7.0f);
        StartCoroutine(freeze);
    }

    GameObject SpawnOnGrid(GameObject obj, Vector3 vec, Quaternion rot)
    {
        vec.x += 0.5f;
        vec.z += 0.5f;

        return Instantiate(obj, vec, rot);
    }

    private void ResetPositions()
    {
        Debug.Log("Resetting positions");
        player1.transform.position = spawnTilePlayer1;
        player1.transform.rotation = Quaternion.Euler(0, 90, 0);


        player2.transform.position = spawnTilePlayer2;
        player2.transform.rotation = Quaternion.Euler(0, -90, 0);

    }

    public void RoundOver()
    {
        manager.listeningForInput = false;

        Debug.Log("here");

        if (player1.activeSelf)
        {
            score1++;
            scoreText1.text = score1.ToString();
            winText.text = "Blue Scores!";
        }
        else
        {
            score2++;
            scoreText2.text = score2.ToString();
            winText.text = "Green Scores!";
        }

        Destroy(player1);
        Destroy(player2);

        StartGame();

        IEnumerator freezer = SwitchRoundFreeze();
        StartCoroutine(freezer);
        

    }

    private IEnumerator SwitchRoundFreeze()
    {
        yield return new WaitForSeconds(3.0f);
        winText.text = "START";
        yield return new WaitForSeconds(1.0f);
        winText.text = "";
        manager.listeningForInput = true;

    }

    private IEnumerator FreezeInput(float sec)
    {
        Debug.Log("HERE");
        //manager.active = false;
        yield return new WaitForSeconds(sec);
        //Debug.Log("now here");
        manager.listeningForInput = true;
    }

}
