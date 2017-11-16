using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [SerializeField]
    string m_dataDownloadLink;

    [SerializeField]
    string m_questionKeyString = "question";
    [SerializeField]
    string m_publishedTimeKeyString = "published_at";
    [SerializeField]
    string m_choicesKeyString = "choices";
    [SerializeField]
    string m_choicesNameKeyString = "choice";
    [SerializeField]
    string m_choicesVotesKeyString = "votes";

    // Use this for initialization
    void Start()
    {
        Instance = this;
    }

    public void RequestData()
    {
        StartCoroutine(Request());
    }

    IEnumerator Request()
    {
        Debug.Log("Requesting values");
        WWW www = new WWW(m_dataDownloadLink);
        yield return www;
        if (www.error != null)
        {
            OnRequestDone(false, null);
            yield break;
        }
        string jsonText = www.text;

        string JSONToParse = "{\"values\":" + jsonText + "}";

        JsonData parsedJsonData = JsonMapper.ToObject(JSONToParse);

        JsonData tempJsonData = parsedJsonData["values"][0];

        RequestData tempData = new RequestData();
        tempData.m_question = tempJsonData[m_questionKeyString].ToString();
        tempData.m_publishedTime = tempJsonData[m_publishedTimeKeyString].ToString();
        for (int i = 0; i < tempJsonData[m_choicesKeyString].Count; i++)
        {
            LanguageChoice tempLanguageChoice = new LanguageChoice();
            tempLanguageChoice.m_name = tempJsonData[m_choicesKeyString][i][m_choicesNameKeyString].ToString();

            int tempVotes = -1;
            int.TryParse(tempJsonData[m_choicesKeyString][i][m_choicesVotesKeyString].ToString(), out tempVotes);

            if (tempVotes != -1)
            {
                tempLanguageChoice.m_votes = tempVotes;
            }
            else
                Debug.Log("Error parsing votes to int...");

            tempData.m_languageChoiceList.Add(tempLanguageChoice);
        }

        //Data parsed, send to gameManager to handle UI
        OnRequestDone(true, tempData);
    }

    void OnRequestDone(bool success, RequestData data)
    {
        GameManager.Instance.OnRequesCallback(success, data);
    }
}

[System.Serializable]
public class RequestData
{
    public string m_question = "";
    public string m_publishedTime = "";

    public List<LanguageChoice> m_languageChoiceList = new List<LanguageChoice>(); 
}

[System.Serializable]
public class LanguageChoice
{
    public string m_name = "";
    public int m_votes = 0;
}
