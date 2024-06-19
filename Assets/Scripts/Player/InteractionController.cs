using System.Collections;
using System.Collections.Generic;
using Knife.HDRPOutline.Core;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [Header("References")]
    public GameObject defaultCrossHair;
    public GameObject interactionCrossHair;
    public Material outlineMaterial;

    [Header("Values")]
    public float interactionDistance;
    public LayerMask layerMask;
    private GameObject lastViewObject;
    public GameObject currentGameObject;
    public GameObject currentItem;
    public GameObject inViewObject;
    

    IEnumerator ResetLastObject() {
        yield return new WaitForSeconds(1);
        inViewObject = null;
    }

    void OutlineHandler(GameObject obj, bool active) {
        if (obj.GetComponent<LODGroup>() != null) {
            OutlineObject[] _outlineObj = obj.GetComponentsInChildren<OutlineObject>();
    
            foreach (OutlineObject _outline in _outlineObj) {
                _outline.enabled = active;
            }
            }
        else {
            if (obj.GetComponent<OutlineObject>() == null) {
                obj.AddComponent<OutlineObject>();
                obj.GetComponent<OutlineObject>().Material = outlineMaterial;
            }
            else {
                obj.GetComponent<OutlineObject>().enabled = active;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, layerMask))
        {
            inViewObject = hit.collider.gameObject;
            
            defaultCrossHair.SetActive(false);
            interactionCrossHair.SetActive(true);
            
            OutlineHandler(inViewObject, true);

            if (lastViewObject != null && inViewObject != lastViewObject) {
                OutlineHandler(lastViewObject, false);
            }

            lastViewObject = inViewObject;
        }
        else {
            defaultCrossHair.SetActive(true);
            interactionCrossHair.SetActive(false);

            if (inViewObject != null) {
                OutlineHandler(inViewObject, false);

                StartCoroutine(ResetLastObject());
            }

        }
    }

    public void Interacte() {
        

        if (inViewObject.CompareTag("DartsTarget")) {
            currentGameObject = inViewObject;
            currentGameObject.GetComponent<DartsTarget>().OnUse();
        }

        if (inViewObject.CompareTag("Flashlight")) {
            currentItem = inViewObject;
            currentItem.GetComponent<Flashlight>().OnTake();
        }

        if (inViewObject.CompareTag("Wardrobe")) {
            currentGameObject = inViewObject;
            currentGameObject.GetComponent<Wardrobe>().OnUse();
        }

        if (inViewObject.CompareTag("Drawer")) {
            inViewObject.GetComponent<Door>().OnUse();
        }
    }
}
