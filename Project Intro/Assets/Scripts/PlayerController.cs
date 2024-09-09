using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;

    public float playerspeed = 10.0f;
    public float playerjumpheight = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = myRB.velocity;

        temp.x = Input.GetAxisRaw("Vertical") * playerspeed;
        temp.z = Input.GetAxisRaw("Horizontal") * playerspeed;

        if(Input.GetKeyDown(KeyCode.Space)
            temp.y = playerjumpheight

        myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);

    }
}
