using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatZone : MonoBehaviour
{
    private float timer = 0;
    private bool isInCombat = false;
    private void Update()
    {

        if (isInCombat) { 
        timer += Time.deltaTime;
        }

        Debug.Log(timer);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Combata girdim Allah Allah Allaahh!");
        if (collision.gameObject.tag == "Player")
        {
            isInCombat = true;

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Combattan cıktım halla hlaa hallaa!");
            isInCombat = false;
            Character _char = collision.gameObject.GetComponent<Character>();
            _char._userProfiler.totalCombatTime += timer;
            timer = 0f;

        }
    }
}
