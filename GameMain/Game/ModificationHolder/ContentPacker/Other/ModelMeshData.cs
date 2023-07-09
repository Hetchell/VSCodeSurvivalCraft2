using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModificationHolder.ContentPacker
{
    public class ModelMeshData
    {
        public string Name;

        public int ParentBoneIndex;

        public List<ModelMeshPartData> MeshParts = new List<ModelMeshPartData>();

        public BoundingBox BoundingBox;
    }
}
