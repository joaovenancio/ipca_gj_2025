using UnityEngine;

public interface IFactoryObject<T>
{
	public void OnRepurpose(T newObject);
}
