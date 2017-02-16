using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{


    private PlayerController controllerA;
    private PlayerController controllerB;
    //GameObject playerB;

    //MoveManager aManager;
    //MoveManager bManager;

    private void Awake()
    {

        //bManager = new MoveManager(playerB);
    }

    void Start()
    {
        GameObject playerA = GameObject.FindWithTag("Player1");
        controllerA = playerA.GetComponent<PlayerController>();


        GameObject playerB = GameObject.FindWithTag("Player2");
        controllerB = playerB.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //player 1 controls
        if (Input.GetKeyDown("w"))
        {
            controllerA.Forward();
        }
        if (Input.GetKeyDown("s"))
        {
            controllerA.Backwards();
        }
        if (Input.GetKeyDown("a"))
        {
            controllerA.Left();
        }
        if (Input.GetKeyDown("d"))
        {
            controllerA.Right();
        }
        if (Input.GetKeyDown("space"))
        {
            controllerA.Execute();
        }
        if (Input.GetKeyDown("q"))
        {
            controllerA.RotateLeft();
        }
        if (Input.GetKeyDown("e"))
        {
            controllerA.RotateRight();
        }
        if (Input.GetKeyDown("x"))
        {
            controllerA.Shoot();
        }



        //player2 controls
        if (Input.GetKeyDown("i"))
        {
            controllerB.Forward();
        }
        if (Input.GetKeyDown("k"))
        {
            controllerB.Backwards();
        }
        if (Input.GetKeyDown("j"))
        {
            controllerB.Left();
        }
        if (Input.GetKeyDown("l"))
        {
            controllerB.Right();
        }
        if (Input.GetKeyDown("return"))
        {
            controllerB.Execute();
        }
        if (Input.GetKeyDown("u"))
        {
            controllerB.RotateLeft();
        }
        if (Input.GetKeyDown("o"))
        {
            controllerB.RotateRight();
        }
        if (Input.GetKeyDown(","))
        {
            controllerB.Shoot();
        }
    }
}
