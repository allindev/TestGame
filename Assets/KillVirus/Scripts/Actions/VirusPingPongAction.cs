using UnityEngine;

public class VirusPingPongAction : MonoBehaviour
{

    private Vector3 _startVector3;
    private Vector3 _endVector3;
    private float _totalTime;
    private float _duration;
    private bool _isUp;

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        _duration = 0.5f;
        _isUp = true;
    }

    public void Initi(Vector3 s, Vector3 e, float duration)
    {
        _startVector3 = s;
        _endVector3 = e;
        _duration = duration;
    }

    public void OnUpdate()
    {
        _totalTime += Time.deltaTime;
        if (_totalTime >= _duration)
        {
            _totalTime -= _duration;
            _isUp = !_isUp;
        }

        transform.localPosition = _isUp ? Vector3.LerpUnclamped(_startVector3, _endVector3, _totalTime / _duration) :
                                     Vector3.LerpUnclamped(_endVector3, _startVector3, _totalTime / _duration);
    }


}