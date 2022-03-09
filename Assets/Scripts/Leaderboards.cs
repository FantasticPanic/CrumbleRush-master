using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboards : MonoBehaviour
{
    [SerializeField]
    private Transform entryContainer;
    [SerializeField]
    private Transform entryTemplate;

    private List<LeaderboardEntry> leaderboardEntryList;
    private List<Transform> leaderboardEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("leaderboardEntryContainer");
        entryTemplate = entryContainer.Find("leaderboardEntryTemplate");

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

       /* Scores scores = new Scores { leaderboardEntryList = leaderboardEntryList };
        string json = JsonUtility.ToJson(scores, true);
        PlayerPrefs.SetString("leaderboardTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("leaderboardTable"));
        Debug.Log(Application.persistentDataPath + "/LeaderboardDataSaved.json");
        System.IO.File.WriteAllText(Application.persistentDataPath +"/LeaderboardDataSaved.json", json);*/

    }

    private void CreateHighscoreEntryTransform(LeaderboardEntry leaderboardEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 40.0f;

        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
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

        transformList.Add(entryTransform);
    }

    private void AddScoreEntry(int score, string name)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
