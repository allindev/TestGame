using UnityEngine;
using UnityEngine.UI;

public class VampireLine : MonoBehaviour
{

    [SerializeField] private RawImage lineImage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float _originScale;

    

    private float _width;
    private float _rectX;
    private bool _isRect;

    private void OnEnable()
    {
        _rectX = 0;
        _isRect = false;
    }
   
    private void Update()
    {
        if (_isRect)
        {
            _rectX += Time.deltaTime * moveSpeed;
            lineImage.uvRect = new Rect(_rectX, 1, _width, 1);
        }
        if (_rectX > 100f)
        {
            _rectX -= 100f;
        }
    }

    public void UpdateLine(Transform start,Transform end,bool isRect)
    {
        _isRect = isRect;

        float length = (end.position - start.position).magnitude;
        Vector3 pos = (start.position + end.position) / 2f;
        transform.position = pos;
        transform.right = (end.position - start.position).normalized;
        _width = length / _originScale;
        Vector2 size = new Vector2(_width * _originScale, 0.48f);
        lineImage.rectTransform.sizeDelta = size;
        lineImage.uvRect = new Rect(_rectX, 1, _width, 1);
    }



}