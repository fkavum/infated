using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextLevel : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
       Debug.Log(gameObject.transform.parent.parent.name);


        if (collision.gameObject.tag == "Player") { 
        Application.LoadLevel(gameObject.transform.parent.parent.name);
        }
    }
}
