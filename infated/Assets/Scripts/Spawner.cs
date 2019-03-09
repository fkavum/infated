using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _GameObject;
    public Vector3 _position;
    // Start is called before the first frame update

    private void Awake()
    {
       _GameObject = Resources.Load<GameObject>("Sancar/Prefab/Sancar");

    }

    void Start()
    {
        Instantiate(_GameObject, _position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
