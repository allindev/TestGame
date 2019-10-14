using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum VirusSoundType
{
    Clear,
    LevelCoin,
    PlayerDeath,
    Prop,
    TotalCoin,
    ViceBullet1Explosion,
    ViceBullet2Explosion,
    ViceBullet3,
    ViceBullet4Explosion,
    ViceBullet6,
    ViceBullet7Explosion,
    ViceBullet8,
    VirusDeath,
    Waring,
    UpgradeGun,
    Unlock
}

public class VirusSoundMrg : Singleton<VirusSoundMrg>
{

    [SerializeField] private List<AudioClip> _bgClips;


    private List<int> _bgclipList; 
    private Dictionary<VirusSoundType, AudioClip> _cacheClips; 
    private AudioSource _audioSource;
    private int _bgClipIndex;
    private void Awake()
    {
        _audioSource = transform.GetComponent<AudioSource>();
        _cacheClips = new Dictionary<VirusSoundType, AudioClip>();
        _bgClipIndex = 0;
        _bgclipList = new List<int> { 0, 1, 2 };
        _bgclipList.Remove(_bgClipIndex);
    }

    private void Start()
    {
        _audioSource.clip = _bgClips[_bgClipIndex];
        _audioSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayBgSound();
        }
    }


    public void PlayBgSound()
    {
        if (_bgclipList.Count == 0)
        {
            _bgclipList = new List<int> { 0, 1, 2 };
            _bgclipList.Remove(_bgClipIndex);
        }
        int index = Random.Range(0, _bgclipList.Count);
        _bgClipIndex = _bgclipList[index];
        _bgclipList.RemoveAt(index);

        Sequence sq = DOTween.Sequence();
        sq.Append(DOVirtual.Float(1, 0, 0.5f, (t) =>
        {
            _audioSource.volume = t;

        })).OnComplete(() => 
        {  
            _audioSource.Pause();
            _audioSource.clip = _bgClips[_bgClipIndex];
            _audioSource.Play();
        });
        sq.Append(DOVirtual.Float(0,1,0.5f, (t) =>
        {
            _audioSource.volume = t;
        }));
    }


    public void PlaySound(VirusSoundType soundType, float vloum = 1)
    {
        if (!_cacheClips.ContainsKey(soundType))
        {
            var clip = Resources.Load<AudioClip>("Sounds/" + soundType);
            _cacheClips.Add(soundType, clip);
        }
        Vector3 pos = transform.position;
        AudioSource.PlayClipAtPoint(_cacheClips[soundType], pos, vloum);
    }



}

