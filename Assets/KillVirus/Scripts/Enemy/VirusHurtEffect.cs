using UnityEngine;

public class VirusHurtEffect : MonoBehaviour
{

    private bool _isScale;
    private int _count;

    private void OnEnable()
    {
        _isScale = false;
        _count = 0;
    }

    private void Update()
    {
        if (_isScale)
        {
            _count++;
            if (_count == 4)
            {
                _count = 0;
                float scaleX = transform.localScale.x;
                Vector3 scale = new Vector3(scaleX / 1.06f, scaleX / 1.06f, 1);
                transform.localScale = scale;
                _isScale = false;
            }
        }
    }


    public void StartHurtEffect()
    {
        if (!_isScale)
        {
            float scaleX = transform.localScale.x;
            Vector3 scale = new Vector3(scaleX * 1.06f, scaleX * 1.06f, 1);
            transform.localScale = scale;
            _isScale = true;
        }
    }


}