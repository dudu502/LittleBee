using LogicFrameSync.Src.LockStep.Frame;
using UnityEngine;
namespace Components
{
    public class MoveComponent : IComponent, IParamsUpdatable
    {
        float Speed = 1;
        Vector2 Dir = Vector2.zero;
        public MoveComponent(float speed, Vector2 dir)
        {
            Dir = dir;
            Speed = speed;
        }
        public float GetSpeed() { return Speed; }
        public Vector2 GetDir() { return Dir; }
        public Vector2 GetPathV2()
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
        public bool SetDir(Vector2 vec)
        {
            if (Dir != vec)
            {
                Dir = vec;
                return true;
            }
            return false;
        }
        public bool Enable { set; get; }
        public int EntityId { set; get; }
        public IComponent Clone()
        {
            MoveComponent com = new MoveComponent(Speed, Dir);
            com.Enable = Enable;
            com.EntityId = EntityId;
            return com;
        }
        public int GetCommand()
        {
            return FrameCommand.SyncMove;
        }

        public void UpdateParams(string[] paramsStrs)
        {
            SetDir(new Vector2(float.Parse(paramsStrs[0]), float.Parse(paramsStrs[1])));
        }

        public override string ToString()
        {
            return string.Format("[MoveComponent Id:{0} Dir:{1},{2} Speed:{3}]",EntityId,Dir.x,Dir.y,Speed);
        }
    }
}


