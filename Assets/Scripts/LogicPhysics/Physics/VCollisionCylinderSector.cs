using System;
using UnityEngine;

[Serializable]
public class VCollisionCylinderSector : VCollisionShape
{
	[HideInInspector, SerializeField]
	private VInt3 localPos = VInt3.zero;

	[HideInInspector, SerializeField]
	private int radius = 500;

	[HideInInspector, SerializeField]
	private int degree = 90;

	[HideInInspector, SerializeField]
	private int rotation;

	[HideInInspector, SerializeField]
	private int height = 500;

	private VInt3 worldPos = VInt3.zero;

	private VInt3 rightDir = VInt3.forward;

	private VInt3 leftDir = VInt3.forward;

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
			return this.radius;
		}
		set
		{
			this.radius = value;
			this.dirty = true;
		}
	}

/*	[CollisionProperty]*/
	public int Degree
	{
		get
		{
			return this.degree;
		}
		set
		{
			this.degree = Mathf.Clamp(value, 1, 360);
			this.dirty = true;
		}
	}

/*    [CollisionProperty]*/
    public int Height
	{
		get
		{
			return this.height;
		}
		set
		{
			this.height = Mathf.Max(0, value);
			this.dirty = true;
		}
	}

/*	[CollisionProperty]*/
	public int Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			this.rotation = value;
			this.dirty = true;
		}
	}

	public override int AvgCollisionRadius
	{
		get
		{
			return this.radius;
		}
	}

	public VCollisionCylinderSector()
	{
		this.dirty = true;
	}

	private static VInt3 ClosestPoint(ref VInt3 point, ref VInt3 lineStart, ref VInt3 lineDir, int lineLen)
	{
		long num = VInt3.DotXZLong(point - lineStart, lineDir);
		num = IntMath.Clamp(num, 0L, (long)(lineLen * 1000));
		return IntMath.Divide(lineDir, num, 1000000L) + lineStart;
	}

	private static int CalcSide(ref VInt3 point, ref VInt3 lineStart, ref VInt3 lineDir)
	{
		return lineDir.x * (point.z - lineStart.z) - (point.x - lineStart.x) * lineDir.z;
	}

	public override bool Intersects(VCollisionSphere s)
	{
		base.ConditionalUpdateShape();
		s.ConditionalUpdateShape();
		VInt3 rhs = s.WorldPos;
		int worldRadius = s.WorldRadius;
		int num = this.height >> 1;
		int num2 = this.worldPos.y - num;
		int num3 = this.worldPos.y + num;
		if (rhs.y + worldRadius <= num2 || rhs.y - worldRadius >= num3)
		{
			return false;
		}
		int num4 = worldRadius;
		if (rhs.y > num3 || rhs.y < num2)
		{
			int num5 = (rhs.y <= num3) ? (num2 - rhs.y) : (rhs.y - num3);
			int num6 = worldRadius * worldRadius - num5 * num5;
			//DebugHelper.Assert(num6 >= 0);
			num4 = IntMath.Sqrt((long)num6);
		}
		long num7 = (long)(num4 + this.radius);
		if ((this.worldPos - rhs).sqrMagnitudeLong2D >= num7 * num7)
		{
			return false;
		}
		int num8 = worldRadius * worldRadius;
		VInt3 lhs = VCollisionCylinderSector.ClosestPoint(ref rhs, ref this.worldPos, ref this.leftDir, this.radius);
		if ((lhs - rhs).sqrMagnitudeLong2D <= (long)num8)
		{
			return true;
		}
		lhs = VCollisionCylinderSector.ClosestPoint(ref rhs, ref this.worldPos, ref this.rightDir, this.radius);
		if ((lhs - rhs).sqrMagnitudeLong2D <= (long)num8)
		{
			return true;
		}
		if (this.degree <= 180)
		{
			if (VCollisionCylinderSector.CalcSide(ref rhs, ref this.worldPos, ref this.leftDir) <= 0 && VCollisionCylinderSector.CalcSide(ref rhs, ref this.worldPos, ref this.rightDir) >= 0)
			{
				return true;
			}
		}
		else if (VCollisionCylinderSector.CalcSide(ref rhs, ref this.worldPos, ref this.leftDir) <= 0 || VCollisionCylinderSector.CalcSide(ref rhs, ref this.worldPos, ref this.rightDir) >= 0)
		{
			return true;
		}
		return false;
	}

	public override bool Intersects(VCollisionBox obb)
	{
		return false;
	}

	public override bool Intersects(VCollisionCylinderSector s)
	{
		return false;
	}

	public override bool EdgeIntersects(VCollisionSphere s)
	{
		return this.Intersects(s);
	}

	public override bool EdgeIntersects(VCollisionBox s)
	{
		return false;
	}

	public override bool EdgeIntersects(VCollisionCylinderSector s)
	{
		return false;
	}

	public override CollisionShapeType GetShapeType()
	{
		return CollisionShapeType.CylinderSector;
	}

	public override void UpdateShape(VInt3 location, VInt3 forward)
	{
		VCollisionSphere.UpdatePosition(ref this.worldPos, ref this.localPos, ref location, ref forward);
		if (this.rotation != 0)
		{
			forward = forward.RotateY(this.rotation);
		}
		VFactor vFactor;
		VFactor vFactor2;
		IntMath.sincos(out vFactor, out vFactor2, (long)(314 * Mathf.Clamp(this.degree, 1, 360)), 36000L);
		long num = vFactor2.nom * vFactor.den;
		long num2 = vFactor2.den * vFactor.nom;
		long b = vFactor2.den * vFactor.den;
		this.rightDir.x = (int)IntMath.Divide((long)forward.x * num + (long)forward.z * num2, b);
		this.rightDir.z = (int)IntMath.Divide((long)(-(long)forward.x) * num2 + (long)forward.z * num, b);
		this.rightDir.y = 0;
		num2 = -num2;
		this.leftDir.x = (int)IntMath.Divide((long)forward.x * num + (long)forward.z * num2, b);
		this.leftDir.z = (int)IntMath.Divide((long)(-(long)forward.x) * num2 + (long)forward.z * num, b);
		this.leftDir.y = 0;
		this.rightDir.Normalize();
		this.leftDir.Normalize();
		this.dirty = false;
	}

	public override void UpdateShape(VInt3 location, VInt3 forward, int moveDelta)
	{
	}

	public override void GetAabb2D(out VInt2 origin, out VInt2 size)
	{
		origin = this.worldPos.xz;
		origin.x -= this.radius;
		origin.y -= this.radius;
		size.x = this.radius + this.radius;
		size.y = size.x;
	}

// 	public override bool AcceptFowVisibilityCheck(COM_PLAYERCAMP inHostCamp, GameFowManager fowMgr)
// 	{
// 		return GameFowCollector.VisitFowVisibilityCheck(this, this.owner, inHostCamp, fowMgr);
// 	}
}
