using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisibilityToggler : MonoBehaviour
{
    public GameObject _Object;
    private Animator _Animator;
    private Renderer _Renderer;
    // Start is called before the first frame update
    void Start()
    {
        _Animator = _Object.GetComponent<Animator>();
        _Object.GetComponent<MeshRenderer>().sortingOrder = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        fadeIn();
    }

    private void OnTriggerExit2D(Collider2D other){
       fadeOut();
    }

    private void fadeIn(){
        _Animator.Play("FadeIn");
    }
    private void fadeOut(){
        _Animator.Play("FadeOut");
    }
}
