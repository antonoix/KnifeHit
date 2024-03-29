namespace Internal.Scripts.UI.GamePlay
{
    public class GameplayResult
    {
        public readonly int CoinsCount;
        public readonly int StarsCount;

        public GameplayResult(int coinsCount, int starsCount)
        {
            CoinsCount = coinsCount;
            StarsCount = starsCount;
        }
    }
}