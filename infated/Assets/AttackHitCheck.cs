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
        public GameObject attackerObject;
        private SpriteRenderer _renderer;
        private CharacterAttack _attack = null;

        public GameObject SplashCheckerObj = null;
        private SplashCheck _splashCheck = null;
        public bool canHurt = false;
        void Start()
        {
            _renderer = spritesheetObject.GetComponent<SpriteRenderer>();
            _attack = attackerObject.GetComponent<CharacterAttack>();
            _splashCheck = SplashCheckerObj.GetComponent<SplashCheck>();
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
            if((other.tag == "Enemy" && this.tag == "Player") || (other.tag == "Player" && this.tag == "Enemy")) {
                if(canHurt){
                    Health hp = other.transform.GetComponent<Health>();
                    if(hp != null){
                        if(_attack.Buff == ChargingMagicType.NONE){
                            hp.Damage(_attack.Damage, this.gameObject, 0.15f, 0.282f);
                        }
                        else if(_attack.Buff == ChargingMagicType.FIRE){
                            hp.Damage(_attack.Damage, this.gameObject, 0.15f, 0.282f, true);
                        }
                        else if(_attack.Buff == ChargingMagicType.ICE){
                            hp.Damage(_attack.Damage, this.gameObject, 0.15f, 0.282f);
                            if(_splashCheck != null){
                                _splashCheck.DealDamage();
                            }

                        }
                        else{}
                        
                    }
                    //Debug.Log("Player hit Enemy");
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
