using System.Collections.Generic;
using System.Linq;
using Assets.Tool.Pools;
using UnityEngine;

public class VirusCureEffect : MonoBehaviour
{

    private List<VirusAddLevelEffect> _activeEffects;
    private Dictionary<ICure, bool> _cacheBools;
    private BaseVirus _baseVirus;
   
    private float _scaleX;
    private float _totalTime;
    private float _duration;

    protected bool IsActive
    {
        get
        {
            if (_cacheBools.Count == 0)
                return false;
            return _cacheBools.Aggregate(true, (current, item) => current && item.Value);
        }
    }


    private void Start()
    {
        _activeEffects = new List<VirusAddLevelEffect>();
        _cacheBools = new Dictionary<ICure, bool>();
        _duration = 0.25f;

        _baseVirus = transform.GetComponent<BaseVirus>();
    }

    private void OnEnable()
    {
        if (_activeEffects != null)
        {
            for (int i = 0; i < _activeEffects.Count; i++)
            {
                var item = _activeEffects[i];
                EffectPools.Instance.DeSpawn(item.gameObject);
                _activeEffects.RemoveAt(i);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < _activeEffects.Count; i++)
        {
            var item = _activeEffects[i];
            item.transform.localPosition += new Vector3(0, Time.deltaTime * 6, 0);
            float y = item.transform.localPosition.y;
            if (item.transform.localPosition.y <= 1.25f)
            {
                item.transform.localScale = new Vector3(_scaleX * y * 2.5f, _scaleX * y * 2.5f, 1);
            }
            else
            {
                y = 2.5f - y;
                item.transform.localScale = new Vector3(_scaleX * y * 2.5f, _scaleX * y * 2.5f, 1);
            }
            if (item.transform.localPosition.y > 2.5f)
            {
                EffectPools.Instance.DeSpawn(item.gameObject);
                _activeEffects.RemoveAt(i);
            }
        }

        if (IsActive)
        {
            _totalTime += Time.deltaTime;
            if (_totalTime > _duration)
            {
                _totalTime -= _duration;
                SpawnEffect();
            }
        }

    }

    private void SpawnEffect()
    {
        _scaleX = transform.localScale.x;
        var obj = EffectPools.Instance.Spawn("AddHealth");
        obj.transform.localScale = Vector3.zero;
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        var addEffect = obj.transform.GetComponent<VirusAddLevelEffect>();
        addEffect.Initi(_baseVirus.CurColorLevel);
        _activeEffects.Add(addEffect);
    }



    public void StartHealthEffect(ICure iCure)
    {
        if (!IsActive)
        {
            SpawnEffect();
        }

        if (!_cacheBools.ContainsKey(iCure))
            _cacheBools.Add(iCure, true);
    }


    public void StopHealthEffect(ICure iCure)
    {
        if (_cacheBools.ContainsKey(iCure))
        {
            _cacheBools.Remove(iCure);
        }
        if (!IsActive)
        {
            _totalTime = 0;
        }
    }



}