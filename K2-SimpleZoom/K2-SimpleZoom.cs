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
        public void OnModUnLoaded()
        {
            SaveManager.SetKey("ScrollValue", null);  // Remove the Save key
            Camera.main.orthographicSize = (float)5.5; // Set the CameraZoomLevel to it's default state
        }
        public void OnModLoaded(ModManifest manifest)
        {
            MenuSetup();
        }

        public void OnFrame(float deltaTime)
        {
            if (DetectMenu())
            {
                IncrementOnKeyPress();
            }
        }

        public void OnLevelChanged(string oldLevel, string newLevel)
        {
            ZoomKeySaveBetweenLevels();
        }

        public void MenuSetup()
        {
            Options.AddSlider("Zoom Sensitvity", "Settings.GameTab", 5, 0, 20);
        }

        public bool DetectMenu()
        {
            bool MenuNotOpen = false;
            if (!MenuManager.IsPaused && !TabMenu.IsOpen && !ConsoleUI.IsOpen && MenuManager.InGame) // Checks if the game is not in the following:  Pause Menu, Phone Menu, Dev Console, and TitleScreen
            {
                if (Asuna.Minimap.MinimapPlayerIcon.Instance != null) // Checks if the Minimap PlayerIcon exists
                {
                    if (!Asuna.Minimap.MinimapUI.Instance.Maximized) // Checks if the Minimap is fullscreened
                    {
                        MenuNotOpen = true;
                    }
                }
                else
                {
                    MenuNotOpen = true;
                }
            }
            return MenuNotOpen;
        }

        private void IncrementOnKeyPress()
        {
            float SaveKeyScrollValue = SaveManager.GetKey("ScrollValue");
            float IncrementValue = Options.Get("Zoom Sensitvity", "Settings.GameTab").Int;
            float CameraZoomLevel = Camera.main.orthographicSize;
            float InputScrollwheelFloat = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            bool ValidCameraZoomLevel = false;

            IncrementValue = IncrementValue / 10; // Menu slider goes from 0 to 20, the default CameraZoomLevel is 5.5

            if (InputScrollwheelFloat > 0) // Zoom in Scrollwheel up
            {
                if (CameraZoomLevel - IncrementValue >= 0.1) // Make sure that the camera isn't inverted, or 0
                {
                    SaveKeyScrollValue -= IncrementValue;
                    ValidCameraZoomLevel = true;
                }
            }
            else if (InputScrollwheelFloat < 0) // Zoom out Scrollwheel down
            {
                SaveKeyScrollValue += IncrementValue;
                ValidCameraZoomLevel = true;
            }
            if (CameraZoomLevel <= 0) // Check if the Game sets the CameraZoomLevel to a value lower or equal to zero, useful on levels such as Sublevel One
            {
                CameraZoomLevel = (float)5.5;
                SaveManager.SetKey("ScrollValue", CameraZoomLevel);
                Camera.main.orthographicSize = CameraZoomLevel;
            }
            if (ValidCameraZoomLevel) // Saves the CameraZoomLevel
            {
                CameraZoomLevel = SaveKeyScrollValue;
                SaveManager.SetKey("ScrollValue", CameraZoomLevel);
                Camera.main.orthographicSize = CameraZoomLevel;
            }
        }

        private void ZoomKeySaveBetweenLevels()
        {
            float ScrollValueFloat = SaveManager.GetKey("ScrollValue");
            float CameraZoomLevel = Camera.main.orthographicSize;

            CameraZoomLevel = ScrollValueFloat;

            Camera.main.orthographicSize = CameraZoomLevel;

        }
    }
}
