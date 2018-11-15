using LogicFrameSync.Src.LockStep.Frame;
using Unity.Mathematics;
using UnityEngine;
namespace Components
{
    public class MoveComponent : IComponent, IParamsUpdatable
    {
        float Speed = 1;
        float2 Dir = float2.zero;
        public MoveComponent(float speed, float2 dir)
        {
            Dir = dir;
            Speed = speed;
        }
        public float GetSpeed() { return Speed; }
        public Vector2 GetDirVector2() { return new Vector2(Dir.x, Dir.y); }

        public float2 GetPathV2()
        {
            return Dir * Speed;
        }
        public bool SetSpeed(float speed_)
        {
            if (Speed != speed_)
            {
                Speed = speed_;
                return true;
            }
            return false;
        }
        public void SetDir(float2 vec)
        {
            Dir = vec;
        }
        public bool Enable { set; get; }
        public string EntityId { set; get; }
        public IComponent Clone()
        {
            MoveComponent com = new MoveComponent(Speed, Dir);
            com.Enable = Enable;
            com.EntityId = EntityId;
            return com;
        }
        public int GetCommand()
        {
            return FrameCommand.SYNC_MOVE;
        }

        public void UpdateParams(string[] paramsStrs)
        {
            SetDir(new float2(float.Parse(paramsStrs[0]), float.Parse(paramsStrs[1])));
        }

        public override string ToString()
        {
            return string.Format("[MoveComponent Id:{0} Dir:{1},{2} Speed:{3}]",EntityId,Dir.x,Dir.y,Speed);
        }
    }
}


