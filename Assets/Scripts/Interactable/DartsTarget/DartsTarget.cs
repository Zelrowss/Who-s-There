using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class DartsTarget : MonoBehaviour
{
    
    [Header("Reference")]
    public GameObject tCamera;
    public GameObject dartPrefab;
    private List<GameObject> allDarts = new List<GameObject>();
    
    [Header("Value")]
    public GameObject currentDart;
    public bool onIt;
    public int pointsCount = 0;
    private int pointNeeded = 0;
    public int dartAmount = 0;


    public void SpawnNewDart() {
        currentDart = GameObject.Instantiate(dartPrefab);
        currentDart.transform.position = tCamera.transform.position - new Vector3(0, 0, 1);
        currentDart.transform.rotation = tCamera.transform.rotation;

        allDarts.Add(currentDart);
    }

    public void OnUse() {
        onIt = true;

        pointNeeded = Random.Range(3, 150);

        SpawnNewDart();

        tCamera.SetActive(true);
    }

    public void OnEscape() {
        onIt = false;
        tCamera.SetActive(false);

        foreach(GameObject dartPrefab in allDarts) {
            Destroy(dartPrefab);
        }

        dartAmount = 0;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onIt && currentDart != null){
            Vector3 screenPosition = Mouse.current.position.ReadValue();
            screenPosition.z = gameObject.transform.position.z - 2.5f;

            Vector3 worldPosition = tCamera.GetComponent<Camera>().ScreenToWorldPoint(screenPosition);
            currentDart.transform.position = worldPosition;
        }
        
        if (onIt && pointsCount >= pointNeeded) {
            OnEscape();
        }
    }
}
