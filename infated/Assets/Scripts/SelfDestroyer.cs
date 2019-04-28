using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    
    public float _timer = 2.0f;
    public bool _haveAnimation = false;
    public Animator animator;
    public string animationName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(_timer <= 0){
            if(!_haveAnimation || animator == null)
                destroySelf();
            else
                animator.Play(animationName);

        }
        _timer -= Time.deltaTime;

    }

    void destroySelf(){
        Object.Destroy(this.gameObject);
    }
}
