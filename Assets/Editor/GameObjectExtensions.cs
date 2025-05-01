using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetActiveAll(this GameObject[] gameObjects, bool state)
    {
        foreach (var obj in gameObjects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }
}