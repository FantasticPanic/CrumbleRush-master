using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private static Player instance;

    public static Player Instance
    {
        get
        {
            //if instance does not exist
            if (instance == null)
            {
                // find the TileManager and made a reference to it
                //we can access this in the PlayerDetection.cs script
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    public float speed;

    private Vector3 dir;

    public GameObject ps;
    public Transform newCameraParent;
    public Transform waterPlane;

    // is the player dead? did they fly off the level
    [SerializeField]
    private bool isDead;

    //is the play button pressed?
    public bool playPressed;

    //is how to play button pressed?
    public bool howToPlayPressed;
    //reset UI button
    public GameObject resetBtn;
    //play game over animation
    public Animator gameOverAnim;
    //play how to play animation
    public Animator howToPlayAnim;
    //player animator
    public Animator playerAnim;

    private float score = 0f;
    private float oldScore = 0f;
    private float pointIncreasedPerSecond = 0f;
    public string playerName = "You";
    //UI Canvas
    public Image background;

    //what is ground for the player
    public LayerMask whatIsGround;

    private Collider ballCollision;

    private bool isPlaying;
    //reference to the contact point
    public Transform contactPoint;
    public Rigidbody RIGID_BODY;


    //TextMesh for score in upper right part of the screen.
    public TextMeshProUGUI scoreText;
    //the score the player died with
    public TextMeshProUGUI lastScoreText;
    //the best score that the player got in game
    public TextMeshProUGUI bestScoreText;
    //tell the player that just got a new highscore!
    public TextMeshProUGUI newHighScore;
    //Title screen dissapears when player starts
    public GameObject titleText;


    public TextMeshProUGUI[] scoreTexts;



    private void Awake()
    {

        RIGID_BODY = this.GetComponent<Rigidbody>();
        
    }

    private void SetRagdollParts(bool state)
    {
        //set an array for all of the colliders that are children of this gameobject
        Rigidbody[] rigidbodies = this.gameObject.GetComponentsInChildren<Rigidbody>();
        //if a collider is not a collider of this gameobject in the hierarchy
        foreach (Rigidbody rb in rigidbodies)
        {
            if (isDead)
            {
                rb.AddExplosionForce( 10f, dir, 10f);
            }

           rb.useGravity = state;
            
        }
    }

    private void SetColliderParts(bool state)
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = state;
        }
    }

   
    // Start is called before the first frame update
    void Start()
    {
        //start the game, no movement
        dir = Vector3.zero;
        SetColliderParts(true);
        isDead = false;
        playPressed = false;
        ballCollision = GetComponent<Collider>();
        ballCollision.enabled = true;

        titleText.SetActive(true);
        
        //ONLY KEEP ON FOR TESTING PURPOSES RESETS THE SCORE
        //PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (playPressed)
        {
            titleText.SetActive(false);
            howToPlayPressed = false;
            //when left click and player is not dead
            if (!isDead) {

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    //play random sound from array of movement sounds
                    SoundManager.SoundInstance.PlayRandomMoveSound();
                    //move the player based on their previous direction
                    if (dir == Vector3.forward)
                    {
                        dir = Vector3.left;
                        playerAnim.SetBool("isRunningLeft", true);
                    }
                    else
                    {
                        dir = Vector3.forward;
                        playerAnim.SetBool("isRunningLeft", false);
                    }
                }
                pointIncreasedPerSecond = 1f;
                IncreaseSpeed();
            }
        }


        else if (isDead)
        {
            pointIncreasedPerSecond = 0;
            ballCollision.enabled = false;

        }
        scoreText.text = "" + (int)score;
        score += pointIncreasedPerSecond * Time.deltaTime;


        // multiple by speed frame by frame count
        //character should move the same on every single device
        float amountToMove = speed * Time.deltaTime;
       

        // direction that player moves multipled by frames
        transform.Translate(dir * amountToMove);

        //if the player's y position is less than 4 
        //game over
        if (transform.position.y <= 3.8)
        {

            GameOver();
            SoundManager.SoundInstance.PlayGameOverSound();
            pointIncreasedPerSecond = 0;
        }

        if (howToPlayPressed)
        {
            howToPlayAnim.SetBool("HowToPlayIsPressed", true);
            if (Input.GetMouseButtonDown(0))
            {
                howToPlayPressed = false;

            }
        }
        else
        {
            howToPlayAnim.SetBool("HowToPlayIsPressed", false);
        }

    }

    

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Pickup")
        {
            col.gameObject.SetActive(false);
            Instantiate(ps, transform.position, Quaternion.identity);
            score += 3;
            scoreText.text = score.ToString();
            CombatTextManager.Instance.CreateText(col.transform.position, "+3", new Color32(255,255,0, 255), false);
            SoundManager.SoundInstance.PlayPickupSound();
        }
    }

    private IEnumerator WaitAndIncreaseScore(float waitTime)
    {
        while (true)
        {         
            score++;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator WaitAndDie(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            transform.GetChild(0).transform.parent = null;
        }
    }

    private IEnumerator RainbowColor(TextMeshProUGUI txt)
    {
        while (true)
        {
            
            
            txt.color = new Color(Random.value, Random.value, Random.value);
            yield return new WaitForSeconds(2f);

        }
    }

    private void GameOver()
    {
        isDead = true;
        resetBtn.SetActive(true);
        gameOverAnim.SetTrigger("GameOver");
        SetColliderParts(false);
        SetRagdollParts(false);
        newCameraParent.transform.parent = null;
        waterPlane.transform.parent = null;
        /////////RAGDOLL/////////
        
        GetComponent<Animator>().enabled = false;
        RIGID_BODY.constraints = RigidbodyConstraints.None;
        
        /////////
        lastScoreText.text = ""+(int)score;

        //save the player's score as best score
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        
        //turn off collider
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;

        // when the player's current score is higher than their best score
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", (int)score);
            newHighScore.gameObject.SetActive(true);
            background.color = new Color32(204, 204, 0, 255);

            //fore each TextMeshProUGUI type called txt in scoreTexts array (as mentioned in the fields)
            //use this if you want the text color to be white
            foreach (TextMeshProUGUI txt in scoreTexts)
            {
               
                txt.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
                
            }

        } 
       bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();


    }

   /* private bool IsGrounded()

    {
        //collider has radius of 0.5f
        Collider[] colliders = Physics.OverlapSphere(contactPoint.position, .5f, whatIsGround);

        //check each of the colliders to see if it is different from the player
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }*/

    public void PlayMode()
    {
        playPressed = true;
    }

    public void InstructionMode()
    {
        if (howToPlayPressed == false)
        {
            howToPlayPressed = true;
        }
    }

    private void IncreaseSpeed()
    {
        if (score > oldScore + 15)
        {
            oldScore = score;
            speed += 0.5f;
        }
    }


    /* void OnTriggerExit(Collider col)
     {
        //if the collider's tag is Tile
         if (col.tag == "Tile")
         {
             RaycastHit hit;

             // Place a ray underneath the player 
             Ray downRay = new Ray(transform.position, -Vector3.up);
                
             //if the raycast does not hit anything (is out of bounds)
             //player is dead
             //reset button appears
             if (!Physics.Raycast(downRay, out hit))
             {
                 isDead = true;
                 resetBtn.SetActive(true);

                 //Stop the main camera from following the player
                 if (transform.childCount > 0)
                 {
                     transform.GetChild(0).transform.parent = null;
                 }
             }
         }
     }*/


}
