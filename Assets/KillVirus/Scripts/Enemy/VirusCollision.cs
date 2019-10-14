using Enemy.Entity;
using UnityEngine;

public class VirusCollision : MonoBehaviour
{


    [SerializeField] private VirusMove _virusMove;

    public bool IsCanCollision
    {
        get
        {
            return transform.position.y < (VirusTool.TopY - _virusMove.Radius - 0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Virus"))
        {
            if (IsCanCollision)
            {
                var virus = transform.GetComponent<BaseVirus>();
                var colvirus = collision.GetComponent<BaseVirus>();

                Vector3 offset = (transform.position - collision.transform.position).normalized;
                if (virus is CollisionVirus)
                {
                    if (virus.SplitLevel > colvirus.SplitLevel)
                    {
                        _virusMove.MoveDirection = _virusMove.MoveDirection + offset * 0.5f;
                        _virusMove.MoveDirection = _virusMove.MoveDirection.normalized;
                        SetMoveDir();
                    }
                    else if (virus.SplitLevel < colvirus.SplitLevel)
                    {
                        _virusMove.MoveDirection = -_virusMove.MoveDirection;
                    }
                    else
                    {
                        _virusMove.MoveDirection = _virusMove.MoveDirection + offset * 2f;
                        _virusMove.MoveDirection = _virusMove.MoveDirection.normalized;
                        SetMoveDir();
                    }
                }
                else
                {
                    _virusMove.MoveDirection = _virusMove.MoveDirection + 2 * offset;
                    _virusMove.MoveDirection = _virusMove.MoveDirection.normalized;
                    SetMoveDir();
                }
            }
        }
    }

    private void SetMoveDir()
    {
        float angle1 = Vector3.Angle(_virusMove.MoveDirection, Vector3.right);
        float angle2 = Vector3.Angle(_virusMove.MoveDirection, -Vector3.right);
        if (angle1 < 5)
        {
            _virusMove.MoveDirection = Quaternion.Euler(0, 0, 10) * _virusMove.MoveDirection;
            return;
        }
        if (angle2 < 5)
        {
            _virusMove.MoveDirection = Quaternion.Euler(0, 0, 10) * _virusMove.MoveDirection;
        }
    }


}