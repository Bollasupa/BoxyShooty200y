using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructTimer : MonoBehaviour {

    //Just a cleanup function, not intended for accurate TTL



    public float timeToLive;
    private float deathTime;

    private void Start()
    {
        deathTime = Time.time + timeToLive;
        IEnumerator deathTimer = DeathTimer();
        StartCoroutine(deathTimer);
    }

    private IEnumerator DeathTimer()
    {
        while(Time.time < deathTime){
            yield return new WaitForSeconds(1);
        }
        Destroy(this.gameObject);
    }
}
