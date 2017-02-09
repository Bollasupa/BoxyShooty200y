using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    /*  Stupid byproduct list:
     * Making players be able to hurt each other by physically touching will break result in bad shit 
     * except I make every object destroy itself (currrently Destroy(other) on bullets)
     * 
     * Players are not killable! They have health and are therefore have to selfdestruct
     */

    //Settings
    public float moveTime = 1;
    public float rotateTime = 0.2f;
    //Each turn is 90 degrees * rotateRatio
    public float rotateDegrees = 90;
    public float delayTime = 1;
    public float waitAfterShootingSeconds = 0.1f;
    public int health = 1;

    //Dependancies
    public GameObject bullet;

    //Variables
        //End of barrel
    private GameObject projectileSpawn;
    //private int projectileLayer;



    //Queues
    Queue<Queue<Func<IEnumerator>>> methodQueueQueue;
    Queue<Queue<Vector3>> directionQueueQueue;
    Queue<Func<IEnumerator>> methodQueue;
    Queue<Vector3> directionQueue;

    Queue<Func<IEnumerator>> activeMethodQueue;
    Queue<Vector3> activeDirectionQueue;

    //semaphores and messages
    private bool executing = false;
    private bool newExecutionWaiting = false;
    private bool lerpCoroutineExecuting = false;

    private bool moving = false;
    private bool isCollision = false;
    private bool dieing = false;

    
    


    public void Start()
    {
        methodQueueQueue = new Queue<Queue<Func<IEnumerator>>>();
        directionQueueQueue = new Queue<Queue<Vector3>>();
        methodQueue = new Queue<Func<IEnumerator>>();
        directionQueue = new Queue<Vector3>();
        projectileSpawn = transform.FindChild("Gun").FindChild("ProjectileSpawn").gameObject;
    }

    public void Update()
    {
        if(health == 0)
        {
            IEnumerator die = Die();
            StartCoroutine(die);
        }
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

    public void Shoot()
    {
        methodQueue.Enqueue(ShootBullet);
    }

    public void RotateLeft()
    {
        methodQueue.Enqueue(Rotate);
        directionQueue.Enqueue(Vector3.left);
    }

    public void RotateRight()
    {
        methodQueue.Enqueue(Rotate);
        directionQueue.Enqueue(Vector3.right);
    }

    private IEnumerator ShootBullet()
    {
        Instantiate(bullet, projectileSpawn.transform.position, transform.rotation);
        yield return new WaitForSeconds(waitAfterShootingSeconds);
        lerpCoroutineExecuting = false;
    }

    private IEnumerator Rotate()
    {
        float startTime = Time.time;
        Quaternion origin = transform.rotation;
        Quaternion destination = origin * Quaternion.AngleAxis(rotateDegrees * activeDirectionQueue.Dequeue().x , Vector3.up);

        while (Quaternion.Angle(destination, transform.rotation) > 2 && !dieing)
        {
            transform.rotation = Quaternion.Slerp(origin, destination, ((Time.time - startTime) / moveTime));
            yield return null;
        }
        transform.rotation = destination;
        lerpCoroutineExecuting = false;
    }

    private IEnumerator Move()
    {
        moving = true;
        float startTime = Time.time;
        Vector3 origin = transform.position;
        Vector3 destination = transform.position + activeDirectionQueue.Dequeue();

        while (0.01f < Vector3.Distance(transform.position, destination) && !isCollision && !dieing)
        {
            transform.position = Vector3.Lerp(origin, destination, ((Time.time - startTime) / moveTime));
            yield return null;
        }

        if (isCollision && !dieing)
        {
            Vector3 collisionPos = transform.position;

            while (0.01f < Vector3.Distance(transform.position, origin))
            {
                transform.position = Vector3.Lerp(collisionPos, origin, ((Time.time - startTime) / moveTime));
                yield return null;
            }

            transform.position = origin;
        }else
        {
            transform.position = destination;
        }

        isCollision = false;
        lerpCoroutineExecuting = false;
        moving = false;
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

            if (dieing)
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

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == this.gameObject.layer)
        {
            if (moving)
            {
                isCollision = true;
            }
        }


        DamageDealer dDealer = other.gameObject.GetComponent<DamageDealer>();
        if (dDealer != null)
        {
            health -= dDealer.DealDamage();
            
        }

        Component killer = other.gameObject.GetComponent(typeof(IKillable));
        if(killer != null)
        {
            IKillable killable = killer as IKillable;
            killable.Kill();
        }
    }

    private IEnumerator Die()
    {
        while (lerpCoroutineExecuting)
        {
            yield return null;
        }

        Destroy(this.gameObject);
    }

}
