using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMagicInteractions : MonoBehaviour
{
    public bool isFacingRight;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = gameObject.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, isFacingRight ? Vector2.right : Vector2.left);
        if(hit.collider != null){
            Debug.Log("Raycast Start: " + hit.collider.tag);
                if(hit.collider.tag == "BurnableIvy"){
                    hit.transform.gameObject.GetComponent<Animator>().Play("IvyBurnDown");
                }
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
