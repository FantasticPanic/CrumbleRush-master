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
    private float pointIncreasedPerSecond = 0f;
    //UI Canvas
    public Image background;

    //what is ground for the player
    public LayerMask whatIsGround;

    private Collider ballCollision;

    private bool isPlaying;
    //a public area list of this gameobject's ragdoll colliders
    public List<Collider> RagdollParts = new List<Collider>();
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
       // SetRagdollParts();
        
    }

    private void SetRagdollParts()
    {
        //set an array for all of the colliders that are children of this gameobject
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        //if a collider is not a collider of this gameobject in the hierarchy
        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
               // c.isTrigger = true;
                RagdollParts.Add(c);
            }
            
        }
    }

   
    // Start is called before the first frame update
    void Start()
    {
        //start the game, no movement
        dir = Vector3.zero;

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
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) && !isDead)
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

                pointIncreasedPerSecond = 1f;
            }
        }


        else if (isDead)
        {
            pointIncreasedPerSecond = 0;
            ballCollision.enabled = false;

        }
        //scoreText.text =  "Score  "+ (int)score;
        scoreText.text = "" + (int)score;
        score += pointIncreasedPerSecond * Time.deltaTime;
        //StartCoroutine(WaitAndIncreaseScore(1));


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

            //Stop the main camera from following the player
            /*   if (this.gameObject.transform.childCount > 0)
               {
                   //StartCoroutine(WaitAndDie(2f));
                   this.gameObject.transform.GetChild(8).transform.parent = null;
               }*/
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
        this.gameObject.GetComponent<Collider>().enabled = false;
        newCameraParent.transform.parent = null;
        waterPlane.transform.parent = null;

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
               
                //txt.color = new Color (Random.value, Random.value, Random.value);
                txt.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
                
            }

        } 
       bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();


    }

    private bool IsGrounded()

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
    }

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


    public void TurnOnRagdoll()
    {
        RIGID_BODY.useGravity = false;
        RIGID_BODY.velocity = Vector3.zero;
        this.gameObject.GetComponent<Collider>().enabled = false;
        playerAnim.enabled = false;
       // playerAnim.avatar = null;

        foreach (Collider c in RagdollParts)
        {
            c.attachedRigidbody.velocity = Vector3.zero;
            //c.isTrigger = false;
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
