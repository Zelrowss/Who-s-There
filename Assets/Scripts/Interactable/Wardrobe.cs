using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{

    public GameObject wCamera;
    private Animation anim;

    IEnumerator SetActiveCamera(bool active, float time) {
        wCamera.SetActive(active);
        yield return new WaitForSeconds(time);

        anim["Wardrobe_doors"].time = 1;
        anim["Wardrobe_doors"].speed = -1;
        anim.Play();

        if (active) {
            yield return new WaitForSeconds(.75f);
            anim.Stop();
        }

    }

    public void OnUse() {
        anim["Wardrobe_doors"].time = 0;
        anim["Wardrobe_doors"].speed = 1;
        anim.Play();
        StartCoroutine(SetActiveCamera(true, 1.15f));
    }

    public void OnEscape() {
        anim["Wardrobe_doors"].time = .25f;
        anim["Wardrobe_doors"].speed = 1;
        anim.Play();
        StartCoroutine(SetActiveCamera(false, 1.15f));
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
