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
using GameEntitySystem;
using System.Collections.Generic;
using TemplatesDatabase;

namespace Game
{
  public class SubsystemLogo : Subsystem, IDrawable
  {
    private SubsystemSky m_subsystemSky;
    private Dictionary<Point3, LogoPoint[]> m_logoPoints = new Dictionary<Point3, LogoPoint[]>();
    public Dictionary<Point3, PatternPoint> m_patternPoints = new Dictionary<Point3, PatternPoint>();
    public Dictionary<Point3, Texture2D> m_patternTextures = new Dictionary<Point3, Texture2D>();
    private PrimitivesRenderer3D m_primitivesRenderer = new PrimitivesRenderer3D();
    private TexturedBatch3D[] m_batchesByType = new TexturedBatch3D[4];
    private TexturedBatch3D[] m_batchesByType2 = new TexturedBatch3D[4];
    private TexturedBatch3D[] m_batchesByType3 = new TexturedBatch3D[1];
    private static int[] m_drawOrders = new int[1]{ 110 };

    public int[] DrawOrders => SubsystemLogo.m_drawOrders;

    public void AddLogoPoint(Point3 commandpoint3, LogoPoint[] logoPoint) => this.m_logoPoints.Add(commandpoint3, logoPoint);

    public void RemoveLogoPoint(Point3 commandpoint3) => this.m_logoPoints.Remove(commandpoint3);

    public void AddPatternPoint(Point3 patternpoint3, PatternPoint patternPoint) => this.m_patternPoints.Add(patternpoint3, patternPoint);

    public void RemovePatternPoint(Point3 patternpoint3) => this.m_patternPoints.Remove(patternpoint3);

    public void AddPatternTexture(Point3 patternpoint3, Texture2D patterntexture) => this.m_patternTextures.Add(patternpoint3, patterntexture);

    public void RemovePatternTexture(Point3 patternpoint3) => this.m_patternTextures.Remove(patternpoint3);

    public void Draw(Camera camera, int drawOrder)
    {
      foreach (Point3 key in this.m_logoPoints.Keys)
      {
        bool flag = Terrain.ExtractData(this.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetCellValue(key.X, key.Y, key.Z)) == 1;
        foreach (LogoPoint logoPoint in this.m_logoPoints[key])
        {
          if (logoPoint.Color.A > (byte) 0)
          {
            Vector3 v1 = logoPoint.Position - camera.ViewPosition;
            float num1 = Vector3.Dot(v1, camera.ViewDirection);
            if ((double) num1 > 0.00999999977648258)
            {
              float num2 = v1.Length();
              if ((double) num2 < (double) this.m_subsystemSky.ViewFogRange.Y)
              {
                float size = logoPoint.Size;
                if ((double) logoPoint.FarDistance > 0.0)
                  size += (logoPoint.FarSize - logoPoint.Size) * MathUtils.Saturate(num2 / logoPoint.FarDistance);
                Vector3 vector3 = (float) (0.0 - (0.00999999977648258 + 0.0199999995529652 * (double) num1)) / num2 * v1;
                Vector3 p1 = logoPoint.Position + size * (-logoPoint.Right - logoPoint.Up) + vector3;
                Vector3 p2 = logoPoint.Position + size * (logoPoint.Right - logoPoint.Up) + vector3;
                Vector3 p3 = logoPoint.Position + size * (logoPoint.Right + logoPoint.Up) + vector3;
                Vector3 p4 = logoPoint.Position + size * (-logoPoint.Right + logoPoint.Up) + vector3;
                if (!flag)
                  this.m_batchesByType[(int) logoPoint.Type].QueueQuad(p1, p2, p3, p4, new Vector2(0.0f, 0.0f), new Vector2(1f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 1f), logoPoint.Color);
                else
                  this.m_batchesByType2[(int) logoPoint.Type].QueueQuad(p1, p2, p3, p4, new Vector2(0.0f, 0.0f), new Vector2(1f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 1f), logoPoint.Color);
              }
            }
          }
        }
      }
      this.PatternLoad(camera);
      this.m_primitivesRenderer.Flush(camera.ViewProjectionMatrix);
    }

