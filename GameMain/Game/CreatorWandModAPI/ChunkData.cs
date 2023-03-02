//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using System;
using System.Collections.Generic;
using CreatorWandModAPI;
using Engine;
using Game;

public class ChunkData
{
	public class Chunk
	{
		public int chunkX;

		public int chunkY;

		public int[] Cells;

		public Chunk(int chunkX, int chunkY)
		{
			this.chunkX = chunkX;
			this.chunkY = chunkY;
		}
	}

	public List<Chunk> chunksData = new List<Chunk>();

	private readonly SubsystemTerrain subsystemTerrain;

	private readonly CreatorAPI creatorAPI;

	public ChunkData(CreatorAPI creatorAPI)
	{
		this.creatorAPI = creatorAPI;
		subsystemTerrain = creatorAPI.componentMiner.Project.FindSubsystem<SubsystemTerrain>(true);
	}

	public Chunk CreateChunk(int x, int z, bool unLimited = false)
	{
		int chunkX = x >> 4;
		int num = z >> 4;
		TerrainChunk terrainChunk = subsystemTerrain.Terrain.GetChunkAtCoords(chunkX, num);
		if (terrainChunk == null)
		{
			if (!unLimited)
			{
				return null;
			}
			terrainChunk = subsystemTerrain.Terrain.AllocateChunk(chunkX, num);
			while (terrainChunk.ThreadState < TerrainChunkState.Valid)
			{
				subsystemTerrain.TerrainUpdater.UpdateChunkSingleStep(terrainChunk, 15);
			}
		}
		Chunk chunk = new Chunk(chunkX, num)
		{
			Cells = new int[terrainChunk.Cells.Length]
		};
		for (int i = 0; i < terrainChunk.Cells.Length; i++)
		{
			chunk.Cells[i] = terrainChunk.Cells[i];
		}
		chunksData.Add(chunk);
		return chunk;
	}

	public int GetCellValueFast(int x, int y, int z)
	{
		int num = x >> 4;
		int num2 = z >> 4;
		Chunk chunk = null;
		foreach (Chunk chunksDatum in chunksData)
		{
			if (chunksDatum.chunkX == num && chunksDatum.chunkY == num2)
			{
				chunk = chunksDatum;
				break;
			}
		}
		if (chunk == null)
		{
			return 0;
		}
		return chunk.Cells[y + (x & 0xF) * 256 + (z & 0xF) * 256 * 16];
	}

	public Chunk GetChunk(int x, int z)
	{
		int num = x >> 4;
		int num2 = z >> 4;
		foreach (Chunk chunksDatum in chunksData)
		{
			if (chunksDatum.chunkX == num && chunksDatum.chunkY == num2)
			{
				return chunksDatum;
			}
		}
		return null;
	}

	public virtual void SetBlock(int x, int y, int z, int value)
	{
		try
		{
			if (y < 0 || y >= 256)
			{
				return;
			}
			int num = y + (x & 0xF) * 256 + (z & 0xF) * 256 * 16;
			Chunk chunk = GetChunk(x, z);
			if (chunk == null)
			{
				chunk = CreateChunk(x, z, creatorAPI.UnLimitedOfCreate);
				if (chunk != null)
				{
					if (num < chunk.Cells.Length)
					{
						chunk.Cells[num] = value;
					}
					return;
				}
            }
            if (chunk == null)
            {
                chunk = CreateChunk(x, z, creatorAPI.UnLimitedOfCreate);
                if (chunk == null)
                {
                    return;
                }
            }
            if (num < chunk.Cells.Length)
            {
                chunk.Cells[num] = value;
            }
            /*if (chunk == null)
			{
				chunk = CreateChunk(x, z, creatorAPI.UnLimitedOfCreate);
				if (chunk != null)
				{
					goto IL_0064;
				}
			}
			if (chunk == null)
			{
				chunk = CreateChunk(x, z, creatorAPI.UnLimitedOfCreate);
				if (chunk == null)
				{
					return;
				}
			}
			goto IL_0064;
IL_0064:
			if (num < chunk.Cells.Length)
			{
				chunk.Cells[num] = value;
			}*/
        }
		catch (Exception ex)
		{
			Log.Error(CreatorMain.Display_Key_Dialog("chunkDataErr") + ex.Message);
		}
	}

	public virtual void SetBlock(Point3 point3, int value)
	{
		SetBlock(point3.X, point3.Y, point3.Z, value);
	}

	public virtual void Render()
	{
		foreach (Chunk chunksDatum in chunksData)
		{
			TerrainChunk terrainChunk = subsystemTerrain.Terrain.GetChunkAtCoords(chunksDatum.chunkX, chunksDatum.chunkY);
			if (terrainChunk == null)
			{
				if (!creatorAPI.UnLimitedOfCreate)
				{
					continue;
				}
				terrainChunk = subsystemTerrain.Terrain.AllocateChunk(chunksDatum.chunkX, chunksDatum.chunkY);
			}
			for (int i = 0; i < terrainChunk.Cells.Length; i++)
			{
				terrainChunk.Cells[i] = chunksDatum.Cells[i];
			}
			terrainChunk.ModificationCounter++;
			if (creatorAPI.UnLimitedOfCreate)
			{
				terrainChunk.State = TerrainChunkState.Valid;
			}
			if (terrainChunk.State > TerrainChunkState.InvalidLight)
			{
				terrainChunk.State = TerrainChunkState.InvalidLight;
			}
			terrainChunk.WasDowngraded = true;
		}
		if (this != creatorAPI.revokeData)
		{
			chunksData.Clear();
		}
	}

	public void Clear()
	{
		chunksData.Clear();
	}
}