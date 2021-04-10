using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDistanceUI : MonoBehaviour
{
    GameObject gc;
    GameController gcScript;

    private void Awake()
    {
        gcScript = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       if (gcScript.gameStart)
        {
            this.gameObject.GetComponent<Text>().text = gcScript.playerDistance.ToString("00000");
        }
    }
}
