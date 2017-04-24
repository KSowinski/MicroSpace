using UnityEngine;

namespace Assets.Scripts
{
    public static class Inpt
    {
        public static bool IsEnabled { get; set; }

        public static bool GetMouseButton(int button)
        {
            return IsEnabled && Input.GetMouseButton(button);
        }

        public static bool GetMouseButtonDown(int button)
        {
            return IsEnabled && Input.GetMouseButtonDown(button);
        }

        public static bool GetKeyDown(KeyCode keyCode)
        {
            return IsEnabled && Input.GetKeyDown(keyCode);
        }

        public static Vector3 MousePosition()
        {
            return Input.mousePosition;
        }
    }
}
