using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darts : MonoBehaviour
{

    private DartsTarget _dartTarget;
    private bool hit = false;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject == null) return;
        if (hit) return;

        hit = true;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Rigidbody>().useGravity = false;

        //GetComponent<AudioSource>().Play();

        _dartTarget.dartAmount += 1;
        _dartTarget.SpawnNewDart();
    }

    public void Launch() {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 2, ForceMode.Impulse);
        rb.useGravity = true;   

        _dartTarget.currentDart = null;
    }
 
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;

        _dartTarget = FindObjectOfType<DartsTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
