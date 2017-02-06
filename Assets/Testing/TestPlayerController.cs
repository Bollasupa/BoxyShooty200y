using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestPlayerController : MonoBehaviour
{
    public GameObject player;
    public Rigidbody rb;
    public float moveTime = 1;

    Queue<Func<IEnumerator>> queue;
    private IEnumerator currentCoroutine;
    private bool executing = false;

    void Start()
    {
        queue = new Queue<Func<IEnumerator>>();
        player = this.gameObject;
        rb = player.GetComponent<Rigidbody>();
    }

    public void Forward()
    {
        queue.Enqueue(MoveForward);

    }

    private IEnumerator MoveForward()
    {
        float startTime = Time.time;
        Vector3 origin = transform.position;
        Vector3 destination = transform.position + Vector3.forward;
        while (transform.position != destination)
        {
            transform.position = Vector3.Lerp(origin, destination, ((Time.time - startTime) / moveTime));
            yield return null;
        }
    }

    public void Backwards()
    {
        queue.Enqueue(MoveBackwards);

    }

    private IEnumerator MoveBackwards()
    {
        float startTime = Time.time;
        Vector3 origin = transform.position;
        Vector3 destination = transform.position + Vector3.back;
        while (transform.position != destination)
        {
            transform.position = Vector3.Lerp(origin, destination, ((Time.time - startTime) / moveTime));
            yield return null;
        }
    }

    public void Right()
    {
        queue.Enqueue(MoveRight);

    }

    private IEnumerator MoveRight()
    {
        float startTime = Time.time;
        Vector3 origin = transform.position;
        Vector3 destination = transform.position + Vector3.right;
        while (transform.position != destination)
        {
            transform.position = Vector3.Lerp(origin, destination, ((Time.time - startTime) / moveTime));
            yield return null;
        }
    }

    public void Left()
    {
        queue.Enqueue(MoveLeft);

    }

    private IEnumerator MoveLeft()
    {
        float startTime = Time.time;
        Vector3 origin = transform.position;
        Vector3 destination = transform.position + Vector3.left;
        while (transform.position != destination)
        {
            transform.position = Vector3.Lerp(origin, destination, ((Time.time - startTime) / moveTime));
            yield return null;
        }
    }

    public void Execute()
    {
        if (executing)
        {
            //add execution reset and switch handling
            return;
        }
        executing = true;
        StartCoroutine("Executor");
    }

    private IEnumerator Executor()
    {
        foreach (Func<IEnumerator> a in queue)
        {
           // float StartTime = Time.time;
            yield return StartCoroutine(a());
           
            //add exectution stop!
        }
        queue.Clear();
        executing = false; 
    }
}
