using System;
using UnityEngine;

[Serializable]
public class VCollisionSphere : VCollisionShape
{
	[HideInInspector, SerializeField]
	private VInt3 localPos = VInt3.zero;

	[HideInInspector, SerializeField]
	private int localRadius = 500;

	private VInt3 worldPos = VInt3.zero;

	private int worldRadius = 500;

/*	[CollisionProperty]*/
	public VInt3 Pos
	{
		get
		{
			return this.localPos;
		}
		set
		{
			this.localPos = value;
			this.dirty = false;
		}
	}

	public VInt3 WorldPos
	{
		get
		{
			base.ConditionalUpdateShape();
			return this.worldPos;
		}
	}

/*	[CollisionProperty]*/
	public int Radius
	{
		get
		{
			return this.localRadius;
		}
		set
		{
			this.localRadius = value;
			this.dirty = true;
		}
	}

	public int WorldRadius
	{
		get
		{
			base.ConditionalUpdateShape();
			return this.worldRadius;
		}
	}

	public override int AvgCollisionRadius
	{
		get
		{
			return this.WorldRadius;
		}
	}

	public VCollisionSphere()
	{
		this.dirty = true;
	}

	public override bool Intersects(VCollisionSphere s)
	{
		base.ConditionalUpdateShape();
		s.ConditionalUpdateShape();
		long num = (long)(this.worldRadius + s.worldRadius);
		return (this.worldPos - s.worldPos).sqrMagnitudeLong <= num * num;
	}

	public override bool Intersects(VCollisionBox obb)
	{
		return obb.Intersects(this);
	}

	public override bool Intersects(VCollisionCylinderSector cs)
	{
		return cs.Intersects(this);
	}

	public override bool EdgeIntersects(VCollisionSphere s)
	{
		base.ConditionalUpdateShape();
		s.ConditionalUpdateShape();
		long num = (long)(this.worldRadius + s.worldRadius);
		long num2 = (long)(this.worldRadius - s.worldRadius);
		long sqrMagnitudeLong2D = (this.worldPos - s.worldPos).sqrMagnitudeLong2D;
		return sqrMagnitudeLong2D <= num * num && sqrMagnitudeLong2D >= num2 * num2;
	}

	public override bool EdgeIntersects(VCollisionBox s)
	{
		return false;
	}

	public override bool EdgeIntersects(VCollisionCylinderSector cs)
	{
		return cs.EdgeIntersects(this);
	}

	public override CollisionShapeType GetShapeType()
	{
		return CollisionShapeType.Sphere;
	}

	public static void UpdatePosition(ref VInt3 worldPos, ref VInt3 localPos, ref VInt3 location, ref VInt3 forward)
	{
		if (localPos.x == 0 && localPos.z == 0)
		{
			worldPos.x = localPos.x + location.x;
			worldPos.y = localPos.y + location.y;
			worldPos.z = localPos.z + location.z;
		}
		else
		{
			VInt3 up = VInt3.up;
			VInt3 vInt = forward;
			VInt3 vInt2 = VInt3.Cross(ref up, ref vInt);
			VInt3 vInt3 = location;
			worldPos = IntMath.Transform(ref localPos, ref vInt2, ref up, ref vInt, ref vInt3);
		}
	}

	public override void UpdateShape(VInt3 location, VInt3 forward)
	{
		VCollisionSphere.UpdatePosition(ref this.worldPos, ref this.localPos, ref location, ref forward);
		this.worldRadius = this.localRadius;
		this.dirty = false;
	}

	public override void UpdateShape(VInt3 location, VInt3 forward, int moveDelta)
	{
	}

	public override void GetAabb2D(out VInt2 origin, out VInt2 size)
	{
		origin = this.WorldPos.xz;
		origin.x -= this.localRadius;
		origin.y -= this.localRadius;
		size.x = this.localRadius + this.localRadius;
		size.y = size.x;
	}

// 	public override bool AcceptFowVisibilityCheck(COM_PLAYERCAMP inHostCamp, GameFowManager fowMgr)
// 	{
// 		return GameFowCollector.VisitFowVisibilityCheck(this, this.owner, inHostCamp, fowMgr);
// 	}
}
