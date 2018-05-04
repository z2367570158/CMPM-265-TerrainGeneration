using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TerrainGenerator : MonoBehaviour {

    [Range(0,256)]
    public int width = 256;

    public int depth = 20;
    public float scale;

    public float offsetX = 100f;
    public float offsetY = 100f;

    public void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();

        terrain.terrainData.heightmapResolution = width + 1;
        terrain.terrainData.size = new Vector3(width, depth, width);
        terrain.terrainData.SetHeights(0, 0, GenerateHeights());

    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, width];

        for(int x=0; x<width; x++)
        {
            for(int y=0; y< width; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float) x / width * scale + offsetX;
        float yCoord = (float) y / width * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
