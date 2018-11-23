using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.GameLogic
{
	public class ActorRootSlot
	{
		private VInt distance = 0;

		private VInt3 prePosition = VInt3.zero;

		private VInt3 translation = VInt3.zero;

		private PoolObjHandle<ActorRoot> childActorRoot;

		public ActorRootSlot(PoolObjHandle<ActorRoot> _child, VInt3 _parentPos)
		{
			this.childActorRoot = _child;
			this.prePosition = _parentPos;
		}

		public ActorRootSlot(PoolObjHandle<ActorRoot> _child, VInt3 _parentPos, VInt3 _trans)
		{
			this.translation = _trans;
			this.prePosition = _parentPos;
			this.distance = this.translation.magnitude;
			this.childActorRoot = _child;
		}

		private void UpdateMoveDelta(VInt3 _newPos)
		{
// 			if (this.childActorRoot.handle.TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Bullet)
// 			{
// 				BulletWrapper bulletWrapper = this.childActorRoot.handle.ActorControl as BulletWrapper;
// 				if (bulletWrapper != null && bulletWrapper.GetMoveCollisiong() && this.prePosition != _newPos)
// 				{
// 					bulletWrapper.SetMoveDelta((_newPos - this.prePosition).magnitude2D);
// 					this.prePosition = _newPos;
// 				}
// 			}
		}

		public void Update(ActorRoot _parent)
		{
			if (!this.childActorRoot)
			{
				return;
			}
			VInt3 vInt = _parent.location + this.translation;
			if (this.translation.x != 0 || this.translation.z != 0)
			{
				VInt3 forward = VInt3.forward;
				VFactor b = VInt3.AngleInt(_parent.forward, forward);
				int num = _parent.forward.x * forward.z - forward.x * _parent.forward.z;
				if (num < 0)
				{
					b = VFactor.twoPi - b;
				}
				VInt3 vInt2 = this.translation.RotateY(ref b);
				vInt = _parent.location + vInt2.NormalizeTo(this.distance.i);
				vInt.y += this.translation.y;
			}
			this.childActorRoot.handle.location = vInt;
			this.childActorRoot.handle.forward = _parent.forward;
			this.UpdateMoveDelta(vInt);
		}
	}
}
