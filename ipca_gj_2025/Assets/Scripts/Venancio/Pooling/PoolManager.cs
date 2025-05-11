using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-9)]
public class PoolManager : Singleton<PoolManager>
{
    //[SerializeField] private TagPoolPair[] _poolsDictionary;
	//private Dictionary<string, Pool> _pools;



	private void Awake()
	{
		//SetupDictionaries();
		SingletonSetup();
	}



	//public Pool GetPool(string poolName)
	//{
	//	if (!_pools.ContainsKey(poolName))
	//	{
	//		Debug.LogWarning("PoolManager -> GetPool: No pool with name " + poolName + " found.", this);
	//		return null;
	//	}

	//	return _pools[poolName];
	//}

	//private void SetupDictionaries()
	//{
	//	_pools = new Dictionary<string, Pool>();

	//	foreach (TagPoolPair pair in _poolsDictionary)
	//	{
	//		_pools.Add(pair.Tag, pair.Pool);
	//	}
	//}
}


//public struct TagPoolPair
//{
//	public string Tag;
//	public Pool Pool;
//}
