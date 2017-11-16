using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageObjBehaviour : MonoBehaviour {

    [SerializeField]
    Text m_votesCountText;
    [SerializeField]
    Text m_languageTitleText;

    public void Init(int votes, string title)
    {
        m_votesCountText.text = votes.ToString();
        m_languageTitleText.text = title;
    }

}
