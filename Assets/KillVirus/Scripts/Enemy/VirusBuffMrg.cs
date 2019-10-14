using System.Collections.Generic;
using EnemyBuffs;
using UnityEngine;

public class VirusBuffMrg : MonoBehaviour
{

    private List<BaseVirusBuff> _buffs;
    private Dictionary<VirusPropEnum, BaseVirusBuff> _cache;

    private void Update()
    {
        for (int i = 0; i < _buffs.Count; i++)
        {
            _buffs[i].OnUpdate();
        }
    }


    public void Initi()
    {
        _buffs = new List<BaseVirusBuff>();
        _cache = new Dictionary<VirusPropEnum, BaseVirusBuff>();
    }


    public void AddBuff(BaseVirusBuff buff)
    {
        if (!_cache.ContainsKey(buff.VirusPropEnum))
        {
            _cache.Add(buff.VirusPropEnum, buff);
            _buffs.Add(buff);
        }
    }


    public void StopBuff(VirusPropEnum virusPropEnum)
    {
        if (_cache.ContainsKey(virusPropEnum))
        {
            var buff = _cache[virusPropEnum];
            buff.Stop();
        }
    }


    public void RemoveBuff(VirusPropEnum virusPropEnum)
    {
        if (_cache.ContainsKey(virusPropEnum))
        {
            var item = _cache[virusPropEnum];
            _buffs.Remove(item);
            _cache.Remove(virusPropEnum);
        }
    }

}