using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
    public class SphereColliderComponent : IComponent,ICollisionUpdatable
    {
        public string EntityId { set; get; }

        public bool Enable { set; get; }

       
        

        public VInt3 Center;
        public float Radius;
        public VCollisionShape Collider { set; get; }

        public SphereColliderComponent(VInt3 center, float radius)
        {
            Center = center;
            Radius = radius;
            Collider = VCollisionShape.CreateSphereColliderShape(center, radius);
        }

        public IComponent Clone()
        {
            SphereColliderComponent comp = new SphereColliderComponent(Center, Radius);
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
            Collider.UpdateShape(location, new VInt3(0,0,1));
        }
    }
}
