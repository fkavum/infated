using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public bool _SmoothCamera = true;
    [Range(0.0f, 1.0f)]
    public float _ScreenPercentage = 0.25f;

    private GameObject _player;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null) return;
        
    }

    void SetPlayer(GameObject p) {
        _player = p;
    }
}
