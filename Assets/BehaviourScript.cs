using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCode : MonoBehaviour
{

    public float MovementSpeedPerSecond = 100.0f;
    public float GravityPerSecond = 200.0f;
    public float GroundLevel = -16.0f;
    void Start()
    {

    }

    void Update()
    {
        bool isMoving = false;

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 characterPosition = gameObject.transform.position;
            characterPosition.y += MovementSpeedPerSecond * Time.deltaTime;
            gameObject.transform.position = characterPosition;
            isMoving = true;
        }

        //if (Input.GetKey(KeyCode.S))
        //{
        //    Vector3 characterPosition = gameObject.transform.position;
        //    characterPosition.y -= VerticalSpeedPerSecond * Time.deltaTime;
        //    gameObject.transform.position = characterPosition;
        //}

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 characterPosition = gameObject.transform.position;
            characterPosition.x -= MovementSpeedPerSecond * Time.deltaTime;
            gameObject.transform.position = characterPosition;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 characterPosition = gameObject.transform.position;
            characterPosition.x += MovementSpeedPerSecond * Time.deltaTime;
            gameObject.transform.position = characterPosition;
        }

        if (isMoving == false)
        {
            Vector3 gravityPosition = gameObject.transform.position;
            gravityPosition.y -= GravityPerSecond * Time.deltaTime;
            if (gravityPosition.y < GroundLevel)
            {
                gravityPosition.y = GroundLevel;
            }
            gameObject.transform.position = gravityPosition;
        }

    }
}

