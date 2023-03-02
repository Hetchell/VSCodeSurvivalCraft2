using Engine;
using Game;
using Survivalcraft.Game.NoiseModifier;
using noiseConst = Survivalcraft.Game.ModificationHolder.ModificationsHolder.NoiseConstants;

namespace ModificationHolder
{
    class Calculator
    {
        private readonly TerrainChunkGeneratorProviderActive activeChunkProvider;
        private readonly NoiseMain noiseMain;
        private readonly float[] holder;
        public Calculator(TerrainChunkGeneratorProviderActive activeChunkProvider, NoiseMain noiseMain, float[] holder){
            this.activeChunkProvider = activeChunkProvider;
            this.noiseMain = noiseMain;
            this.holder = holder;
        }
        
        public float CalculateOceanShoreDistance(float x0, float z0)
        {
            float f0 = 0;
            if (this.activeChunkProvider.m_islandSize != null)
            {
                float f1 = this.activeChunkProvider.m_oceanCorner.X + holder[0] * this.noiseMain.OctavedNoise(z0, 0f, 0.005f / holder[1], 4, 1.95f, 1f, false);
                float f2 = this.activeChunkProvider.m_oceanCorner.Y + holder[0] * this.noiseMain.OctavedNoise(0f, x0, 0.005f / holder[1], 4, 1.95f, 1f, false);
                float f3 = this.activeChunkProvider.m_oceanCorner.X + holder[0] * this.noiseMain.OctavedNoise(z0 + 1000f, 0f, 0.005f / holder[1], 4, 1.95f, 1f, false) + this.activeChunkProvider.m_islandSize.Value.X;
                float f4 = this.activeChunkProvider.m_oceanCorner.Y + holder[0] * this.noiseMain.OctavedNoise(0f, x0 + 1000f, 0.005f / holder[1], 4, 1.95f, 1f, false) + this.activeChunkProvider.m_islandSize.Value.Y;
                f0 += MathUtils.Min(x0 - f1, z0 - f2, f3 - x0, f4 - z0);
            }
            else
            {
                float f5 = this.activeChunkProvider.m_oceanCorner.X + holder[0] * this.noiseMain.OctavedNoise(z0, 0f, 0.005f / holder[1], 4, 1.95f, 1f, false);
                float f6 = this.activeChunkProvider.m_oceanCorner.Y + holder[0] * this.noiseMain.OctavedNoise(0f, x0, 0.005f / holder[1], 4, 1.95f, 1f, false);
                f0 += MathUtils.Min(x0 - f5, z0 - f6);
            }
            //f0 = 3.3f;
            return f0;
            //if (this.activeChunkProvider.m_islandSize != null)
            //{
            //    float num = this.activeChunkProvider.CalculateOceanShoreX(z0);
            //    float num2 = this.activeChunkProvider.CalculateOceanShoreZ(x0);
            //    float num3 = this.activeChunkProvider.CalculateOceanShoreX(z0 + 1000f) + this.activeChunkProvider.m_islandSize.Value.X;
            //    float num4 = this.activeChunkProvider.CalculateOceanShoreZ(x0 + 1000f) + this.activeChunkProvider.m_islandSize.Value.Y;
            //    return MathUtils.Min(x0 - num, z0 - num2, num3 - x0, num4 - z0);
            //}
            //float num5 = this.activeChunkProvider.CalculateOceanShoreX(z0);
            //float num6 = this.activeChunkProvider.CalculateOceanShoreZ(x0);
            //return MathUtils.Min(x0 - num5, z0 - num6);
        }

        public float CalculateMountainRangeFactor(float x, float z)
        {
            float f = this.noiseMain.OctavedNoise(
                x + this.activeChunkProvider.m_mountainsOffset.X,
                z + this.activeChunkProvider.m_mountainsOffset.Y,
                noiseConst.constants[9] / this.activeChunkProvider.TGBiomeScaling + (float)Math.Pow(3, 35) * 0,
                3,
                1.91f,
                0.75f,
                true);
            //f = 54.5f;
            //Debug.WriteLine("CalculateMountainRangeFactor output : " + f);
            //f = 0.87777777f;
            return f;
            //return MathUtils.Clamp(f, 0.0f, 1.0f);
        }

        public float CalculateHeight(float a, float b)
        {
            float f = this.CalculateHeightCriticalMethodApparently(a, b, 1.0f, false);
            //f = this.activeChunkProvider.CalculateHeight(a, b);
            //f = 0.33334f;
            return f ;
        }

