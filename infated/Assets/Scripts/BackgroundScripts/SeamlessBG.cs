using UnityEngine;
using System.Collections;

public class SeamlessBG : MonoBehaviour
{
    private float _horizontalLength;       //A float to store the x-axis length of the collider2D attached to the GameObject.
    private GameObject[] objectBGs = new GameObject[3];
    private TrackBG[] trackBGs = new TrackBG[3];
    private float spriteX, spriteY;

    public Camera _camera;
    public GameObject _backgroundTexture;

    //Awake is called before Start.
    private void Awake()
    {
        Sprite _Sprite = _backgroundTexture.GetComponent<SpriteRenderer>().sprite;   
        spriteX = _Sprite.textureRect.width / _Sprite.pixelsPerUnit;
        spriteY = _Sprite.textureRect.height / _Sprite.pixelsPerUnit;

        objectBGs[0] = Instantiate(_backgroundTexture);
        objectBGs[1] = Instantiate(_backgroundTexture);
        objectBGs[2] = Instantiate(_backgroundTexture);

        objectBGs[0].transform.parent = this.transform;
        objectBGs[1].transform.parent = this.transform;
        objectBGs[2].transform.parent = this.transform;

        objectBGs[0].GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        objectBGs[1].GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        objectBGs[2].GetComponent<SpriteRenderer>().sortingLayerName = "Background";

        trackBGs[0] = objectBGs[0].AddComponent<TrackBG>();
        trackBGs[1] = objectBGs[1].AddComponent<TrackBG>();
        trackBGs[2] = objectBGs[2].AddComponent<TrackBG>();

        trackBGs[0].Initialize(_camera, spriteX, spriteY, 'L');
        trackBGs[1].Initialize(_camera, spriteX, spriteY, 'M');
        trackBGs[2].Initialize(_camera, spriteX, spriteY, 'R');

    }

    //Update runs once per frame
    private void Update()
    {
        Debug.Log(trackBGs[0].GetID() + " " + trackBGs[1].GetID() + " " + trackBGs[2].GetID() );
    }

    public void Rearrange(char newmid)
    {
        int l_index = 0, m_index = 0, r_index = 0;
        if (newmid == 'L')
        {
            for (int i = 0; i < 3; i++)
            {
                if (trackBGs[i].GetID() == 'L') { l_index = i; }
                else if (trackBGs[i].GetID() == 'R'){
                    r_index = i;
                    objectBGs[i].transform.Translate(new Vector3(-3 * spriteX, 0, 0));
                }
                else if (trackBGs[i].GetID() == 'M'){ m_index = i; }
            }

            //Make L->M, M->R, R->L
            trackBGs[l_index].SetID('M');
            trackBGs[r_index].SetID('L');
            trackBGs[m_index].SetID('R');
            Debug.Log("Rearranged: L");
        }
        else if (newmid == 'R')
        {
            for (int i = 0; i < 3; i++)
            {
                if (trackBGs[i].GetID() == 'R'){ r_index = i; }
                else if (trackBGs[i].GetID() == 'L') {
                    l_index = i;
                    objectBGs[i].transform.Translate(new Vector3(3 * spriteX, 0, 0));
                }
                else if (trackBGs[i].GetID() == 'M') { m_index = i; }
            }

            //Make M->L, R->M, L->R
            trackBGs[l_index].SetID('R');
            trackBGs[r_index].SetID('M');
            trackBGs[m_index].SetID('L');
            Debug.Log("Rearranged: R");
        }
        else
        {
            Debug.Log("Rearranged: M");
            return;
        }
    }
}