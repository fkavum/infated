using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockConcatanate : MonoBehaviour
{
    public GameObject self = null;
    public int amount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableSpriteMask(){
        foreach(Transform child in transform){
            if(child.gameObject.name == "magic_spritesheet_6"){
                child.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
            }
            if(child.gameObject.name == "New Sprite Mask"){
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    void Concatanate(){
        if(amount == 0){
            return;
        }

        Vector3 pos = transform.position;
        pos.y += 1.25f;
        GameObject iceBlockInstance = Instantiate(self, pos, Quaternion.identity);
        iceBlockInstance.GetComponent<IceBlockConcatanate>().amount = amount - 1;
        DisableSpriteMask();
    }
}
