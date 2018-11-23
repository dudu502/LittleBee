using System;
using UnityEngine;

[Serializable]
public class VCollisionBox : VCollisionShape
{
	[HideInInspector, SerializeField]
	private VInt3 localPos = VInt3.zero;

	[HideInInspector, SerializeField]
	private VInt3 size = VInt3.one;

	private VInt3 worldPos = VInt3.zero;

	private VInt3 worldExtends = VInt3.half;

	private int worldRadius;

	private VInt3[] axis = new VInt3[]
	{
		new VInt3(1000, 0, 0),
		new VInt3(0, 1000, 0),
		new VInt3(0, 0, 1000)
	};

	private VInt3 _tempDist;

	private long _tempRadius;

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
			this.dirty = true;
		}
	}

/*	[CollisionProperty]*/
	public VInt3 Size
	{
		get
		{
			return this.size;
		}
		set
		{
			this.size = value;
			this.dirty = true;
		}
	}

	public VInt3 WorldExtends
	{
		get
		{
			base.ConditionalUpdateShape();
			return this.worldExtends;
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

	public VInt3 AxisX
	{
		get
		{
			return this.axis[0];
		}
	}

	public VInt3 AxisY
	{
		get
		{
			return this.axis[1];
		}
	}

	public VInt3 AxisZ
	{
		get
		{
			return this.axis[2];
		}
	}

	public override int AvgCollisionRadius
	{
		get
		{
			base.ConditionalUpdateShape();
			int num = this.worldExtends.x + this.worldExtends.z;
			return num >> 1;
		}
	}

	public VCollisionBox()
	{
		this.isBox = true;
	}

	public override void UpdateShape(VInt3 location, VInt3 forward)
	{
		this.axis[2] = forward;
		this.axis[0] = VInt3.Cross(ref this.axis[1], ref this.axis[2]);
		this.worldPos = IntMath.Transform(ref this.localPos, ref this.axis[0], ref this.axis[1], ref this.axis[2], ref location);
		this.worldExtends.x = this.size.x >> 1;
		this.worldExtends.y = this.size.y >> 1;
		this.worldExtends.z = this.size.z >> 1;
		this.worldRadius = Mathf.Max(this.worldExtends.x, Mathf.Max(this.worldExtends.y, this.worldExtends.z));
		this.dirty = false;
	}

	public override void UpdateShape(VInt3 location, VInt3 forward, int moveDelta)
	{
		this.axis[2] = forward;
		this.axis[0] = VInt3.Cross(ref this.axis[1], ref this.axis[2]);
		location += forward.NormalizeTo(moveDelta >> 1);
		this.worldPos = IntMath.Transform(ref this.localPos, ref this.axis[0], ref this.axis[1], ref this.axis[2], ref location);
		this.worldExtends.x = this.size.x >> 1;
		this.worldExtends.y = this.size.y >> 1;
		this.worldExtends.z = this.size.z + moveDelta >> 1;
		this.worldRadius = Mathf.Max(this.worldExtends.x, Mathf.Max(this.worldExtends.y, this.worldExtends.z));
		this.dirty = false;
	}

	public override CollisionShapeType GetShapeType()
	{
		return CollisionShapeType.Box;
	}

	public bool IntersectsOBB(VCollisionBox b)
	{
		base.ConditionalUpdateShape();
		b.ConditionalUpdateShape();
		long num = (long)(this.worldRadius + b.worldRadius);
		if ((this.worldPos - b.worldPos).sqrMagnitudeLong > num * num)
		{
			return false;
		}
		VInt3 vInt = new VInt3(IntMath.Divide(VInt3.Dot(ref this.axis[0], ref b.axis[0]), 1000), IntMath.Divide(VInt3.Dot(ref this.axis[0], ref b.axis[1]), 1000), IntMath.Divide(VInt3.Dot(ref this.axis[0], ref b.axis[2]), 1000));
		VInt3 vInt2 = new VInt3(IntMath.Divide(VInt3.Dot(ref this.axis[1], ref b.axis[0]), 1000), IntMath.Divide(VInt3.Dot(ref this.axis[1], ref b.axis[1]), 1000), IntMath.Divide(VInt3.Dot(ref this.axis[1], ref b.axis[2]), 1000));
		VInt3 vInt3 = new VInt3(IntMath.Divide(VInt3.Dot(ref this.axis[2], ref b.axis[0]), 1000), IntMath.Divide(VInt3.Dot(ref this.axis[2], ref b.axis[1]), 1000), IntMath.Divide(VInt3.Dot(ref this.axis[2], ref b.axis[2]), 1000));
		VInt3 abs = vInt.abs;
		VInt3 abs2 = vInt2.abs;
		VInt3 abs3 = vInt3.abs;
		VInt3 vInt4 = b.worldPos - this.worldPos;
		vInt4 = new VInt3(IntMath.Divide(VInt3.Dot(ref vInt4, ref this.axis[0]), 1000), IntMath.Divide(VInt3.Dot(ref vInt4, ref this.axis[1]), 1000), IntMath.Divide(VInt3.Dot(ref vInt4, ref this.axis[2]), 1000));
		int num2 = this.worldExtends.x * 1000;
		int num3 = VInt3.Dot(ref b.worldExtends, ref abs);
		if (Mathf.Abs(vInt4.x) * 1000 > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.y * 1000;
		num3 = VInt3.Dot(ref b.worldExtends, ref abs2);
		if (Mathf.Abs(vInt4.y) * 1000 > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.z * 1000;
		num3 = VInt3.Dot(ref b.worldExtends, ref abs3);
		if (Mathf.Abs(vInt4.z) * 1000 > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.x * abs.x + this.worldExtends.y * abs2.x + this.worldExtends.z * abs3.x;
		num3 = b.worldExtends.x * 1000;
		if (Math.Abs(vInt4.x * vInt.x + vInt4.y * vInt2.x + vInt4.z * vInt3.x) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.x * abs.y + this.worldExtends.y * abs2.y + this.worldExtends.z * abs3.y;
		num3 = b.worldExtends.y * 1000;
		if (Math.Abs(vInt4.x * vInt.y + vInt4.y * vInt2.y + vInt4.z * vInt3.y) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.x * abs.z + this.worldExtends.y * abs2.z + this.worldExtends.z * abs3.z;
		num3 = b.worldExtends.z * 1000;
		if (Math.Abs(vInt4.x * vInt.z + vInt4.y * vInt2.z + vInt4.z * vInt3.z) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.y * abs3.x + this.worldExtends.z * abs2.x;
		num3 = b.worldExtends.y * abs.z + b.worldExtends.z * abs.y;
		if (Mathf.Abs(vInt4.z * vInt2.x - vInt4.y * vInt3.x) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.y * abs3.y + this.worldExtends.z * abs2.y;
		num3 = b.worldExtends.x * abs.z + b.worldExtends.z * abs.x;
		if (Mathf.Abs(vInt4.z * vInt2.y - vInt4.y * vInt3.y) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.y * abs3.z + this.worldExtends.z * abs2.z;
		num3 = b.worldExtends.x * abs.y + b.worldExtends.y * abs.x;
		if (Mathf.Abs(vInt4.z * vInt2.z - vInt4.y * vInt3.z) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.x * abs3.x + this.worldExtends.z * abs.x;
		num3 = b.worldExtends.y * abs2.z + b.worldExtends.z * abs2.y;
		if (Mathf.Abs(vInt4.x * vInt3.x - vInt4.z * vInt.x) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.x * abs3.y + this.worldExtends.z * abs.y;
		num3 = b.worldExtends.x * abs2.z + b.worldExtends.z * abs2.x;
		if (Mathf.Abs(vInt4.x * vInt3.y - vInt4.z * vInt.y) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.x * abs3.z + this.worldExtends.z * abs.z;
		num3 = b.worldExtends.x * abs2.y + b.worldExtends.y * abs2.x;
		if (Mathf.Abs(vInt4.x * vInt3.z - vInt4.z * vInt.z) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.x * abs2.x + this.worldExtends.y * abs.x;
		num3 = b.worldExtends.y * abs3.z + b.worldExtends.z * abs3.y;
		if (Mathf.Abs(vInt4.y * vInt.x - vInt4.x * vInt2.x) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.x * abs2.y + this.worldExtends.y * abs.y;
		num3 = b.worldExtends.x * abs3.z + b.worldExtends.z * abs3.x;
		if (Mathf.Abs(vInt4.y * vInt.y - vInt4.x * vInt2.y) > num2 + num3)
		{
			return false;
		}
		num2 = this.worldExtends.x * abs2.z + this.worldExtends.y * abs.z;
		num3 = b.worldExtends.x * abs3.y + b.worldExtends.y * abs3.x;
		return Mathf.Abs(vInt4.y * vInt.z - vInt4.x * vInt2.z) <= num2 + num3;
	}

	public override bool Intersects(VCollisionBox obb)
	{
		return this.IntersectsOBB(obb);
	}

	public override bool Intersects(VCollisionSphere s)
	{
		base.ConditionalUpdateShape();
		s.ConditionalUpdateShape();
		this._tempRadius = (long)(this.worldRadius + s.WorldRadius);
		this._tempDist = this.worldPos - s.WorldPos;
		if (this._tempDist.sqrMagnitudeLong > this._tempRadius * this._tempRadius)
		{
			return false;
		}
		VInt3 vInt = s.WorldPos;
		long num = (long)s.WorldRadius;
		VInt3 rhs = this.ClosestPoint(vInt);
		return (vInt - rhs).sqrMagnitudeLong <= num * num;
	}

	public override bool Intersects(VCollisionCylinderSector cs)
	{
		return cs.Intersects(this);
	}

	public VInt3 ClosestPoint(VInt3 targetPoint)
	{
		VInt3 vInt = targetPoint - this.worldPos;
		VInt3 lhs = this.worldPos;
		int num = this.worldExtends.x * 1000;
		int num2 = this.worldExtends.y * 1000;
		int num3 = this.worldExtends.z * 1000;
		lhs += IntMath.Divide(this.axis[0], (long)Mathf.Clamp(VInt3.Dot(ref vInt, ref this.axis[0]), -num, num), 1000000L);
		lhs += IntMath.Divide(this.axis[1], (long)Mathf.Clamp(VInt3.Dot(ref vInt, ref this.axis[1]), -num2, num2), 1000000L);
		return lhs + IntMath.Divide(this.axis[2], (long)Mathf.Clamp(VInt3.Dot(ref vInt, ref this.axis[2]), -num3, num3), 1000000L);
	}

	public override bool EdgeIntersects(VCollisionSphere s)
	{
		return false;
	}

	public override bool EdgeIntersects(VCollisionBox s)
	{
		return false;
	}

	public override bool EdgeIntersects(VCollisionCylinderSector cs)
	{
		return cs.EdgeIntersects(this);
	}

	public Vector3[] GetPoints()
	{
		Vector3[] array = new Vector3[]
		{
			new Vector3(-1f, 1f, -1f),
			new Vector3(1f, 1f, -1f),
			new Vector3(1f, 1f, 1f),
			new Vector3(-1f, 1f, 1f),
			new Vector3(-1f, -1f, -1f),
			new Vector3(1f, -1f, -1f),
			new Vector3(1f, -1f, 1f),
			new Vector3(-1f, -1f, 1f)
		};
        /*		Vector3 zero = Vector3.get_zero();*/
        Vector3 zero = Vector3.zero;
        Vector3 vector = (Vector3)this.worldExtends;
		Vector3 vector2 = (Vector3)this.worldPos;
		Vector3 vector3 = (Vector3)this.axis[0];
		Vector3 vector4 = (Vector3)this.axis[1];
		Vector3 vector5 = (Vector3)this.axis[2];
		for (int i = 0; i < 8; i++)
		{
			zero.x = array[i].x * vector.x;
			zero.y = array[i].y * vector.y;
			zero.z = array[i].z * vector.z;
			Vector3 vector6 = vector3 * zero.x;
			vector6 += vector4 * zero.y;
			vector6 += vector5 * zero.z;
			array[i] = vector6 + vector2;
		}
		return array;
	}

	public override void GetAabb2D(out VInt2 origin, out VInt2 size)
	{
		int num = this.worldRadius;
		origin.x = this.WorldPos.x - num;
		origin.y = this.WorldPos.z - num;
		size.x = num + num;
		size.y = size.x;
	}

// 	public override bool AcceptFowVisibilityCheck(COM_PLAYERCAMP inHostCamp, GameFowManager fowMgr)
// 	{
// 		return GameFowCollector.VisitFowVisibilityCheck(this, this.owner, inHostCamp, fowMgr);
// 	}
}
