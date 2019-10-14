
public class VirusGameData
{
    public int TotalCoin;

    public int CurLevelCoin;

    public int Level;

    public VirusGameData()
    {
        TotalCoin = 0;
        Level = 1;
    }

}

public class VirusGameDataAdapter
{
    private static VirusGameData _gameData;

    public static DifficultLevel CurDifficultLevel { set; get; }

    public static void Load()
    {
        _gameData = new VirusGameData();
        CurDifficultLevel = DifficultLevel.Difficult;
    }


    public static void AddTotalCoin(int value)
    {
        _gameData.TotalCoin += value;
    }

    public static void AddLevelCoin(int value)
    {
        _gameData.CurLevelCoin += value;
    }

    public static void AddLevel()
    {
        _gameData.Level++;
    }


    public static void MinusTotalCoin(int value)
    {
        _gameData.TotalCoin -= value;
    }

    public static void ResetLevelCoin()
    {
        _gameData.CurLevelCoin = 0;
    }

    public static int GetTotalCoin()
    {
        return _gameData.TotalCoin;
    }

    public static int GetCurLevelCoin()
    {
        return _gameData.CurLevelCoin;
    }

    public static int GetLevel()
    {
        return _gameData.Level;
    }


}