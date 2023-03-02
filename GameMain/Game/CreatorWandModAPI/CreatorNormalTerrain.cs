using Engine;
using Game;
using System;

namespace CreatorWandModAPI
{
    public class CreatorNormalTerrain : ITerrainContentsGenerator
    {
        public int OceanLevel => throw new NotImplementedException();

        public float CalculateHeight(float x, float z)
        {
            throw new NotImplementedException();
        }

        public int CalculateHumidity(float x, float z)
        {
            throw new NotImplementedException();
        }

        public float CalculateMountainRangeFactor(float x, float z)
        {
            throw new NotImplementedException();
        }

        public float CalculateOceanShoreDistance(float x, float z)
        {
            throw new NotImplementedException();
        }

        public int CalculateTemperature(float x, float z)
        {
            throw new NotImplementedException();
        }

        public Vector3 FindCoarseSpawnPosition()
        {
            throw new NotImplementedException();
        }

        public void GenerateChunkContentsPass1(TerrainChunk chunk)
        {
            throw new NotImplementedException();
        }

        public void GenerateChunkContentsPass2(TerrainChunk chunk)
        {
            throw new NotImplementedException();
        }

        public void GenerateChunkContentsPass3(TerrainChunk chunk)
        {
            throw new NotImplementedException();
        }

        public void GenerateChunkContentsPass4(TerrainChunk chunk)
        {
            throw new NotImplementedException();
        }
    }
}