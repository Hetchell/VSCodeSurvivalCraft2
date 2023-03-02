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
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
  public class ComponentNewSimpleModel : ComponentModel
  {
    public SubsystemGameInfo m_subsystemGameInfo;
    public ComponentSpawn m_componentSpawn;
    public float scale = 1f;

    public override void Animate()
    {
      if (this.m_componentSpawn != null)
      {
        this.Opacity = new float?((double) this.m_componentSpawn.SpawnDuration > 0.0 ? (float) MathUtils.Saturate((this.m_subsystemGameInfo.TotalElapsedGameTime - this.m_componentSpawn.SpawnTime) / (double) this.m_componentSpawn.SpawnDuration) : 1f);
        if (this.m_componentSpawn.DespawnTime.HasValue)
          this.Opacity = new float?(MathUtils.Min(this.Opacity.Value, (float) MathUtils.Saturate(1.0 - (this.m_subsystemGameInfo.TotalElapsedGameTime - this.m_componentSpawn.DespawnTime.Value) / (double) this.m_componentSpawn.DespawnDuration)));
      }
      this.SetBoneTransform(this.Model.RootBone.Index, new Matrix?(Matrix.CreateScale(this.scale) * this.m_componentFrame.Matrix));
      base.Animate();
    }

    public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
    {
      this.m_subsystemGameInfo = this.Project.FindSubsystem<SubsystemGameInfo>(true);
      this.m_componentSpawn = this.Entity.FindComponent<ComponentSpawn>();
      base.Load(valuesDictionary, idToEntityMap);
    }
  }
}
