using UnityEngine;

namespace Btools.Extensions
{
    public static class ExtensionMethods
    {
        /// <summary>Gets the gameobject from the RaycastHit</summary>
        /// <param name="Raycast">The raycast to convert to a gameobject</param>
        /// <returns>the gameobject from the RaycastHit</returns>
        public static GameObject GetGameobject(this RaycastHit Raycast)
        {
            return Raycast.transform.gameObject;
        }

        /// <summary>Gets the gameobject from the RaycastHit2D</summary>
        /// <param name="Raycast">The raycast to convert to a gameobject</param>
        /// <returns>the gameobject from the RaycastHit2D</returns>
        public static GameObject GetGameobject(this RaycastHit2D Raycast)
        {
            return Raycast.transform.gameObject;
        }
    }
}