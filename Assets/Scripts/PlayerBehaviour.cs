using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Tooltip("The Player's movement speed")]
    int speed;

    private void Start()
    {
        speed = 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            transform.Translate(new Vector3(horizontal, 0, vertical) * (speed * 2) * Time.deltaTime); // Run
        else
            transform.Translate(new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime); // Walk

        //Debug.Log($"Horizontal value: {horizontal}, Vertical Value: {vertical}");

    }
}
