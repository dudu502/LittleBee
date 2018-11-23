using System;

public interface IPooledMonoBehaviour
{
	void OnCreate();

	void OnGet();

	void OnRecycle();
}
