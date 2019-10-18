using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using Events;
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
            btnMain.onClick.AddListener(() => { ShowPanel(UpgradePanelEnum.Main); });
            btnWeapon.onClick.AddListener(() => { ShowPanel(UpgradePanelEnum.Weapon); });
            btnMainFire.onClick.AddListener(OnClickUpgradeMainFire);
            btnMainSpeed.onClick.AddListener(OnClickUpgradeMainSpeed);
            btnWeaponFire.onClick.AddListener(OnClickUpgradeWeaponFire);
            btnWeaponSpeed.onClick.AddListener(OnClickUpgradeWeaponSpeed);
        }

        private void OnClickUpgradeWeaponFire()
        {
            RefreshWeaponPanelUIInfo();
        }

        private void OnClickUpgradeWeaponSpeed()
        {
           
            RefreshWeaponPanelUIInfo();
            
        }

        private void OnClickUpgradeMainFire()
        {
            UpgradeMainFire();
            RefreshMainPanelUIInfo();
            EventManager.TriggerEvent(new VirusGameStateEvent(VirusGameState.UpgradeShoot));
        }

        private void OnClickUpgradeMainSpeed()
        {
            UpgradeMainSpeed();
            RefreshMainPanelUIInfo();
            EventManager.TriggerEvent(new VirusGameStateEvent(VirusGameState.UpgradeShoot));
        }

        private void UpgradeMainFire()
        {
            int needCoin = VirusTool.GetUpgradeCoin(VirusPlayerDataAdapter.GetShootLevel());
            VirusGameDataAdapter.MinusTotalCoin(needCoin);
            VirusPlayerDataAdapter.AddShootPower(1);
        }

         private void UpgradeMainSpeed()
         {
             int needCoin = VirusTool.GetUpgradeCoin(VirusPlayerDataAdapter.GetShootSpeed());
             VirusGameDataAdapter.MinusTotalCoin(needCoin);
             VirusPlayerDataAdapter.AddShootSpeed();

        }

        private void UpgradeWeaponFire(int needCoin)
        {
            VirusGameDataAdapter.MinusTotalCoin(needCoin);

        }
        private void UpgradeWeaponSpeed(int needCoin)
        {
            VirusGameDataAdapter.MinusTotalCoin(needCoin);
        }
        
        private void ShowPanel(UpgradePanelEnum upgradePanelEnum)
        {
            switch (upgradePanelEnum)
            {
                case UpgradePanelEnum.Main:
                    mainPanel.SetActive(true);
                    weaponPanel.SetActive(false);
                    RefreshMainPanelUIInfo();
                    break;
                case UpgradePanelEnum.Weapon:
                    weaponPanel.SetActive(true);
                    mainPanel.SetActive(false);
                    RefreshWeaponPanelUIInfo();
                    break;
            }
        }


        private void RefreshMainPanelUIInfo(){
            int fireNeedCoin = VirusTool.GetUpgradeCoin(VirusPlayerDataAdapter.GetShootLevel());
            int speedNeedCoin = VirusTool.GetUpgradeCoin(VirusPlayerDataAdapter.GetShootSpeed());
           
            textMainFireLv.text = $"[Lv{VirusPlayerDataAdapter.GetShootLevel()}]";
            textMainFireValue.text = $"{VirusPlayerDataAdapter.GetShootLevel()*10}";
            textMainFireCoin.text = $"x{fireNeedCoin}";

            textMainSpeedLv.text = $"[Lv{VirusPlayerDataAdapter.GetShootSpeed()}]";
            textMainSpeedValue.text = $"{VirusPlayerDataAdapter.GetShootSpeed()*10}";
            textMainSpeedCoin.text = $"x{speedNeedCoin}";
            
            btnMainFire.interactable = CheckCoin(fireNeedCoin);
            btnMainSpeed.interactable = CheckCoin(speedNeedCoin);
        }


        private void RefreshWeaponPanelUIInfo(){
            var lv = VirusPlayerDataAdapter.GetCurWeaponLevel();
            var weapon = VirusPlayerDataAdapter.GetWeaponData(lv);

            int fireNeedCoin = VirusTool.GetUpgradeCoin(weapon.fire);
            int speedNeedCoin = VirusTool.GetUpgradeCoin(weapon.speed);
           
            textWeaponFireLv.text = $"[Lv{weapon.fire}]";
            textWeaponFireValue.text = $"{weapon.fire*10}";
            textWeaponFireCoin.text = $"x{fireNeedCoin}";

            textWeaponSpeedLv.text = $"[Lv{weapon.speed}]";
            textWeaponSpeedValue.text = $"{weapon.speed*10}";
            textWeaponSpeedCoin.text = $"x{speedNeedCoin}";
            
            btnWeaponFire.interactable = CheckCoin(fireNeedCoin);
            btnWeaponSpeed.interactable = CheckCoin(speedNeedCoin);
        }

        private bool CheckCoin(int needCoin){
            int coin = VirusGameDataAdapter.GetTotalCoin();
            return coin >= needCoin;
        }
        public override void Active()
        {
           gameObject.SetActive(true);
        }

        public override void UnActive()
        {
            _isActive = false;
            gameObject.SetActive(false);
            // _resultBg.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
            // {
            //     gameObject.SetActive(false);
            // });
        }
      


    }
}
