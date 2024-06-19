using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    
    public void OnUse() {
        isOpen = !isOpen;

        Animation _anim = GetComponent<Animation>();

        string animationName = _anim.clip.name;
        AnimationState state = _anim[animationName];

        if (isOpen) {
            state.time = 0;
            state.speed = 1;
            _anim.Play();
        }
        else {
            state.time = 1;
            state.speed = -1;
            _anim.Play();
        }
    }
}
