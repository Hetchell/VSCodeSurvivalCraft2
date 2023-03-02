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
