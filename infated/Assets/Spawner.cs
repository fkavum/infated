using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject _GameObject;
    public Vector3 _position;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(_GameObject, _position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
