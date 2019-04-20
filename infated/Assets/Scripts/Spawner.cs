using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    public GameObject _Character;
    public GameObject _Gui;
    public Vector3 _Position;
    public Camera _Camera;
    // Start is called before the first frame update

    private void Awake()
    {
        if(_Gui){
            _Character.GetComponent<GUI_Writer>().assignGUI(_Gui);
        }
    }

    void Start()
    {
        GameObject instance = Instantiate(_Character, _Position, Quaternion.identity);
        _Camera.GetComponent<CameraFollow>().SetTarget(instance.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
