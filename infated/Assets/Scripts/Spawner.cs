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
<<<<<<< Updated upstream
       _GameObject = Resources.Load<GameObject>("Sancar/Prefab/Sancar");

=======
        _camera.GetComponent<CameraFollow>();
        _GameObject = Resources.Load<GameObject>("Sancar/Prefab/Sancar");
       
>>>>>>> Stashed changes
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
