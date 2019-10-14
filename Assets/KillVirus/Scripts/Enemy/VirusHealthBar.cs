using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class VirusHealthBar : MonoBehaviour
{


    [SerializeField] private Text _healthText;


    public void Initi(string value)
    {
        int lenth = value.Length;
        switch (lenth)
        {
            case 1:
                _healthText.transform.localScale = new Vector3(2f, 2f, 1);
                break;
            case 2:
                _healthText.transform.localScale = new Vector3(1.6f, 1.6f, 1);
                break;
            case 3:
                _healthText.transform.localScale = new Vector3(1.4f, 1.4f, 1);
                break;
            case 4:
                _healthText.transform.localScale = new Vector3(1.2f, 1.2f, 1);
                break;
            case 5:
                _healthText.transform.localScale = Vector3.one;
                break;
            case 6:
                _healthText.transform.localScale = new Vector3(0.85f, 0.85f, 1);
                break;
        }
        _healthText.text = value;
    }


    public void SetValue(float value)
    {
        int vv = Mathf.CeilToInt(value);
        _healthText.text = VirusTool.GetStrByIntger(vv);
    }


}