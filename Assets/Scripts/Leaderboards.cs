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

    private void Awake()
    {
        entryContainer = transform.Find("leaderboardEntryContainer");
        entryTemplate = entryContainer.Find("leaderboardEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        float templateHeight = 40.0f;

        for (int i = 0; i < 20; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);

            int rank = i + 1;
            string rankString;
            switch (rank)
            {
                default:
                    rankString = rank + "TH";
                    break;

                case 1: rankString = "1st";
                    break;
                case 2: rankString = "2nd";
                    break;
                case 3: rankString = "3rd";
                    break;

            }
            entryTransform.Find("rankText").GetComponent<TextMeshProUGUI>().text = rankString;
            int score = Random.Range(0, 10000);
            entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();
            string name = "abcdefghijklmn";
            entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;

        }
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
