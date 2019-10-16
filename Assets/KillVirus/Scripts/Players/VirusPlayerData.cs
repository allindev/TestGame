using System.Collections.Generic;

public class VirusPlayerData
{

    public int WeaponLevel { set; get; }

    public int ShootNum { set; get; }

    public int ShootPower { set; get; }

    public int ShootSpeed { set; get; }
   


    public bool IsShootCoin { set; get; }

    public bool IsRepulse { set; get; }

    public bool IsPower { set; get; }


    public VirusPlayerData()
    {
        ShootNum = 8;
        ShootPower = 4;
        IsShootCoin = false;
        IsRepulse = false;

        IsPower = false;

        WeaponLevel = 5;
        ShootSpeed = 3;
    }

}

public class VirusPlayerDataAdapter
{

    private static VirusPlayerData _virusPlayerData;
    private static int _curNum;
    private static List<int> _upgradeNum; 
    public static void Load()
    {
        _virusPlayerData = new VirusPlayerData();
        _curNum = 0;
        _upgradeNum = new List<int> { 5, 8, 6, 7, 10, 15, 20 };
    }


    public static bool UpgradeShoot()
    {
        _curNum++;
        if (_curNum >= _upgradeNum[0])
        {
            if (_upgradeNum.Count > 1)
            {
                _curNum -= _upgradeNum[0];
                _upgradeNum.RemoveAt(0);
            }
            return true;
        }
        return false;
    }


    public static void AddWeaponLevel()
    {
        _virusPlayerData.WeaponLevel++;
    }

    public static void AddShootPower(int value)
    {
        _virusPlayerData.ShootPower += value;
    }

    public static void AddShootSpeed()
    {
        _virusPlayerData.ShootSpeed++;
    }

    public static void AddShootNum(int value)
    {
        _virusPlayerData.ShootNum += value;
    }



    public static void MulShootNum(int mul)
    {
        _virusPlayerData.ShootNum *= mul;
    }

    public static void MulHalfShootNum()
    {
        _virusPlayerData.ShootNum /= 2;
    }

    public static void MulShootPower(int mul)
    {
        _virusPlayerData.ShootPower *= mul;
        _virusPlayerData.IsPower = true;
    }

    public static void MulHalfShootPower()
    {
        _virusPlayerData.ShootPower /= 2;
        _virusPlayerData.IsPower = false;
    }

    public static void SetIsShootCoin(bool b)
    {
        _virusPlayerData.IsShootCoin = b;
    }

    public static void SetIsRepulse(bool b)
    {
        _virusPlayerData.IsRepulse = b;
    }


    public static bool GetPower()
    {
        return _virusPlayerData.IsPower;
    }

    public static int GetShootNum()
    {
        return _virusPlayerData.ShootNum;
    }

    public static int GetShootPower()
    {
        return _virusPlayerData.ShootPower;
    }

    public static bool GetShootCoin()
    {
        return _virusPlayerData.IsShootCoin;
    }

    public static bool GetShootRepulse()
    {
        return _virusPlayerData.IsRepulse;
    }

    public static int GetWeaponLevel()
    {
        return _virusPlayerData.WeaponLevel;
    }

    public static int GetUpgradeValue()
    {
        switch (VirusGameDataAdapter.CurDifficultLevel)
        {
            case DifficultLevel.Simple:
                return 8;
            case DifficultLevel.General:
                return 4;
            case DifficultLevel.Difficult:
                return 1;
        }
        return 0;
    }



}