        public float CalculateHeightCriticalMethodApparently(float x, float z, float y, bool allowMethodOverrideInternal)
        {
            float f_0 = noiseConst.constants[10] + noiseConst.constants[11] * MathUtils.PowSign(2f * this.noiseMain.OctavedNoise(x + this.activeChunkProvider.m_mountainsOffset.X, z + this.activeChunkProvider.m_mountainsOffset.Y, 0.01f, 1, 2f, 0.5f, false) - 1f, 0.5f);
            float f_1 = allowMethodOverrideInternal ? this.CalculateOceanShoreDistance(x, z) : this.activeChunkProvider.CalculateOceanShoreDistance(x, z);
            float f_2 = MathUtils.Saturate(2f - 0.05f * MathUtils.Abs(f_1));
            float f_3 = MathUtils.Saturate(MathUtils.Sin(noiseConst.constants[12] * f_1));
            float f_4 = MathUtils.Saturate(MathUtils.Saturate((0f - f_0) * f_1) - 0.85f * f_3);
            float f_5 = MathUtils.Saturate(MathUtils.Saturate(0.05f * (0f - f_1 - 10f)) - f_3);
            float f_6 = allowMethodOverrideInternal ? this.CalculateMountainRangeFactor(x, z) : this.activeChunkProvider.CalculateMountainRangeFactor(x, z);
            float f_7 = (1f - f_2) * this.noiseMain.OctavedNoise(x, z, 0.001f / this.activeChunkProvider.TGBiomeScaling, 2, 2f, 0.5f, false);
            float f_8 = (1f - f_2) * this.noiseMain.OctavedNoise(x, z, 0.0017f / this.activeChunkProvider.TGBiomeScaling, 2, 4f, 0.7f, false);
            float f_9 = (1f - f_5) * (1f - f_2) * TerrainChunkGeneratorProviderActive.Squish(f_6, 1f - noiseConst.constants[13], 1f - noiseConst.constants[14]);
            float f_10 = (1f - f_5) * TerrainChunkGeneratorProviderActive.Squish(f_6, 1f - noiseConst.constants[14], 1f);
            float f_11 = 1f * this.noiseMain.OctavedNoise(x, z, noiseConst.constants[15], (int)noiseConst.constants[16], 1.93f, noiseConst.constants[17], false);
            float amplitudeStep = MathUtils.Lerp(0.75f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, 1.33f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, f_7);
            float f_12 = 1.5f * this.noiseMain.OctavedNoise(x, z, TerrainChunkGeneratorProviderActive.TGMountainsDetailFreq, TerrainChunkGeneratorProviderActive.TGMountainsDetailOctaves, 1.98f, amplitudeStep, false) - 0.5f;
            float f_13 = MathUtils.Lerp(60f, 30f, MathUtils.Saturate(1f * f_10 + 0.5f * f_9 + MathUtils.Saturate(1f - f_1 / 30f)));
            float f_14 = MathUtils.Lerp(-2f, -4f, MathUtils.Saturate(f_10 + 0.5f * f_9));
            float f_15 = MathUtils.Saturate(1.5f - f_13 * MathUtils.Abs(2f * this.noiseMain.OctavedNoise(x + this.activeChunkProvider.m_riversOffset.X, z + this.activeChunkProvider.m_riversOffset.Y, 0.001f, 4, 2f, 0.5f, false) - 1f));
            float f_16 = -50f * f_4 + noiseConst.constants[18];
            float f_17 = MathUtils.Lerp(0f, 8f, f_7);
            float f_18 = MathUtils.Lerp(0f, -6f, f_8);
            float f_19 = noiseConst.constants[19] * f_9 * f_11;
            float f_20 = noiseConst.constants[20] * f_10 * f_12;
            float f_21 = noiseConst.constants[21] * f_15;
            float f_22 = f_16 + f_17 + f_18 + f_20 + f_19;
            float f_23 = MathUtils.Min(MathUtils.Lerp(f_22, f_14, f_21), f_22) * y;
            return MathUtils.Clamp(64f + f_23, 10f, 251f);
        }

