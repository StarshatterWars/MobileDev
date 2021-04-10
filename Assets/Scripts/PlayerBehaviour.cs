using UnityEngine;

/// <summary> 
/// Responsible for moving the player automatically and 
/// receiving input. 
/// </summary> 
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    /// <summary> 
    /// A reference to the Rigidbody component 
    /// </summary> 
    private Rigidbody rb;

    [Tooltip("How fast the ball moves left/right")]
    public float dodgeSpeed = 5;

    [Tooltip("How fast the ball moves forwards automatically")]
    [Range(0, 6)]
    public float rollSpeed = 5;

    [Header("Swipe Properties")]
    [Tooltip("How far will the player move up on swiping")]
    public float swipeMove = 2f;

    [Tooltip("How far must the player swipe before we will execute the action (in inches)")]
    public float minSwipeDistance = 0.25f;

    ///<summary>
    ///Used to hold the value that converts minSwipeDistance to pixels
    ///</summary>
    private float minSwipeDistancePixels;
    ///<summary>
    ///Stores the starting position of mobile touch events
    ///</summary>
    private Vector2 touchStart;

    // Start is called before the first frame update
    void Start()
    {
        // Get access to our Rigidbody component 
        rb = GetComponent<Rigidbody>();

        minSwipeDistancePixels = minSwipeDistance * Screen.dpi;
    }

    ///<summary>
    ///Update is called once per frame
    ///</summary> 

    private void Update()
    {
        // Check if we are running on a mobile device 
        #if UNITY_IOS || UNITY_ANDROID
        // Check if Input has registered more than zero touches 
        if (Input.touchCount> 0)
        {
            //Store the first touch detected. 
            Touch touch =Input.touches[0];
            SwipeTeleport(touch);
        }
    #endif
    }

    /// <summary>
    /// FixedUpdate is called at a fixed framerate and is a prime place to put
    /// Anything based on time.
    /// </summary>
    private void FixedUpdate()
    {
        // Check if we're moving to the side
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        // Check if we are running either in the Unity Editor or in a
        // stand-alone build.
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
            
            // Check if we're moving to the side
            horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
            
            // If the mouse is held down(or the screen is tapped on Mobile)
            if (Input.GetMouseButton(0))
            {
                horizontalSpeed = CalculateMovement(Input.mousePosition);
            }
            
            // Check if we are running on a mobile device 
        #elif UNITY_IOS || UNITY_ANDROID
            //Check if Input has registered more than zero touches
            if (Input.touchCount > 0)
            {
                //Store the first touch detected.
                Touch touch = Input.touches[0];
                horizontalSpeed = CalculateMovement(touch.position);
            }
        #endif
        rb.AddForce(horizontalSpeed, 0, rollSpeed);
        //Debug.LogError(rb.velocity);
    }

    ///<summary>
    /// Will figure out where to move the player horizontally
    ///</summary>
    ///<param name="pixelPos">The position the player has touched/clickedon</param>
    ///<returns>The direction to move in the x-axis</returns>

    private float CalculateMovement(Vector3 pixelPos)
    {
        //Converts to a 0 to 1 scale
        var worldPos = Camera.main.ScreenToViewportPoint(pixelPos);
        float xMove = 0;
        // If we press the right side of the screen 

        if (worldPos.x < 0.5f)
        {
            xMove=-1;
        }
        else
        {
            // Otherwise we're on the left
            xMove = 1;
        }
        // replace horizontalSpeed with our own value 
        return xMove * dodgeSpeed;
    }

    ///<summary>
    ///Will teleport the player if swiped to the left or right
    ///</summary>
    ///<param name="touch">Current touch event</param> 
    private void SwipeTeleport(Touch touch)
    {
        //Check if the touch just started
        if (touch.phase == TouchPhase.Began)
        {
            //If so, set touchStart
            touchStart = touch.position;
        }
        //If the touch has ended
        else if(touch.phase ==TouchPhase.Ended)
        {
            // Get the position the touch ended at 
            Vector2 touchEnd = touch.position;

            // Calculate the difference between the beginning and
            // end of the touch on the x-axis. 
            float x = touchEnd.x - touchStart.x;

            //If we are not moving far enough, don't do the teleport
            if (Mathf.Abs(x) < minSwipeDistancePixels)
            {
                return;
            }

            Vector3 moveDirection;
            // If moved negatively in the x-axis, move left
            if (x < 0)
            {
                moveDirection = Vector3.left;
            }
            else
            {
                // Otherwise we're on the right
                moveDirection = Vector3.right;
            }

            RaycastHit hit;
            // Only move if we wouldn't hit something
            if(!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                // Move the player
                rb.MovePosition(rb.position + (moveDirection * swipeMove));
            }   
        }
    }
}