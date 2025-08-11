using ANToolkit;
using ANToolkit.Debugging;
using ANToolkit.Save;
using ANToolkit.UI;
using Asuna.UI;
using Modding;
using UnityEngine;

namespace K2SimpleZoom
{
    public class K2SZ
    {
        public void DoNothing()
        {
            //Hi!  This function does nothing, if you are looking for something suspicious, you shoud probably look at https://media1.tenor.com/m/cSZp25bpjjoAAAAd/work-cat.gif.  If you are looking for mistakes, or inefficiencies, do let me know, I'd love to fix them.
        }
        public void CleanMod()
        {
            SaveManager.SetKey("ScrollValue", null);  // Remove the Save key
            Camera.main.orthographicSize = (float)5.5; // Set the cameraZoomLevel to it's default state
        }

        public void MenuSetup()
        {
            Debug.Log("K2-SimpleZoom installed.");
            SaveManager.SetKey("ScrollValue", Camera.main.orthographicSize);
            Options.AddSlider("Zoom Sensitvity", "Settings.GameTab", 5, 0, 20);
            Options.Add("Scroll Direction", 0, "Settings.GameTab", "Default", "Inverted");
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

        public bool CheckValidMousePosition()
        {
            Vector3 mousePos = Input.mousePosition;

            if (mousePos.x >= 0 && mousePos.x <= Screen.width && mousePos.y >= 0 && mousePos.y <= Screen.height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void IncrementOnKeyPress()
        {
            if (DetectMenu() && CheckValidMousePosition())
            {
                float saveKeyScrollValue = SaveManager.GetKey("ScrollValue");
                float incrementValue = Options.Get("Zoom Sensitvity", "Settings.GameTab").Int;
                float cameraZoomLevel = Camera.main.orthographicSize;
                float inputScrollwheelFloat = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
                bool ValidcameraZoomLevel = false;

                incrementValue /= 10; // Menu slider goes from 0 to 20, the default cameraZoomLevel is 5.5

                if (Options.Get("Scroll Direction", "Settings.GameTab").Int == 1) // Inverts the Scroll direction
                {
                    incrementValue -= incrementValue * 2;
                }

                if (inputScrollwheelFloat > 0) // Zoom in Scrollwheel up
                {
                    if (cameraZoomLevel - incrementValue >= 0.1) // Make sure that the camera isn't inverted, or 0
                    {
                        saveKeyScrollValue -= incrementValue;
                        ValidcameraZoomLevel = true;
                    }
                }
                else if (inputScrollwheelFloat < 0) // Zoom out Scrollwheel down
                {
                    saveKeyScrollValue += incrementValue;
                    ValidcameraZoomLevel = true;
                }
                if (cameraZoomLevel <= 0) // Check if the Game sets the cameraZoomLevel to a value lower or equal to zero, useful on levels from the "Skip to Content" menu such as "Sublevel One" 
                {
                    cameraZoomLevel = (float)5.5;
                    SaveManager.SetKey("ScrollValue", cameraZoomLevel);
                    Camera.main.orthographicSize = cameraZoomLevel;
                }
                if (ValidcameraZoomLevel) // Saves the cameraZoomLevel
                {
                    cameraZoomLevel = saveKeyScrollValue;
                    SaveManager.SetKey("ScrollValue", cameraZoomLevel);
                    Camera.main.orthographicSize = cameraZoomLevel;
                }

            }
        }

        public void ZoomKeySaveBetweenLevels()
        {
            float ScrollValueFloat = SaveManager.GetKey("ScrollValue");
            float cameraZoomLevel = ScrollValueFloat;

            Camera.main.orthographicSize = cameraZoomLevel;

        }
    }
}
