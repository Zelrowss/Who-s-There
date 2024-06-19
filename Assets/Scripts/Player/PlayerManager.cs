using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerManager : MonoBehaviour
{

    [Header("Reference")]
    public Volume _volume;
    public PlayerAudioManager _playerAudioManager;

    [Header("Values")]
    public bool isHiden = false;

    void Awake() {
        _volume = GetComponentInChildren<Volume>();
        _playerAudioManager = GetComponentInChildren<PlayerAudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
