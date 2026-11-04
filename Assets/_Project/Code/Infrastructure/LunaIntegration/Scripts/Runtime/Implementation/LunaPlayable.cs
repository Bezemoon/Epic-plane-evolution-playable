namespace LunaIntegration
{
    public class LunaPlayable
    {
        public void Install()
        {
            Luna.Unity.Playable.InstallFullGame();
        }

        public void NotifyGameEnded()
        {
            Luna.Unity.LifeCycle.GameEnded();
        }
    }
}