        //For reference only. Direct reference to analog of TerrainChunkGeneratorProviderActive.CalculateHeight in ModifiedHolder. 
        private float a(float x, float z)
        {
            //float num = this.activeChunkProvider.TGOceanSlope + this.activeChunkProvider.TGOceanSlopeVariation * MathUtils.PowSign(2f * SimplexNoise.OctavedNoise(x + this.activeChunkProvider.m_mountainsOffset.X, z + this.activeChunkProvider.m_mountainsOffset.Y, 0.01f, 1, 2f, 0.5f, false) - 1f, 0.5f);
            float f_0 = noiseConst.constants[10] + noiseConst.constants[11] * MathUtils.PowSign(2f * this.noiseMain.OctavedNoise(x + this.activeChunkProvider.m_mountainsOffset.X, z + this.activeChunkProvider.m_mountainsOffset.Y, 0.01f, 1, 2f, 0.5f, false) - 1f, 0.5f);
            //float num2 = this.CalculateOceanShoreDistance(x, z);
            float f_1 = this.CalculateOceanShoreDistance(x, z);
            //float num3 = MathUtils.Saturate(2f - 0.05f * MathUtils.Abs(num2));
            float f_2 = MathUtils.Saturate(2f - 0.05f * MathUtils.Abs(f_1));
            //float num4 = MathUtils.Saturate(MathUtils.Sin(this.activeChunkProvider.TGIslandsFrequency * num2));
            float f_3 = MathUtils.Saturate(MathUtils.Sin(noiseConst.constants[12] * f_1));
            //float num5 = MathUtils.Saturate(MathUtils.Saturate((0f - num) * num2) - 0.85f * num4);
            float f_4 = MathUtils.Saturate(MathUtils.Saturate((0f - f_0) * f_1) - 0.85f * f_3);
            //float num6 = MathUtils.Saturate(MathUtils.Saturate(0.05f * (0f - num2 - 10f)) - num4);
            float f_5 = MathUtils.Saturate(MathUtils.Saturate(0.05f * (0f - f_1 - 10f)) - f_3);
            //float v = this.CalculateMountainRangeFactor(x, z);
            float f_6 = this.CalculateMountainRangeFactor(x, z);
            //float f = (1f - num3) * SimplexNoise.OctavedNoise(x, z, 0.001f / this.activeChunkProvider.TGBiomeScaling, 2, 2f, 0.5f, false);
            float f_7 = (1f - f_2) * this.noiseMain.OctavedNoise(x, z, 0.001f / this.activeChunkProvider.TGBiomeScaling, 2, 2f, 0.5f, false);
            //float f2 = (1f - num3) * SimplexNoise.OctavedNoise(x, z, 0.0017f / this.activeChunkProvider.TGBiomeScaling, 2, 4f, 0.7f, false);
            float f_8 = (1f - f_2) * this.noiseMain.OctavedNoise(x, z, 0.0017f / this.activeChunkProvider.TGBiomeScaling, 2, 4f, 0.7f, false);
            //float num7 = (1f - num6) * (1f - num3) * TerrainChunkGeneratorProviderActive.Squish(v, 1f - this.activeChunkProvider.TGHillsPercentage, 1f - this.activeChunkProvider.TGMountainsPercentage);
            float f_9 = (1f - f_5) * (1f - f_2) * TerrainChunkGeneratorProviderActive.Squish(f_6, 1f - noiseConst.constants[13], 1f - noiseConst.constants[14]);
            //float num8 = (1f - num6) * TerrainChunkGeneratorProviderActive.Squish(v, 1f - this.activeChunkProvider.TGMountainsPercentage, 1f);
            float f_10 = (1f - f_5) * TerrainChunkGeneratorProviderActive.Squish(f_6, 1f - noiseConst.constants[14], 1f);
            //float num9 = 1f * SimplexNoise.OctavedNoise(x, z, this.activeChunkProvider.TGHillsFrequency, this.activeChunkProvider.TGHillsOctaves, 1.93f, this.activeChunkProvider.TGHillsPersistence, false);
            float f_11 = 1f * this.noiseMain.OctavedNoise(x, z, noiseConst.constants[15], (int)noiseConst.constants[16], 1.93f, noiseConst.constants[17], false);
            //float amplitudeStep = MathUtils.Lerp(0.75f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, 1.33f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, f);
            float amplitudeStep = MathUtils.Lerp(0.75f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, 1.33f * TerrainChunkGeneratorProviderActive.TGMountainsDetailPersistence, f_7);
            //float num10 = 1.5f * SimplexNoise.OctavedNoise(x, z, TerrainChunkGeneratorProviderActive.TGMountainsDetailFreq, TerrainChunkGeneratorProviderActive.TGMountainsDetailOctaves, 1.98f, amplitudeStep, false) - 0.5f;
            float f_12 = 1.5f * this.noiseMain.OctavedNoise(x, z, TerrainChunkGeneratorProviderActive.TGMountainsDetailFreq, TerrainChunkGeneratorProviderActive.TGMountainsDetailOctaves, 1.98f, amplitudeStep, false) - 0.5f;
            //float num11 = MathUtils.Lerp(60f, 30f, MathUtils.Saturate(1f * num8 + 0.5f * num7 + MathUtils.Saturate(1f - num2 / 30f)));
            float f_13 = MathUtils.Lerp(60f, 30f, MathUtils.Saturate(1f * f_10 + 0.5f * f_9 + MathUtils.Saturate(1f - f_1 / 30f)));
            //float x2 = MathUtils.Lerp(-2f, -4f, MathUtils.Saturate(num8 + 0.5f * num7));
            float f_14 = MathUtils.Lerp(-2f, -4f, MathUtils.Saturate(f_10 + 0.5f * f_9));
            //float num12 = MathUtils.Saturate(1.5f - num11 * MathUtils.Abs(2f * SimplexNoise.OctavedNoise(x + this.activeChunkProvider.m_riversOffset.X, z + this.activeChunkProvider.m_riversOffset.Y, 0.001f, 4, 2f, 0.5f, false) - 1f));
            float f_15 = MathUtils.Saturate(1.5f - f_13 * MathUtils.Abs(2f * this.noiseMain.OctavedNoise(x + this.activeChunkProvider.m_riversOffset.X, z + this.activeChunkProvider.m_riversOffset.Y, 0.001f, 4, 2f, 0.5f, false) - 1f));
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
            return MathUtils.Clamp(64f + f_23, 10f, 251f);

        }

    }
}