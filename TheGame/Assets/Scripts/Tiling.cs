using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 2;                 // The offset so that we don't get any weird errors
    public bool hasARightBuddy = false;     // Used if we need to instantiate stuff
    public bool hasALeftBuddy = false;      // Used if we need to instantiate stuff
    public bool reverseScale = false;       // Used if object is not tilable
    private float spriteWidth = 0f;         // The width of our element
    private Camera cam;
    private Transform myTransform;

    void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Use this for initialization
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Does it still need Buddies? If not, do nothing
        if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            // Calculate the cameras extend (half the width) of what the camera can see in world coordinates
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // Calculate the x position where the camera can see the edge of the sprite (element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            // Checking if we can see the edge of the element
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                makeNewBuddy(1);
                hasARightBuddy = true;
            } else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                makeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    // a function that creates a buddy on the side required
    void makeNewBuddy(int rightOrLeft)
    {
        // Calculating the new position for our new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, 
                                          myTransform.position.y,
                                          myTransform.position.z);

        // Instantiating our new buddy and storing him in the variable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // If not tilable let's reverse the x size of our object
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1,
                                               newBuddy.localScale.y,
                                               newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        } else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
