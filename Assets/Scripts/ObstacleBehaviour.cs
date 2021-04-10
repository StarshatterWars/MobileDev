using UnityEngine;
using UnityEngine.SceneManagement; // LoadScene 

public class ObstacleBehaviour : MonoBehaviour
{

    [Tooltip("How long to wait before restarting the game")]
    public float waitTime = 2.0f;

    public GameController gcScript;

    public void Awake()
    {
        gcScript = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // First check if we collided with the player 
        if (collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            // Destroy the player 
            Destroy(collision.gameObject);

            // Call the function ResetGame after waitTime has passed 
            Invoke("ResetGame", waitTime);
        }
    }

    /// <summary> 
    /// Will restart the currently loaded level 
    /// </summary> 
    void ResetGame()
    {
        gcScript.UpdateMaxScore();
        // Restarts the current level 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}