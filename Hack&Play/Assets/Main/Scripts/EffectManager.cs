using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    // Start is called before the first frame update
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
