using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infated.CoreEngine
{
    public class CharacterMana : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(currentRegenCooldown <= 0)
            {
                currentRegenCooldown = 0;
                Mana += RegenAmount;
            }
            else
            {
                currentRegenCooldown -= Time.deltaTime;
            }
        }

        public float RegenCooldown = 4.0f;
        public float RegenAmount = 2.5f;

        private float Mana = 100.0f;
        private float currentRegenCooldown = 0.0f;

        public bool spendMana(float amount)
        {
            if(this.Mana >= amount)
            {
                this.Mana -= amount;
                currentRegenCooldown = RegenCooldown;
                return true;
            }
            return false;
        }



    }
}