using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode
    {
        NoiseMap, ColorMap, Mesh
    };

    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;

    [Range(0, 1f)]
    public float persistance;
    public float lacunarity;

    public float heightMultiplier;

    public int seed;
    public Vector2 offset;

    [Range(0, 1f)]
    public float volcanoHeight;

    public bool autoUpdate;


    public TerrainType[] regions;


    public void GenerateMap()
    {

        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[0].height)
                    {
                        colorMap[y * mapWidth + x] = regions[0].color;
                        break;
                    }
                    else if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = Color.Lerp(regions[i - 1].color, regions[i].color, Mathf.InverseLerp(regions[i - 1].height, regions[i].height, currentHeight));
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplier,volcanoHeight),TextureGenerator.TextureFromColorMap(colorMap,mapWidth,mapHeight));

    }



    void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;

        if (mapHeight < 1)
            mapHeight = 1;

        if (noiseScale <= 0)
            noiseScale = 0.01f;

        if (lacunarity < 1)
            lacunarity = 1;

        if (octaves < 0)
            octaves = 0;
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}
