using DG.Tweening;
using UnityEngine;

public class ViceWeaponEffect : MonoBehaviour
{

    [SerializeField] private float leftEndX;
    [SerializeField] private float rightEndX;

    [SerializeField] private Transform _leftWing;
    [SerializeField] private Transform _rightWing;



    public void Ready()
    {
        Vector3 leftPos = _leftWing.transform.localPosition;
        Vector3 rightPos = _rightWing.transform.localPosition;
        _leftWing.transform.localPosition = new Vector3(-8f, leftPos.y, 0);
        _rightWing.transform.localPosition = new Vector3(8f, rightPos.y, 0);
    }

    public void FadeOut()
    {
        _leftWing.transform.DOLocalMoveX(leftEndX, 1f);
        _rightWing.transform.DOLocalMoveX(rightEndX, 1f);
    }

    public void FadeIn()
    {
        _leftWing.DOLocalMoveY(-8f, 1f);
        _rightWing.DOLocalMoveY(-8f, 1f);
    }



}