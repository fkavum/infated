using UnityEngine;
using System.Collections;
using Unity.Collections;
using Infated.Tools;

namespace Infated.CoreEngine
{
    public class AttackHitCheck : MonoBehaviour
    {
        // Start is called before the first frame update

        private bool isFacingRight = false;
        public GameObject spritesheetObject;
        private SpriteRenderer _renderer;
        private CharacterMana _mana = null;
        public bool canHurt = false;
        void Start()
        {
            _renderer = spritesheetObject.GetComponent<SpriteRenderer>();
            _mana = spritesheetObject.GetComponent<CharacterMana>();
        }

        // Update is called once per frame
        void Update()
        {
            //False means left, true means right
            bool flip = _renderer.flipX;
            Vector3 scale = transform.localScale;
            if(isFacingRight ^ flip){
                scale.x *= -1;
            }
            isFacingRight = flip;
            transform.localScale = scale;
        }

        void DealDamage(Collider2D other){
            if(other.tag == "Enemy" && this.tag == "Player"){
                if(canHurt){
                    other.transform.GetComponent<Health>().Damage(10, this.gameObject, 5, 0.5f);
                    Debug.Log("Player hit Enemy");
                }
            }
            else if(other.tag == "Player" && this.tag == "Enemy"){
                if(canHurt) {
                    other.transform.GetComponent<Health>().Damage(10, this.gameObject, 5, 0.5f);
                    Debug.Log("Enemy hit Player");
                }   
            }
        }
        void OnTriggerEnter2D(Collider2D other){
            DealDamage(other);
        }

        void OnTriggerStay2D(Collider2D other){
            DealDamage(other);
        }
    }
}
