//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;

namespace CreatorWandModAPI
{
    public static class MirrorBlockBehavior
    {
        public static bool IsMirror;

        public static void ChangeIsMirror()
        {
            if (IsMirror)
            {
                IsMirror = false;
            }
            else
            {
                IsMirror = true;
            }
        }

        public static void Preview(CreatorAPI creatorAPI, ref Point3 Start, ref Point3 End)
        {
        }
    }
}