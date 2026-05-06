using UnityEngine;
using System;

namespace StreetFoodVendor.UI
{
    /// <summary>
    /// Central manager for all UI screens. Handles switching between different UI states.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("UI Controllers")]
        public HUDController HUD;
        public CustomerUIController CustomerUI;
        public CookingUIController CookingUI;
        public DaySummaryController DaySummary;
        public ShopUIController ShopUI;
        public MainMenuController MainMenu;
        public GameOverController GameOver;

        [Header("Screen Effects")]
        public CanvasGroup PoliceVignette;
        public CanvasGroup WeatherOverlay;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowMainMenu()
        {
            HideAll();
            MainMenu.Show();
        }

        public void ShowHUD()
        {
            HideAll();
            HUD.Show();
        }

        public void ShowDaySummary()
        {
            HideAll();
            DaySummary.Show();
        }

        public void ShowShop()
        {
            HideAll();
            ShopUI.Show();
        }

        public void ShowGameOver()
        {
            HideAll();
            GameOver.Show();
        }

        private void HideAll()
        {
            HUD.Hide();
            CustomerUI.Hide();
            CookingUI.Hide();
            DaySummary.Hide();
            ShopUI.Hide();
            MainMenu.Hide();
            GameOver.Hide();
        }

        public void UpdatePoliceVignette(float intensity)
        {
            if (PoliceVignette != null)
            {
                PoliceVignette.alpha = intensity;
            }
        }
    }
}
