using CreatorWandModAPI;
using Engine;
using Game;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesDatabase;

namespace CreatorWand2
{
    public static class CW2EntityManager
    {
        public static bool RemoveEntity(Point3 point3)
        {
            ComponentBlockEntity blockEntity = GameManager.Project.FindSubsystem<SubsystemBlockEntities>(throwOnError: true).GetBlockEntity(point3.X, point3.Y, point3.Z);
            if (blockEntity != null)
            {
                GameManager.Project.RemoveEntity(blockEntity.Entity, true);
                GameManager.Project.Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void AddEntity(Point3 point3)
        {
            AddEntity(point3, GameManager.Project.FindSubsystem<SubsystemTerrain>(true).Terrain.GetCellValueFast(point3.X, point3.Y, point3.Z));
        }
        public static void AddEntity(Point3 point3, int cellValue)
        {
            switch (BlocksManager.Blocks[Terrain.ExtractContents(cellValue)])
            {
                case FurnaceBlock _:
                    while (RemoveEntity(point3)) ;
                    DatabaseObject FurnaceBlockdatabaseObject = GameManager.Project.GameDatabase.Database.FindDatabaseObject("Furnace", GameManager.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                    ValuesDictionary FurnaceBlockvaluesDictionary = new ValuesDictionary();
                    FurnaceBlockvaluesDictionary.PopulateFromDatabaseObject(FurnaceBlockdatabaseObject);
                    FurnaceBlockvaluesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point3);
                    Entity FurnaceBlockentity = GameManager.Project.CreateEntity(FurnaceBlockvaluesDictionary);
                    GameManager.Project.AddEntity(FurnaceBlockentity);
                    GameManager.Project.Save();
                    return;
                case ChestBlock _:
                    DatabaseObject ChestBlockdatabaseObject = GameManager.Project.GameDatabase.Database.FindDatabaseObject("Chest", GameManager.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                    ValuesDictionary ChestBlockvaluesDictionary = new ValuesDictionary();
                    ChestBlockvaluesDictionary.PopulateFromDatabaseObject(ChestBlockdatabaseObject);
                    ChestBlockvaluesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point3);
                    Entity ChestBlockentity = GameManager.Project.CreateEntity(ChestBlockvaluesDictionary);
                    GameManager.Project.AddEntity(ChestBlockentity);
                    GameManager.Project.Save();
                    return;
                case CraftingTableBlock _:
                    DatabaseObject CraftingTableBlockdatabaseObject = GameManager.Project.GameDatabase.Database.FindDatabaseObject("CraftingTable", GameManager.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                    ValuesDictionary CraftingTableBlockvaluesDictionary = new ValuesDictionary();
                    CraftingTableBlockvaluesDictionary.PopulateFromDatabaseObject(CraftingTableBlockdatabaseObject);
                    CraftingTableBlockvaluesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point3);
                    Entity CraftingTableBlockentity = GameManager.Project.CreateEntity(CraftingTableBlockvaluesDictionary);
                    GameManager.Project.AddEntity(CraftingTableBlockentity);
                    GameManager.Project.Save();
                    return;
                case DispenserBlock _:
                    DatabaseObject DispenserBlockdatabaseObject = GameManager.Project.GameDatabase.Database.FindDatabaseObject("Dispenser", GameManager.Project.GameDatabase.EntityTemplateType, throwIfNotFound: true);
                    ValuesDictionary DispenserBlockvaluesDictionary = new ValuesDictionary();
                    DispenserBlockvaluesDictionary.PopulateFromDatabaseObject(DispenserBlockdatabaseObject);
                    DispenserBlockvaluesDictionary.GetValue<ValuesDictionary>("BlockEntity").SetValue("Coordinates", point3);
                    Entity DispenserBlockentity = GameManager.Project.CreateEntity(DispenserBlockvaluesDictionary);
                    GameManager.Project.AddEntity(DispenserBlockentity);
                    GameManager.Project.Save();
                    return;
                default:
                    return;
            }
        }
    }
}
