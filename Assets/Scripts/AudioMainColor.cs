using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMainColor : MonoBehaviour
{
    [SerializeField] private int _band;
    [SerializeField] private float _startScale, _scaleMultiplier;
    [SerializeField] private bool _useBuffer = true;

    [SerializeField] private AudioPeer audioPeer;

    [SerializeField] private Color _color;

    private Material _objectMaterial;

    GravityAttraction ga;

    [SerializeField] Color[] colors;


    void Start()
    {
        _objectMaterial = transform.GetComponent<Renderer>().material;

        if (gameObject.GetComponent<GravityAttraction>() != null)
        {
            ga = gameObject.GetComponent<GravityAttraction>();
        }
    }


    void Update()
    {

        if (ga != null)
        {
            _band = ga.attractionGroup;
            _color = colors[ga.attractionGroup];
        }

        if (_useBuffer)
        {
            _objectMaterial.SetColor("_Color", _color * (audioPeer._audioBandBuffer[_band] * _scaleMultiplier + _startScale));
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer._freqBands[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
        }

    }
}
