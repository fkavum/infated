using UnityEngine;
using System.Collections;
using Infated.Tools;

namespace Infated.CoreEngine
{	
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

        void OnTriggerEnter2D(Collider2D other){
            HitCheck(other);
        }
        void OnTriggerStay2D(Collider2D other){
            HitCheck(other);
        }

        void HitCheck(Collider2D other){
            Debug.Log("HitCheck");
            if(other.tag == "Enemy"){
                Health hp = other.GetComponent<Health>();
                if(hp != null){
                    hp.Damage(10, this.gameObject, 0.5f, 0.5f, true);
                }
            }
        }

        protected virtual void PlaySound(AudioClip sfx)
        {
            // we create a temporary game object to host our audio source
            GameObject temporaryAudioHost = new GameObject("TempAudio");
            // we set the temp audio's position

            // we add an audio source to that host
            AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
            // we set that audio source clip to the one in paramaters
            audioSource.clip = sfx;
            // we set the audio source volume to the one in parameters
            audioSource.volume = 100;
            // we start playing the sound
            audioSource.Play();

            Destroy(temporaryAudioHost, sfx.length);
        }
    }
}
