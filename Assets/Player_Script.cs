using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public enum CharacterState
{
    Grounded,
    Airborn,
    Jumping,
    Total
}


public class Player_Script : MonoBehaviour
{

    //Jumping animation
    public void OnJump(){myAnimator.SetBool("InJump",true);}
    public void OfJump(){myAnimator.SetBool("InJump", false);}
    public bool InJump = false;
    public void JumpGround() { myAnimator.SetFloat("JumpState", 0); }
    public void JumpAir() { myAnimator.SetFloat("JumpState", 1); }
    public void JumpUpdate() { myAnimator.SetFloat("JumpState", 2); }

    //Components
    public Transform target;
    [SerializeField] Camera myCamera;
    public Animator myAnimator = null;
    public Rigidbody2D myRigidbody = null;
    public Healthbar healthBar;
    public SceneLoader cameraScript;

    //Integers
    int counter = 0;
    public int maxHealth = 100;
    public int currentHealth;

    //floats
    public float hight = 0;
    public float cameraMovePos = 0f;
    public float pointCounter = 0;
    public float maximumJump = 300f;
    public float movementSpeedPerSecond = 100.0f;
    public float immunity = 0;
    float timeScale;

    //Enums
    public CharacterState groundCheckValue = CharacterState.Grounded;
    public CharacterState jumpState = CharacterState.Airborn;

    void Start()
    {
        myCamera = Camera.main;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    //On a collision it sets the player to grounded if it collides with ground.
    void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                jumpState = CharacterState.Grounded; 
            }
            if (collision.gameObject.tag == "Spikes")
            {

            TakeDamage(10);
            Vector3 characterVelocity = myRigidbody.velocity;
            characterVelocity.y += 10;
            myRigidbody.velocity = characterVelocity;
            }
            if (collision.gameObject.tag == "Enemy")
            {
                TakeDamage(10);
                Vector3 characterVelocity = myRigidbody.velocity;
                characterVelocity.y += 10;
                myRigidbody.velocity = characterVelocity;
            }
            if (collision.gameObject.tag == "Coin")
            {
            pointCounter += 1;
            }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && jumpState != CharacterState.Jumping)
        {
            jumpState = CharacterState.Airborn;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpState = CharacterState.Grounded;
        }
    }

    //Takes Damage
    void TakeDamage(int damage)
    {
        if (immunity < 0.01)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            immunity = 2;
        }
    }

    //Lowers all status effects by a specifeid amount.
    void LowerStatusEffect(float effectReduction)
    {

        immunity -= effectReduction;
    }

    private void Reset()
    {
        Vector3 characterPosition = myRigidbody.transform.position;
        Vector3 characterVelocity = myRigidbody.velocity;
        characterVelocity.x = 0;
        characterPosition.x = -110; 
        characterVelocity.y = 0;
        characterPosition.y = -2;
        myRigidbody.transform.position = characterPosition;
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        Debug.Log("reset");
    }
    void Update()
    {
        cameraMovePos = gameObject.transform.position.x;
        cameraScript.MoveCamera(cameraMovePos);

        timeScale = Time.deltaTime * 1;
        //Counts frames but is turned of for now.
        if (counter++ % 10000 == 0 && false)
        {
            Debug.Log(counter);
        }

        LowerStatusEffect(timeScale);

        //Player to camera screen convertion
        Vector3 screenPos = myCamera.WorldToScreenPoint(target.position);
        hight = screenPos.y;
        if (hight < 0)
        {
            Vector3 characterPosition = gameObject.transform.position;
            characterPosition.y = 10;
            gameObject.transform.position = characterPosition;
            Vector3 characterVelocity = myRigidbody.velocity;
            characterVelocity.y = 0;
            myRigidbody.velocity = characterVelocity;
            TakeDamage(20);
        }

        //Start of jump code.
        if (jumpState == CharacterState.Grounded && Input.GetKey(KeyCode.Space))
        {
            jumpState = CharacterState.Jumping;
        }
        if (jumpState == CharacterState.Jumping && Input.GetKeyUp(KeyCode.Space))
        {
            jumpState = CharacterState.Airborn;
        }
        if (jumpState == CharacterState.Jumping)
        {
            //Counts how far you jumped.
            maximumJump = maximumJump - 300 * Time.deltaTime;
            OnJump();
            //Pushes you up acounting for the accelleration of the player.
            Vector3 characterPosition = gameObject.transform.position;
            Vector3 characterVelocity = myRigidbody.velocity;
            characterPosition.y += movementSpeedPerSecond * Time.deltaTime + characterVelocity.y * Time.deltaTime;
            gameObject.transform.position = characterPosition;
            //At the top of the jump, removes it.
            if (maximumJump < 0)
            {
                jumpState = CharacterState.Airborn;
                OfJump();
            }
            
        }

        //Debugging code for making the player not stop a jump prematurelly.
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (maximumJump > 250)
            {
                jumpState = CharacterState.Grounded;
                Debug.Log("Stop");
            }
            else
            {
                Debug.Log("Jump!");
                jumpState = CharacterState.Airborn;
                OfJump();
            }
        }

        //If on ground reset jump.
        if (jumpState == CharacterState.Grounded)
        {
            maximumJump = 300f;
            myRigidbody.gravityScale = 3;
            OfJump();
        }

        //Horizontal movement.
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 characterVelocity = myRigidbody.velocity;
            if (myRigidbody.velocity.x > 0)
            {
                characterVelocity.x = 0;
            }
            characterVelocity.x -= movementSpeedPerSecond * Time.deltaTime;
            myRigidbody.velocity = characterVelocity;
            Vector3 localScale = gameObject.transform.localScale;
            localScale.x = -1;
            gameObject.transform.localScale = localScale;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 characterVelocity = myRigidbody.velocity;
            if (myRigidbody.velocity.x < 0)
            {
                characterVelocity.x = 0;
            }
            characterVelocity.x += movementSpeedPerSecond * Time.deltaTime;
            myRigidbody.velocity = characterVelocity;
            Vector3 localScale = gameObject.transform.localScale;
            localScale.x = 1;
            gameObject.transform.localScale = localScale;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Reset();
        }
        
        if (jumpState == CharacterState.Airborn)
        {
            myRigidbody.gravityScale = 10;
        }

        if(currentHealth < 1)
        {
            Reset();
        }
        

    }
}
