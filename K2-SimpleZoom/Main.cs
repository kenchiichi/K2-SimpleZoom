using Asuna.Dialogues;
using Modding;

namespace K2SimpleZoom
{
    public class K2SimpleZoom : ITCMod
    {
        private readonly K2SZ K2SZ = new K2SZ();

        public void OnDialogueStarted(Dialogue dialogue)
        {
            K2SZ.DoNothing();
        }

        public void OnLineStarted(DialogueLine line)
        {
            K2SZ.DoNothing();
        }

        public void OnModUnLoaded()
        {
            K2SZ.CleanMod();
        }

        public void OnModLoaded(ModManifest manifest)
        {
            K2SZ.MenuSetup(manifest);

        }

        public void OnFrame(float deltaTime)
        {
            K2SZ.IncrementOnKeyPress();
        }

        public void OnLevelChanged(string oldLevel, string newLevel)
        {
            K2SZ.ZoomKeySaveBetweenLevels();
        }
    }
}
