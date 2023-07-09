using Survivalcraft.Game.ModificationHolder;
using Game;
using Survivalcraft.Game.NoiseModifier;
using Random = Game.Random;
using Engine;
using mod = Survivalcraft.Game.ModifierHolder; //old class
using noiseConst = Survivalcraft.Game.ModificationHolder.ModificationsHolder.NoiseConstants;

namespace ModificationHolder
{

    class ModifierHolderTerrainGen
    {
        private readonly bool useConstantp = TerrainGenerator.TerrainProperties.allowConstantPermutationArray;
        //instances
        private readonly SubsystemTerrain terrainInput;
        public readonly WorldSettings worldSettings;
        private readonly TerrainChunkGeneratorProviderActive activeChunkProvider;
        private readonly BlockPlacement placer;
        private readonly Calculator calc;
        private readonly float[] holder; 
        private double[] r = new double[1100];
        private double[] ar = new double[1100];
        private double[] br = new double[1100];
        private NoiseMain noiseMain;
        private NoiseMain noiseMin;
        private NoiseMain noiseMax;
        // private int x_prev;
        // private int y_prev;
        private Random rand;
        private readonly double[] s = new double[]
        {
            -101.1819239019000,
            -19.29182010183082,
            1.18192929109293,
            188.18182300,
            55.38192111,
            64.18192929192,
            69.19109102001,
            87.18181929281,
            -0.0007726161839119
            -0.0181832727173,
            15.1828332111111,
            1.1918918297129712,
            -133.131937918291212,
            -187.281281921093813,
            -54.19138128182192,
            31.39128913719814,
            16.1929139813081093,
            17.19029201933,
            8.0009999999
        };

        public ModifierHolderTerrainGen(SubsystemTerrain terrainInput, TerrainChunkGeneratorProviderActive activeChunkProvider, float num, Random rand)
        {
            this.terrainInput = terrainInput;
            this.activeChunkProvider = activeChunkProvider;
            this.worldSettings = activeChunkProvider.m_worldSettings;
            this.holder = new float[]
            {
                MathUtils.Clamp(2f * num, 0f, 150f) * noiseConst.constants[7], //TGShoreFluctuations7
                MathUtils.Clamp(0.04f * num, 0.5f, 3f) * noiseConst.constants[8], //TGShoreFluctuationsScaling8
            };
            this.rand = rand;
            this.noiseMain = new NoiseMain(rand, (int)noiseConst.constants[4], useConstantp);
            this.noiseMin = new NoiseMain(rand, 8);
            this.noiseMax = new NoiseMain(rand, 8);
            this.placer = new BlockPlacement(this, activeChunkProvider);
            this.calc = new Calculator(activeChunkProvider, this.noiseMain, this.holder);
            InitNoiseArray(this);
        }

        public Calculator getCalculator() {
            return this.calc;
        }

        private static void InitNoiseArray(ModifierHolderTerrainGen obj) {
            for(int i = 0; i < obj.r.Length; i++) {
                obj.r[i] = 0.0D;
            }
            for(int i = 0; i < obj.ar.Length; i++) {
                obj.ar[i] = 0.0D;
            }
            for(int i = 0; i < obj.br.Length; i++) {
                obj.br[i] = 0.0D;
            }
        }

        public void GenerateTerrain(TerrainChunk chunkIn, bool type)
        {
            if (type)
            {
                this.GenerateTerrainMod(chunkIn, 0, 0, 16, 8);//0, 0, 16, 8
                //this.GenerateTerrain(chunk, chunk.Origin.X, chunk.Origin.Y, 3, 3);
                //this.GenerateTerrain(chunk, 14, 27, 16, 5);
                //this.GenerateTerrainOr(chunkIn, 0, 0, 16, 8);
            }
            else
            {
                this.GenerateTerrainMod(chunkIn, 0, 8, 16, 16);//0, 8, 16, 16
                //this.GenerateTerrainOr(chunkIn, 0, 8, 16, 16);
            }
        }

