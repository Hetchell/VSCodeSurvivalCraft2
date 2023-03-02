using Engine;
using System;

namespace Game
{
    public class SubsystemWeather1
    {
        public static SubsystemWeather subsystem = GameManager.Project.FindSubsystem<SubsystemWeather>(throwOnError: true);

        public static void SetPrecipitationTime(bool IsPrecipitation)
        {
            try
            {
                if (IsPrecipitation)
                {
                    subsystem.m_precipitationStartTime = subsystem.m_subsystemGameInfo.TotalElapsedGameTime;
                    subsystem.m_lightningIntensity = subsystem.m_random.Float(0.33f, 0.66f);
                    subsystem.m_precipitationEndTime = subsystem.m_precipitationStartTime + 60.0 * (double)subsystem.m_random.Float(3f, 6f);
                }
                else
                {
                    subsystem.m_precipitationStartTime = subsystem.m_subsystemGameInfo.TotalElapsedGameTime + 60.0 * (double)subsystem.m_random.Float(5f, 45f);
                    subsystem.m_lightningIntensity = (((double)subsystem.m_random.Float(0f, 1f) < 0.5) ? subsystem.m_random.Float(0.33f, 1f) : 0f);
                    subsystem.m_precipitationEndTime = subsystem.m_precipitationStartTime + 60.0 * (double)subsystem.m_random.Float(3f, 6f);
                }
            }
            catch (Exception ex)
            {
                Log.Warning("Err:" + ex);
            }
        }
    }
}