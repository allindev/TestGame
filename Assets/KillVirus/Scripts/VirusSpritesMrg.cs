using System.Collections.Generic;
using UnityEngine;

public class VirusSpritesMrg : Singleton<VirusSpritesMrg>
{

    private Dictionary<string, Dictionary<string, Sprite>> _spriteCache;

    private List<string> _virusNames;

    [SerializeField] private List<Sprite> _ringSprites; 
    [SerializeField] private List<Sprite> _circleSprites;
    [SerializeField] private List<Sprite> _cureLineSprites;
    [SerializeField] private List<Sprite> _cureAddSprites;
    [SerializeField] private List<Sprite> _virusPropSprites;
    [SerializeField] private List<Sprite> _fragmentSprites;
    [SerializeField] private List<Sprite> _dotSprites; 



    private void Awake()
    {
        _spriteCache = new Dictionary<string, Dictionary<string, Sprite>>();
        _virusNames = new List<string>();
        _virusNames.Add("三分病毒");
        _virusNames.Add("再生病毒");
        _virusNames.Add("减速病毒");
        _virusNames.Add("发射病毒");
        _virusNames.Add("吞噬病毒");
        _virusNames.Add("吸血病毒");
        _virusNames.Add("吸附病毒");
        _virusNames.Add("快速病毒");
        _virusNames.Add("治愈病毒");
        _virusNames.Add("爆炸病毒");
        _virusNames.Add("碰撞病毒");
        _virusNames.Add("膨胀病毒");
        _virusNames.Add("追踪病毒");
        _virusNames.Add("防御病毒");
        _virusNames.Add("飞镖病毒");
        _virusNames.Add("普通病毒");
    }

    private void Start()
    {
        for (int i = 0; i < _virusNames.Count; i++)
        {
            string virusname = _virusNames[i];
            var list = Resources.LoadAll<Sprite>(virusname);
            Dictionary<string, Sprite> tempCache = new Dictionary<string, Sprite>();
            for (int j = 0; j < list.Length; j++)
            {
                var sprite = list[j];
                tempCache.Add(sprite.name, sprite);
            }
            _spriteCache.Add(virusname, tempCache);
        }
    }



    public Sprite GetRingSprite(int index)
    {
        return _ringSprites[index];
    }

    public Sprite GetCircleSprite(int index)
    {
        return _circleSprites[index];
    }

    public Sprite GetCureLineSprite(int index)
    {
        return _cureLineSprites[index];
    }

    public Sprite GetCureAddSprite(int index)
    {
        return _cureAddSprites[index];
    }

    public Sprite GetVirusPropSprite(VirusPropEnum propEnum)
    {
        int index = (int)propEnum;
        return _virusPropSprites[index];
    }

    public Sprite GetSpriteByName(string virusName, string spriteName)
    {
        if (_spriteCache.ContainsKey(virusName))
        {
            var temp = _spriteCache[virusName];
            if (temp.ContainsKey(spriteName))
                return temp[spriteName];
        }
        return null;
    }

    public Sprite GetFragmentSprite(int index)
    {
        if (index >= _fragmentSprites.Count)
            return null;

        return _fragmentSprites[index];
    }

    public Sprite GetDotSprite(int index)
    {
        return _dotSprites[index];
    }


}