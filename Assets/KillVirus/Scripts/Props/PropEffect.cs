using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;

public class PropEffect : MonoBehaviour
{

    [SerializeField] private GameObject rotateObj;
    [SerializeField] private GameObject propObj;

    private int _num;
    private float _totalTime;
    private float _duration;
    private bool _isUpdate;
    private SpriteRenderer _spriteRenderer;
    
    public void Initi(float duration)
    {
        _num = 0;
        _totalTime = 0;
        _duration = duration;
        _isUpdate = true;

        propObj.SetActive(true);
        rotateObj.SetActive(true);
        _spriteRenderer = propObj.GetComponent<SpriteRenderer>();
    }


    private void Awake()
    {
        Initi(5);
    }

    private void Update()
    {
        rotateObj.transform.localEulerAngles += new Vector3(0, 0, 150 * Time.deltaTime);
        if (_isUpdate)
        {
            _totalTime += Time.deltaTime;
            if (_totalTime > _duration)
            {
                _isUpdate = false;
                _totalTime = 0;
            }
        }
        else
        {
            _totalTime += Time.deltaTime;
            if (_totalTime >= 2f)
                PropPools.Instance.DeSpawn(gameObject);
            _num++;
            _spriteRenderer.color = _num % 10 < 3 ? new Color(1, 1, 1, 80f / 255) : new Color(1, 1, 1, 1);
        }
       
    }


}