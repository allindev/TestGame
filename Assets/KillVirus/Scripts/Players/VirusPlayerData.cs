using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System. Serializable]
public class VirusPlayerData
{
    public int ShootNum;
    public int ShootPower;
    public int ShootSpeed;
    public int ShootLevel;

    public bool IsShootCoin;
    public bool IsRepulse;
    public bool IsPower;
    
    public int WeaponLevel;
    public int CurUseWeaponLevel;
    public List<WeaponData> Weapons;

    [System. Serializable]
    public class WeaponData
    {
        public int id;
        public int level;
        public int fire;
        public int speed;
        public bool unLock;
    }
    public VirusPlayerData()
    {
        ShootNum = 8;
        ShootLevel = 1;
        ShootPower = 4;
        IsShootCoin = false;
        IsRepulse = false;
        IsPower = false;
        WeaponLevel = 5;
        CurUseWeaponLevel = WeaponLevel;
        ShootSpeed = 3;
        var weapons = new List<WeaponData>();
        for (int i = 0; i < 7; i++)
        {
            var weapon = new WeaponData(){
                id = i,
                level = i+1,
                fire = 1,
                speed = 1,
                unLock = false,
            };
            weapons.Add(weapon);
        }
        Weapons = weapons;
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

    public static int GetCurWeaponLevel()
    {
        return _virusPlayerData.CurUseWeaponLevel;
    }

    public static VirusPlayerData.WeaponData GetWeaponData(int level)
    {
        return _virusPlayerData.Weapons.Find(s=>s.level == level);
    }


    public static void SetCurWeaponLevel(int level)
    {
        _virusPlayerData.CurUseWeaponLevel = level;
    }

    public static void AddWeaponLevel()
    {
        _virusPlayerData.WeaponLevel++;
    }

    public static int GetShootLevel()
    {
        return _virusPlayerData.ShootLevel;
    }

    public static void AddShootLevel()
    {
        _virusPlayerData.ShootLevel++;
    }

    public static void AddShootPower(int value)
    {
        _virusPlayerData.ShootPower += value;
    }

    public static int GetShootSpeed()
    {
        return _virusPlayerData.ShootSpeed;
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