using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentNameUI : MonoBehaviour
{

    public TMP_Text m_TextComponent;

    // Start is called before the first frame update
    void Start()
    {
        SetName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            m_TextComponent.text = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            m_TextComponent.text = "";
        }
    }
}
