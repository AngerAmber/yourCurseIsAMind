using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFlip : MonoBehaviour
{
    public CrabWalk crabWalk = null;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 crabScale = crabWalk.transform.localScale;
        crabScale.x = -crabScale.x;
        crabWalk.transform.localScale = crabScale;
        crabWalk.MovementSign *= -1;
    }
}