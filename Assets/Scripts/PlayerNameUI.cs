using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameUI : MonoBehaviour
{
    GameObject gc;
    private bool nameSet;
    public TMP_Text m_TextComponent;

    private void Awake()
    {
        gc = GameObject.Find("Game Controller");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gc.GetComponent<GameController>().nameSet)
        {
            m_TextComponent.text = gc.GetComponent<GameController>().playerName;
            gc.GetComponent<GameController>().nameSet = true;
        }
    }
}

