using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio512Cubes : MonoBehaviour
{
    [SerializeField] private GameObject _sampleCubePrefab;
    private GameObject[] _sampleCubes = new GameObject[512];
    [SerializeField] private float _maxScale = 1000f;
    [SerializeField] private float _maxHeight = 20f;

    [SerializeField] private AudioPeer audioPeer;

    [SerializeField] private float _rotationAfterInstantiate = -45f;

    [SerializeField] private Color _cubesColor;

    private float[] _bufferDescrease = new float[512];
    private float[] _bandsBuffer = new float[512];

    Material cubeMaterial;

    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            _bandsBuffer[i] = 0;
            GameObject _instanceSampleCube = Instantiate(_sampleCubePrefab);
            _instanceSampleCube.transform.position = this.transform.position;
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "sampleCube " + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            _instanceSampleCube.transform.position = Vector3.forward * 15;
            _sampleCubes[i] = _instanceSampleCube;
            _instanceSampleCube.transform.localPosition = new Vector3(_instanceSampleCube.transform.localPosition.x, 0, _instanceSampleCube.transform.localPosition.z);
        }
        this.transform.eulerAngles = new Vector3(0, this.transform.rotation.y, _rotationAfterInstantiate);
    }

    void Update()
    {
        for (int i = 0; i < 512; i++)
        {

            float audioValue = 0f;

            if (_sampleCubes != null)
            {
                audioValue = (audioPeer._samplesLeft[i] * _maxScale) + 0.2f;

                if (audioValue >= _bandsBuffer[i])
                {
                    _bandsBuffer[i] = audioValue;
                    _bufferDescrease[i] = 0.001f;
                }
                else
                {
                    //                _bandBuffer[i] -= _bufferDescrease[i];
                    //                _bufferDescrease[i] *= 1.2f;
                    _bufferDescrease[i] = (_bandsBuffer[i] - audioValue) / 8;
                    _bandsBuffer[i] -= _bufferDescrease[i];
                }

                if (_maxHeight >= _bandsBuffer[i])
                {
                    _sampleCubes[i].transform.localScale = new Vector3(0.05f, _bandsBuffer[i], 0.05f);
                }
                else
                {
                    _sampleCubes[i].transform.localScale = new Vector3(0.05f, _maxHeight, 0.05f);
                }

                _sampleCubes[i].GetComponent<Renderer>().material.SetColor("_Color", _cubesColor * audioPeer._samplesLeft[i] * _maxScale);
            }
        }
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y + 0.703125f, 0);
    }
}
