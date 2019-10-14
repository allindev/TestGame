using UnityEngine;

public class BubbleEffect : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _spriteRenderer;


    private float _minScale;
    private float _maxScale;
    private float _curScale;
    private bool _isZoomOut;


    
    public void Initi(float min, float max, int index,int sortIndex)
    {
        _minScale = min;
        _maxScale = max;
        _isZoomOut = Random.Range(0, 2) == 1;
        _curScale = Random.Range(_minScale, _maxScale);
        transform.localScale = new Vector3(_curScale, _curScale, 1);
        _spriteRenderer.sprite = VirusSpritesMrg.Instance.GetDotSprite(index);
        _spriteRenderer.sortingOrder = sortIndex;
    }

    public void SetSprite(ColorLevel colorLevel)
    {
        int index = (int) colorLevel;
        _spriteRenderer.sprite = VirusSpritesMrg.Instance.GetDotSprite(index);
    }


    private void Update()
    {
        if (_isZoomOut)
        {
            SetScale(1 / _curScale);
            _curScale += Time.deltaTime;
            if (_curScale >= _maxScale)
            {
                _isZoomOut = false;
            }
            SetScale(_curScale);
        }
        else
        {
            SetScale(1 / _curScale);
            _curScale -= Time.deltaTime;
            if (_curScale <= _minScale)
            {
                _isZoomOut = true;
            }
            SetScale(_curScale);
        }
    }


    private void SetScale(float value)
    {
        Vector3 d = transform.localScale;
        Vector3 e = new Vector3(d.x * value, d.x * value, 1);
        transform.localScale = e;
    }
   

}