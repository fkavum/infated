using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _GameObject;
    public Vector3 _position;
    public Camera _camera;
    // Start is called before the first frame update

    private void Awake()
    {
       _GameObject = Resources.Load<GameObject>("Sancar/Prefab/Sancar");
    }

    void Start()
    {
        GameObject instance = Instantiate(_GameObject, _position, Quaternion.identity);
        _camera.GetComponent<CameraFollow>().SetTarget(instance.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