    public override void Load(ValuesDictionary valuesDictionary)
    {
      this.m_subsystemSky = this.Project.FindSubsystem<SubsystemSky>(true);
      this.m_batchesByType[0] = this.m_primitivesRenderer.TexturedBatch(ContentManager.Get<Texture2D>("Textures/Csharp0"), depthStencilState: DepthStencilState.DepthRead, rasterizerState: RasterizerState.CullCounterClockwiseScissor, blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp);
      this.m_batchesByType[1] = this.m_primitivesRenderer.TexturedBatch(ContentManager.Get<Texture2D>("Textures/Csharp0"), depthStencilState: DepthStencilState.DepthRead, rasterizerState: RasterizerState.CullCounterClockwiseScissor, blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp);
      this.m_batchesByType[2] = this.m_primitivesRenderer.TexturedBatch(ContentManager.Get<Texture2D>("Textures/Csharp0"), depthStencilState: DepthStencilState.DepthRead, rasterizerState: RasterizerState.CullCounterClockwiseScissor, blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp);
      this.m_batchesByType[3] = this.m_primitivesRenderer.TexturedBatch(ContentManager.Get<Texture2D>("Textures/Csharp0"), depthStencilState: DepthStencilState.DepthRead, rasterizerState: RasterizerState.CullCounterClockwiseScissor, blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp);
      this.m_batchesByType2[0] = this.m_primitivesRenderer.TexturedBatch(ContentManager.Get<Texture2D>("Textures/Csharp1"), depthStencilState: DepthStencilState.DepthRead, rasterizerState: RasterizerState.CullCounterClockwiseScissor, blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp);
      this.m_batchesByType2[1] = this.m_primitivesRenderer.TexturedBatch(ContentManager.Get<Texture2D>("Textures/Csharp2"), depthStencilState: DepthStencilState.DepthRead, rasterizerState: RasterizerState.CullCounterClockwiseScissor, blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp);
      this.m_batchesByType2[2] = this.m_primitivesRenderer.TexturedBatch(ContentManager.Get<Texture2D>("Textures/Csharp3"), depthStencilState: DepthStencilState.DepthRead, rasterizerState: RasterizerState.CullCounterClockwiseScissor, blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp);
      this.m_batchesByType2[3] = this.m_primitivesRenderer.TexturedBatch(ContentManager.Get<Texture2D>("Textures/Csharp4"), depthStencilState: DepthStencilState.DepthRead, rasterizerState: RasterizerState.CullCounterClockwiseScissor, blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp);
    }

    public void PatternLoad(Camera camera)
    {
      foreach (Point3 key in this.m_patternPoints.Keys)
      {
        PatternPoint patternPoint = this.m_patternPoints[key];
        Vector3 v1 = patternPoint.Position - camera.ViewPosition;
        float num1 = Vector3.Dot(v1, camera.ViewDirection);
        if ((double) num1 > 0.00999999977648258)
        {
          float num2 = v1.Length();
          if ((double) num2 < (double) this.m_subsystemSky.ViewFogRange.Y)
          {
            float size = patternPoint.Size;
            if ((double) patternPoint.FarDistance > 0.0)
              size += (patternPoint.FarSize - patternPoint.Size) * MathUtils.Saturate(num2 / patternPoint.FarDistance);
            Vector3 vector3 = (float) (0.0 - (0.00999999977648258 + 0.0199999995529652 * (double) num1)) / num2 * v1;
            Vector3 p1 = patternPoint.Position + size * (-patternPoint.Right - patternPoint.Up) + vector3;
            Vector3 p2 = patternPoint.Position + size * (patternPoint.Right - patternPoint.Up) + vector3;
            Vector3 p3 = patternPoint.Position + size * (patternPoint.Right + patternPoint.Up) + vector3;
            Vector3 p4 = patternPoint.Position + size * (-patternPoint.Right + patternPoint.Up) + vector3;
            try
            {
              this.m_batchesByType3[0] = this.m_primitivesRenderer.TexturedBatch(this.m_patternTextures[key], depthStencilState: DepthStencilState.DepthRead, rasterizerState: RasterizerState.CullCounterClockwiseScissor, blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearClamp);
              this.m_batchesByType3[0].QueueQuad(p1, p2, p3, p4, new Vector2(0.0f, 0.0f), new Vector2(1f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 1f), patternPoint.Color);
            }
            catch
            {
            }
          }
        }
      }
    }
  }
}