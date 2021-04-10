using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject spButton;
    public GameObject mpButton;
    public GameObject goButton;

    public GameObject nameFieldInput;

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
}
