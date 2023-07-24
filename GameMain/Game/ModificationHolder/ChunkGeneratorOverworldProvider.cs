using Engine;
using ExperimentalTerrain;
using Game;

namespace ModificationHolder {

    public class ChunkGeneratorOverworldProvider : TerrainChunkGeneratorProviderActive
    {
        private static readonly bool USE_SUPER_SPAWN = false;
        private static readonly bool PASS_1_HIJACK_EN = true;
        private static readonly bool PASS_2_HIJACK_EN = false;
        private ModifierHolderTerrainGen modifyTerrain;
        private ExperimentalTerrainGenerator expgen;
        public ChunkGeneratorOverworldProvider(SubsystemTerrain subsystemTerrain) : base(subsystemTerrain)
        {
            Game.Random random = new Game.Random(this.m_seed);
            float num = (this.m_islandSize != null) ? MathUtils.Min(this.m_islandSize.Value.X, this.m_islandSize.Value.Y) : float.MaxValue;
            this.modifyTerrain = new ModifierHolderTerrainGen(subsystemTerrain, this, num, random);
			this.expgen = new ExperimentalTerrainGenerator(OceanLevel, random);
        }

        public override Vector3 FindCoarseSpawnPosition() {
            if (USE_SUPER_SPAWN) return base.FindCoarseSpawnPosition();
            return new(0, 300, 0);
        }

        public override void GenerateChunkContentsPass1(TerrainChunk chunk, bool non_air)
        {
            this.GenerateSurfaceParameters(chunk, 0, 0, 16, 8);
            if (!non_air) {
                if (ExperimentalTerrainGenerator.inExperimentStage) {
                    chunk.provider_state_pass1 = WorldProviderState.HIJACK_SELF_TERRAIN;
				    this.expgen.PopulateWithBlocks(chunk);
			    }
                if (PASS_1_HIJACK_EN) {
                    chunk.provider_state_pass1 = WorldProviderState.HIJACK_MAIN_SEQUENCE;
                    this.modifyTerrain.GenerateTerrain(chunk, true);
                } else {
                    chunk.provider_state_pass1 = WorldProviderState.MAIN_SEQUENCE;
                    base.GenerateChunkContentsPass1(chunk, non_air);
                }
                return;
            }
            chunk.provider_state_pass1 = WorldProviderState.AIR_FILLED;
        }

        public override void GenerateChunkContentsPass2(TerrainChunk chunk, bool non_air)
        {
            this.GenerateSurfaceParameters(chunk, 0, 8, 16, 16);
            if (!non_air) {
                if (ExperimentalTerrainGenerator.inExperimentStage) {
                    chunk.provider_state_pass2 = WorldProviderState.HIJACK_SELF_TERRAIN;
				    return;
			    }
                if (PASS_2_HIJACK_EN) {
                    chunk.provider_state_pass2 = WorldProviderState.HIJACK_SECONDARY_SEQ;
                    this.modifyTerrain.GenerateTerrain(chunk, false);   
                } else {
                    chunk.provider_state_pass2 = WorldProviderState.SECONDARY_SEQ;
                    base.GenerateChunkContentsPass2(chunk, non_air);
                }
                return;
            }
            chunk.provider_state_pass2 = WorldProviderState.AIR_FILLED;
        }

        public override string GetType(DebuggerHelper debugger) {
			return "ChunkGeneratorOverworldProvider";
		}

        public enum WorldProviderState {
            AIR_FILLED,
            HIJACK_MAIN_SEQUENCE,
            HIJACK_SECONDARY_SEQ,
            HIJACK_SELF_TERRAIN,
            MAIN_SEQUENCE,
            SECONDARY_SEQ
        }
    }

}