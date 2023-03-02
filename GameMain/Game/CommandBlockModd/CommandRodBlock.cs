using Engine;
using Engine.Graphics;
using Color = Engine.Color;

namespace Game
{
  public class CommandRodBlock : Block
  {
    public const int Index = 334;
    public BlockMesh m_standaloneBlockMesh = new BlockMesh();
    public Texture2D texture;

    public override void Initialize()
    {
      Model model = ContentManager.Get<Model>("Models/Stick");
      this.texture = ContentManager.Get<Texture2D>("Textures/Lightstick");
      Matrix absoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh("Stick").ParentBone);
      this.m_standaloneBlockMesh.AppendModelMeshPart(model.FindMesh("Stick").MeshParts[0], absoluteTransform * Matrix.CreateTranslation(0.0f, -0.5f, 0.0f), false, false, true, false, Color.White);
      base.Initialize();
    }

    public override void GenerateTerrainVertices(
      BlockGeometryGenerator generator,
      TerrainGeometry geometry,
      int value,
      int x,
      int y,
      int z)
    {
    }

    public override void DrawBlock(
      PrimitivesRenderer3D primitivesRenderer,
      int value,
      Color color,
      float size,
      ref Matrix matrix,
      DrawBlockEnvironmentData environmentData)
    {
      BlocksManager.DrawMeshBlock(primitivesRenderer, this.m_standaloneBlockMesh, this.texture, color, 2f * size, ref matrix, environmentData);
    }
  }
}
