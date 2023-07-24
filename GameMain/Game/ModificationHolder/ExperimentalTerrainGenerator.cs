using Game;
using Random = Game.Random;

namespace ExperimentalTerrain {

    public class ExperimentalTerrainGenerator {


        public const bool inExperimentStage = false;
        private static int AIR = BlocksManager.getID(block => block is AirBlock);
        private static int WATER = BlocksManager.getID(block => block is WaterBlock);
        private static int STONE = BlocksManager.getID(block => block is GraniteBlock);
        private static int MARKER = BlocksManager.getID(block => block is BedrockBlock);
        private int seaLevel;
        private Random rand;

        public ExperimentalTerrainGenerator(int seaLevel, Random rand) {
            this.seaLevel = seaLevel;
            this.rand = rand;
        }

        public void PopulateWithBlocks(TerrainChunk chunkIn) {
            if (GatedGenerationSquare(chunkIn, 15)) {
                chunkIn.isEmpty = false;
                for (int x = 0; x < 16; x++) {
                    for (int z = 0; z < 16; z++) {
                        int k = TerrainChunk.CalculateCellIndex(x, 0, z);
                        for (int y = 0; y < 255; y++) {
                            int value = this.getBlockAt(x + chunkIn.Origin.X, y, z + chunkIn.Origin.Y);
                            chunkIn.SetCellValueFast(k + y, value);
                        }
                    }
                }                
            } else {
                chunkIn.isEmpty = true;
                chunkIn.FillAir();
            }
        }

        private int getBlockAt(int x, int y, int z) {
            double yt = y;
            double level = this.GetThreshold(x, z, y);
            int blockId;
            if (yt > level) {
                blockId = AIR;
            } else {
                blockId = STONE;
                if (y < this.seaLevel) {
                    blockId = WATER;
                }
            }
            if (x == 0 && z == 0) {
                blockId = MARKER;
            }
            return blockId;
        }

        private double GetThreshold(int x, int z, int y) {
            double d0 = Math.Sin(x / 20) * 20;
            double d1 = Math.Sin(z / 20) * 20;
            double d2 = x + z + d0 + d1;
            //return Math.Sin(d2 / 20) * 20 + 100;
            return Calculate3DAt((double)x / 20, (double)z / 20, y / 32) * 20 + 100;
        }

        private static double Calculate2DAt(double x, double z) {
            return Math.Sin(x + Math.Sin(z) + z + Math.Sin(x));
        }

        private static double Calculate3DAt(double x, double z, double y) {
            double d0 = Math.Abs(x);
            return Math.Sin(Math.Pow(d0, 0.5556d) + Math.Sin(z));
        }

        public static bool GatedGeneration(TerrainChunk chunkIn, int sizeX, int sizeZ, int lowX, int lowZ) {
            int chunkX = chunkIn.Coords.X;
            int chunkZ = chunkIn.Coords.Y;
            bool z0 = false;
            bool z1 = false;
            for (int x = lowX; x < sizeX + lowX; x++) {
                z0 |= chunkX == x;
            }
            for (int z = lowZ; z < sizeZ + lowZ; z++) {
                z1 |= chunkZ == z;
            }
            return z0 && z1;
        }

        public static bool GatedGenerationSquare(TerrainChunk chunkIn, int halfside) {
            return GatedGeneration(chunkIn, halfside * 2, halfside * 2, -halfside, -halfside);
        }

    }

}