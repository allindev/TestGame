using System;
using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FastlaneFloatTip : MonoBehaviour
{

    [SerializeField] private Text tipText;

    public void Float(string tipStr, Action callBack)
    {
        tipText.color = new Color(0, 1, 1, 1);
        tipText.text = tipStr;
        transform.DOMoveY(-1, 1f);
        tipText.DOFade(0, 1f).OnComplete(callBack.Invoke);
    }
   

}