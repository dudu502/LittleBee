using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class BoxColliderComponent : IComponent,ICollisionUpdatable
    {
        public string EntityId { set; get; }

        public bool Enable { set; get; }

        public VInt3 Center;

        public VInt3 Size;
        public VCollisionShape Collider { set; get; }
        public BoxColliderComponent(VInt3 center,VInt3 size)
        {
            Center = center;
            Size = size;
            Collider = VCollisionShape.CreateBoxColliderShape(center, size);
        }

        public IComponent Clone()
        {
            BoxColliderComponent comp = new BoxColliderComponent(Center, Size);
            comp.Enable = Enable;
            comp.EntityId = EntityId;
            return comp;
        }

        public int GetCommand()
        {
            return 0;
        }

        public void UpdateCollision(VInt3 location)
        {
            Collider.UpdateShape(location, new VInt3(0, 0, 1));
        }
    }
}
