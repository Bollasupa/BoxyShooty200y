using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBullet : MonoBehaviour, IKillable {

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
