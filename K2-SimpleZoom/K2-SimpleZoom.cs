using ANToolkit;
using ANToolkit.Debugging;
using ANToolkit.Save;
using ANToolkit.UI;
using Asuna.Dialogues;
using Asuna.UI;
using Modding;
using UnityEngine;

namespace K2SimpleZoom
{
    public class K2SimpleZoom : ITCMod
    {
        public Transform Target;

        public void OnDialogueStarted(Dialogue dialogue) { }
        public void OnLineStarted(DialogueLine line) { }
        public void OnModUnLoaded() { }
        public void OnModLoaded(ModManifest manifest)
        {
            Options.AddSlider("Zoom Sensitvity", "Settings.GameTab", 5, 0, 20);
        }

        public void OnFrame(float deltaTime)
        {
            if (!MenuManager.IsPaused && !TabMenu.IsOpen && !ConsoleUI.IsOpen && MenuManager.InGame) // in most cases this is fine
            {
                if (Asuna.Minimap.MinimapPlayerIcon.Instance != null) // this is needed due to the if statement below this causing exceptins
                {
                    if (!Asuna.Minimap.MinimapUI.Instance.Maximized) // this causes exceptions if you are inside a building, and creates a bunch of lag, and needs the above if statement to be where it is
                    {
                        ZoomScrollOnFrame();
                    }
                }
                else
                {
                    ZoomScrollOnFrame();
                }
            }
        }

        public void OnLevelChanged(string oldLevel, string newLevel)
        {
            ZoomKeySaveBetweenLevels();
        }

        private void ZoomKeySaveBetweenLevels()
        {
            float ScrollValueFloat = SaveManager.GetKey("ScrollValue");
            float Camerafloat = Camera.main.orthographicSize;

            Camerafloat = ScrollValueFloat;

            Camera.main.orthographicSize = Camerafloat;
        }

        private void ZoomScrollOnFrame()
        {
            float Camerafloat = Camera.main.orthographicSize;
            float InputKeyBool = UnityEngine.Input.GetAxis("Mouse ScrollWheel");

            if (InputKeyBool > 0)
            {
                IncrementDownOnKeyPress();
            }
            else if (InputKeyBool < 0)
            {
                IncrementUpOnKeyPress();
            }
            else if (Camerafloat <= 0)
            {
                Camerafloat = (float)5.5;

                SaveManager.SetKey("ScrollValue", Camerafloat);
                Camera.main.orthographicSize = Camerafloat;
            }
        }

        private void IncrementUpOnKeyPress()
        {
            float ScrollValueFloat = SaveManager.GetKey("ScrollValue");
            float Camerafloat = Camera.main.orthographicSize;
            float IncrementValue = Options.Get("Zoom Sensitvity", "Settings.GameTab").Int;
            IncrementValue = IncrementValue / 10;

            ScrollValueFloat += IncrementValue;
            Camerafloat = ScrollValueFloat;

            SaveManager.SetKey("ScrollValue", Camerafloat);
            Camera.main.orthographicSize = Camerafloat;
        }

        private void IncrementDownOnKeyPress()
        {
            float ScrollValueFloat = SaveManager.GetKey("ScrollValue");
            float Camerafloat = Camera.main.orthographicSize;
            float IncrementValue = Options.Get("Zoom Sensitvity", "Settings.GameTab").Int;
            IncrementValue = IncrementValue / 10;

            if (ScrollValueFloat - IncrementValue > 0.1)
            {
                ScrollValueFloat -= IncrementValue;
                Camerafloat = ScrollValueFloat;

                SaveManager.SetKey("ScrollValue", Camerafloat);
                Camera.main.orthographicSize = Camerafloat;
            }
        }
    }
}
