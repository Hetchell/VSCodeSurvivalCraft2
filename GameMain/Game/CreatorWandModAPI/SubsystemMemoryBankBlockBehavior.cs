//Code is not mine, is Suceru's https://github.com/Suceru/Survivalcraft2CreatorWandMod

using Engine;
using Game;

namespace CreatorWandModAPI
{
    public class SubsystemMemoryBankBlockBehavior1 : SubsystemMemoryBankBlockBehavior
    {
        public static string fName = "MemoryBankBlockBehavior";

        public override int[] HandledBlocks => new int[1]
        {
            186
        };

        public override bool OnEditInventoryItem(IInventory inventory, int slotIndex, ComponentPlayer componentPlayer)
        {
            int value = inventory.GetSlotValue(slotIndex);
            int count = inventory.GetSlotCount(slotIndex);
            int id = Terrain.ExtractData(value);
            MemoryBankData memoryBankData = GetItemData(id);
            memoryBankData = ((memoryBankData != null) ? ((MemoryBankData)memoryBankData.Copy()) : new MemoryBankData());
            /*if (SettingsManager.UsePrimaryMemoryBank)
            {
                DialogsManager.ShowDialog(componentPlayer.GuiWidget, new EditMemoryBankDialog(memoryBankData, delegate
                {
                    int data2 = StoreItemDataAtUniqueId(memoryBankData);
                    int value3 = Terrain.ReplaceData(value, data2);
                    inventory.RemoveSlotItems(slotIndex, count);
                    inventory.AddSlotItems(slotIndex, value3, 1);
                }));
            }
            else*/
            {
                DialogsManager.ShowDialog(componentPlayer.GuiWidget, new EditMemoryBankDialogAPI(memoryBankData, delegate
                {
                    int data = StoreItemDataAtUniqueId(memoryBankData);
                    int value2 = Terrain.ReplaceData(value, data);
                    inventory.RemoveSlotItems(slotIndex, count);
                    inventory.AddSlotItems(slotIndex, value2, 1);
                }));
            }

            return true;
        }

        public override bool OnEditBlock(int x, int y, int z, int value, ComponentPlayer componentPlayer)
        {
            MemoryBankData memoryBankData = GetBlockData(new Point3(x, y, z)) ?? new MemoryBankData();
            /*if (SettingsManager.UsePrimaryMemoryBank)
            {
                DialogsManager.ShowDialog(componentPlayer.GuiWidget, new EditMemoryBankDialog(memoryBankData, delegate
                {
                    SetBlockData(new Point3(x, y, z), memoryBankData);
                    int face2 = ((MemoryBankBlock)BlocksManager.Blocks[186]).GetFace(value);
                    SubsystemElectricity subsystemElectricity2 = base.SubsystemTerrain.Project.FindSubsystem<SubsystemElectricity>(throwOnError: true);
                    ElectricElement electricElement2 = subsystemElectricity2.GetElectricElement(x, y, z, face2);
                    if (electricElement2 != null)
                    {
                        subsystemElectricity2.QueueElectricElementForSimulation(electricElement2, subsystemElectricity2.CircuitStep + 1);
                    }
                }));
            }
            else*/
            {
                DialogsManager.ShowDialog(componentPlayer.GuiWidget, new EditMemoryBankDialogAPI(memoryBankData, delegate
                {
                    SetBlockData(new Point3(x, y, z), memoryBankData);
                    int face = ((MemoryBankBlock)BlocksManager.Blocks[186]).GetFace(value);
                    SubsystemElectricity subsystemElectricity = base.SubsystemTerrain.Project.FindSubsystem<SubsystemElectricity>(throwOnError: true);
                    ElectricElement electricElement = subsystemElectricity.GetElectricElement(x, y, z, face);
                    if (electricElement != null)
                    {
                        subsystemElectricity.QueueElectricElementForSimulation(electricElement, subsystemElectricity.CircuitStep + 1);
                    }
                }));
            }

            return true;
        }
    }
}
