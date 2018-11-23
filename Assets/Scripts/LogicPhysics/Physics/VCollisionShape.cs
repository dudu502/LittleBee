using Assets.Scripts.GameLogic;
using Assets.Scripts.Common;
using System;
using UnityEngine;

[Serializable]
public abstract class VCollisionShape
{
	[HideInInspector]
	[NonSerialized]
	public bool dirty = true;

	[HideInInspector]
	[NonSerialized]
	public bool isBox;

	[HideInInspector]
	[NonSerialized]
	public PoolObjHandle<ActorRoot> owner;

	private static DictionaryView<int, SCollisionComponent> s_componentCache = new DictionaryView<int, SCollisionComponent>();

	public abstract int AvgCollisionRadius
	{
		get;
	}

	public void OnEnable()
	{
		this.dirty = true;
		this.owner.Release();
	}

	public void ConditionalUpdateShape()
	{
		if (this.dirty)
		{
			ActorRoot handle = this.owner.handle;
// 			if (this.isBox && handle.TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Bullet)
// 			{
// 				BulletWrapper bulletWrapper = handle.ActorControl as BulletWrapper;
// 				if (bulletWrapper != null && bulletWrapper.GetMoveCollisiong())
// 				{
// 					this.UpdateShape(handle.location, handle.forward, bulletWrapper.GetMoveDelta());
// 				}
// 				else
// 				{
// 					this.UpdateShape(handle.location, handle.forward);
// 				}
// 			}
// 			else
 			{
				this.UpdateShape(handle.location, handle.forward);
			}
		}
	}
    public static VCollisionShape CreateBoxColliderShape(VInt3 center,VInt3 size)
    {
        return new VCollisionBox() {
            Pos = center,
            Size = size,
        };
    }

    public static VCollisionShape CreateSphereColliderShape(VInt3 center,float radius)
    {
        return new VCollisionSphere() {
            Pos = center,
            Radius = (int)Math.Round(radius),
        };
    }
    public static VCollisionShape createFromCollider(GameObject gameObject)
	{
/*		DebugHelper.Assert(!Singleton<BattleLogic>.instance.isFighting || Singleton<GameLogic>.instance.bInLogicTick || Singleton<FrameSynchr>.instance.isCmdExecuting);*/
		Collider component = gameObject.GetComponent<Collider>();
		if (component == null)
		{
			return null;
		}
		VCollisionShape result = null;
		if (component is BoxCollider)
		{
			BoxCollider boxCollider = component as BoxCollider;
			result = new VCollisionBox
			{
                Pos = (VInt3)boxCollider.center,
                Size = (VInt3)boxCollider.size
            };
		}
		else if (component is CapsuleCollider)
		{
			CapsuleCollider capsuleCollider = component as CapsuleCollider;
			VCollisionSphere vCollisionSphere = new VCollisionSphere();
			Vector3 center = capsuleCollider.center;
			center.y -= capsuleCollider.height * 0.5f;
			vCollisionSphere.Pos = (VInt3)center;
			vCollisionSphere.Radius = ((VInt)capsuleCollider.radius).i;
			result = vCollisionSphere;
		}
		else if (component is SphereCollider)
		{
			SphereCollider sphereCollider = component as SphereCollider;
			result = new VCollisionSphere
			{
				Pos = (VInt3)sphereCollider.center,
				Radius = ((VInt)sphereCollider.radius).i
			};
		}
		return result;
	}

	public static void ClearCache()
	{
		VCollisionShape.s_componentCache.Clear();
	}

