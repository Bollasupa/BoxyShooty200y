using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveTime = 1;
    public float rotateTime = 0.2f;
    //Each turn is 90 degrees * rotateRatio
    public float rotateRatio = 0.5f;
    public float delayTime = 1;

    Queue<Queue<Func<IEnumerator>>> methodQueueQueue;
    Queue<Queue<Vector3>> directionQueueQueue;
    Queue<Func<IEnumerator>> methodQueue;
    Queue<Vector3> directionQueue;

    Queue<Func<IEnumerator>> activeMethodQueue;
    Queue<Vector3> activeDirectionQueue;

    private bool executing = false;
    private bool newExecutionWaiting = false;
    private bool lerpCoroutineExecuting = false;
    private bool doubleQueueing = false;

    public void Start()
    {
        methodQueueQueue = new Queue<Queue<Func<IEnumerator>>>();
        directionQueueQueue = new Queue<Queue<Vector3>>();
        methodQueue = new Queue<Func<IEnumerator>>();
        directionQueue = new Queue<Vector3>();
    }

    public void Forward()
    {
        methodQueue.Enqueue(Move);
        directionQueue.Enqueue(Vector3.forward);
    }

    public void Backwards()
    {
        methodQueue.Enqueue(Move);
        directionQueue.Enqueue(Vector3.back);
    }

    public void Right()
    {
        methodQueue.Enqueue(Move);
        directionQueue.Enqueue(Vector3.right);
    }

    public void Left()
    {
        methodQueue.Enqueue(Move);
        directionQueue.Enqueue(Vector3.left);

    }

    public void RotateLeft()
    {
        //methodQueue.Enqueue(Rotate);
        directionQueue.Enqueue(Vector3.left);
    }

    public void RotateRight()
    {
        //methodQueue.Enqueue(Rotate);
        directionQueue.Enqueue(Vector3.right);
    }

    //private IEnumerator Rotate()
    //{
    //    float startTime = Time.time;
    //    Quaternion origin = transform.rotation;
    //    Quaternion.

    //    Quaternion destination = Quaternion.LookRotation(Quaternion.Euler(origin)


    //    while (transform.rotation != destination)
    //    {
    //        transform.rotation = Quaternion.Lerp(origin, destination, ((Time.time - startTime) / moveTime));
    //        yield return null;
    //    }
    // put lerp=false here
    //}

    private IEnumerator Move()
    {
        float startTime = Time.time;
        Vector3 origin = transform.position;
        Vector3 destination = transform.position + activeDirectionQueue.Dequeue();
        while (0.01f < Vector3.Distance(transform.position, destination))
        {
            transform.position = Vector3.Lerp(origin, destination, ((Time.time - startTime) / moveTime));
            yield return null;
        }
        transform.position = destination;
        lerpCoroutineExecuting = false;
    }

    public void Execute()
    {
        methodQueueQueue.Enqueue(methodQueue);
        directionQueueQueue.Enqueue(directionQueue);
        methodQueue = new Queue<Func<IEnumerator>>();
        directionQueue = new Queue<Vector3>();
        IEnumerator executor = Executor();
        StartCoroutine(executor);
    }

    private IEnumerator Executor()
    {
        yield return new WaitForSeconds(delayTime);

        if (executing)
        {
            newExecutionWaiting = true;
            while (executing)
            {
                yield return null;
            }
        }

        //semaphores
        executing = true;
        newExecutionWaiting = false;

        //switch queues
        activeMethodQueue = methodQueueQueue.Dequeue();
        activeDirectionQueue = directionQueueQueue.Dequeue();

        foreach (Func<IEnumerator> a in activeMethodQueue)
        {

            if (newExecutionWaiting)
            {
                break;
            }
            
            //"Lerping" Coroutines shall set to false on exit,
            //other coroutines shall set to false immediately (I.E. Shoot) 
            lerpCoroutineExecuting = true;
            StartCoroutine(a());

            while (lerpCoroutineExecuting)
            {
                yield return null;
            }
        }

        //clean up queues
        activeMethodQueue = null;
        activeDirectionQueue = null; 

        //release semaphore
        executing = false;
    }

}
