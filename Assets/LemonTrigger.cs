using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LemonTrigger : MonoBehaviour
{
    public float counter = 00;
    public TextMeshProUGUI textMesh;
    public Player_Script playerScript;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            playerScript.pointCounter = playerScript.pointCounter + 1;
            textMesh.text = "x" + playerScript.pointCounter.ToString();
        }
    }
}
