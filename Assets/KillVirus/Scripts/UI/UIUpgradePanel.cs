using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using Tool;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIUpgradePanel : BasePanel
    {

        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject weaponPanel;
        [SerializeField] private Button btnMain;
        [SerializeField] private Button btnWeapon;


        [SerializeField] private Button btnMainSpeed;
        [SerializeField] private Button btnMainFire;
        [SerializeField] private Text textMainFireCoin;
        [SerializeField] private Text textMainSpeedCoin;
        [SerializeField] private Text textMainFireLv;
        [SerializeField] private Text textMainFireValue;
        [SerializeField] private Text textMainSpeedLv;
        [SerializeField] private Text textMainSpeedValue;


        [SerializeField] private Button btnWeaponFire;
        [SerializeField] private Button btnWeaponSpeed;
        [SerializeField] private Text textWeaponFireCoin;
        [SerializeField] private Text textWeaponSpeedCoin;
        [SerializeField] private Text textWeaponFireLv;
        [SerializeField] private Text textWeaponFireValue;
        [SerializeField] private Text textWeaponSpeedLv;
        [SerializeField] private Text textWeaponSpeedValue;

        [SerializeField] private Transform weaponList;
        enum UpgradePanelEnum
        {
            Main,
            Weapon,
        }
        private bool _isActive;
        private void Awake()
        {
            btnMain.onClick.AddListener(() =>
            {
                ShowPanel(UpgradePanelEnum.Main);
            });

            btnWeapon.onClick.AddListener(() =>
            {
                ShowPanel(UpgradePanelEnum.Weapon);
            });


        }

        
        private void ShowPanel(UpgradePanelEnum upgradePanelEnum)
        {
            switch (upgradePanelEnum)
            {
                case UpgradePanelEnum.Main:
                    mainPanel.SetActive(true);
                    FillMainPanelUIInfo();
                    weaponPanel.SetActive(false);
                    break;
                case UpgradePanelEnum.Weapon:
                    weaponPanel.SetActive(true);
                    FillWeaponPanelUIInfo();
                    mainPanel.SetActive(false);
                    break;
            }
        }


        private void FillMainPanelUIInfo(){
            textMainFireLv.text = $"[Lv{1}]";

        }

        private void FillWeaponPanelUIInfo(){
            
        }

        public override void Active()
        {
           
        }

        public override void UnActive()
        {
            _isActive = false;
            // _resultBg.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
            // {
            //     gameObject.SetActive(false);
            // });
        }
      


    }
}
