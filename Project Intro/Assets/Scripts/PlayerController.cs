using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;
    Camera playerCamera;

    Transform cameraholder;

    Vector2 Camrotation;

    public Transform WeaponSlot;

    public bool sprintMode = false;

    [Header("PlayerStats")]
    public int maxHealth = 100;
    public int Health = 100;
    public int Healthrestore = 25;

    [Header("WeaponStats")]
    public bool canFire = true;
    public int firemode = 0;
    public int weaponID = 0;
    public float fireRate = 0;
    public float maxAmmo = 120;
    public float currentAmmo = 60;
    public float Ammorestoreamount = 0;
    public float currentClip = 0;
    public float clipsize = 1;
    public GameObject Shot;
    public GameObject shot2;
    public float bulletspeed = 15f; 
    public float bulletlifespan = 0;


    [Header("Movement Settings")]
    public float playerspeed = 5.0f;
    public float sprintMultiplier = 1.5f;
    public float playerjumpheight = 15.0f;
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
        playerCamera = Camera.main;
        cameraholder = transform.GetChild(0);

        Camrotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.transform.position = cameraholder.position;

        Camrotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        Camrotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        Camrotation.y = Mathf.Clamp(Camrotation.y, -camRotationLimit, camRotationLimit);

        playerCamera.transform.rotation = Quaternion.Euler(-Camrotation.y, Camrotation.x, 0);
        transform.localRotation = Quaternion.AngleAxis(Camrotation.x, Vector3.up);

        if(Input.GetMouseButtonDown(0) && canFire && currentClip > 0 && weaponID >= 0)
        {
            GameObject s = Instantiate(Shot, WeaponSlot.position, WeaponSlot.rotation);
            s.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * bulletspeed);
            Destroy(s,bulletlifespan);

            canFire = false;
            currentClip--;
            StartCoroutine("cooldownFire");
        }
        if (Input.GetKeyDown(KeyCode.R))
            reloadClip();

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

            if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
                temp.y = playerjumpheight;

            myRB.velocity = (transform.forward * temp.x) + (transform.right * temp.z) + (transform.up * temp.y);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon1")
        {

            other.gameObject.transform.SetPositionAndRotation(WeaponSlot.position, WeaponSlot.rotation);

            other.gameObject.transform.SetParent(WeaponSlot);

            switch(other.gameObject.name)
            {
                case "Weapon1":
                    canFire = true;
                    firemode = 0;
                    weaponID = 1;
                    fireRate = 0.25f;
                    maxAmmo = 100;
                    currentAmmo = 25;
                    Ammorestoreamount = 25;
                    currentClip = 20;
                    clipsize = 25;
                    bulletlifespan = 60;
                    break;

            }
            if (other.gameObject.tag == "aa12")
            {
                other.gameObject.transform.SetPositionAndRotation(WeaponSlot.position, WeaponSlot.rotation);

                other.gameObject.transform.SetParent(WeaponSlot);

                switch(other.gameObject.name)
                {
                    case "aa12":
                        canFire = true;
                        firemode = 0;
                        weaponID = 1;
                        fireRate = 0.5f;
                        maxAmmo = 80;
                        currentAmmo = 10;
                        Ammorestoreamount = 20;
                        currentClip = 20;
                        clipsize = 20;
                        bulletlifespan = 60;
                        break;

                    default:
                        break;
                }
            }
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
        if ((currentAmmo < maxAmmo) && collision.gameObject.tag == "Weapon1")
        {
            currentAmmo += Ammorestoreamount;

            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;

            Destroy(collision.gameObject);
        }
        if ((currentAmmo < maxAmmo) && collision.gameObject.tag == "aa12")
        {
            currentAmmo += Ammorestoreamount;

            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Weapon1")
            collision.gameObject.transform.SetParent(WeaponSlot);

        if (collision.gameObject.tag == "aa12")
            collision.gameObject.transform.SetParent(WeaponSlot);
    }

    public void reloadClip()
    {
        if (currentClip >= clipsize)
            return;

        else;
        {
            float reloadCount = clipsize - currentClip;

            if (currentAmmo > reloadCount);
            {
                currentClip += currentAmmo;

                currentAmmo = 0;
                return;
                {
                    currentClip += reloadCount;
                    currentAmmo -= reloadCount;
                }
            }
        }
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}
