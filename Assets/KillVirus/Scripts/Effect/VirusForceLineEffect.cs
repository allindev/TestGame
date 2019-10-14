using UnityEngine;

public class VirusForceLineEffect : MonoBehaviour
{

    public void UpdateLine(Transform startPos, Transform endPos)
    {
        Vector3 offset = endPos.position - startPos.position;
        float dis = offset.magnitude;
        transform.position = startPos.position;
        transform.right = offset.normalized;
        transform.localScale = new Vector3(dis / 8.07f, 1, 1);
    }

}