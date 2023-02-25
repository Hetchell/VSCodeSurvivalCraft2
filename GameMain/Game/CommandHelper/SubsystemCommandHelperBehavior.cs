using System;
using Engine;
using Color = Engine.Color;
using Rectangle = Engine.Rectangle;
namespace Game
{
    public class SubsystemCommandHelperBehavior : SubsystemBlockBehavior
    {

        public override int[] HandledBlocks
        {
            get
            {
                return new int[] { 502 };
            }
        }

        public override bool OnUse(Ray3 ray3something, ComponentMiner componentMiner)
        {
            var bodyResult = componentMiner.PickBody(ray3something);
            if (bodyResult.HasValue)
            {
                var creature = bodyResult.Value.ComponentBody.Entity.FindComponent<ComponentCreature>();
                if (creature != null)
                {
                    string name = creature.DisplayName.Replace(' ', '_').ToLower();
                    ClipboardManager.ClipboardString = name;
                    componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(name + "is copied to the clipboard", Color.White, false, false);
                    return false;
                }
            }

            var terrainResult = componentMiner.PickTerrainForDigging(ray3something);
            if (terrainResult.HasValue)
            {
                var val = terrainResult.Value.Value.ToString();
                ClipboardManager.ClipboardString = val;
                componentMiner.ComponentPlayer.ComponentGui.DisplaySmallMessage(val + "is copied to the clipboard", Color.White, false, false);
            }
            return false;
        }

        public override bool OnEditInventoryItem(IInventory inventory, int slotIndex, ComponentPlayer componentPlayer)
        {
            DialogsManager.ShowDialog(componentPlayer.ViewWidget.GameWidget, new CommandHelperDialog(Project.FindSubsystem<SubsystemCommandEngine>()));
            return false;
        }
    }
}
