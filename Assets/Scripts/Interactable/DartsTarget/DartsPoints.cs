using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartsPoints : MonoBehaviour
{
    public int points;

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("Darts")) {
            print("touché : " + points);
            GameObject.FindObjectOfType<DartsTarget>().pointsCount += points;
        }
    }
}
