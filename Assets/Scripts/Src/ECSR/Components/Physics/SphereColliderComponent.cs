using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class SphereColliderComponent : AbstractComponent, ICollisionUpdatable
    {

        public VInt3 Center;
        public float Radius;
        public VCollisionShape Collider { set; get; }

        public SphereColliderComponent(VInt3 center, float radius)
        {
            Center = center;
            Radius = radius;
            Collider = VCollisionShape.CreateSphereColliderShape(center, radius);
        }

        override public IComponent Clone()
        {
            SphereColliderComponent comp = new SphereColliderComponent(Center, Radius);
            comp.Enable = Enable;
            comp.EntityId = EntityId;
            return comp;
        }

        override public int GetCommand()
        {
            return 0;
        }
        public override string ToString()
        {
            return string.Format("[SphereColliderComponent Id:{0} Center:{1} Rad:{2}]",EntityId,Center.ToString(),Radius);
        }
        public void UpdateCollision(VInt3 location)
        {
            Collider.UpdateShape(location, new VInt3(0,0,1));
        }
    }
}
