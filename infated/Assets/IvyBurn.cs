﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IvyBurn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void destroySelf(){
        Object.Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Ivy " + other.tag);
        
    }

}