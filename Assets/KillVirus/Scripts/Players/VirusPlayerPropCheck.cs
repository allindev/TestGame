using UnityEngine;

public class VirusPlayerPropCheck : MonoBehaviour
{

    public bool IsControl { set; get; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Prop"))
        {
            if (IsControl)
            {
                ScenePropMrg.Instance.Remove(collision.gameObject);
                var baseProp = collision.transform.GetComponent<VirusBaseProp>();
                baseProp.Excute(transform);
                VirusSoundMrg.Instance.PlaySound(VirusSoundType.Prop);
            }
        }
    }

}