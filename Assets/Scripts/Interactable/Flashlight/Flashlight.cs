using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlightPos;
    private Rigidbody rb;

    public void OnTake(){
        transform.parent = flashlightPos.transform;
        transform.SetPositionAndRotation(flashlightPos.transform.position, flashlightPos.transform.rotation);
        GetComponent<MeshCollider>().enabled = false;
        rb.useGravity = false;
    }

    public void OnThrow() {
        transform.parent = null;
        GetComponent<MeshCollider>().enabled = true;
        rb.useGravity = true;

        rb.AddForce(transform.forward * 2, ForceMode.Impulse);
    }

    public void OnUse() {
        GetComponentInChildren<Light>().enabled = !GetComponentInChildren<Light>().enabled;

        GetComponent<AudioSource>().Play();
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
