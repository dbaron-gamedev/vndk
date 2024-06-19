using UnityEngine;

namespace UI.Buttons
{
    public class Button : MonoBehaviour
    {
        public Texture2D cursor;
        private readonly Vector2 _hotSpot = Vector2.zero;
        
        public void OnMouseOver()
        {
            Cursor.SetCursor(cursor, _hotSpot, CursorMode.ForceSoftware);
        }

        public void OnMouseExit()
        {
            Cursor.SetCursor(null, _hotSpot, CursorMode.ForceSoftware);
        }
    }
}