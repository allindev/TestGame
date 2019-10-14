
namespace Events
{
    public struct VirusGameStateEvent
    {
        public VirusGameState GameState;

        public VirusGameStateEvent(VirusGameState gameState)
        {
            GameState = gameState;
        }
    }
}
