using UnityEngine;
using System.Collections.Generic; // List

/// <summary> 
/// Controls the main gameplay 
/// </summary> 
public class GameController : MonoBehaviour
{
    [Tooltip("A reference to the tile we want to spawn")]
    public Transform tile;

    [Tooltip("A reference to the obstacle we want to spawn")]
    public Transform obstacle;

    [Tooltip("Where the first tile should be placed at")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 15)]
    public int initSpawnNum = 10;

    [Tooltip("How many tiles to spawn initially with no obstacles")]
    public int initNoObstacles = 4;
    /// <summary> 
    /// Where the next tile should be spawned at. 
    /// </summary> 
    private Vector3 nextTileLocation;

    /// <summary> 
    /// How should the next tile be rotated? 
    /// </summary> 
    private Quaternion nextTileRotation;

    public GameObject playerPrefab;
    private GameObject playerObj;
    public Vector3 playerLoc;

    public string playerName;
    public int playerDistance;
    public int playerMaxDistance;

    private bool playerSet;
    public bool gameStart;
    public float speedIncrease;

    public GameObject mainCamera;
    private CameraBehaviour camScript;

    public bool nameSet;

    private void Awake()
    {
        camScript = mainCamera.GetComponent<CameraBehaviour>();
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        RestartLevel();

        playerSet = false;
        nameSet = false;
        gameStart = false;
       
    }

    public void RestartLevel()
    {
        playerName = PlayerPrefs.GetString("PlayerName");
        playerMaxDistance = PlayerPrefs.GetInt("PlayerDistance");
        // Set our starting point 
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        for (int i = 0; i < initSpawnNum; ++i)
        {
            SpawnNextTile(i >= initNoObstacles);
        }
        playerDistance = 0;
        speedIncrease = 0;
    }


    void GameInit()
    {
        playerDistance = 0;
        speedIncrease = 0;
    }

    /// <summary>
    /// Will spawn a tile at a certain location and setup the next position 
    /// </summary>
    /// <param name="spawnObstacles">If we should spawn an obstacle</param>

    public void SpawnNextTile(bool spawnObstacles = true)
    {
        var newTile = Instantiate(tile, nextTileLocation,
                                  nextTileRotation);

        // Figure out where and at what rotation we should spawn 
        // the next item 
        var nextTile = newTile.Find("NextTile");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;

        if (spawnObstacles)
        {
            SpawnObstacle(newTile);
        }
    }

    void SpawnObstacle(Transform newTile)
    {
        // Now we need to get all of the possible places to spawn the 
        // obstacle 
        var obstacleSpawnPoints = new List<GameObject>();

        // Go through each of the child game objects in our tile 
        foreach (Transform child in newTile)
        {
            // If it has the ObstacleSpawn tag 
            if (child.CompareTag("ObstacleSpawn"))
            {
                // We add it as a possibility 
                obstacleSpawnPoints.Add(child.gameObject);
            }
        }

        // Make sure there is at least one 
        if (obstacleSpawnPoints.Count > 0)
        {
            // Get a random object from the ones we have 
            var spawnPoint = obstacleSpawnPoints[Random.Range(0,
                                          obstacleSpawnPoints.Count)];

            // Store its position for us to use 
            var spawnPos = spawnPoint.transform.position;

            // Create our obstacle 
            var newObstacle = Instantiate(obstacle, spawnPos,
                                          Quaternion.identity);

            // Have it parented to the tile
            newObstacle.SetParent(spawnPoint.transform);
        }
    }

    private void Update()
    {
        if (!playerSet)
            MakePlayer();
        if (playerObj == null)
            return;
        if(playerObj.transform.position.z > 0)
        {
            playerDistance = (int)playerObj.transform.position.z;
        }
        speedIncrease = playerObj.transform.position.z / 10;
    }

    private void MakePlayer()
    {
        playerObj = Instantiate(playerPrefab, playerLoc, Quaternion.identity) as GameObject;
        playerObj.name = "Player";
        playerSet = true;
        nameSet = false;
        camScript.target = playerObj.transform;
        gameStart = true;
    }

    public void UpdateMaxScore()
    {
        if(playerDistance > playerMaxDistance)
        {
            playerMaxDistance = playerDistance;
            PlayerPrefs.SetInt("PlayerDistance", playerMaxDistance);
        }
    }
}