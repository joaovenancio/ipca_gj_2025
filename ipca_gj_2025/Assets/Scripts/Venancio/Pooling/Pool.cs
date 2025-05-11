using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic pool class for managing reusable objects that implement the IPoolable interface.
/// </summary>
/// <typeparam name="T">The type of objects to be pooled, which must be a MonoBehaviour and implement IPoolable.
/// </typeparam>
public class Pool<T> where T : MonoBehaviour, IPoolable
{
	private Queue<T> _freeObjects;
	private HashSet<T> _usedObjects;
	private Transform _objectsContainer;

	public Transform ObjectsContainer { get => _objectsContainer; }



	public Pool(Transform poolHolder)
	{
		if (!_objectsContainer) _objectsContainer = new GameObject(typeof(T).Name + "Pool").transform;
		_objectsContainer.SetParent(poolHolder);
		_objectsContainer.localPosition = Vector3.zero;

		_freeObjects = new Queue<T>();
		_usedObjects = new HashSet<T>();
	}



	/// <summary>
	/// Frees a poolable object, deactivates it, and returns it to the pool.
	/// </summary>
	/// <param name="poolableObject">The object to be freed and returned to the pool.</param>
	public void Free(T poolableObject)
	{
		poolableObject.gameObject.SetActive(false);
		poolableObject.OnFree();
		poolableObject.transform.SetParent(_objectsContainer);

		_usedObjects.Remove(poolableObject);

		_freeObjects.Enqueue(poolableObject);
	}

	/// <summary>
	/// Retrieves a poolable object from the pool and activates it if specified.
	/// </summary>
	/// <param name="isEnabled">Whether the retrieved object should be activated.</param>
	/// <returns>The retrieved poolable object, or null if the pool is empty.</returns>
	public T Retrieve(bool isEnabled)
	{
		if (_freeObjects.Count == 0) return null;

		T poolableObject = _freeObjects.Dequeue();

		poolableObject.OnRetrieve();
		poolableObject.gameObject.SetActive(isEnabled);

		_usedObjects.Add(poolableObject);

		return poolableObject;
	}

	/// <summary>
	/// Retrieves a poolable object from the pool and activates it.
	/// </summary>
	/// <returns>The retrieved poolable object, or null if the pool is empty.</returns>
	public T Retrieve()
	{
		if (_freeObjects.Count == 0) return null;

		T poolableObject = _freeObjects.Dequeue();

		poolableObject.OnRetrieve();
		poolableObject.gameObject.SetActive(true);

		_usedObjects.Add(poolableObject);

		return poolableObject;
	}

	/// <summary>
	/// Adds a new object to the pool.
	/// </summary>
	/// <param name="objectToAdd">The object to be added to the pool.</param>
	public void Add(T objectToAdd)
	{
		_usedObjects.Add(objectToAdd);
	}
}