	public static VCollisionShape InitActorCollision(ActorRoot actor)
	{
		if (actor.shape == null)
		{
			SCollisionComponent sCollisionComponent = null;
			if (!VCollisionShape.s_componentCache.TryGetValue(actor.gameObject.GetInstanceID(), out sCollisionComponent))
			{
				sCollisionComponent = actor.gameObject.GetComponent<SCollisionComponent>();
				VCollisionShape.s_componentCache[actor.gameObject.GetInstanceID()] = sCollisionComponent;
			}
			VCollisionShape vCollisionShape;
			if (sCollisionComponent)
			{
				vCollisionShape = sCollisionComponent.CreateShape();
			}
// 			else if (actor.CharInfo != null && actor.CharInfo.collisionType != CollisionShapeType.None)
// 			{
// 				vCollisionShape = actor.CharInfo.CreateCollisionShape();
// 			}
			else
			{
				vCollisionShape = VCollisionShape.createFromCollider(actor.gameObject);
			}
			if (vCollisionShape != null)
			{
				vCollisionShape.Born(actor);
			}
		}
		return actor.shape;
	}

	public static VCollisionShape InitActorCollision(ActorRoot actor, GameObject gameObj, string actionName)
	{
		VCollisionShape vCollisionShape = null;
		if (actor.shape == null)
		{
			SCollisionComponent sCollisionComponent = null;
			if (null != gameObj && !VCollisionShape.s_componentCache.TryGetValue(actor.gameObject.GetInstanceID(), out sCollisionComponent))
			{
				sCollisionComponent = gameObj.GetComponent<SCollisionComponent>();
				VCollisionShape.s_componentCache[actor.gameObject.GetInstanceID()] = sCollisionComponent;
			}
			if (sCollisionComponent)
			{
				vCollisionShape = sCollisionComponent.CreateShape();
			}
// 			else if (actor.CharInfo != null && actor.CharInfo.collisionType != CollisionShapeType.None)
// 			{
// 				vCollisionShape = actor.CharInfo.CreateCollisionShape();
// 			}
			else if (gameObj)
			{
				vCollisionShape = VCollisionShape.createFromCollider(gameObj);
			}
			if (vCollisionShape != null)
			{
				vCollisionShape.Born(actor);
			}
		}
		return actor.shape;
	}

	public virtual void Born(ActorRoot actor)
	{
		actor.shape = this;
		this.owner = new PoolObjHandle<ActorRoot>(actor);
	}

	public bool Intersects(VCollisionShape shape)
	{
		bool result = false;
		if (shape != null)
		{
			CollisionShapeType shapeType = shape.GetShapeType();
			if (shapeType == CollisionShapeType.Box)
			{
				result = this.Intersects((VCollisionBox)shape);
			}
			else if (shapeType == CollisionShapeType.CylinderSector)
			{
				result = this.Intersects((VCollisionCylinderSector)shape);
			}
			else
			{
				result = this.Intersects((VCollisionSphere)shape);
			}
		}
		return result;
	}

	public bool EdgeIntersects(VCollisionShape shape)
	{
		bool result = false;
		if (shape != null)
		{
			CollisionShapeType shapeType = shape.GetShapeType();
			if (shapeType == CollisionShapeType.Box)
			{
				result = this.EdgeIntersects((VCollisionBox)shape);
			}
			else if (shapeType == CollisionShapeType.CylinderSector)
			{
				result = this.EdgeIntersects((VCollisionCylinderSector)shape);
			}
			else
			{
				result = this.EdgeIntersects((VCollisionSphere)shape);
			}
		}
		return result;
	}

	public abstract bool Intersects(VCollisionBox obb);

	public abstract bool Intersects(VCollisionSphere s);

	public abstract bool Intersects(VCollisionCylinderSector cs);

	public abstract bool EdgeIntersects(VCollisionBox obb);

	public abstract bool EdgeIntersects(VCollisionSphere s);

	public abstract bool EdgeIntersects(VCollisionCylinderSector cs);

	public abstract void UpdateShape(VInt3 location, VInt3 forward);

	public abstract void UpdateShape(VInt3 location, VInt3 forward, int moveDelta);

	public abstract CollisionShapeType GetShapeType();

	public abstract void GetAabb2D(out VInt2 lt, out VInt2 size);

/*	public abstract bool AcceptFowVisibilityCheck(COM_PLAYERCAMP inHostCamp, GameFowManager fowMgr);*/
}
