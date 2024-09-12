using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;
    Camera playerCamera;

    Vector2 Camrotation;

    public bool sprintMode = false;

    [Header("Movement settings")]
    public float playerspeed = 5.0f;
    public float sprintMultiplier = 2.5f;
    public float playerjumpheight = 5f;
    public float groundDetectDistance = 1f;
    

    [Header("User settings")]
    public bool sprintToggleoption = false;
    public float mouseSensitivity = 2.0f;
    public float xsensitivity = 2.0f;
    private float ysensitivity = 2.0f;
    public float camRotationLimit = 90f;

    public PlayerController(float mouseSensitivity)
    {
        this.mouseSensitivity = mouseSensitivity;
    }

    public float Ysensitivity {get => ysensitivity; set => ysensitivity = value;}

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        playerCamera = transform.GetChild(0).GetComponent<Camera>();

        Camrotation = Vector2.zero;
        Cursor.visible =  false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Camrotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        Camrotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        Camrotation.y = Mathf.Clamp(Camrotation.y, -camRotationLimit, camRotationLimit);

        playerCamera.transform.localRotation = Quaternion.AngleAxis(Camrotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(Camrotation.x, Vector3.up);

        Vector3 temp = myRB.velocity;

        float verticalMove = Input.GetAxisRaw("Vertical");
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        if(!sprintToggleoption)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                sprintMode = true;

                    if (Input.GetKeyUp(KeyCode.LeftShift))
                sprintMode = false;
        }

        if(sprintToggleoption)
        {
            if (Input.GetKey(KeyCode.LeftShift) && verticalMove > 0)
                    sprintMode = true;

            if (verticalMove <= 0)
                sprintMode = false;
        }
        {
            if (!sprintMode)
                temp.x = verticalMove * playerspeed;

            if (!sprintMode)
                temp.x = verticalMove * playerspeed * sprintMultiplier;
            
            temp.z = horizontalMove * playerspeed;
        }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0)
                sprintMode = true;
        if (Input.GetAxisRaw("Vertical") <= 0)
            sprintMode = false;

            if (sprintMode)
                temp.x = Input.GetAxisRaw("Vertical") * playerspeed;

        if (!sprintMode)
            temp.x = Input.GetAxisRaw("Vertical") * playerspeed * sprintMultiplier;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            sprintMode = false;

        temp.x = Input.GetAxisRaw("Horizontal") * playerspeed;

        temp.z = Input.GetAxisRaw("Vertical") * playerspeed;

        if (Input.GetKeyDown(KeyCode.Space))
            temp.y = playerjumpheight;

        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
            temp.y = playerjumpheight;

        myRB.velocity = (temp.z * transform.forward) + (temp.x * transform.right) + (transform.up * temp.y);

    }
}
