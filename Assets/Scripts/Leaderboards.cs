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

 

    private static Leaderboards instance;

    public static Leaderboards Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Leaderboards>();
            }
            return instance;
        }
    }

    private List<LeaderboardEntry> leaderboardEntryList;
    private List<Transform> leaderboardEntryTransformList;

    private void Awake()
    {
        leaderboardContainer = transform.GetChild(0).gameObject;
        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("LeaderboardSavedData");
        Scores scores = JsonUtility.FromJson<Scores>(jsonString);

        if (scores == null)
        {
            Debug.Log("Grabbing leaderboard data....");
            AddScoreEntry(100, "Manilla Mike");
            AddScoreEntry(222, "myname_geooff");
            AddScoreEntry(351, "marks_wherehouz");
            AddScoreEntry(515, "tony hawk");
            AddScoreEntry(3514, "Chef Ramsay");
            AddScoreEntry(194, "mad_money_shaun");
            AddScoreEntry(325, "gamscam");
            AddScoreEntry(436, "lastBastion");
            AddScoreEntry(766, "adjudicator far");
            AddScoreEntry(667, "thug spunky");
            AddScoreEntry(142, "ciaraFan96");
            AddScoreEntry(624, "GLOCK PAUL");
            AddScoreEntry(414, "Will Smith");
            AddScoreEntry(868, "fart/nasty");
        }


        /////////////////////////////////
        ///TESTING SAVING AND LOADING OF SCORES DATA
        /* string jsonString = PlayerPrefs.GetString("leaderboardTable");
         Scores score = System.IO.File.ReadAllText(Application.persistentDataPath + "/LeaderboardDataSaved.json"); 
         Scores scores =  JsonUtility.FromJson<Scores>(jsonString);*/

        // string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/LeaderboardDataSaved.json");
        //Scores scores = JsonUtility.FromJson<Scores>(jsonString);
        //////////////////////////////
        ///

        jsonString = PlayerPrefs.GetString("LeaderboardSavedData");
        scores = JsonUtility.FromJson<Scores>(jsonString);

        

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
            if (name == Player.Instance.playerName)
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
        //string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/LeaderboardDataSaved.json");
        // Scores scores = JsonUtility.FromJson<Scores>(jsonString);
        string jsonString = PlayerPrefs.GetString("LeaderboardSavedData");
        Scores scores = JsonUtility.FromJson<Scores>(jsonString);

        if (scores == null)
        {
            scores = new Scores()
            {
                leaderboardEntryList = new List<LeaderboardEntry>()
            };
        }
        //Add new scores
        scores.leaderboardEntryList.Add(leaderboardEntry);

        // string json = JsonUtility.ToJson(scores, true);
        // System.IO.File.WriteAllText(Application.persistentDataPath + "/LeaderboardDataSaved.json", json);

        string json = JsonUtility.ToJson(scores);
        PlayerPrefs.SetString("LeaderboardSavedData", json);
        PlayerPrefs.Save();
    }


    private void AddPlayerScore()
    {
            AddScoreEntry(int.Parse(Player.Instance.bestScoreText.text.ToString()), Player.Instance.playerName);
            Debug.Log("Player score added");
        
    }

    public void TurnOn()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
       // AddPlayerScore();
        leaderboardContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
