using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public GameObject spButton;
    public GameObject mpButton;
    public GameObject goButton;

    public GameObject nameFieldInput;
    public TMP_Text m_TextComponent;
    
    //private GameController gcScript;

    public string playerName;

    public void Awake()
    {
        //gcScript = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        nameFieldInput.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInputOn()
    {
        nameFieldInput.SetActive(true);
    }

    public void SetInputOff()
    {
        nameFieldInput.SetActive(false);
    }

    public void SetInputValue()
    {
        playerName = m_TextComponent.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        SetInputOff();
    }
}
