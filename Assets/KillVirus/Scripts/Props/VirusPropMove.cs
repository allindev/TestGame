using UnityEngine;

public class VirusPropMove : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float _radius;
    [SerializeField] private float _borderRadius;

    private Vector3 _moveDir;


    public void Initi()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        _moveDir = new Vector2(x, y);
    }


    private void Update()
    {
        Vector3 delta = _moveDir * _speed * Time.deltaTime;
        if (transform.position.x + delta.x - _radius < VirusTool.LeftX)
        {
            transform.position += new Vector3(VirusTool.LeftX + _radius - transform.position.x, delta.y, 0);
            _moveDir = new Vector3(Mathf.Abs(_moveDir.x), _moveDir.y, 0);
            return;
        }
        if (transform.position.x + delta.x + _radius >= VirusTool.RightX)
        {
            transform.position += new Vector3(VirusTool.RightX - _radius - transform.position.x, delta.y, 0);
            _moveDir = new Vector3(-Mathf.Abs(_moveDir.x), _moveDir.y, 0);
            return;
        }
        if (transform.position.y + delta.y + _radius > VirusTool.TopY && _moveDir.y > 0)
        {
            transform.position += new Vector3(delta.x, VirusTool.TopY - _radius - transform.position.y, 0);
            _moveDir = new Vector3(_moveDir.x, -Mathf.Abs(_moveDir.y), 0);
            return;
        }
        transform.position += delta;
        if (transform.position.y <= VirusTool.BottomY - _borderRadius)
        {
            PropPools.Instance.DeSpawn(gameObject);
        }
    }


}