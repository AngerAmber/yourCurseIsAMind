using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabWalk : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float MovementSpeedPerSecond = 10.0f;
    public float MovementSign = 1.0f;

    void FixedUpdate()
    {
        Vector3 characterVelocity = myRigidBody.velocity;
        characterVelocity.x = 0.0f;
        characterVelocity -= MovementSign * MovementSpeedPerSecond * transform.right.normalized;
        myRigidBody.velocity = characterVelocity;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
