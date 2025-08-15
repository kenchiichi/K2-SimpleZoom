using ANToolkit.Save;
using Asuna.Dialogues;
using Modding;
using UnityEngine;

namespace K2SimpleZoom
{
    public class K2SimpleZoom : ITCMod
    {
        private readonly K2SZ K2SZ = new K2SZ();

        public void OnDialogueStarted(Dialogue dialogue)
        {

        }

        public void OnLineStarted(DialogueLine line)
        {

        }

        public void OnModUnLoaded()
        {
            SaveManager.SetKey("ScrollValue", null);  // Remove the Save key
            Camera.main.orthographicSize = (float)5.5; // Set the cameraZoomLevel to it's default state
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
