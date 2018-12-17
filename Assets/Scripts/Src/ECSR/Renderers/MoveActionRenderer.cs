using Components;
using DG.Tweening;
using Entitas;
using LogicFrameSync.Src.LockStep;
using UnityEngine;

namespace Renderers
{
    /// <summary>
    /// 移动行为
    /// 视图显示器
    /// </summary>
    public class MoveActionRenderer:ActionRenderer
    {
        Tweener tweener;
        protected override void OnRender(Entitas.Entity entity)
        {
            base.OnRender(entity);

            TransformComponent com_Pos = entity.GetComponent<TransformComponent>();
            MoveComponent com_Move = entity.GetComponent<MoveComponent>();
            if (com_Pos != null)
            {
                double lerp = SimulationManager.Instance.GetFrameLerp() * SimulationManager.Instance.GetFrameMsLength() / 1000 / Time.deltaTime;
                var pos1 = com_Pos.GetPositionVector2();
                var nextPos = pos1 + com_Move.GetDirVector2() * (com_Move.GetSpeed() * (float)(Time.deltaTime / SimulationManager.Instance.GetFrameMsLength() / 1000));

                if (tweener != null)
                    tweener.Kill();
                tweener = null;
                tweener = transform.DOLocalMove(Vector2.Lerp(pos1, nextPos, (float)lerp), .5f, true);

                //transform.localPosition = Vector2.Lerp(pos1, nextPos, (float)lerp);
            }
        }

        
    }
}
