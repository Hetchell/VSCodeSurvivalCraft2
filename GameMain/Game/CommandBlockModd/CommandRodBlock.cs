/*
MIT License

Copyright (c) 2017 Lixue_jiu

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
