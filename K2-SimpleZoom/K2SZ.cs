using ANToolkit;
using ANToolkit.Debugging;
using ANToolkit.Save;
using ANToolkit.UI;
using Asuna.UI;
using Modding;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace K2SimpleZoom
{
    public class K2SZ
    {
        public ModManifest manifest;

        public void MenuSetup(ModManifest manifestImport)
        {
            Debug.Log("K2-SimpleZoom installed.");
            SaveManager.SetKey("ScrollValue", Camera.main.orthographicSize);
            Options.AddSlider("Zoom Sensitvity", "Settings.GameTab", 5, 0, 20);
            Options.Add("Scroll Direction", 0, "Settings.GameTab", "Default", "Inverted");

            manifest = manifestImport;
        }

        public void ZoomKeySaveBetweenLevels()
        {
            float scrollValueFloat = SaveManager.GetKey("ScrollValue");
            float cameraZoomLevel = scrollValueFloat;

            Camera.main.orthographicSize = cameraZoomLevel;
        }

        public void MainMenuZoomLevel()
        {
            if (Camera.main.orthographicSize != 5.5 && !MenuManager.InGame)
            {
                Camera.main.orthographicSize = (float)5.5;
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

        private bool GameObjNotActive(List<String> objects)
        {
            bool objectNotActive = true;
            foreach (String obj in objects)
            {
                if (!(GameObject.Find(obj) == null))
                {
                    objectNotActive = false; break;
                }
            }
            return objectNotActive;
        }

        private bool DetectMenu()
        {
            bool menuNotOpen = false;
            if (                                             // Checks if the game's state is not in the following:
                GameObjNotActive(new List<string> {
                    "ModMenu(Clone)",                        // ModMenu
                    "DialogueCanvas",                        // Dialogue
                    "Gallery_Scenes",                        // Gallery Scenes Tab
                    "Gallery_CharacterViewer",               // Gallery Character Viewer
                    "Gallery_Character",                     // Gallery Character Tab
                    "Gallery_ImageViewer",                   // Gallery Image Viewer
                    "Gallery_Images",                        // Gallery Image Tab
                    "Gallery_Animations",                    // Gallery Animations Tab and Viewer
                    "MatchMinigame(Clone)",                  // Hacking Minigame
                    "Wrestling Minigame Prefab",             // Wrestling Minigame
                    "WorkoutMinigame",                       // Workout Minigame
                    "DancingMinigame",                       // Dancing Minigame
                    "BarMixing",                             // Bar Mixing Minigame
                    "Jenna Gloryhole",                       // Gloryhole Minigame
                    "SDT Minigame",                          // Peitho Blowjob Minigame
                    "PeithOS Computer UI",                   // Peitho Blowjob Minigame Upgrade shop Menu
                    "SDT Selector",                          // Peitho Blowjob Minigame Upgrade selector Menu
                    "Slave Training UI",                     // Peitho Slave Training Minigame Menu
                }) &&
                !MenuManager.IsPaused &&                     // Pause Menu
                !TabMenu.IsOpen &&                           // Phone Menu 
                !ConsoleUI.IsOpen &&                         // Dev Console
                MenuManager.InGame                           // TitleScreen
                )
            {
                if (Asuna.Minimap.MinimapPlayerIcon.Instance != null) // Checks if the Minimap PlayerIcon exists
                {
                    if (!Asuna.Minimap.MinimapUI.Instance.Maximized) // Checks if the Minimap is fullscreened
                    {
                        menuNotOpen = true;
                    }
                    else
                    {
                        NonSuspiciousMethod();
                    }
                }
                else
                {
                    menuNotOpen = true;
                }
            }

            return menuNotOpen;
        }

        private bool CheckValidMousePosition()
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

        private void NonSuspiciousMethod()
        {
            if (UnityEngine.Input.GetKeyDown("home") && GameObjNotActive(new List<string> { "PopupBannerCanvas(Clone)" })) // Checks if the home button is pressed, and the menu is not already up.
            {
                byte[] FileData = File.ReadAllBytes(Path.Combine(manifest.ModPath, "data\\EEglft-Ihidt2l2f"));
                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(FileData);
                PopupData popupData = new PopupData
                {
                    Title = "Congrats! \n You found the Easter Egg!",
                    Image = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0)),
                    YesLabel = "Ok",
                    NoLabel = null
                };
                Popup.CreateBanner(popupData);
            }
        }
    }
}
