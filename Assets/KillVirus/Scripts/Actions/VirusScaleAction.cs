using DG.Tweening;
using UnityEngine;

public class VirusScaleAction : MonoBehaviour
{

    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;


    private float _scale;

    private float _zoomOutDuration;
    private float _zoomInduration;
    private float _totalTime;
 
    private bool _isZoomOut;
    private bool _isReady;
    public void Initi()
    {
        _isZoomOut = false;
        _isReady = false;
        _scale = maxScale;
        transform.localScale = new Vector3(_scale, _scale, 1);

        _zoomOutDuration = Random.Range(1f, 3f);
        _zoomInduration = Random.Range(1f, 3f);
    }

    public void OnUpdate()
    {
        if (!_isZoomOut)
        {
            _totalTime += Time.deltaTime;
            if (_totalTime > _zoomInduration && !_isReady)
            {
                _isReady = true;
                float d = Mathf.Abs(maxScale - minScale) * 2;
                DOVirtual.Float(maxScale,minScale,d, (t) =>
                {
                    transform.localScale = new Vector3(t, t, 1);

                }).OnComplete(() =>
                {
                    _isZoomOut = true;
                    _totalTime = 0;
                    _isReady = false;
                });
            }
        }
        else
        {
            _totalTime += Time.deltaTime;
            if (_totalTime > _zoomOutDuration && !_isReady)
            {
                _isReady = true;
                float d = Mathf.Abs(maxScale - minScale) * 2;
                DOVirtual.Float(minScale, maxScale, d, (t) =>
                {
                    transform.localScale = new Vector3(t, t, 1);

                }).OnComplete(() =>
                {
                    _isZoomOut = false;
                    _totalTime = 0;
                    _isReady = false;
                });
            }
        }
        
       
    }


}