using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{


    private PlayerController controllerA;
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
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetKeyDown("return"))
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



    }
}
