using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Tool.Pools
{

    [CustomEditor(typeof(Pools))]
    public class PoolsEditor : Editor
    {
        private List<bool> _prefabFoldouts;
        private Pools _poolsTarget;

        private GUIStyle _boxStyle;
        private GUIStyle boxStyle
        {
            get
            {
                if (_boxStyle == null)
                {
                    _boxStyle = new GUIStyle(GUI.skin.box);

                    var tex = new Texture2D(1, 1);
                    tex.hideFlags = HideFlags.HideAndDontSave;
                    tex.SetPixel(0, 0, Color.white);
                    tex.Apply();

                    _boxStyle.normal.background = tex;

                    tex = new Texture2D(1, 1);
                    tex.hideFlags = HideFlags.HideAndDontSave;
                    tex.SetPixel(0, 0, Color.green);
                    tex.Apply();

                    _boxStyle.hover.background = tex;

                    var p = _boxStyle.padding;
                    p.left = p.right = 20;
                    p.top = 6;
                    _boxStyle.padding = p;

                    _boxStyle.fontSize = 15;
                }

                return _boxStyle;
            }
        }


        private GUIStyle _binStyleEven;
        private GUIStyle binStyleEven
        {
            get
            {
                if (_binStyleEven == null)
                {
                    _binStyleEven = new GUIStyle(GUI.skin.box);

                    var tex = new Texture2D(1, 1);
                    tex.hideFlags = HideFlags.HideAndDontSave;
                    tex.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.2f));
                    tex.Apply();

                    _binStyleEven.normal.background = tex;
                    var p = _binStyleEven.padding;
                    p.left = 14;
                    _binStyleEven.padding = p;
                }

                return _binStyleEven;
            }
        }


        private GUIStyle _binStyleOdd;
        private GUIStyle binStyleOdd
        {
            get
            {
                if (_binStyleOdd == null)
                {
                    _binStyleOdd = new GUIStyle(GUI.skin.box);

                    var tex = new Texture2D(1, 1);
                    tex.hideFlags = HideFlags.HideAndDontSave;
                    tex.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.1f));
                    tex.Apply();

                    _binStyleOdd.normal.background = tex;
                    var p = _binStyleEven.padding;
                    p.left = 14;
                    _binStyleOdd.padding = p;
                }

                return _binStyleOdd;
            }
        }


        private GUIStyle _buttonStyle;
        private GUIStyle buttonStyle
        {
            get
            {
                if (_buttonStyle == null)
                {
                    _buttonStyle = new GUIStyle(GUI.skin.button);

                    var normalTex = new Texture2D(1, 1);
                    normalTex.hideFlags = HideFlags.HideAndDontSave;
                    normalTex.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.8f));
                    normalTex.Apply();

                    var activeTex = new Texture2D(1, 1);
                    activeTex.hideFlags = HideFlags.HideAndDontSave;
                    activeTex.SetPixel(0, 0, Color.yellow);
                    activeTex.Apply();

                    var hoverTex = new Texture2D(1, 1);
                    hoverTex.hideFlags = HideFlags.HideAndDontSave;
                    hoverTex.SetPixel(0, 0, Color.red);
                    hoverTex.Apply();

                    _buttonStyle.normal.background = normalTex;
                    _buttonStyle.hover.background = hoverTex;
                    _buttonStyle.active.background = activeTex;

                    _buttonStyle.normal.textColor = Color.black;
                }

                return _buttonStyle;
            }
        }


        #region Methods

        public void OnEnable()
        {
            _poolsTarget = target as Pools;
            _poolsTarget.recycleBinCollection = (target as Pools).recycleBinCollection;

            _prefabFoldouts = new List<bool>();
            if (_poolsTarget.recycleBinCollection != null)
                for (int n = 0; n < _poolsTarget.recycleBinCollection.Count; n++)
                    _prefabFoldouts.Add(true);

            clearNullReferences();
        }


        void OnDisable()
        {
            if (_boxStyle != null)
            {
                DestroyImmediate(_boxStyle.normal.background);
                DestroyImmediate(_boxStyle.hover.background);
            }

            if (_binStyleEven != null)
                DestroyImmediate(_binStyleEven.normal.background);

            if (_binStyleOdd != null)
                DestroyImmediate(_binStyleOdd.normal.background);

            if (_buttonStyle != null)
            {
                DestroyImmediate(_buttonStyle.normal.background);
                DestroyImmediate(_buttonStyle.hover.background);
                DestroyImmediate(_buttonStyle.active.background);
            }

            _boxStyle = null;
            _binStyleEven = null;
            _binStyleOdd = null;
            _buttonStyle = null;
        }


        /// <summary>
        /// null checks the TrashMans List and removes any cans with no prefab found. This will clear out any GameObjects
        /// from the scene that no longer exist
        /// </summary>
        private void clearNullReferences()
        {
            if (_poolsTarget.recycleBinCollection == null)
                return;

            int n = 0;
            while (n < _poolsTarget.recycleBinCollection.Count)
            {
                if (_poolsTarget.recycleBinCollection[n].Prefab == null)
                    _poolsTarget.recycleBinCollection.RemoveAt(_poolsTarget.recycleBinCollection.Count - 1);
                else
                    n++;
            }
        }


        /// <summary>
        /// adds a new bin to the TrashMan collection
        /// </summary>
        /// <param name="go">Go.</param>
        private void addRecycleBin(GameObject go)
        {
            if (_poolsTarget.recycleBinCollection == null)
                _poolsTarget.recycleBinCollection = new List<Pool>();

            if (_poolsTarget.recycleBinCollection != null)
            {
                foreach (var recycleBin in _poolsTarget.recycleBinCollection)
                {
                    if (recycleBin.Prefab.gameObject.name == go.name)
                    {
                        EditorUtility.DisplayDialog("Pools", "Pools already manages a GameObject with the name '" + go.name + "'.\n\nIf you are attempting to manage multiple GameObjects sharing the same name, you will need to first give them unique names.", "OK");
                        return;
                    }
                }
            }

            var newPrefabPool = new Pool();
            newPrefabPool.Prefab = go;

            _poolsTarget.recycleBinCollection.Add(newPrefabPool);
            while (_poolsTarget.recycleBinCollection.Count > _prefabFoldouts.Count)
                _prefabFoldouts.Add(false);
        }


        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                if (_prefabFoldouts.Count < _poolsTarget.recycleBinCollection.Count)
                {
                    for (var i = 0; i < _poolsTarget.recycleBinCollection.Count - _prefabFoldouts.Count; i++)
                        _prefabFoldouts.Add(false);
                }
                //base.OnInspectorGUI();
                //return;
            }

            base.OnInspectorGUI();

            GUILayout.Space(15f);
            dropAreaGUI();

            if (_poolsTarget.recycleBinCollection == null)
                return;

            GUILayout.Space(5f);
            GUILayout.Label("Pools", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();

            for (int n = 0; n < _poolsTarget.recycleBinCollection.Count; n++)
            {
                var prefabPool = _poolsTarget.recycleBinCollection[n];

                // wrapper vertical allows us to style each element
                EditorGUILayout.BeginVertical(n % 2 == 0 ? binStyleEven : binStyleOdd);

                // PrefabPool DropDown
                EditorGUILayout.BeginHorizontal();
                _prefabFoldouts[n] = EditorGUILayout.Foldout(_prefabFoldouts[n], prefabPool.Prefab.name, EditorStyles.foldout);
                if (GUILayout.Button("X", buttonStyle, GUILayout.Width(20f), GUILayout.Height(15f)) && EditorUtility.DisplayDialog("Remove Recycle Bin", "Are you sure you want to remove this recycle bin?", "Yes", "Cancel"))
                    _poolsTarget.recycleBinCollection.RemoveAt(n);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);

                if (_prefabFoldouts[n])
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10f);
                    EditorGUILayout.BeginVertical();

                    // PreAlloc
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Preallocate Count", "Total items to create at scene start"), EditorStyles.label, GUILayout.Width(115f));
                    prefabPool.InstancesToPreallocate = EditorGUILayout.IntField(prefabPool.InstancesToPreallocate);
                    if (prefabPool.InstancesToPreallocate < 0)
                        prefabPool.InstancesToPreallocate = 0;
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.BeginDisabledGroup(prefabPool.ImposeHardLimit);
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("Allocate Block Count", "Once the bin limit is reached, this is how many new objects will be created as necessary"), EditorStyles.label, GUILayout.Width(115f));
                        prefabPool.InstancesToAllocateIfEmpty = EditorGUILayout.IntField(prefabPool.InstancesToAllocateIfEmpty);
                        if (prefabPool.InstancesToAllocateIfEmpty < 1)
                            prefabPool.InstancesToAllocateIfEmpty = 1;
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.EndDisabledGroup();


                    // automaticallyRecycleParticleSystems
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Recycle ParticleSystems", "If true, the GameObject must contain a ParticleSystem! It will be automatically despawned after system.duration."), EditorStyles.label, GUILayout.Width(115f));
                    prefabPool.AutomaticallyRecycleParticleSystems = EditorGUILayout.Toggle(prefabPool.AutomaticallyRecycleParticleSystems);
                    EditorGUILayout.EndHorizontal();


                    // HardLimit
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Enable Hard Limit ", "If true, the bin will return null if a new item is requested and the Limit was reached"), EditorStyles.label, GUILayout.Width(115f));
                    prefabPool.ImposeHardLimit = EditorGUILayout.Toggle(prefabPool.ImposeHardLimit);
                    EditorGUILayout.EndHorizontal();

                    if (prefabPool.ImposeHardLimit)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20f);
                        GUILayout.Label(new GUIContent("Limit", "Max number of items allowed in the bin when Hard Limit is true"), EditorStyles.label, GUILayout.Width(100f));
                        prefabPool.HardLimit = EditorGUILayout.IntField(prefabPool.HardLimit);
                        if (prefabPool.HardLimit < 1)
                            prefabPool.HardLimit = 1;
                        EditorGUILayout.EndHorizontal();
                    }


                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Enable Culling", "If true, items in excess of Cull Above will be destroyed automatically"), EditorStyles.label, GUILayout.Width(115f));
                    prefabPool.CullExcessPrefabs = EditorGUILayout.Toggle(prefabPool.CullExcessPrefabs);
                    EditorGUILayout.EndHorizontal();


                    if (prefabPool.CullExcessPrefabs)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20f);
                        GUILayout.Label(new GUIContent("Cull Above", "Max number of items to allow. If item count exceeds this they will be culled"), EditorStyles.label, GUILayout.Width(100f));
                        prefabPool.InstancesToMaintainInPool = EditorGUILayout.IntField(prefabPool.InstancesToMaintainInPool);
                        if (prefabPool.InstancesToMaintainInPool < 0)
                            prefabPool.InstancesToMaintainInPool = 0;
                        if (prefabPool.ImposeHardLimit && prefabPool.InstancesToMaintainInPool > prefabPool.HardLimit)
                            prefabPool.InstancesToMaintainInPool = prefabPool.HardLimit;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20f);
                        GUILayout.Label(new GUIContent("Cull Delay", "Duration in seconds between cull checks. Note that the master cull check only occurs every 5 seconds"), EditorStyles.label, GUILayout.Width(100f));
                        prefabPool.CullInterval = EditorGUILayout.FloatField(prefabPool.CullInterval);
                        if (prefabPool.CullInterval < 0)
                            prefabPool.CullInterval = 0;
                        EditorGUILayout.EndHorizontal();
                    }

                   

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }


                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }


        private void dropAreaGUI()
        {
            var evt = UnityEngine.Event.current;
            var dropArea = GUILayoutUtility.GetRect(0f, 60f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drop a Prefab or GameObject here to create a new can in your Pools", boxStyle);

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    {
                        if (!dropArea.Contains(evt.mousePosition))
                            break;

                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                        if (evt.type == EventType.DragPerform)
                        {
                            DragAndDrop.AcceptDrag();
                            foreach (var draggedObject in DragAndDrop.objectReferences)
                            {
                                var go = draggedObject as GameObject;
                                if (!go)
                                    continue;

                                // TODO: perhaps we should only allow prefabs or perhaps allow GO's in the scene as well?
                                // uncomment to allow only prefabs
                                //						if( PrefabUtility.GetPrefabType( go ) == PrefabType.None )
                                //						{
                                //							EditorUtility.DisplayDialog( "Trash Man", "Trash Man cannot manage the object '" + go.name + "' as it is not a prefab.", "OK" );
                                //							continue;
                                //						}

                                addRecycleBin(go);
                            }
                        }

                        UnityEngine.Event.current.Use();
                        break;
                    } // end DragPerform
            } // end switch
        }

        #endregion

    }

}
