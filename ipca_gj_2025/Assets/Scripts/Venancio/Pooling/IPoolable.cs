using UnityEngine;

public interface IPoolable
{
    public GameObject MyInstance { get;}

	public void OnFree();
    public void OnRetrieve();
}
