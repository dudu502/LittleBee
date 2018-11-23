using System;

namespace Assets.Scripts.Common
{
	public struct PoolObjHandle<T> : IEquatable<PoolObjHandle<T>> where T : PooledClassObject
	{
		public uint _handleSeq;

		public T _handleObj;

		public T handle
		{
			get
			{
				return this._handleObj;
			}
		}

		public PoolObjHandle(T obj)
		{
			if (obj != null && obj.usingSeq > 0u)
			{
				this._handleSeq = obj.usingSeq;
				this._handleObj = obj;
			}
			else
			{
				this._handleSeq = 0u;
				this._handleObj = (T)((object)null);
			}
		}

		public void Validate()
		{
			this._handleSeq = ((this._handleObj == null) ? 0u : this._handleObj.usingSeq);
		}

		public void Release()
		{
			this._handleObj = (T)((object)null);
			this._handleSeq = 0u;
		}

		public bool Equals(PoolObjHandle<T> other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return obj != null && base.GetType() == obj.GetType() && this == (PoolObjHandle<T>)obj;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static implicit operator bool(PoolObjHandle<T> ptr)
		{
			return ptr._handleObj != null && ptr._handleObj.usingSeq == ptr._handleSeq;
		}

		public static bool operator ==(PoolObjHandle<T> lhs, PoolObjHandle<T> rhs)
		{
			return lhs._handleObj == rhs._handleObj && lhs._handleSeq == rhs._handleSeq;
		}

		public static bool operator !=(PoolObjHandle<T> lhs, PoolObjHandle<T> rhs)
		{
			return lhs._handleObj != rhs._handleObj || lhs._handleSeq != rhs._handleSeq;
		}

		public static implicit operator T(PoolObjHandle<T> ptr)
		{
			return ptr.handle;
		}
	}
}