        //original
        public void GenerateTerrainOr(TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            int num = x2 - x1;
            int num2 = z2 - z1;
            Terrain terrain = this.terrainInput.Terrain;
            int num3 = chunk.Origin.X + x1;
            int num4 = chunk.Origin.Y + z1;
            TerrainChunkGeneratorProviderActive.Grid2d grid2d = new TerrainChunkGeneratorProviderActive.Grid2d(num, num2);
            TerrainChunkGeneratorProviderActive.Grid2d grid2d2 = new TerrainChunkGeneratorProviderActive.Grid2d(num, num2);
            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    grid2d.Set(j, i, this.activeChunkProvider.CalculateOceanShoreDistance((float)(j + num3), (float)(i + num4)));
                    grid2d2.Set(j, i, this.activeChunkProvider.CalculateMountainRangeFactor((float)(j + num3), (float)(i + num4)));
                }
            }
            TerrainChunkGeneratorProviderActive.Grid3d grid3d = new TerrainChunkGeneratorProviderActive.Grid3d(num / 4 + 1, 33, num2 / 4 + 1);
            for (int k = 0; k < grid3d.SizeX; k++)
            {
                for (int l = 0; l < grid3d.SizeZ; l++)
                {
                    int num5 = k * 4 + num3;
                    int num6 = l * 4 + num4;
                    float num7 = this.activeChunkProvider.CalculateHeight((float)num5, (float)num6);
                    float v = this.activeChunkProvider.CalculateMountainRangeFactor((float)num5, (float)num6);
                    float num8 = MathUtils.Lerp(this.activeChunkProvider.TGMinTurbulence, 1f, TerrainChunkGeneratorProviderActive.Squish(v, this.activeChunkProvider.TGTurbulenceZero, 1f));
                    for (int m = 0; m < grid3d.SizeY; m++)
                    {
                        int num9 = m * 8;
                        float num10 = this.activeChunkProvider.TGTurbulenceStrength * num8 * MathUtils.Saturate(num7 - (float)num9) * (2f * SimplexNoise.OctavedNoise((float)num5, (float)num9, (float)num6, this.activeChunkProvider.TGTurbulenceFreq, this.activeChunkProvider.TGTurbulenceOctaves, 4f, this.activeChunkProvider.TGTurbulencePersistence, false) - 1f);
                        float num11 = (float)num9 + num10;
                        float num12 = num7 - num11;
                        num12 += MathUtils.Max(4f * (this.activeChunkProvider.TGDensityBias - (float)num9), 0f);
                        grid3d.Set(k, m, l, num12);
                    }
                }
            }
            int oceanLevel = this.activeChunkProvider.OceanLevel;
            for (int n = 0; n < grid3d.SizeX - 1; n++)
            {
                for (int num13 = 0; num13 < grid3d.SizeZ - 1; num13++)
                {
                    for (int num14 = 0; num14 < grid3d.SizeY - 1; num14++)
                    {
                        float num15;
                        float num16;
                        float num17;
                        float num18;
                        float num19;
                        float num20;
                        float num21;
                        float num22;
                        grid3d.Get8(n, num14, num13, out num15, out num16, out num17, out num18, out num19, out num20, out num21, out num22);
                        float num23 = (num16 - num15) / 4f; //(num16 - num15) / 4f
                        float num24 = (num18 - num17) / 4f; //(num18 - num17) / 4f
                        float num25 = (num20 - num19) / 4f; //(num20 - num19) / 4f
                        float num26 = (num22 - num21) / 4f; //(num22 - num21) / 4f
                        float num27 = num15;
                        float num28 = num17;
                        float num29 = num19;
                        float num30 = num21;
                        for (int num31 = 0; num31 < 4; num31++)
                        {
                            float num32 = (num29 - num27) / 4f;
                            float num33 = (num30 - num28) / 4f;
                            float num34 = num27;
                            float num35 = num28;
                            for (int num36 = 0; num36 < 4; num36++)
                            {
                                float num37 = (num35 - num34) / 8f;
                                float num38 = num34;
                                int num39 = num31 + n * 4;
                                int num40 = num36 + num13 * 4;
                                int x3 = x1 + num39;
                                int z3 = z1 + num40;
                                float x4 = grid2d.Get(num39, num40);
                                float num41 = grid2d2.Get(num39, num40);
                                int temperatureFast = chunk.GetTemperatureFast(x3, z3);
                                int humidityFast = chunk.GetHumidityFast(x3, z3);
                                float f = num41 - 0.01f * (float)humidityFast;
                                float num42 = MathUtils.Lerp(100f, 0f, f);
                                float num43 = MathUtils.Lerp(300f, 30f, f);
                                bool flag = (temperatureFast > 8 && humidityFast < 8 && num41 < 0.97f) || (MathUtils.Abs(x4) < 16f && num41 < 0.97f);
                                int num44 = TerrainChunk.CalculateCellIndex(x3, 0, z3);
                                for (int num45 = 0; num45 < 8; num45++)
                                {
                                    int num46 = num45 + num14 * 8;
                                    int value = 0;
                                    if (num38 < 0f)
                                    {
                                        if (num46 <= oceanLevel)
                                        {
                                            value = 18; //18
                                        }
                                    }
                                    else
                                    {
                                        value = ((!flag) ? ((num38 >= num43) ? 67 : 3) : ((num38 >= num42) ? ((num38 >= num43) ? 67 : 3) : 4));
                                    }
                                    chunk.SetCellValueFast(num44 + num46, value);
                                    num38 += num37;
                                }
                                num34 += num32;
                                num35 += num33;
                            }
                            num27 += num23;
                            num28 += num24;
                            num29 += num25;
                            num30 += num26;
                        }
                    }
                }
            }
        }

        private void GenerateTerrainMod(TerrainChunk chunk, int x1, int z1, int x2, int z2)
        {
            bool z_0 = TerrainGenerator.TerrainProperties.MethodControl.overrideOceanShoreDistance;
            bool z_1 = TerrainGenerator.TerrainProperties.MethodControl.overrideMountainRangeFactor;
            bool z_2 = TerrainGenerator.TerrainProperties.MethodControl.overrideCalculateHeight;
            int I = 0;
            float a0 = 684.412f;
            float a1 = a0;
            float a2 = a0 * 2.0f;
            int i0 = x2 - x1;
            int i1 = z2 - z1;
            Terrain terrain = this.terrainInput.Terrain;
            int k0 = chunk.Origin.X + x1;
            int k1 = chunk.Origin.Y + z1;
            Grid2d grid2d = new Grid2d(i0, i1);
            Grid2d grid2d2 = new Grid2d(i0, i1);
            /* Entire loop may be safe => or differences are extremely subtle.*/
            //not sure what this deals with. But incorrect x0 and z0 give different generation blocktype to original. 
            for (int i = 0; i < i1; i++)
            {
                for (int j = 0; j < i0; j++)
                {
                    float f0;
                    float x0 = (float)(j + k0);
                    float z0 = (float)(i + k1);
                    if (this.activeChunkProvider.m_islandSize != null)
                    {
                       float f1 = this.activeChunkProvider.m_oceanCorner.X + holder[0] * this.noiseMain.OctavedNoise(z0, 0f, 0.005f / holder[1], 4, 1.95f, 1f, false);
                       float f2 = this.activeChunkProvider.m_oceanCorner.Y + holder[0] * this.noiseMain.OctavedNoise(0f, x0, 0.005f / holder[1], 4, 1.95f, 1f, false);
                       float f3 = this.activeChunkProvider.m_oceanCorner.X + holder[0] * this.noiseMain.OctavedNoise(z0 + 1000f, 0f, 0.005f / holder[1], 4, 1.95f, 1f, false) + this.activeChunkProvider.m_islandSize.Value.X;
                       float f4 = this.activeChunkProvider.m_oceanCorner.Y + holder[0] * this.noiseMain.OctavedNoise(0f, x0 + 1000f, 0.005f / holder[1], 4, 1.95f, 1f, false) + this.activeChunkProvider.m_islandSize.Value.Y;
                       f0 = MathUtils.Min(x0 - f1, z0 - f2, f3 - x0, f4 - z0);
                    }
                    else
                    {
                       float f5 = this.activeChunkProvider.m_oceanCorner.X + holder[0] * this.noiseMain.OctavedNoise(z0, 0f, 0.005f / holder[1], 4, 1.95f, 1f, false);
                       float f6 = this.activeChunkProvider.m_oceanCorner.Y + holder[0] * this.noiseMain.OctavedNoise(0f, x0, 0.005f / holder[1], 4, 1.95f, 1f, false);
                       f0 = MathUtils.Min(x0 - f5, z0 - f6);
                    }
                    f0 = z_0 ? this.calc.CalculateOceanShoreDistance(x0, z0) : f0 * 1;
                    grid2d.Set(j, i, f0);
                    f0 = !z_1 ? this.noiseMain.OctavedNoise(
                       x0 + this.activeChunkProvider.m_mountainsOffset.X,
                       z0 + this.activeChunkProvider.m_mountainsOffset.Y,
                       noiseConst.constants[9] / this.activeChunkProvider.TGBiomeScaling,
                       3,
                       1.91f,
                       0.75f,
                       true
                    ) : this.calc.CalculateMountainRangeFactor(x0, z0);
                    grid2d2.Set(j, i, f0);
                    //what the above code is: 
                    //grid2d.Set(j, i, this.activeChunkProvider.CalculateOceanShoreDistance((float)(j + k0), (float)(i + k1)));
                    //grid2d2.Set(j, i, this.activeChunkProvider.CalculateMountainRangeFactor((float)(j + k0), (float)(i + k1)));
                }
            }
            Grid3d grid3d = new Grid3d(i0 / 4 + 1, 33, i1 / 4 + 1);
            this.r = this.noiseMain.UseImprovedNoiseGenerateNoiseOctaves(this.r, k0, 0, k1, i0 / 4 + 1, 33, i1 / 4 + 1, a0  * Math.Pow(2, 10), a1 , a2);
            this.ar = this.noiseMain.UseImprovedNoiseGenerateNoiseOctaves(this.ar, k0, 0, k1, i0 / 4 + 1, 33, i1 / 4 + 1, a0 * Math.Pow(2, 10), a1, a2);
            this.br = this.noiseMain.UseImprovedNoiseGenerateNoiseOctaves(this.br, k0, 0, k1, i0 / 4 + 1, 33, i1 / 4 + 1, a0 * Math.Pow(2, 10), a1, a2);
            /* entire loop is safe*/
            for (int k = 0; k < grid3d.SizeX; k++)
            {
                for (int l = 0; l < grid3d.SizeZ; l++)
                {
                    int x = k * 4 + k0;
                    int y = l * 4 + k1;
                    //float num = this.activeChunkProvider.TGOceanSlope + this.activeChunkProvider.TGOceanSlopeVariation * MathUtils.PowSign(2f * SimplexNoise.OctavedNoise(x + this.activeChunkProvider.m_mountainsOffset.X, z + this.activeChunkProvider.m_mountainsOffset.Y, 0.01f, 1, 2f, 0.5f, false) - 1f, 0.5f);
                    float f_0 = noiseConst.constants[10] + noiseConst.constants[11] * MathUtils.PowSign(2f * this.noiseMain.OctavedNoise(x + this.activeChunkProvider.m_mountainsOffset.X, y + this.activeChunkProvider.m_mountainsOffset.Y, 0.01f, 1, 2f, 0.5f, false) - 1f, 0.5f);
                    //float num2 = this.CalculateOceanShoreDistance(x, z);
                    float f_1 = this.calc.CalculateOceanShoreDistance(x, y);
                    //float num3 = MathUtils.Saturate(2f - 0.05f * MathUtils.Abs(num2));
                    float f_2 = MathUtils.Saturate(2f - 0.05f * MathUtils.Abs(f_1));
                    //float num4 = MathUtils.Saturate(MathUtils.Sin(this.activeChunkProvider.TGIslandsFrequency * num2));
                    float f_3 = MathUtils.Saturate(MathUtils.Sin(noiseConst.constants[12] * f_1));
                    //float num5 = MathUtils.Saturate(MathUtils.Saturate((0f - num) * num2) - 0.85f * num4);
                    float f_4 = MathUtils.Saturate(MathUtils.Saturate((0f - f_0) * f_1) - 0.85f * f_3);
                    //float num6 = MathUtils.Saturate(MathUtils.Saturate(0.05f * (0f - num2 - 10f)) - num4);
                    float f_5 = MathUtils.Saturate(MathUtils.Saturate(0.05f * (0f - f_1 - 10f)) - f_3);
                    //float v = this.CalculateMountainRangeFactor(x, z);
                    float f_6 = this.calc.CalculateMountainRangeFactor(x, y);
                    //float f = (1f - num3) * SimplexNoise.OctavedNoise(x, z, 0.001f / this.activeChunkProvider.TGBiomeScaling, 2, 2f, 0.5f, false);
                    float f_7 = (1f - f_2) * this.noiseMain.OctavedNoise(x, y, 0.001f / this.activeChunkProvider.TGBiomeScaling, 2, 2f, 0.5f, false);
                    //float f2 = (1f - num3) * SimplexNoise.OctavedNoise(x, z, 0.0017f / this.activeChunkProvider.TGBiomeScaling, 2, 4f, 0.7f, false);
                    float f_8 = (1f - f_2) * this.noiseMain.OctavedNoise(x, y, 0.0017f / this.activeChunkProvider.TGBiomeScaling, 2, 4f, 0.7f, false);
                    //float num7 = (1f - num6) * (1f - num3) * TerrainChunkGeneratorProviderActive.Squish(v, 1f - this.activeChunkProvider.TGHillsPercentage, 1f - this.activeChunkProvider.TGMountainsPercentage);
                    float f_9 = (1f - f_5) * (1f - f_2) * TerrainChunkGeneratorProviderActive.Squish(f_6, 1f - noiseConst.constants[13], 1f - noiseConst.constants[14]);
                    //float num8 = (1f - num6) * TerrainChunkGeneratorProviderActive.Squish(v, 1f - this.activeChunkProvider.TGMountainsPercentage, 1f);
                    float f_10 = (1f - f_5) * TerrainChunkGeneratorProviderActive.Squish(f_6, 1f - noiseConst.constants[14], 1f);
                    //float num9 = 1f * SimplexNoise.OctavedNoise(x, z, this.activeChunkProvider.TGHillsFrequency, this.activeChunkProvider.TGHillsOctaves, 1.93f, this.activeChunkProvider.TGHillsPersistence, false);
                    float f_11 = 1f * this.noiseMain.OctavedNoise(x, y, noiseConst.constants[15], (int)noiseConst.constants[16], 1.93f, noiseConst.constants[17], false);
                    //float amplitudeStep = MathUtils.Lerp(0.75f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, 1.33f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, f);
                    float amplitudeStep = MathUtils.Lerp(0.75f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, 1.33f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, f_7);
                    //float num10 = 1.5f * SimplexNoise.OctavedNoise(x, z, TerrainChunkGeneratorProviderActive.TGMountainsDetailFreq, TerrainChunkGeneratorProviderActive.TGMountainsDetailOctaves, 1.98f, amplitudeStep, false) - 0.5f;
                    float f_12 = 1.5f * this.noiseMain.OctavedNoise(x, y, TerrainChunkGeneratorProviderActive.TGMountainsDetailFreq, TerrainChunkGeneratorProviderActive.TGMountainsDetailOctaves, 1.98f, amplitudeStep, false) - 0.5f;
                    //float num11 = MathUtils.Lerp(60f, 30f, MathUtils.Saturate(1f * num8 + 0.5f * num7 + MathUtils.Saturate(1f - num2 / 30f)));
                    float f_13 = MathUtils.Lerp(60f, 30f, MathUtils.Saturate(1f * f_10 + 0.5f * f_9 + MathUtils.Saturate(1f - f_1 / 30f)));
                    //float x2 = MathUtils.Lerp(-2f, -4f, MathUtils.Saturate(num8 + 0.5f * num7));
                    float f_14 = MathUtils.Lerp(-2f, -4f, MathUtils.Saturate(f_10 + 0.5f * f_9));
                    //float num12 = MathUtils.Saturate(1.5f - num11 * MathUtils.Abs(2f * SimplexNoise.OctavedNoise(x + this.activeChunkProvider.m_riversOffset.X, z + this.activeChunkProvider.m_riversOffset.Y, 0.001f, 4, 2f, 0.5f, false) - 1f));
                    float f_15 = MathUtils.Saturate(1.5f - f_13 * MathUtils.Abs(2f * this.noiseMain.OctavedNoise(x + this.activeChunkProvider.m_riversOffset.X, y + this.activeChunkProvider.m_riversOffset.Y, 0.001f, 4, 2f, 0.5f, false) - 1f));
                    //float num13 = -50f * num5 + this.activeChunkProvider.TGHeightBias;
                    float f_16 = -50f * f_4 + noiseConst.constants[18];
                    //float num14 = MathUtils.Lerp(0f, 8f, f);
                    float f_17 = MathUtils.Lerp(0f, 8f, f_7);
                    //float num15 = MathUtils.Lerp(0f, -6f, f2);
                    float f_18 = MathUtils.Lerp(0f, -6f, f_8);
                    //float num16 = this.activeChunkProvider.TGHillsStrength * num7 * num9;
                    float f_19 = noiseConst.constants[19] * f_9 * f_11;
                    //float num17 = this.activeChunkProvider.TGMountainsStrength * num8 * num10;
                    float f_20 = noiseConst.constants[20] * f_10 * f_12;
                    //float f3 = this.activeChunkProvider.TGRiversStrength * num12;
                    float f_21 = noiseConst.constants[21] * f_15;
                    //float num18 = num13 + num14 + num15 + num17 + num16;
                    float f_22 = f_16 + f_17 + f_18 + f_20 + f_19;
                    //float num19 = MathUtils.Min(MathUtils.Lerp(num18, x2, f3), num18);
                    float f_23 = MathUtils.Min(MathUtils.Lerp(f_22, f_14, f_21), f_22);
                    //float f_23 = num19;
                    float v;
                    float f0 = z_2 ? this.calc.CalculateHeight(x, y) : MathUtils.Clamp(64f + f_23, 10f, 251f);
                    //num7 = this.activeChunkProvider.CalculateHeight(x, y);
                    v = this.calc.CalculateMountainRangeFactor((float)x, (float)y);
                    float f1 = MathUtils.Lerp(noiseConst.constants[0], 1f, Squish(v, noiseConst.constants[1], 1f));
                    for (int m = 0; m < grid3d.SizeY; m++)
                    {
                        double d0 = this.ar[I] / 512.0D;
                        double d1 = this.br[I] / 512.0D;
                        double d2 = (this.r[I] / 10.0D + 1.0D) / 2.0D;
                        double d3;
                        if (d2 < 0.0D)
                        {
                            d3 = d0;
                        }
                        else
                        {
                            d3 = d2 > 1.0D ? d1 : d0 + (d1 - d0) * d2;
                        }
                        int num9 = m * 8;
                        float num10 = noiseConst.constants[2] * f1 * MathUtils.Saturate(
                            f0 - (float)num9
                            ) * 
                            (2f * this.noiseMain.OctavedNoise(
                                (float)x, 
                                (float)num9, 
                                (float)y, 
                                noiseConst.constants[3], 
                                (int)noiseConst.constants[4], 
                                4f, 
                                noiseConst.constants[5]
                                ) - 1f
                                );
                        float num11 = (float)(num9 + num10 + 1 * d3)
                            ;
                        float num12 = f0 - num11 + 18 * 0;
                        num12 += MathUtils.Max(4f * (noiseConst.constants[6] - (float)num9), 0f);
                        //Console.WriteLine("Value of num12 is " + num12);
                        grid3d.Set(k, m , l, num12);
                    }
                }
            }
            this.placer.SetBlock(this.worldSettings, new Grid2d[] { grid2d, grid2d2 }, grid3d, chunk, x1, z1, x2, z2);
        }

 
        
        public class Grid2d
        {

            public int m_sizeX;

            public int m_sizeY;

            public float[] m_data;

            public int SizeX
            {
                get
                {
                    return this.m_sizeX;
                }
            }

            
            public int SizeY
            {
                get
                {
                    return this.m_sizeY;
                }
            }

           
            public Grid2d(int sizeX, int sizeY)
            {
                sizeY = Math.Abs(sizeY);
                sizeX = Math.Abs(sizeX);
                this.m_sizeX = sizeX;
                this.m_sizeY = sizeY;
                this.m_data = new float[this.m_sizeX * this.m_sizeY];
            }

            public float Get(int x, int y)
            {
                return this.m_data[(x + y * this.m_sizeX)];
            }

            public void Set(int x, int y, float value)
            {
                this.m_data[x + y * this.m_sizeX] = value;
            }

            public float Sample(float x, float y)
            {
                int i = (int)MathUtils.Floor(x);
                int j = (int)MathUtils.Floor(y);
                int k = (int)MathUtils.Ceiling(x);
                int l = (int)MathUtils.Ceiling(y);
                float f = x - (float)i;
                float f2 = y - (float)j;
                float x2 = this.m_data[i + j * this.m_sizeX];
                float x3 = this.m_data[i + j * this.m_sizeX];
                float x4 = this.m_data[i + l * this.m_sizeX];
                float x5 = this.m_data[i + l * this.m_sizeX];
                float x6 = MathUtils.Lerp(x2, x3, f);
                float x7 = MathUtils.Lerp(x4, x5, f);
                return MathUtils.Lerp(x6, x7, f2);
            }

        }

        public class Grid3d
        {

            public int m_sizeX;

            public int m_sizeY;

            public int m_sizeZ;

            public int m_sizeXY;

            public double[] m_data;
            public int SizeX
            {
                get
                {
                    return this.m_sizeX;
                }
            }

            public int SizeY
            {
                get
                {
                    return this.m_sizeY;
                }
            }

            public int SizeZ
            {
                get
                {
                    return this.m_sizeZ;
                }
            }

            public Grid3d(int sizeX, int sizeY, int sizeZ)
            {
                sizeX = Math.Abs(sizeX);
                sizeY = Math.Abs(sizeY);
                sizeZ = Math.Abs(sizeZ);
                this.m_sizeX = sizeX;
                this.m_sizeY = sizeY;
                this.m_sizeZ = sizeZ;
                this.m_sizeXY = this.m_sizeX * this.m_sizeY;
                this.m_data = new double[this.m_sizeX * this.m_sizeY * this.m_sizeZ];
            }

            public void Get8(int x, int y, int z, out double v111, out double v211, out double v121, out double v221, out double v112, out double v212, out double v122, out double v222)
            {
                int i = x + y * this.m_sizeX + z * this.m_sizeXY;
                v111 = this.m_data[i];
                v211 = this.m_data[i + 1];
                v121 = this.m_data[i + this.m_sizeX];
                v221 = this.m_data[i + 1 + this.m_sizeX];
                v112 = this.m_data[i + this.m_sizeXY];
                v212 = this.m_data[i + 1 + this.m_sizeXY];
                v122 = this.m_data[i + this.m_sizeX + this.m_sizeXY];
                v222 = this.m_data[i + 1 + this.m_sizeX + this.m_sizeXY];
            }

            public double Get(int x, int y, int z)
            {
                return this.m_data[x + y * this.m_sizeX + z * this.m_sizeXY];
            }

            public double Get(int I)
            {
                I %= this.m_data.Length;
                return this.m_data[I];
            }

            public void Set(int index, double value)
            {
                this.m_data[index] = value;
            }
            public void Set(int x, int y, int z, double value)
            {
                this.m_data[x + y * this.m_sizeX + z * this.m_sizeXY] = value;
            }

            private double Sample(float x, float y, float z)
            {
                int num = (int)MathUtils.Floor(x);
                int num2 = (int)MathUtils.Ceiling(x);
                int num3 = (int)MathUtils.Floor(y);
                int num4 = (int)MathUtils.Ceiling(y);
                int num5 = (int)MathUtils.Floor(z);
                int num6 = (int)MathUtils.Ceiling(z);
                float f = x - (float)num;
                float f2 = y - (float)num3;
                float f3 = z - (float)num5;
                double x2 = this.m_data[num + num3 * this.m_sizeX + num5 * this.m_sizeX * this.m_sizeY];
                double x3 = this.m_data[num2 + num3 * this.m_sizeX + num5 * this.m_sizeX * this.m_sizeY];
                double x4 = this.m_data[num + num4 * this.m_sizeX + num5 * this.m_sizeX * this.m_sizeY];
                double x5 = this.m_data[num2 + num4 * this.m_sizeX + num5 * this.m_sizeX * this.m_sizeY];
                double x6 = this.m_data[num + num3 * this.m_sizeX + num6 * this.m_sizeX * this.m_sizeY];
                double x7 = this.m_data[num2 + num3 * this.m_sizeX + num6 * this.m_sizeX * this.m_sizeY];
                double x8 = this.m_data[num + num4 * this.m_sizeX + num6 * this.m_sizeX * this.m_sizeY];
                double x9 = this.m_data[num2 + num4 * this.m_sizeX + num6 * this.m_sizeX * this.m_sizeY];
                double x10 = MathUtils.Lerp(x2, x3, f);
                double x11 = MathUtils.Lerp(x4, x5, f);
                double x12 = MathUtils.Lerp(x6, x7, f);
                double x13 = MathUtils.Lerp(x8, x9, f);
                double x14 = MathUtils.Lerp(x10, x11, f2);
                double x15 = MathUtils.Lerp(x12, x13, f2);
                return MathUtils.Lerp(x14, x15, f3);
            }

        }

        public static float Squish(float v, float r, float t)
        {
            float x = (v - r) / (t - r);
            if (!(x < 0f))
            {
                if (!(x > 1f))
                {
                    return x;
                }

                return 1f;
            }

            return 0f;
        }
    }
}
