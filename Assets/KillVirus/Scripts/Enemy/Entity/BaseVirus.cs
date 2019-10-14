using Events;
using Tool;
using UnityEngine;

public abstract class BaseVirus : MonoBehaviour
{

    public RectiveProperty<float> VirusHealth { set; get; }

    protected BaseVirusSprite VirusSprite { set; get; }

    protected abstract VirusHealthBar HealthBar { get; }

    protected abstract VirusMove VirusMove { get; }


    public bool IsDeath { set; get; }

    public float ScaleX { set; get; }

    public SplitLevel SplitLevel { set; get; }

    public ColorLevel CurColorLevel { set; get; }

    protected ColorLevel OriginColorLevel { set; get; }

    protected int TotalHealth { set; get; }

    

 
    


    protected abstract void RunAway();


    public virtual void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
    {
        IsDeath = true;
        int level = VirusGameDataAdapter.GetLevel();
        int coin = VirusTool.GetVirusDeathCoin(level);
        if (isEffect)
        {
            float scale = transform.localScale.x;
            int index = Random.Range(1, 5);
            var explosion = EffectPools.Instance.Spawn("Explosion" + index);
            explosion.transform.localScale = new Vector3(scale * 2f, scale * 2f, 1);
            explosion.transform.position = transform.position;
            explosion.GetComponent<VirusFragmentMrg>().Initi(index - 1, Random.Range(5, 10));
            VirusSoundMrg.Instance.PlaySound(VirusSoundType.VirusDeath);
            VirusCameraShake.Instance.Shake();
        }

        if (isCoin)
        {
            if (coin > 0)
            {
                VirusGameDataAdapter.AddLevelCoin(coin);
                int count = Random.Range(1, 4);
                for (int i = 0; i < count; i++)
                {
                    EventManager.TriggerEvent(new UIVirusAddLevelCoinEvent(transform.position));
                }
            }
        }

        if (isProp)
        {
            string propName = VirusTool.GetPropName(level);
            if (!propName.Equals("None") && SplitLevel > SplitLevel.Level1)
            {
                var prop = PropPools.Instance.Spawn(propName + "Prop");
                prop.transform.position = transform.position;
                prop.GetComponent<VirusPropMove>().Initi();
                ScenePropMrg.Instance.Add(prop);
            }
        }
        VirusMrg.Instance.Remove(gameObject);

    }


    protected void Divide(string virusName,ColorLevel colorLevel,int count)
    {
        if (SplitLevel > SplitLevel.Level1)
        {
            Vector3 pos = transform.position;
            var level = SplitLevel - 1;
            var move = transform.GetComponent<VirusMove>();
            for (int i = 0; i < count; i++)
            {
                VirusData data1 = new VirusData();
                data1.VirusColorLevel = VirusTool.GetColorLevel(colorLevel);
                data1.SplitLevel = level;
                data1.MoveSpeed = move.OriginSpeed;
                data1.MoveDirection = Quaternion.Euler(0, 0, Random.Range(-45f, 45f)) * Vector3.up;
                data1.HealthValue = VirusTool.GetVirusHealthByColorLevel(virusName, VirusGameDataAdapter.GetLevel(), data1.VirusColorLevel);
                VirusMrg.Instance.SpawnVirus(virusName, data1, pos, false);
            }
        }
    }






    public virtual void Born(VirusData virusData)
    {
        VirusSprite = transform.GetComponent<BaseVirusSprite>();
        VirusSprite.Initi(virusData.VirusColorLevel);
        VirusHealth = new RectiveProperty<float>();
        VirusHealth.Value = virusData.HealthValue;
        HealthBar.Initi(VirusTool.GetStrByIntger(virusData.HealthValue));
        VirusHealth.Subscibe(HealthBar.SetValue);

        SplitLevel = virusData.SplitLevel;
        CurColorLevel = virusData.VirusColorLevel;
        OriginColorLevel = virusData.VirusColorLevel;
        TotalHealth = virusData.HealthValue;

        ScaleX = VirusTool.GetScaleByLevel(virusData.SplitLevel);
        transform.localScale = new Vector3(ScaleX, ScaleX, 1);
        VirusMove.Initi(virusData.MoveSpeed, virusData.MoveDirection);

        transform.GetComponent<VirusBuffMrg>().Initi();
        IsDeath = false;
    }


    public abstract void Injured(float damageValue, bool isEffect);


    public virtual float StartCure(ICure iCure,float healthValue)
    {
        if (VirusHealth.Value + healthValue >= TotalHealth)
        {
            float vv = TotalHealth - VirusHealth.Value;
            VirusHealth.Value = TotalHealth;
            return vv;
        }
        VirusHealth.Value += healthValue;
        return healthValue;
    }


    public abstract void StopCure(ICure iCure);




}