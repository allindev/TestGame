public enum VirusEnum
{
    再生病毒,
    减速病毒,
    发射病毒,
    吞噬病毒,
    吸血病毒,
    吸附病毒,
    快速病毒,
    普通病毒,
    治愈病毒,
    爆炸病毒,
    碰撞病毒,
    膨胀病毒,
    追踪病毒,
    防御病毒,
    飞镖病毒,
}

public enum ColorLevel
{
    Level0 = 0,
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    Level6,
    Level7,
    Level8
}

public enum SplitLevel
{
    Level1 = 0,
    Level2,
    Level3,
    Level4,
    Level5,
}

public enum VirusPropEnum
{
    Active = 0,
    Big,
    CallFriend,
    LimitMove,
    ReinforceShootSpeed,
    ReinforceShootPower,
    ShootCoin,
    Weaken,
    ShootRepulse,//丢弃
}

public enum ShootEffectEnum
{
    Normal,
    Coin
}

public enum VirusName
{
    NormalVirus = 1,
    FastVirus,
    CureVirus,
    CollisionVirus,
    SlowDownVirus,
    RegenerativeVirus,
    SwallowVirus,
    ExplosiveVirus,
    AdsorptionVirus,
    TrackingVirus,
    ExpansionVirus,
    ThreePointsVirus,
    LaunchVirus,
    VampireVirus,
    DefendingVirus,
    DartVirus
}

public enum VirusGameState
{
    None,
    Load,
    ShowTitle,
    UpgradeShoot,
    UpgradeWeapon,
    GamePlay,
    Settle,
    NextLevel,
    GameOver
}

public enum DifficultLevel
{
    Simple,
    General,
    Difficult
}