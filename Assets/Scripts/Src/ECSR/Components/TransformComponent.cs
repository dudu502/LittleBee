using LogicFrameSync.Src.LockStep.Frame;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// transform组件
    /// 包含位置信息，旋转
    /// </summary>
    public class TransformComponent:IComponent
    {
        public float2 LocalPosition {  set; get; }
        public TransformComponent Parent = null;

        /// <summary>
        /// 位移矩阵
        /// 齐次坐标下
        /// </summary>
        float3x3 float3x3_translation = new float3x3();

        /// <summary>
        /// 旋转矩阵
        /// 齐次坐标下
        /// </summary>
        float3x3 float3x3_rotation = new float3x3();
        public float RotateDegreeZ = 0;
        public string ParentEntityId = "";
        public TransformComponent(float2 pos)
        {
            LocalPosition = pos;
        }
        public override string ToString()
        {
            return string.Format("[TransformComponent Id:{0} Pos:{1} Rot_Degree:{2}]", EntityId, LocalPosition,RotateDegreeZ);
        }

        public Vector2 GetPositionVector2() { return new Vector2(LocalPosition.x, LocalPosition.y); }

        /// <summary>
        /// 位移
        /// </summary>
        /// <param name="value"></param>
        public void Translate(float2 value)
        {
            float3x3_translation.c0 = new float3(1, 0, value.x);
            float3x3_translation.c1 = new float3(0, 1, value.y);
            float3x3_translation.c2 = new float3(0, 0, 1);
            float3 localpos = new float3(LocalPosition,1);
            LocalPosition = math.mul(localpos, float3x3_translation).xy;
        }
        public void RotateDeltaDegree(float deltaDegree)
        {
            RotateDegreeZ += deltaDegree;
            Rotate(deltaDegree);
        }
        /// <summary>
        /// 旋转角度
        /// </summary>
        /// <param name="degree"></param>
        void Rotate(float degree)
        {

            float radians = math.radians(degree);
            float3x3_rotation.c0 = new float3(math.cos(radians), math.sin(radians), 0);
            float3x3_rotation.c1 = new float3(-math.sin(radians), math.cos(radians), 0);
            float3x3_rotation.c2 = new float3(0, 0, 1);
            float3 localpos = new float3(LocalPosition, 1);
            LocalPosition = math.mul(localpos, float3x3_rotation).xy;
        }


        public bool Enable { set; get; }
        public string EntityId { set; get; }
        public IComponent Clone()
        {
            TransformComponent com = new TransformComponent(LocalPosition);
            com.Enable = Enable;
            com.EntityId = EntityId;
            com.RotateDegreeZ = RotateDegreeZ;
            com.Parent = Parent;
            return com;
        }
        public int GetCommand()
        {
            return FrameCommand.SYNC_TRANSFORM;
        }

      


        public static float2 LocalPosition2WorldPosition(TransformComponent transformComponent)
        {
            if (transformComponent.Parent != null)
            {
                TransformComponent parent = transformComponent.Parent.Clone() as TransformComponent;
                TransformComponent clone = transformComponent.Clone() as TransformComponent;
                clone.RotateDeltaDegree(-parent.RotateDegreeZ);
                clone.Translate(parent.LocalPosition);
                parent.LocalPosition = clone.LocalPosition;
                return LocalPosition2WorldPosition(parent);
            }
            return transformComponent.LocalPosition;
        }

    }
}
