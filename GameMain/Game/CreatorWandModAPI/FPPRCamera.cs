//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Engine.Input;
using Game;

namespace CreatorWandModAPI
{
    public class FPPRCamera : BasePerspectiveCamera
    {
        private Vector3 m_position;

        private Vector3 m_direction;

        private Vector3 m_velocity;

        private float m_rollSpeed;

        private float m_pitchSpeed;

        private float m_rollAngle;

        public override bool UsesMovementControls => false;

        public override bool IsEntityControlEnabled => true;

        public FPPRCamera(GameWidget gameWidget)
            : base(gameWidget)
        {
        }

        public override void Activate(Camera previousCamera)
        {
            SetupPerspectiveCamera(previousCamera.ViewPosition, previousCamera.ViewDirection, previousCamera.ViewUp);
        }

        public override void Update(float dt)
        {
            Vector3 vector = Vector3.Zero;
            Vector2 vector2 = Vector2.Zero;
            ComponentInput componentInput = base.GameWidget.PlayerData.ComponentPlayer?.ComponentInput;
            if (componentInput != null)
            {
                vector = componentInput.PlayerInput.CameraMove * new Vector3(1f, 0f, 1f);
                vector2 = componentInput.PlayerInput.CameraLook;
            }

            int num = Keyboard.IsKeyDown(Key.Shift) ? 1 : 0;
            bool flag = Keyboard.IsKeyDown(Key.Control);
            Vector3 direction = m_direction;
            Vector3 unitY = Vector3.UnitY;
            Vector3 vector3 = Vector3.Normalize(Vector3.Cross(direction, unitY));
            float num2 = 10f;
            if (num != 0)
            {
                num2 *= 5f;
            }

            if (flag)
            {
                num2 /= 5f;
            }

            Vector3 v = Vector3.Zero + num2 * vector.X * vector3 + num2 * vector.Y * unitY + num2 * vector.Z * direction;
            m_rollSpeed = MathUtils.Lerp(m_rollSpeed, -1.5f * vector2.X, 3f * dt);
            m_rollAngle += m_rollSpeed * dt;
            m_rollAngle *= MathUtils.Pow(0.33f, dt);
            m_pitchSpeed = MathUtils.Lerp(m_pitchSpeed, -0.2f * vector2.Y, 3f * dt);
            m_pitchSpeed *= MathUtils.Pow(0.33f, dt);
            m_velocity += 1.5f * (v - m_velocity) * dt;
            _ = base.GameWidget.Target.ComponentBody.Position;
            m_position += m_velocity * dt;
            m_direction = Vector3.Transform(m_direction, Matrix.CreateFromAxisAngle(unitY, 0.05f * m_rollAngle));
            m_direction = Vector3.Transform(m_direction, Matrix.CreateFromAxisAngle(vector3, 0.2f * m_pitchSpeed));
            SetupPerspectiveCamera(m_position, m_direction, Vector3.TransformNormal(Vector3.UnitY, Matrix.CreateFromAxisAngle(m_direction, 0f - m_rollAngle)));
        }
    }
}