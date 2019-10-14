using System;
using System.Collections.Generic;
using Assets.Tool.Pools;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Pool
{

    public event Action<GameObject> OnSpawnedEvent;
    
    public event Action<GameObject> OnDespawnedEvent;

   
    public GameObject Prefab;

   
    public int InstancesToPreallocate = 5;

   
    public int InstancesToAllocateIfEmpty = 1;

   
    public bool ImposeHardLimit = false;

   
    public int HardLimit = 5;

   
    public bool CullExcessPrefabs = false;

   
    public int InstancesToMaintainInPool = 5;

  
    public float CullInterval = 10f;

    
    public bool AutomaticallyRecycleParticleSystems = false;

    
    public bool PersistBetweenScenes = false;


  
    Stack<GameObject> _gameObjectPool;

    
    float _timeOfLastCull = float.MinValue;

    
    int _spawnedInstanceCount = 0;


    #region Private
   
    private void AllocateGameObjects(int count)
    {
        if (ImposeHardLimit && _gameObjectPool.Count + count > HardLimit)
            count = HardLimit - _gameObjectPool.Count;

        for (int n = 0; n < count; n++)
        {
            GameObject go = GameObject.Instantiate(Prefab.gameObject) as GameObject;
            go.name = Prefab.name;

            if (go.transform as RectTransform)
                go.transform.SetParent(Pools.Instance.transform, false);
            else
                go.transform.parent = Pools.Instance.transform;

            go.SetActive(false);
            _gameObjectPool.Push(go);
        }
    }
   
    private GameObject Pop()
    {
        if (ImposeHardLimit && _spawnedInstanceCount >= HardLimit)
            return null;

        if (_gameObjectPool.Count > 0)
        {
            _spawnedInstanceCount++;
            return _gameObjectPool.Pop();
        }

        AllocateGameObjects(InstancesToAllocateIfEmpty);
        return Pop();
    }

    #endregion


    #region Public
  
    public void Initialize()
    {
        _gameObjectPool = new Stack<GameObject>(InstancesToPreallocate);
        AllocateGameObjects(InstancesToPreallocate);
    }

    public void CullExcessObjects()
    {
        if (!CullExcessPrefabs || _gameObjectPool.Count <= InstancesToMaintainInPool)
            return;

        if (Time.time > _timeOfLastCull + CullInterval)
        {
            _timeOfLastCull = Time.time;
            for (int n = InstancesToMaintainInPool; n <= _gameObjectPool.Count; n++)
                Object.Destroy(_gameObjectPool.Pop());
        }
    }
   
    public GameObject Spawn()
    {
        var go = Pop();

        if (go != null)
        {
            if (OnSpawnedEvent != null)
                OnSpawnedEvent(go);

            if (AutomaticallyRecycleParticleSystems)
            {
                var system = go.GetComponent<ParticleSystem>();
                if (system)
                {
                    Pools.DespawnAfterDelay(go, system.main.duration + system.main.startLifetimeMultiplier);
                }
                else
                {
                    Debug.LogError("automaticallyRecycleParticleSystems is true but there is no ParticleSystem on this GameObject!");
                }
            }
        }

        return go;
    }
   
    public void Despawn(GameObject go)
    {
        go.SetActive(false);

        _spawnedInstanceCount--;
        _gameObjectPool.Push(go);
        if (OnDespawnedEvent != null)
            OnDespawnedEvent(go);
    }
   
    public void ClearBin(bool shouldDestroyAllManagedObjects)
    {
        while (_gameObjectPool.Count > 0)
        {
            var go = _gameObjectPool.Pop();

            if (shouldDestroyAllManagedObjects)
                Object.Destroy(go);
        }
    }

    #endregion



}
