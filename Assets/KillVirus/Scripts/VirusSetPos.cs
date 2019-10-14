using System.Collections.Generic;
using UnityEngine;


public class VirusSetPos : MonoBehaviour
{

    [SerializeField] private List<Transform> objList;

    [ContextMenu("SetPos")]
    public void SetPos()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            var item = objList[i];
            Vector3 euler = new Vector3(0, 0, 24 * i);
            item.transform.localEulerAngles = euler;
            item.transform.localPosition = Quaternion.Euler(euler) * Vector3.up * 0.3f;
        }
    }


    [ContextMenu("Set120DegreePos")]
    public void Set120DegreePos()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            var item = objList[i];
            Vector3 euler = new Vector3(0, 0, 36 * i);
            Vector3 dir = Quaternion.Euler(euler) * Vector3.down;
            item.transform.up = dir;
            item.transform.localPosition = Quaternion.Euler(euler) * Vector3.down * 0.2f;
        }
    }

    
}