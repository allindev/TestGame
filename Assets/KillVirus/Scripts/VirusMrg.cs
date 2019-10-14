using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Tool.Pools;
using EnemyBuffs;
using Events;
using Tool;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class VirusMrg : Singleton<VirusMrg>
{
  
    private List<GameObject> _virusObjects;
    private int _minOrderIndex;
    private int _maxOrderIndex;


    private List<VirusBuffMrg> _virusBuff1Mrgs;
    private List<VirusBuffMrg> _virusBuff2Mrgs;
    private List<VirusBuffMrg> _virusBuff3Mrgs;


    private int _originCount;
    private int _curCount;
    private RectiveProperty<float> _percent;

    private bool _isTiped;
    private bool _isSpawned;

    
    private void Awake()
    {
        _virusObjects = new List<GameObject>();
        _virusBuff1Mrgs = new List<VirusBuffMrg>();
        _virusBuff2Mrgs = new List<VirusBuffMrg>();
        _virusBuff3Mrgs = new List<VirusBuffMrg>();

        _percent = new RectiveProperty<float>();
    }

    private IEnumerator Spawn()
    {
        List<WaveVirusItem> items = WaveVirusDataAdapter.GetWaveVirus();

        int count = items.Count;
        int cc = 0;
        _isSpawned = false;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 0.8f));
            VirusData data = new VirusData();
            Vector3 dir = Quaternion.Euler(0, 0, Random.Range(-20f, 20f)) * Vector3.down;
            SplitLevel splitLevel = items[cc].SplitLevel;
            string virusName = items[cc].VirusName.ToString();

            int level = VirusGameDataAdapter.GetLevel();
            data.VirusColorLevel = items[cc].ColorLevel;
            data.HealthValue = VirusTool.GetVirusHealthByColorLevel(virusName, level, data.VirusColorLevel);
            data.MoveSpeed = 3;
            data.MoveDirection = dir;
            data.SplitLevel = splitLevel;
            SpawnVirus(virusName, data);
           
            cc++;
            if (cc >= count)
            {
                _isSpawned = true;
                WaveVirusDataAdapter.WaveIndex++;
                if (WaveVirusDataAdapter.WaveIndex >= 2)
                    WaveVirusDataAdapter.IsNextWave = false;
                yield break;
            }
        }
      
    }

    private void SpawnVirus(string virusName, VirusData data)
    {
        var obj = Pools.Spawn(virusName);
        obj.transform.localScale = Vector3.one;
        var virus = obj.transform.GetComponent<BaseVirus>();
        var virusMove = obj.transform.GetComponent<VirusMove>();
        if (!virusName.Equals("AdsorptionVirus"))
        {
            obj.transform.GetComponent<VirusSpriteSetter>().Set(_minOrderIndex);
            obj.GetComponent<BubbleEffectMrg>().Initi(_minOrderIndex, data.SplitLevel, data.VirusColorLevel);
            _minOrderIndex++;
        }
        else
        {
            obj.transform.GetComponent<VirusSpriteSetter>().Set(_maxOrderIndex);
            obj.GetComponent<BubbleEffectMrg>().Initi(_minOrderIndex, data.SplitLevel, data.VirusColorLevel);
            _maxOrderIndex--;
        }

        virus.Born(data);
       
        float minX = VirusTool.LeftX + virusMove.Radius / 2;
        float maxX = VirusTool.RightX - virusMove.Radius / 2;
        float x = Random.Range(minX, maxX);
        float y = VirusTool.TopY + virusMove.Radius / 2;
        obj.transform.position = new Vector3(x, y, 0);

        _virusObjects.Add(obj);
    }



    #region Public Method

    public void GameStart()
    {
        _minOrderIndex = 0;
        _maxOrderIndex = 5000;
        _percent.Value = 1;
        _curCount = 0;
        _isTiped = false;

        _virusObjects = new List<GameObject>();
        _originCount = WaveVirusDataAdapter.Load(VirusGameDataAdapter.GetLevel());
        StartCoroutine(Spawn());
    }


    public void BindView(UIMainPanel mainPanel)
    {
        _percent.Subscibe(mainPanel.SetLeftVirus);
    }


    public GameObject SpawnVirus(string virusName, VirusData data, Vector3 pos, bool isAdd)
    {
        var obj = Pools.Spawn(virusName);
        var virus = obj.transform.GetComponent<BaseVirus>();
        if (!virusName.Equals("AdsorptionVirus"))
        {
            obj.transform.GetComponent<VirusSpriteSetter>().Set(_minOrderIndex);
            obj.GetComponent<BubbleEffectMrg>().Initi(_minOrderIndex, data.SplitLevel, data.VirusColorLevel);
            _minOrderIndex++;
        }
        else
        {
            obj.transform.GetComponent<VirusSpriteSetter>().Set(_maxOrderIndex);
            obj.GetComponent<BubbleEffectMrg>().Initi(_minOrderIndex, data.SplitLevel, data.VirusColorLevel);
            _maxOrderIndex--;
        }
        virus.Born(data);
        obj.transform.position = pos;
        if (isAdd)
        {
            var n = (VirusName)Enum.Parse(typeof(VirusName), virusName);
            _originCount += VirusTool.GetChildSplit(data.SplitLevel, n);
        }

        _virusObjects.Add(obj);
        return obj;
    }


    public void Remove(GameObject virus)
    {
        if (_virusObjects.Contains(virus))
        {
            _virusObjects.Remove(virus);
            _curCount++;
            float v = 1 - _curCount * 1.0f / _originCount;
            if (_percent.Value > v)
            {
                _percent.Value = v;
            }
        }
        else
        {
            virus.SetActive(true);
            virus.transform.localScale = Vector3.one * 5;
        }
       
        var buff = virus.GetComponent<VirusBuffMrg>();
        if (_virusBuff1Mrgs.Contains(buff))
            _virusBuff1Mrgs.Remove(buff);

        if (_virusBuff2Mrgs.Contains(buff))
            _virusBuff2Mrgs.Remove(buff);

        if (_virusBuff3Mrgs.Contains(buff))
            _virusBuff3Mrgs.Remove(buff);

        if (_virusObjects.Count <= 1 && WaveVirusDataAdapter.IsNextWave && !_isTiped && _isSpawned)
        {
            EventManager.TriggerEvent(new UIVirusTipEvent());
            StartCoroutine(Spawn());
            _isTiped = true;
        }
        if (_virusObjects.Count == 0 && !WaveVirusDataAdapter.IsNextWave && _isSpawned)
        {
            EventManager.TriggerEvent(new VirusGameStateEvent(VirusGameState.Settle));
        }
    }


    public GameObject GetTargetVirus()
    {
        float max = 10000;
        int index = -1;
        if (VirusGameMrg.Instance.VirusPlayer != null)
        {
            Transform player = VirusGameMrg.Instance.VirusPlayer.transform;
            for (int i = 0; i < _virusObjects.Count; i++)
            {
                float dis = (_virusObjects[i].transform.position - player.position).sqrMagnitude;
                if (dis < max)
                {
                    max = dis;
                    index = i;
                }
            }
            if (index >= 0)
            {
                return _virusObjects[index];
            }
        }
        return null;
    }


    public GameObject GetRandomVirus()
    {
        if (_virusObjects.Count == 0)
            return null;
        int index = Random.Range(0, _virusObjects.Count);
        return _virusObjects[index];
    }


    public void MinusVirusCount(BaseVirus virus)
    {
        VirusName n = (VirusName)Enum.Parse(typeof(VirusName), virus.name);
        int count = VirusTool.GetSplit(virus.SplitLevel, n);
        _originCount -= count;
    }


    #endregion


    #region Virus Method

    public void ActiveVirus()
    {
        _virusBuff1Mrgs = _virusObjects.Select(t => t.GetComponent<VirusBuffMrg>()).ToList();
        for (int i = 0; i < _virusBuff1Mrgs.Count; i++)
        {
            var buff = _virusBuff1Mrgs[i];
            buff.AddBuff(new ActiveVirusBuff(buff.transform));
        }


    }


    public void UnActiveVirus()
    {
        for (int i = 0; i < _virusBuff1Mrgs.Count; i++)
        {
            var buff = _virusBuff1Mrgs[i];
            if (buff != null && buff.gameObject.activeSelf)
                buff.StopBuff(VirusPropEnum.Active);
        }
    }


    public void StartBiggerVirus()
    {
        _virusBuff2Mrgs = _virusObjects.Select(t => t.GetComponent<VirusBuffMrg>()).ToList();
        for (int i = 0; i < _virusBuff2Mrgs.Count; i++)
        {
            var buff = _virusBuff2Mrgs[i];
            buff.AddBuff(new BiggerVirusBuff(buff.transform));
        }
    }


    public void RecoverBiggerVirus()
    {
        for (int i = 0; i < _virusBuff2Mrgs.Count; i++)
        {
            var buff = _virusBuff2Mrgs[i];
            if (buff != null && buff.gameObject.activeSelf)
                buff.StopBuff(VirusPropEnum.Big);
        }
    }


    public void WeakenVirus()
    {
        _virusBuff3Mrgs = _virusObjects.Select(t => t.GetComponent<VirusBuffMrg>()).ToList();
        for (int i = 0; i < _virusBuff3Mrgs.Count; i++)
        {
            var buff = _virusBuff3Mrgs[i];
            buff.AddBuff(new WeakenVirusBuff(buff.transform));
        }
    }


    public void NotWeakenVirus()
    {
        for (int i = 0; i < _virusBuff3Mrgs.Count; i++)
        {
            var buff = _virusBuff3Mrgs[i];
            if (buff != null && buff.gameObject.activeSelf)
                buff.StopBuff(VirusPropEnum.Weaken);
        }
    }

    #endregion


}