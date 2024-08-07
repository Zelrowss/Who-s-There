using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheCache : MonoBehaviour
{
    public int cameraNumber;
    public int cameraSelect;
    public bool monsterApparation;
    public security_cam_system script;
    public GameObject objectToModif;

    void Start()
    {
        StartCoroutine(selectCam());

    }

    // Update is called once per frame
    void Update()
    {
        if(cameraSelect == 0)
        {
            //Debug.Log("0");
            cameraSelect = 0;
        }
        else if(cameraSelect == 1)
        {
            //Debug.Log("1");
            cameraSelect = 1;
        }
        else if(cameraSelect == 2)
        {
            //Debug.Log("2");
            cameraSelect = 2;
        }
        else if (cameraSelect == 3)
        {
            //Debug.Log("3");
            cameraSelect = 3;
        }
        else if (cameraSelect == 4)
        {
            //Debug.Log("4");
            cameraSelect = 4;
        }
        else if (cameraSelect == 5)
        {
            //Debug.Log("5");
            cameraSelect = 5;
        }
    }

    public void DoSomething()
    {
        objectToModif.GetComponent<Renderer> ().material.color = new Color(255, 254, 252);
    }

    IEnumerator selectCam()
    {
        yield return new WaitForSeconds(15);
        cameraSelect = Random.Range(1, cameraNumber);

        if (cameraNumber != null)
        {
            yield return new WaitForSeconds(15);
            monsterApparation = true;
            DoSomething();
        }
    }
}
