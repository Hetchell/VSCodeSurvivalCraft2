using Grid2d = ModificationHolder.ModifierHolderTerrainGen.Grid2d;
using Grid3d = ModificationHolder.ModifierHolderTerrainGen.Grid3d;
using Game;
using MathUtils = Engine.MathUtils;

namespace ModificationHolder
{
    class BlockPlacement
    {
        private ModifierHolderTerrainGen proto0;
        private TerrainChunkGeneratorProviderActive activeChunkProvider;

        public BlockPlacement(ModifierHolderTerrainGen proto0, TerrainChunkGeneratorProviderActive activeChunkProvider){
            this.proto0 = proto0;
            this.activeChunkProvider = activeChunkProvider;
        }

        public void SetBlock(WorldSettings worldsettings, Grid2d[] grid2DCache, Grid3d grid3DCache, TerrainChunk chunk, params int[] par){
            double[] multipliers = new double[21];
            multipliers[0] = 0.25D;
            multipliers[1] = 0.25D;
            multipliers[2] = 0.25D;
            multipliers[3] = 0.25D;
            multipliers[4] = 0.25D;
            multipliers[5] = 0.25D;
            multipliers[6] = 0.125D;
            multipliers[7] = 0.01D;//0.1
            multipliers[8] = 1.0D;
            multipliers[9] = 1.0D;
            multipliers[10] = 1.0D;
            multipliers[11] = 1.0D;
            multipliers[12] = 1.0D;//1
            multipliers[13] = 1.0D;//1
            multipliers[14] = 1.0D;
            multipliers[15] = 1.0D;
            multipliers[16] = 1.0D;
            multipliers[17] = 1.0D;
            multipliers[18] = 1.0D;
            multipliers[19] = 1.0D;
            multipliers[20] = 1.0D;
            this.SetBlock(worldsettings, grid2DCache, grid3DCache, chunk, multipliers, par[0], par[1], par[2], par[3], 0, 0, 0);
        }
        private void SetBlock(WorldSettings worldsettings, Grid2d[] grid2DCache, Grid3d grid3DCache, TerrainChunk chunk, double[] q, params int[] par)
        {
            //Sequence is safe below. 
            int u = this.activeChunkProvider.OceanLevel * 1 + worldsettings.SeaLevelOffset * 0;
            //u = 15;
            int oceanLevel = u;
            int[] p = new int[6];
            for (int j = par[4]; j < grid3DCache.SizeZ - 1; j++)//x, i
            {
                p[0] = j * grid3DCache.SizeY;
                p[1] = (j + 1) * grid3DCache.SizeY;
                for (int k = par[5]; k < grid3DCache.SizeY - 1; k++)//z, j
                {
                    p[2] = (p[0] + k) * grid3DCache.SizeX;
                    p[3] = (p[0] + k + 1) * grid3DCache.SizeX;
                    p[4] = (p[1] + k) * grid3DCache.SizeX;
                    p[5] = (p[1] + k + 1) * grid3DCache.SizeX;
                    for (int i = par[6]; i < grid3DCache.SizeX - 1; i++)//y, k
                    {
                        double d4 = grid3DCache.m_data[p[2] + i] * q[17];
                        double d5 = grid3DCache.m_data[p[3] + i] * q[18];
                        double d6 = grid3DCache.m_data[p[4] + i] * q[19];
                        double d7 = grid3DCache.m_data[p[5] + i] * q[20];
                        double d0 = (grid3DCache.m_data[p[2] + i + 1] - d4) * q[0];
                        double d1 = (grid3DCache.m_data[p[3] + i + 1] - d5) * q[1];
                        double d2 = (grid3DCache.m_data[p[4] + i + 1] - d6) * q[2];
                        double d3 = (grid3DCache.m_data[p[5] + i + 1] - d7) * q[3];
                        for (int a = 0; a < 4; a++)//4
                        {
                            double d8 = (d6 - d4) * q[4];
                            double d9 = (d7 - d5) * q[5];
                            double d10 = d4 * q[15];
                            double d11 = d5 * q[16];
                            for (int b = 0; b < 4; b++)
                            {
                                double d12 = (d11 - d10) * q[6];
                                double d13 = d10;
                                //int x = a + i * 4;
                                //int y = b + j * 4;
                                //int x3 = x1 + a + i * 4;
                                //int z3 = z1 + b + j * 4;
                                float x4 = grid2DCache[0].Get(a + i * 4, b + j * 4);
                                float x5 = grid2DCache[1].Get(a + i * 4, b + j * 4);
                                int temperatureFast = chunk.GetTemperatureFast(par[0] + a + i * 4, par[1] + b + j * 4);
                                int humidityFast = chunk.GetHumidityFast(par[0] + a + i * 4, par[1] + b + j * 4);
                                float f = x5 - (float)q[7] * (float)humidityFast;
                                float f0 = MathUtils.Lerp(100f, 0f, f);
                                float f1 = MathUtils.Lerp(300f, 30f, f);
                                bool Desertificationflag = (temperatureFast > 8 && humidityFast < 8 && x5 < 0.97f) || (MathUtils.Abs(x4) < 16f && x5 < 0.97f);
                                //Desertificationflag = false;
                                for (int c = 0; c < 8; c++)
                                {
                                    byte blockID = 0;
                                    if (d13 < 0f)
                                    {
                                        if (c + k * 8 <= oceanLevel)
                                        {
                                            blockID = BlocksManager.getID(block => block is MagmaBlock);
                                            //blockID = 18;
                                        }
                                    }
                                    else
                                    {
                                        byte graniteBlockId = BlocksManager.getID(block => block is GraniteBlock);
                                        byte sandstoneBlockId = BlocksManager.getID(block => block is SandstoneBlock);
                                        byte basaltBlockId = BlocksManager.getID(block => block is GlassBlock);
                                        if (!Desertificationflag)
                                        {
                                            if (d13 >= f1)
                                            {
                                                blockID = basaltBlockId;
                                            }
                                            else
                                            {
                                                blockID = graniteBlockId;
                                            }
                                        }
                                        else
                                        {
                                            if (d13 >= f0)
                                            {
                                                if (d13 >= f1)
                                                {
                                                    blockID = basaltBlockId;
                                                }
                                                else
                                                {
                                                    blockID = graniteBlockId;
                                                }
                                            }
                                            else
                                            {
                                                blockID = sandstoneBlockId;
                                            }
                                        }
                                    }
                                    chunk.SetCellValueFast(par[0] + a + i * 4, c + k * 8, par[1] + b + j * 4, blockID);
                                    d13 += d12 * q[14];
                                }
                                d10 += d8 * q[8];
                                d11 += d9 * q[9];
                            }
                            d4 += d0 * q[10];
                            d5 += d1 * q[11];
                            d6 += d2 * q[12];
                            d7 += d3 * q[13];
                        }
                    }
                }
            }
        }
    }

}