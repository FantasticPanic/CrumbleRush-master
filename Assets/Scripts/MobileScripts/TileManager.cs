using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TileManager : MonoBehaviour
{
    //create an array of game objects for the tiles
    public GameObject[] tilePrefabs;

    //currentTile that is being called
    public GameObject currentTile;

    private static TileManager instance;

    //allows us to reuse the tiles
    private Stack<GameObject> leftTiles = new Stack<GameObject>();

    private Stack<GameObject> topTiles = new Stack<GameObject>();

    //allows us to use TileManager wherever
    public static TileManager Instance
    {
        get
        {
            //if instance does not exist
            if (instance == null)
            {
                // find the TileManager and made a reference to it
                //we can access this in the PlayerDetection.cs script
                instance = FindObjectOfType<TileManager>();
            }
            return instance;
        }
    }

    //putting the game object as a stack allows us to use it in conjuction with the TileManager instance
    public Stack<GameObject> LeftTiles { get => leftTiles; set => leftTiles = value; }
    public Stack<GameObject> TopTiles { get => topTiles; set => topTiles = value; }



    // Start is called before the first frame update
    void Start()
    {
        CreateTiles(30);

        //call SpawnTile() 25 times
        // if you lower the number, you'll probably actually see the tiles spawning in.
        for (int i = 0; i < 30; i++)
        {
            SpawnTile();
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    //create a queue for the tiles
    public void CreateTiles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //adds the left and topTiles according to how they are in the array
            leftTiles.Push(Instantiate(tilePrefabs[0]));
            topTiles.Push(Instantiate(tilePrefabs[1]));
            //change topTile names to TopTile
            //Peek() does not remove object from the queue but it views it
            topTiles.Peek().name = "TopTile";
            topTiles.Peek().SetActive(false);
            //change LeftTile names to LeftTile
            leftTiles.Peek().name = "LeftTile";
            leftTiles.Peek().SetActive(false);
        }
    }

    public void SpawnTile()
    {
        //if left or top tiles is equal to zero
        if (leftTiles.Count == 0 || topTiles.Count == 0)
        {
            CreateTiles(10);
        }

        //generate a random number between 0 and 1
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            //Pop() removes the tile from the stack
            GameObject temp = leftTiles.Pop();

            //set them back to true after we initially set them to false;
            temp.SetActive(true);

            //set position to attach point on current tile.
            temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = temp;

        }
        else if (randomIndex == 1)
        {
            //Pop() removes the tile from the stack
            GameObject temp = topTiles.Pop();

            //set them back to true after we initially set them to false;
            temp.SetActive(true);

            //set position to attach point on current tile.
            temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = temp;
        }

        int spawnPickup = Random.Range(0, 10);
        GameObject pickup = currentTile.transform.GetChild(1).gameObject;

        if (spawnPickup == 0)
        {
            pickup.SetActive(true);
            pickup.transform.DORotate(new Vector3(0,0,360),1, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.OutElastic);
            
        }

    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetGameDesktop()
    {
        SceneManager.LoadScene(1);
    }
}
