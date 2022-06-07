using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserLookAt : MonoBehaviour
{
    public Vector3 lookAtPos;

    void Update()
    {
        // This makes the particle system go along the ray casted by the enemy
        transform.LookAt(lookAtPos);

    }

}
