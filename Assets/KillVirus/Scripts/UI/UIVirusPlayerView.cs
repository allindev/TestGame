using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIVirusPlayerView : MonoBehaviour
    {


        [SerializeField]
        private float _offset;

        private Dictionary<VirusPropEnum, UIPropItem> _cacheObjects;
        private Dictionary<int, bool> _cacheBools;
        private Dictionary<VirusPropEnum, int> _propDictionary;


        public void OnAwake()
        {
            _cacheObjects = new Dictionary<VirusPropEnum, UIPropItem>();
            _cacheBools = new Dictionary<int, bool>();
            _propDictionary = new Dictionary<VirusPropEnum, int>();

            for (int i = 0; i < 9; i++)
            {
                _cacheBools.Add(i, false);
            }
        }

        public void UpdatePropItem(VirusPropEnum virusPropEnum, float t)
        {
            if (_cacheObjects.ContainsKey(virusPropEnum))
            {
                _cacheObjects[virusPropEnum].OnUpdate(t);
            }
        }


        public void Add(VirusPropEnum virusPropEnum)
        {
            if (_cacheObjects.ContainsKey(virusPropEnum))
            {
                _cacheObjects[virusPropEnum].Reiniti();
            }
            else
            {
                int index = GetEmptyIndex();

                GameObject obj = PropPools.Instance.Spawn("PropItem");
                var uipropItem = obj.GetComponent<UIPropItem>();
                uipropItem.Initi(virusPropEnum);

                obj.transform.SetParent(transform); 
                var rectTransform = obj.transform as RectTransform;
                if (rectTransform != null)
                    rectTransform.anchoredPosition = new Vector2(0, _offset * index);

                _cacheBools[index] = true;
                _propDictionary.Add(virusPropEnum, index);
                _cacheObjects.Add(virusPropEnum, uipropItem);
            }
        }


        public void Remove(VirusPropEnum propEnum)
        {
            int index = _propDictionary[propEnum];
            _cacheBools[index] = false;
            var obj = _cacheObjects[propEnum].gameObject;
            _cacheObjects.Remove(propEnum);
            _propDictionary.Remove(propEnum);
            PropPools.Instance.DeSpawn(obj);
        }


        private int GetEmptyIndex()
        {
            for (int i = 0; i < 9; i++)
            {
                if (!_cacheBools[i])
                {
                    return i;
                }
            }
            return 0;
        }



    }
}

