using UnityEngine;

namespace StreetFoodVendor.UI
{
    public abstract class UIControllerBase : MonoBehaviour
    {
        public virtual void Show() { gameObject.SetActive(true); }
        public virtual void Hide() { gameObject.SetActive(false); }
    }

    public class HUDController : UIControllerBase { }
    public class CustomerUIController : UIControllerBase { }
    public class CookingUIController : UIControllerBase { }
    public class DaySummaryController : UIControllerBase { }
    public class ShopUIController : UIControllerBase { }
    public class MainMenuController : UIControllerBase { }
    public class GameOverController : UIControllerBase { }
}
