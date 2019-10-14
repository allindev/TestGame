using UnityEngine;

public class VirusStretchAction : MonoBehaviour
{

    [SerializeField] private Vector3 _moveDir;
    [SerializeField] private Vector3 _originPos;

    private static float _minLegnth = 1f;
    private static float _maxLegnth = 1.4f;
    private float _curLength;

    private float _waittotalTime;
    private float _waitDuration;

    private float _totalTime;
    private float _duration;

    private bool _isStretch;
    private bool _isOut;


    private Vector3 _tartgetPos;

    private void OnEnable()
    {
        _waitDuration = Random.Range(0.1f, 0.3f);
        transform.localPosition = _originPos;
        _isStretch = Random.Range(0, 2) == 1;
        if (_isStretch)
        {
            _curLength = Random.Range(_minLegnth, _maxLegnth);
            _duration = _curLength / 5f;
        }
    }

    public void OnUpdate(float delta)
    {
        if (!_isStretch)
        {
            _waittotalTime += delta;
            if (_waittotalTime >= _waitDuration)
            {
                _isStretch = true;
                _waittotalTime -= _waitDuration;
                _curLength = Random.Range(_minLegnth, _maxLegnth);
                _duration = _curLength / 5f;
                _totalTime = 0;
                _isOut = true;
                _tartgetPos = _curLength * _moveDir.normalized + _originPos;
            }
        }
        else
        {
            _totalTime += Time.deltaTime;
            transform.localPosition = _isOut ? Vector3.LerpUnclamped(_originPos, _tartgetPos, _totalTime / _duration) :                                       Vector3.LerpUnclamped(_tartgetPos, _originPos, _totalTime / _duration);
            if (_totalTime >= _duration)
            {
                _totalTime -= _duration;
                _isOut = !_isOut;
                if (_isOut)
                {
                    _isStretch = false;
                    _waitDuration = Random.Range(0.1f, 0.3f);
                    _waittotalTime = 0;
                }
            }
        }
    }



    [ContextMenu("AutoSet")]
    protected void AutoSet()
    {
        _moveDir = transform.localPosition.normalized;
    }

    [ContextMenu("AutoSetPos")]
    protected void AutoSetPos()
    {
        _originPos = transform.localPosition;
    }




}