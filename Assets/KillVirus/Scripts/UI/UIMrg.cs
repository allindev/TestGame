using UnityEngine;

namespace UI
{
    public class UIMrg : MonoBehaviour
    {

        [SerializeField] private UIMainPanel mainPanel;
        [SerializeField] private UITittlePanel titlePanel;
        [SerializeField] private UISettlePanel settlePanel;
        [SerializeField] private UICoinPanel coinPanel;
        [SerializeField] private UIRestartPanel restartPanel;
        [SerializeField] private UIUpgradePanel upgradePanel;


        public UIMainPanel MainPanel { get { return mainPanel; } }

        public UITittlePanel TitlePanel { get { return titlePanel; } }

        public UISettlePanel SettlePanel { get { return settlePanel; } }

        public UICoinPanel CoinPanel { get { return coinPanel; } }

        public UIRestartPanel RestartPanel { get { return restartPanel; } }
        public UIUpgradePanel UpgradePanel { get { return upgradePanel; } }


        public void FadeIn()
        {
            titlePanel.Active();
            UpgradePanel.Active();
        }


        public void FadeOut()
        {
            titlePanel.UnActive();
        }


    }

}
