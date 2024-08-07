using UnityEngine;
using UnityEngine.UI;

public class interaction : MonoBehaviour
{
    public float interactionDistance = 10f;
    public LayerMask interactionLayers;
    public security_cam_system script;
    public CacheCache cacheCacheScript;

    public Image premierCoeur;
    public Image DeuxiemeCoeur;
    public Image TroisiemeCoeur;

    public bool un;
    public bool deux;
    public bool trois;

    private void DisableHeart(Image heart)
    {
        if (heart != null)
        {
            heart.enabled = false;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayers))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    script.previousCam();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    script.nextCam();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            int currentCamera = script.GetCurrentCamera();
            Debug.Log("Current camera index: " + currentCamera);

            if (currentCamera == cacheCacheScript.cameraSelect)
            {
                Debug.Log("gg");
                cacheCacheScript.cameraSelect = Random.Range(1, cacheCacheScript.cameraNumber);

                if (!un)
                {
                    un = true;
                }
                else if (!deux)
                {
                    deux = true;
                }
                else if (!trois)
                {
                    trois = true;
                }
            }
            else
            {
                Debug.Log("C'est le caca");

                if (premierCoeur != null)
                {
                    DisableHeart(premierCoeur);
                    premierCoeur = null;
                }
                else if (DeuxiemeCoeur != null)
                {
                    DisableHeart(DeuxiemeCoeur);
                    DeuxiemeCoeur = null;
                }
                else if (TroisiemeCoeur != null)
                {
                    DisableHeart(TroisiemeCoeur);
                    TroisiemeCoeur = null;
                }
            }
        }
    }
}
