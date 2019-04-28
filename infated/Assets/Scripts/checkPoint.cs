using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Character _char = other.gameObject.GetComponent<Character>();
            _char._userProfiler.checkPointReached(gameObject.name);
            gameObject.SetActive(false);
           
        }
    }


}
