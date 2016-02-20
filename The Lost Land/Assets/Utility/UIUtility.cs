using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Collections;

public class UIUtility
{
    public static void RemoveAll(Button button)
    {
        if(button)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public static void AddAction(Button button, System.Action action)
    {
        if(button)
        {
            button.onClick.AddListener(new UnityAction(action));
        }
    }
}
