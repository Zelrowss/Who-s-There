using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class security_cam_system : MonoBehaviour
{
    public List<GameObject> cameras;
    public int cameraSelected;
    public security_cam_system other_button;

    public void nextCam()
    {
        cameraSelected = cameraSelected + 1;
        if(cameraSelected > cameras.Count - 1)
        {
            cameraSelected = 0;
        }
        if(cameraSelected > 0)
        {
            cameras[cameraSelected - 1].SetActive(false);
        }
        if (cameraSelected == 0)
        {
            cameras[cameras.Count - 1].SetActive(false);
        }
        cameras[cameraSelected].SetActive(true);
        other_button.cameraSelected = cameraSelected;
        Debug.Log(cameraSelected);
    }
    public void previousCam()
    {
        cameraSelected = cameraSelected - 1;
        if (cameraSelected < 0)
        {
            cameraSelected = cameras.Count - 1;
        }
        if (cameraSelected == cameras.Count - 1)
        {
            cameras[0].SetActive(false);
        }
        if (cameraSelected < cameras.Count - 1)
        {
            cameras[cameraSelected + 1].SetActive(false);
        }
        cameras[cameraSelected].SetActive(true);
        other_button.cameraSelected = cameraSelected;
        Debug.Log(cameraSelected);
    }

    public int GetCurrentCamera()
    {
        return cameraSelected;
    }

}

