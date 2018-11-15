using LogicFrameSync.Src.LockStep.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    public class PositionComponent:IComponent
    {
        Vector2 Pos;
        public PositionComponent(Vector2 v3)
        {
            Pos = v3;
        }
        public override string ToString()
        {
            return string.Format("[PositionComponent Id:{0} Pos:{1},{2}]",EntityId, Pos.x, Pos.y);
        }
        public bool SetPosition(Vector2 pos)
        {
            Pos += pos;
            return true;
        }



        public Vector2 GetPosition() { return Pos; }



        float3x3 float3x3_translation = new float3x3();
        float3x3 float3x3_rotation = new float3x3();
        public float3 LocalPosition { set; get; }

        quaternion Quaternion;

        public void Translate(float2 value)
        {
            float3x3_translation.c0 = new float3(1, 0, value.x);
            float3x3_translation.c1 = new float3(0, 1, value.y);
            float3x3_translation.c2 = new float3(0, 0, 1);
            LocalPosition = math.mul(LocalPosition, float3x3_translation);
        }
        public void Rotate(float degree)
        {
            float radians = math.radians(degree);
            float3x3_rotation.c0 = new float3(math.cos(radians), math.sin(radians), 0);
            float3x3_rotation.c1 = new float3(-math.sin(radians), math.cos(radians), 0);
            float3x3_rotation.c2 = new float3(0, 0, 1);
            Quaternion = new quaternion(float3x3_rotation);
            LocalPosition = math.mul(LocalPosition, float3x3_rotation);
        }


        public bool Enable { set; get; }
        public string EntityId { set; get; }
        public IComponent Clone()
        {
            PositionComponent com = new PositionComponent(Pos);
            com.Enable = Enable;
            com.EntityId = EntityId;
            return com;
        }
        public int GetCommand()
        {
            return FrameCommand.SYNC_POSITION;
        }
    }
}
