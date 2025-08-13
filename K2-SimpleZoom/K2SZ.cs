using ANToolkit;
using ANToolkit.Debugging;
using ANToolkit.Save;
using ANToolkit.UI;
using Asuna.UI;
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

        public bool FindGameObj(string obj)
        {
            return GameObject.Find(obj) == null;
        }

        public bool DetectMenu()
        {

            bool menuNotOpen = false;
            if (                                             // Checks if the game's state is not in the following:  
                !MenuManager.IsPaused &&                     // Pause Menu
                !TabMenu.IsOpen &&                           // Phone Menu 
                !ConsoleUI.IsOpen &&                         // Dev Console
                MenuManager.InGame &&                        // TitleScreen
                FindGameObj("ModMenu(Clone)") &&             // ModMenu
                FindGameObj("DialogueCanvas") &&             // Dialogue
                FindGameObj("MatchMinigame(Clone)") &&       // Hacking Minigame
                FindGameObj("Wrestling Minigame Prefab") &&  // Wrestling Minigame
                FindGameObj("WorkoutMinigame") &&            // Workout Minigame
                FindGameObj("DancingMinigame") &&            // Dancing Minigame (probably unnecessary)
                FindGameObj("BarMixing") &&                  // Bar Mixing Minigame (probably unnecessary)
                FindGameObj("Jenna Gloryhole") &&            // Gloryhole Minigame
                FindGameObj("SDT Minigame") &&               // Peitho Training Minigame
                FindGameObj("PeithOS Computer UI") &&        // Peitho Blowjob Training Minigame Upgrade shop Menu
                FindGameObj("SDT Selector") &&               // Peitho Blowjob Training Minigame Upgrade selector Menu
                FindGameObj("Slave Training UI")             // Peitho Slave Training Minigame Menu
                )
            {
                if (Asuna.Minimap.MinimapPlayerIcon.Instance != null) // Checks if the Minimap PlayerIcon exists
                {
                    if (!Asuna.Minimap.MinimapUI.Instance.Maximized) // Checks if the Minimap is fullscreened
                    {
                        menuNotOpen = true;
                    }
                }
                else
                {
                    menuNotOpen = true;
                }
            }
            return menuNotOpen;
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
                bool validCameraZoomLevel = false;

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
                        validCameraZoomLevel = true;
                    }
                }
                else if (inputScrollwheelFloat < 0) // Zoom out Scrollwheel down
                {
                    saveKeyScrollValue += incrementValue;
                    validCameraZoomLevel = true;
                }
                if (cameraZoomLevel <= 0) // Check if the Game sets the cameraZoomLevel to a value lower or equal to zero, useful on levels from the "Skip to Content" menu such as "Sublevel One" 
                {
                    cameraZoomLevel = (float)5.5;
                    SaveManager.SetKey("ScrollValue", cameraZoomLevel);
                    Camera.main.orthographicSize = cameraZoomLevel;
                }
                if (validCameraZoomLevel) // Saves the cameraZoomLevel
                {
                    cameraZoomLevel = saveKeyScrollValue;
                    SaveManager.SetKey("ScrollValue", cameraZoomLevel);
                    Camera.main.orthographicSize = cameraZoomLevel;
                }

            }
        }

        public void ZoomKeySaveBetweenLevels()
        {
            float scrollValueFloat = SaveManager.GetKey("ScrollValue");
            float cameraZoomLevel = scrollValueFloat;

            Camera.main.orthographicSize = cameraZoomLevel;
        }
    }
}
