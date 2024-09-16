using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;
    Camera playerCamera;

    Vector2 Camrotation;

    public Transform WeaponSlot;

    public bool sprintMode = false;

    [Header("PlayerStats")]
    public int maxHealth = 100;
    public int Health = 100;
    public int Healthrestore = 25;

    [Header("WeaponStats")]
    public bool canFire = true

    [Header("Movement settings")
    public float playerspeed = 5.0f;
    public float sprintMultiplier = 2.5f;
    public float playerjumpheight = 1f;
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

    public float Ysensitivity { get => ysensitivity; set => ysensitivity = value; }

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        playerCamera = transform.GetChild(0).GetComponent<Camera>();

        Camrotation = Vector2.zero;
        Cursor.visible = false;
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

        temp.x = Input.GetAxisRaw("Horizontal") * playerspeed;
        temp.z = Input.GetAxisRaw("Vertical") * playerspeed;

        if (!sprintToggleoption)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                sprintMode = true;

            if (Input.GetKeyUp(KeyCode.LeftShift))
                sprintMode = false;
        }

        if (sprintToggleoption)
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

            if (sprintToggleoption)

                if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0)
                    sprintMode = true;

            if (Input.GetAxisRaw("Vertical") <= 0)
                sprintMode = false;
        }
        {
            if (!sprintMode)
                temp.x = verticalMove * playerspeed;

            if (sprintMode)
                temp.x = verticalMove * playerspeed * sprintMultiplier;

            temp.z = horizontalMove * playerspeed;

            if (Input.GetKeyDown(KeyCode.Space))
                temp.y = playerjumpheight;

            if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
                temp.y = playerjumpheight;

            myRB.velocity = (transform.forward * temp.x) + (transform.right * temp.z) + (transform.up * temp.y);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((Health < maxHealth) && collision.gameObject.tag == "HealthPickup")
        {
            Health += Healthrestore;

            if (Health > maxHealth)
                Health = maxHealth;

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "weapon")
            collision.gameObject.transform.SetParent(WeaponSlot);
    }

    private void cooldown(bool condition float timelimit)
    {
        float timer = 0;

        if (timer < timelimit)
            timer += Time.deltaTime;

        else
            condition = true;

    }
    IEnumerable cooldown(float time)
    {
        new WaitForSeconds(time);
        //canfire = true;
    }
}
