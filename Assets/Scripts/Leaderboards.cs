using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboards : MonoBehaviour
{
    [SerializeField]
    public Transform entryContainer;
    [SerializeField]
    public Transform entryTemplate;
    [SerializeField]
    private GameObject leaderboardContainer;

    private bool containsPlayer = false;

    private static Leaderboards leaderboardInstance = null;

    public static Leaderboards LeaderboardsInstance
    {
        get
        {
            if (leaderboardInstance == null)
            {
                leaderboardInstance = FindObjectOfType<Leaderboards>();
            }
            return leaderboardInstance;
        }
    }

    private List<LeaderboardEntry> leaderboardEntryList;
    private List<Transform> leaderboardEntryTransformList;

    private void Awake()
    {
        //  entryContainer = transform.Find("leaderboardEntryContainer");
        //  entryTemplate = entryContainer.Find("leaderboardEntryTemplate");
        leaderboardContainer = transform.GetChild(0).gameObject;
        entryTemplate.gameObject.SetActive(false);


        /* leaderboardEntryList = new List<LeaderboardEntry>()
         {
             new LeaderboardEntry {
                 score = 500,
                 name = "AAA"
             },
             new LeaderboardEntry {
                 score = 200,
                 name = "King Doof"
             },
         };*/

        /* string jsonString = PlayerPrefs.GetString("leaderboardTable");
         Scores score = System.IO.File.ReadAllText(Application.persistentDataPath + "/LeaderboardDataSaved.json"); 
         Scores scores =  JsonUtility.FromJson<Scores>(jsonString);*/

        string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/LeaderboardDataSaved.json");
        Scores scores = JsonUtility.FromJson<Scores>(jsonString);
        

        //sorting alogrithm

        for (int i = 0; i < scores.leaderboardEntryList.Count; i++)
        {
            for (int j = i+1; j < scores.leaderboardEntryList.Count; j++)
            {
                if (scores.leaderboardEntryList[j].score > scores.leaderboardEntryList[i].score)
                {
                    LeaderboardEntry temp = scores.leaderboardEntryList[i];
                    scores.leaderboardEntryList[i] = scores.leaderboardEntryList[j];
                    scores.leaderboardEntryList[j] = temp;
                }
            }
        }

            leaderboardEntryTransformList = new List<Transform>();
        foreach (LeaderboardEntry leaderboardEntry in scores.leaderboardEntryList)
        {           
                CreateHighscoreEntryTransform(leaderboardEntry, entryContainer, leaderboardEntryTransformList);
            
        }
    }

    private void CreateHighscoreEntryTransform(LeaderboardEntry leaderboardEntry, Transform container, List<Transform> transformList)
    {

        float templateHeight = 60.0f;
        int rank = transformList.Count + 1;

        if (rank < 16)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            string rankString;


            switch (rank)
            {
                default:
                    rankString = rank + "TH";
                    break;

                case 1:
                    rankString = "1st";
                    break;
                case 2:
                    rankString = "2nd";
                    break;
                case 3:
                    rankString = "3rd";
                    break;

            }
            entryTransform.Find("rankText").GetComponent<TextMeshProUGUI>().text = rankString;
            long score = leaderboardEntry.score;
            entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();
            string name = leaderboardEntry.name;
            entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;
            entryTransform.Find("entryBackground").gameObject.SetActive(rank % 2 == 1);


            //highlight player entry on the leaderboard
            if (rank == 1)
            {
                entryTransform.Find("rankText").GetComponent<TextMeshProUGUI>().color = Color.yellow;
                entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().color = Color.yellow;
                entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().color = Color.yellow;
            }

            //Set trophies on 1st, 2nd and 3rd positions
            switch (rank)
            {
                default:
                    entryTransform.Find("trophy").gameObject.SetActive(false);
                    break;
                case 1:
                    entryTransform.Find("trophy").gameObject.GetComponent<Image>().color = new Color32(209, 212, 37, 255);
                    break;
                case 2:
                    entryTransform.Find("trophy").GetComponent<Image>().color = new Color32(215, 215, 215, 255);
                    break;
                case 3:
                    entryTransform.Find("trophy").GetComponent<Image>().color = new Color32(164, 92, 56, 255);
                    break;
            }

            transformList.Add(entryTransform);
        }

    }

    public void AddScoreEntry(int score, string name)
    {
        LeaderboardEntry leaderboardEntry = new LeaderboardEntry { score = score, name = name };
        
        //Load saved scores
        string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/LeaderboardDataSaved.json");
        Scores scores = JsonUtility.FromJson<Scores>(jsonString);

        //Add new scores
        scores.leaderboardEntryList.Add(leaderboardEntry);

        string json = JsonUtility.ToJson(scores, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/LeaderboardDataSaved.json", json);
    }


    private void AddPlayerScore()
    {
            AddScoreEntry(int.Parse(Player.Instance.bestScoreText.text.ToString()), Player.Instance.playerName);
            Debug.Log("Player score added");
        
    }

    public void TurnOn()
    {
        UIManager.Instance.TurnOn(transform.GetChild(0).gameObject);
    }

    public void TurnOff()
    {
        UIManager.Instance.TurnOff(transform.GetChild(0).gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        AddPlayerScore();
        leaderboardContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
