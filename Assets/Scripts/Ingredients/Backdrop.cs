using UnityEngine;

namespace Ingredients
{
    public class Backdrop : MonoBehaviour
    {
        public void DisplayBackdrop()
        {
            gameObject.SetActive(true);
        }

        public void HideBackdrop()
        {
            gameObject.SetActive(false);
        }
    }
}
