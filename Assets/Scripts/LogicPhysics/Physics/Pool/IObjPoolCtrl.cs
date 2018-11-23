using System;

namespace Assets.Scripts.Common
{
	public interface IObjPoolCtrl
	{
		void Release(PooledClassObject obj);
	}
}
