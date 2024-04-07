namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayResult
    {
        public readonly int CoinsCount;
        public readonly int StarsCount;
        public readonly int UsedAxes;

        public GameplayResult(int coinsCount, int starsCount, int usedAxes)
        {
            CoinsCount = coinsCount;
            StarsCount = starsCount;
            UsedAxes = usedAxes;
        }
    }
}