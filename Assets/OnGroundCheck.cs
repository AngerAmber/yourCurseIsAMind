using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class OnGroundCheck : MonoBehaviour
{
    CharacterState JumpState;
    void OnCollision2D (Collision2D collision)
    {
        if (JumpState == CharacterState.Grounded)
        {
            return;
        }
        else
        {
            JumpState = CharacterState.Grounded;
        }
    }
}
