using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class PerlinNoiseTerrain : MonoBehaviour
{

    public int depth = 20;

    public int width = 256;
    public int height = 256;

    public float scale = 20f;

    public float startOffsetX = 100f;
    public float startOffsetY = 100f;

    private float offsetX;
    private float offsetY;

    [SerializeField] private int _band;
    [SerializeField] private bool _useBuffer = true;
    [SerializeField] private AudioPeer audioPeer;
    [SerializeField] private float _scaleMultiplier = 1f, _startScale = 0.5f;

    Terrain terrain;

    private void Start()
    {
        terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    private void Update()
    {
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        if (_useBuffer)
        {
            terrainData.size = new Vector3(width, depth * (audioPeer._audioBandBuffer[_band] * _scaleMultiplier + _startScale), height);
        }
        else
        {
            terrainData.size = new Vector3(width, depth * (audioPeer._audioBand[_band] * _scaleMultiplier + _startScale), height);
        }

     //   Debug.Log(audioPeer._audioBandBuffer[audioPeer.GetMaximumAmplitudeBandIndexBuffer()]);

        if (audioPeer.GetMaximumAmplitudeBandIndexBuffer() == 3)
        {
            offsetX = startOffsetX * audioPeer._audioBandBuffer[audioPeer.GetMaximumAmplitudeBandIndexBuffer()];
            offsetY = startOffsetY * audioPeer._audioBandBuffer[audioPeer.GetMaximumAmplitudeBandIndexBuffer()];
        }

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                heights[i, j] = CalculateHeight(i, j);
            }
        }

        return heights;
    }

    float CalculateHeight (int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

}
