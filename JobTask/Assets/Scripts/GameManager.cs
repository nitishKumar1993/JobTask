using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [SerializeField]
    Text m_titleText;
    [SerializeField]
    Button m_refreshBtn;

    [SerializeField]
    GameObject m_languageChoicePrefab;
    [SerializeField]
    GameObject m_languageScrollViewContentGO;

    // Use this for initialization
    void Start () {
        Instance = this;
	}

    //Button handler for refresh button
    public void OnRefreshBtnClicked()
    {
        ChangeRefreshBtnState(false);
        RequestDataFromServer();
    }

    void RequestDataFromServer()
    {
        ResourceManager.Instance.RequestData();
    }

    public void OnRequesCallback(bool success, RequestData data)
    {
        if (success && data != null)
        {
            m_titleText.text = data.m_question;

            for (int i = 0; i < data.m_languageChoiceList.Count; i++)
            {
                GameObject tempLanguage = Instantiate(m_languageChoicePrefab, m_languageScrollViewContentGO.transform);
                if (tempLanguage.GetComponent<LanguageObjBehaviour>())
                {
                    tempLanguage.GetComponent<LanguageObjBehaviour>().Init(data.m_languageChoiceList[i].m_votes, data.m_languageChoiceList[i].m_name);
                }
            }
        }
        else
        {
            if (!success)
                Debug.Log("Error getting data from server");
            else if (data == null)
                Debug.Log("Error parsing data from server");
        }

        ChangeRefreshBtnState(true);
    }

    void ChangeRefreshBtnState(bool state)
    {
        m_refreshBtn.interactable = state;
    }
}
