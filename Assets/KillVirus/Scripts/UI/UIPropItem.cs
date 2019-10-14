using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPropItem : MonoBehaviour
    {

        [SerializeField]
        private List<Image> linesList;
        [SerializeField]
        private Image propImage;



        public void Initi(VirusPropEnum propEnum)
        {
            for (int i = 0; i < linesList.Count; i++)
            {
                var line = linesList[i];
                line.fillAmount = 1;
            }
            propImage.sprite = VirusSpritesMrg.Instance.GetVirusPropSprite(propEnum);
        }


        public void Reiniti()
        {
            for (int i = 0; i < linesList.Count; i++)
            {
                var line = linesList[i];
                line.fillAmount = 1;
            }
        }


        public void OnUpdate(float fillAmount)
        {

            if (fillAmount <= 1 && fillAmount > 2.0f / 3.0f)
            {
                var v1 = (fillAmount - 2.0f / 3.0f) / (1.0f / 3.0f);
                if (v1 < 0.01f)
                    v1 = 0;
                linesList[0].fillAmount = v1;
            }
            else if (fillAmount <= 2.0f / 3.0f && fillAmount > 1.0f / 3.0f)
            {
                var v2 = (fillAmount - 1.0f / 3.0f) / (1.0f / 3.0f);
                if (v2 < 0.01f)
                    v2 = 0;
                linesList[1].fillAmount = v2;
            }
            else if (fillAmount <= 1.0f / 3.0f && fillAmount >= 0)
            {
                var v3 = fillAmount / (1.0f / 3.0f);
                if (v3 < 0.01f)
                {
                    v3 = 0;
                }
                linesList[2].fillAmount = v3;
            }

        }

    }

}
