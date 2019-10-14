using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class VirusPingPongRotateAction : MonoBehaviour
{

    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool _isLeft;

    [SerializeField] private Image _circle;

    private int _num = 255;
    private void Update()
    {
        _num += 5;
        _num %= 255;
        _circle.color = new Color(1, 1, 1, _num / 255f);
        if (_isLeft)
        {
            transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * rotateSpeed);
            if (transform.localEulerAngles.z > _maxAngle && transform.localEulerAngles.z < 90)
                _isLeft = false;
        }
        else
        {
            transform.localEulerAngles -= new Vector3(0, 0, Time.deltaTime * rotateSpeed);
            if (transform.localEulerAngles.z < 360 + _minAngle && transform.localEulerAngles.z > 270f)
                _isLeft = true;
        }

    }



}