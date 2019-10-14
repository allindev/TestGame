using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Tool.Pools
{
    public class Pools : Singleton<Pools>
    {
      
        [HideInInspector] public List<Pool> recycleBinCollection;

        [SerializeField] private float cullExcessObjectsInterval = 10f;
        [SerializeField] private bool persistBetweenScenes;


        private Dictionary<int, Pool> _instanceIdToRecycleBin = new Dictionary<int, Pool>();
        private Dictionary<string, int> _poolNameToInstanceId = new Dictionary<string, int>();
        [HideInInspector] public Transform Transform;


        #region MonoBehaviour

        private void Awake()
        {

            Transform = gameObject.transform;
            InitializePrefabPools();

            PersistBetweenScenes = persistBetweenScenes;
            if (PersistBetweenScenes)
                DontDestroyOnLoad(gameObject);
            if (cullExcessObjectsInterval > 0)
                StartCoroutine(CullExcessObjects());
            //SceneManager.activeSceneChanged += ActiveSceneChanged;
        }


        private void ActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (oldScene.name == null)
                return;

            for (var i = recycleBinCollection.Count - 1; i >= 0; i--)
            {
                if (!recycleBinCollection[i].PersistBetweenScenes)
                    RemoveRecycleBin(recycleBinCollection[i]);
            }
        }

       

        #endregion

        #region Private

        private IEnumerator CullExcessObjects()
        {
            var waiter = new WaitForSeconds(cullExcessObjectsInterval);

            while (true)
            {
                for (var i = 0; i < recycleBinCollection.Count; i++)
                    recycleBinCollection[i].CullExcessObjects();

                yield return waiter;
            }
        }

        private void InitializePrefabPools()
        {
            if (recycleBinCollection == null)
                return;

            foreach (var recycleBin in recycleBinCollection)
            {
                if (recycleBin == null || recycleBin.Prefab == null)
                    continue;

                recycleBin.Initialize();
                _instanceIdToRecycleBin.Add(recycleBin.Prefab.GetInstanceID(), recycleBin);
                _poolNameToInstanceId.Add(recycleBin.Prefab.name, recycleBin.Prefab.GetInstanceID());
            }
        }
       
        private static GameObject Spawn(int gameObjectInstanceId, Vector3 position, Quaternion rotation)
        {
            if (Instance._instanceIdToRecycleBin.ContainsKey(gameObjectInstanceId))
            {
                var newGo = Instance._instanceIdToRecycleBin[gameObjectInstanceId].Spawn();

                if (newGo != null)
                {
                    var newTransform = newGo.transform;

                    if (newTransform as RectTransform)
                        newTransform.SetParent(null, false);
                    else
                        newTransform.parent = null;

                    newTransform.position = position;
                    newTransform.rotation = rotation;

                    newGo.SetActive(true);
                }

                return newGo;
            }

            return null;
        }
       
        private IEnumerator internalDespawnAfterDelay(GameObject go, float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            Despawn(go);
        }

        #endregion

        #region Public
        
        public static void ManageRecycleBin(Pool pool)
        {
            // make sure we can safely add the bin!
            if (Instance._poolNameToInstanceId.ContainsKey(pool.Prefab.name))
            {
                Debug.LogError("Cannot manage the recycle bin because there is already a GameObject with the name (" +
                               pool.Prefab.name + ") being managed");
                return;
            }

            Instance.recycleBinCollection.Add(pool);
            pool.Initialize();
            Instance._instanceIdToRecycleBin.Add(pool.Prefab.GetInstanceID(), pool);
            Instance._poolNameToInstanceId.Add(pool.Prefab.name, pool.Prefab.GetInstanceID());
        }

        public static void RemoveRecycleBin(Pool recycleBin, bool shouldDestroyAllManagedObjects = true)
        {
            var recycleBinName = recycleBin.Prefab.name;

            if (Instance._poolNameToInstanceId.ContainsKey(recycleBinName))
            {
                Instance._poolNameToInstanceId.Remove(recycleBinName);
                Instance._instanceIdToRecycleBin.Remove(recycleBin.Prefab.GetInstanceID());
                Instance.recycleBinCollection.Remove(recycleBin);
                recycleBin.ClearBin(shouldDestroyAllManagedObjects);
            }
        }
       
        public static GameObject Spawn(GameObject go, Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion))
        {
            if (Instance._instanceIdToRecycleBin.ContainsKey(go.GetInstanceID()))
            {
                return Spawn(go.GetInstanceID(), position, rotation);
            }
            else
            {
                Debug.LogWarning("attempted to spawn go (" + go.name +
                                 ") but there is no recycle bin setup for it. Falling back to Instantiate");
                var newGo = GameObject.Instantiate(go, position, rotation) as GameObject;
                if (newGo.transform is RectTransform)
                    newGo.transform.SetParent(null, false);
                else
                    newGo.transform.parent = null;

                return newGo;
            }
        }
        
        public static GameObject Spawn(string gameObjectName, Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion))
        {
            int instanceId = -1;
            if (Instance._poolNameToInstanceId.TryGetValue(gameObjectName, out instanceId))
            {
                return Spawn(instanceId, position, rotation);
            }
            else
            {
                Debug.LogError("attempted to spawn a GameObject from recycle bin (" + gameObjectName +
                               ") but there is no recycle bin setup for it");
                return null;
            }
        }
        
        public static void Despawn(GameObject go)
        {
            if (go == null)
                return;

            var goName = go.name;
            if (!Instance._poolNameToInstanceId.ContainsKey(goName))
            {
                Destroy(go);
            }
            else
            {
                Instance._instanceIdToRecycleBin[Instance._poolNameToInstanceId[goName]].Despawn(go);

                if (go.transform is RectTransform)
                    go.transform.SetParent(Instance.transform, false);
                else
                    go.transform.parent = Instance.transform;
            }
        }
       
        public static void DespawnAfterDelay(GameObject go, float delayInSeconds)
        {
            if (go == null)
                return;

            Instance.StartCoroutine(Instance.internalDespawnAfterDelay(go, delayInSeconds));
        }
      
        public static Pool RecycleBinForGameObjectName(string gameObjectName)
        {
            if (Instance._poolNameToInstanceId.ContainsKey(gameObjectName))
            {
                var instanceId = Instance._poolNameToInstanceId[gameObjectName];
                return Instance._instanceIdToRecycleBin[instanceId];
            }
            return null;
        }
      
        public static Pool RecycleBinForGameObject(GameObject go)
        {
            Pool recycleBin;
            if (Instance._instanceIdToRecycleBin.TryGetValue(go.GetInstanceID(), out recycleBin))
                return recycleBin;
            return null;
        }

        #endregion


    }
}
