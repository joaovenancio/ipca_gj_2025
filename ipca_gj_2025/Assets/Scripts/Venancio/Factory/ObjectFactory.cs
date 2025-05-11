using UnityEngine;

/// <summary>
/// A generic factory class for creating and managing objects that implement the IPoolable and IFactoryObject interfaces.
/// </summary>
/// <typeparam name="T">The type of objects to be created and managed, which must be a MonoBehaviour and implement IPoolable and IFactoryObject.</typeparam>
public class ObjectFactory<T> where T : MonoBehaviour, IPoolable, IFactoryObject<T>
{
	private bool _isActiveByDefaultOnCreate = true;
	private Pool<T> _pool;
	private int _startingCapacity;
	private GameObject _defaultPrefab; //TODO: Implement this
	private Transform _objectsContainer;

	/// <summary>
	/// Gets the starting capacity of the pool.
	/// </summary>
	public int StartingCapacity { get => _startingCapacity; }
	/// <summary>
	/// Gets or sets the default prefab used by the factory when setting up the starting objects defined by StartingCapacity.
	/// </summary>
	public GameObject DefaultPrefab { get => _defaultPrefab; set => _defaultPrefab = value; }
	/// <summary>
	/// Gets or sets the container for the objects created by the factory.
	/// </summary>
	public Transform ObjectsContainer { get => _objectsContainer; set => _objectsContainer = value; }



	#region Constructors
	public ObjectFactory()
	{
		_objectsContainer = null;
		Pool<T> _pool = new Pool<T>(_objectsContainer);
		_defaultPrefab = null;
		_isActiveByDefaultOnCreate = true;
	}

	public ObjectFactory(bool isActiveByDefaultOnCreate)
	{
		_objectsContainer = null;
		_pool = new Pool<T>(_objectsContainer);
		_defaultPrefab = null;
		_isActiveByDefaultOnCreate = isActiveByDefaultOnCreate;
	}

	public ObjectFactory(Transform objectsContainer)
	{
		_objectsContainer = objectsContainer;
		_pool = new Pool<T>(_objectsContainer);
		_defaultPrefab = null;
		_isActiveByDefaultOnCreate = true;
	}

	public ObjectFactory(bool isActiveByDefaultOnCreate, GameObject defaultPrefab)
	{
		_objectsContainer = null;
		_pool = new Pool<T>(_objectsContainer);
		_defaultPrefab = defaultPrefab;
		_isActiveByDefaultOnCreate = isActiveByDefaultOnCreate;
	}

	public ObjectFactory(bool isActiveByDefaultOnCreate, Transform objectsContainer )
	{
		_objectsContainer = objectsContainer;
		_pool = new Pool<T>(_objectsContainer);
		_defaultPrefab = null;
		_isActiveByDefaultOnCreate = isActiveByDefaultOnCreate;
	}

	public ObjectFactory(bool isActiveByDefaultOnCreate, Transform objectsContainer, GameObject defaultPrefab)
	{
		_objectsContainer = objectsContainer;
		_pool = new Pool<T>(_objectsContainer);
		_defaultPrefab = defaultPrefab;
		_isActiveByDefaultOnCreate = isActiveByDefaultOnCreate;
	}
	#endregion



	/// <summary>
	/// Creates an object from the pool or instantiates a new one if the pool is empty.
	/// </summary>
	/// <param name="objectToCreate">The object to create or retrieve from the pool.</param>
	/// <returns>The created or retrieved object.</returns>
	public T Create(T objectToCreate)
	{
		T poolObject = _pool.Retrieve(false);

		if (poolObject)
		{
			poolObject.OnRepurpose(objectToCreate);
		}
		else
		{
			poolObject = GameObject.Instantiate(objectToCreate.gameObject).GetComponent<T>();
			_pool.Add(poolObject);
		}

		poolObject.gameObject.SetActive(_isActiveByDefaultOnCreate);

		return poolObject;
	}

	/// <summary>
	/// Creates an object from the pool or instantiates a new one if the pool is empty, with specified position preferences.
	/// </summary>
	/// <param name="objectToCreate">The object to create or retrieve from the pool.</param>
	/// <param name="preferences">The position preferences for the created or retrieved object.</param>
	/// <returns>The created or retrieved object.</returns>
	public T Create(T objectToCreate, PositionPreferences preferences)
	{
		T poolObject = _pool.Retrieve(false);

		if (poolObject)
		{
			poolObject.OnRepurpose(objectToCreate);
		}
		else
		{
			poolObject = GameObject.Instantiate(objectToCreate.gameObject).GetComponent<T>();
			_pool.Add(poolObject);
		}

		poolObject.transform.SetParent(preferences.Container);
		if (preferences.IsLocalPosition) poolObject.transform.localPosition = preferences.Position;
		else poolObject.transform.position = preferences.Position;

		poolObject.gameObject.SetActive(_isActiveByDefaultOnCreate);

		return poolObject;
	}

	/// <summary>
	/// Destroys an object by returning it to the pool.
	/// </summary>
	/// <param name="objectToDestroy">The object to be destroyed.</param>
	public void Destroy(T objectToDestroy)
	{
		_pool.Free(objectToDestroy);
	}
}
