using UnityEngine;

public class VirusEffect : MonoBehaviour
{

    [SerializeField] private GameObject ring;

    private float _totalTime;
    private float _scaleOffset;
    private float _phase;



    private void OnEnable()
    {
        _phase = Random.Range(0, Mathf.PI * 2);
    }

    private void Awake()
    {
        _scaleOffset = 0.05f;
    }

    private void Update()
    {
        _totalTime += Time.deltaTime * 8;
        float scalex = Mathf.Sin(_totalTime + _phase);
        float scaleY = Mathf.Cos(_totalTime + _phase);
        ring.transform.localScale = Vector3.one + new Vector3(scalex, scaleY, 0) * _scaleOffset;
    }


}