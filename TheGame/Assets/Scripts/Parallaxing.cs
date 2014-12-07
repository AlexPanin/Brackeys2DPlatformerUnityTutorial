using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds;             // Array of all the back- and foregrounds to be parallaxed
    private float[] parallaxScales;             // The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;                // How smooth the parallax is going to be. Has to be above 0

    private Transform cam;                      // Reference to the main camera's transform
    private Vector3 previousCamPos;             // The position of the camera in the previous frame

    // Called before Start(). Great for references
    void Awake()
    {
        // set up the camera reference
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {
        // The previous frame had the current frame's camera position
        previousCamPos = cam.position;

        // Assigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales [i] = backgrounds [i].position.z * -1;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        // For each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // The parallax is the opposite of the camera movement
            // because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales [i];

            // Set a target x position which is current position plus the parallax
            float backgrounTargetPosX = backgrounds [i].position.x + parallax;

            // Create a target position which is the background's current position with it's target x position
            Vector3 backgrounTargetPos = new Vector3(backgrounTargetPosX, 
                                                      backgrounds [i].position.y, 
                                                      backgrounds [i].position.z);

            // Fade between current position and the target position using lerp
            backgrounds [i].position = Vector3.Lerp(backgrounds [i].position,
                                                   backgrounTargetPos,
                                                   smoothing * Time.deltaTime);
        }

        // Set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}
