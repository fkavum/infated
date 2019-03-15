using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBG : MonoBehaviour
{
    private Camera _Camera;
    private float spriteX, spriteY;
    [SerializeField]
    private char id;
    private SeamlessBG parentObj;

    public void Initialize(Camera cam, float spriteX, float spriteY, char id)
    {
        _Camera = cam;
        this.spriteX = spriteX;
        this.spriteY = spriteY;
        this.id = id;

        if (id == 'M')
        {
            //Middle
            transform.position.Set(0, 0, 0);
        }
        else if(id == 'R')
        {
            //Right
            transform.Translate(new Vector3(spriteX, 0, 0));
        }
        else if(id == 'L')
        {
            //Left
            transform.Translate(new Vector3(-spriteX, 0, 0));
        }

        parentObj = GetComponentInParent<SeamlessBG>();

    }

    public void SetID(char id) {
        this.id = id;
    }
    public char GetID()
    {
        return id;
    }
    private void Awake()
    {
     
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(id == 'M')
        {
            return;
        }

        Rect camSize = _Camera.pixelRect;
        Vector3 camPos = _Camera.transform.position;

        if (camPos.x < spriteX / 2 + transform.position.x && camPos.x > transform.position.x - spriteX / 2)
        {
            parentObj.Rearrange(id);
        }
    }
}
