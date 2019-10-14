using Enemy.Entity;
using UnityEngine;

public class VirusMove : MonoBehaviour
{

    [SerializeField] private float _radius;

    [SerializeField] private float _speed;
    private float _speedMul;
    private float _palsyMul;
    private float _totalTime;
    private Vector3 _moveDir;
    private bool _isPalsy;


    public Vector3 MoveDirection { get { return _moveDir; } set { _moveDir = value; } }

    public float Radius { get { return _radius * transform.localScale.x; } }

    public float OriginSpeed { set; get; }

    public float BorderRadius { set; get; }

    public bool IsUpdate { set; get; }

    public bool IsTracking { set; get; }

    public float Speed { set { _speed = value; } get { return _speed; } }



    public void Palsy()
    {
        _isPalsy = true;
        _totalTime = 3f;
    }


    public void Initi(float speed, Vector3 moveDirection)
    {
        _speedMul = 1;
        _palsyMul = 1;
        _speed = speed;
        OriginSpeed = speed;
        _moveDir = moveDirection.normalized;
        IsUpdate = true;

        BorderRadius = Radius;
        _totalTime = 0;
    }


    public void SpeedUp(float mul)
    {
        _speed /= _speedMul;
        _speedMul = mul;
        _speed *= _speedMul;
    }




    private void Update()
    {
        if (IsUpdate)
        {
            if (IsTracking)
            {
                TrackingPlayer();
            }

            _speed /= _palsyMul;
            if (_isPalsy)
            {
                _palsyMul -= Time.deltaTime;
                if (_palsyMul <= 0.5f)
                    _palsyMul = 0.5f;

                _totalTime -= Time.deltaTime;
                if (_totalTime < 0)
                    _isPalsy = false;
            }
            else
            {
                _palsyMul += Time.deltaTime;
                if (_palsyMul >= 1)
                    _palsyMul = 1;
            }
            _speed *= _palsyMul;

            Vector3 delta = _moveDir * _speed * Time.deltaTime;
            if (transform.position.x + delta.x - Radius < VirusTool.LeftX)
            {
                transform.position += new Vector3(VirusTool.LeftX + Radius - transform.position.x, delta.y, 0);
                _moveDir = new Vector3(Mathf.Abs(_moveDir.x), _moveDir.y, 0);
                return;
            }
            if (transform.position.x + delta.x + Radius >= VirusTool.RightX)
            {
                transform.position += new Vector3(VirusTool.RightX - Radius - transform.position.x, delta.y, 0);
                _moveDir = new Vector3(-Mathf.Abs(_moveDir.x), _moveDir.y, 0);
                return;
            }
            if (transform.position.y + delta.y + Radius > VirusTool.TopY && _moveDir.y > 0)
            {
                transform.position += new Vector3(delta.x, VirusTool.TopY - Radius - transform.position.y, 0);
                _moveDir = new Vector3(_moveDir.x, -Mathf.Abs(_moveDir.y), 0);
                return;
            }
            transform.position += delta;
            if (transform.position.y <= VirusTool.BottomY - BorderRadius)
            {
                float y = VirusTool.TopY + BorderRadius;
                transform.position = new Vector3(transform.position.x, y, 0);
            }
        }
    }


    private void TrackingPlayer()
    {
        bool b = VirusGameMrg.Instance.VirusPlayer != null && VirusGameMrg.Instance.VirusPlayer.gameObject.activeSelf;
        if (b)
        {
            Vector3 pos = VirusGameMrg.Instance.VirusPlayer.transform.position;
            Vector3 targetDir = (pos - transform.position).normalized;
            Vector3 offset = targetDir - _moveDir;

            _moveDir = (_moveDir * 10 + offset * 1).normalized;
        }
    }


}