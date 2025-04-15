using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class UIButtonBinder : MonoBehaviour
{
    [System.Serializable]
    public class ButtonBinding
    {
        public string buttonName;
        public string methodName;
    }

    public List<ButtonBinding> bindings = new List<ButtonBinding>();

    private IEnumerator Start()
    {
        yield return null;

        var menu = MenuController.Instance;
        if (menu == null)
        {
            Debug.LogError("MenuController.Instance is null.");
            yield break;
        }

        foreach (var binding in bindings)
        {
            GameObject go = GameObject.Find(binding.buttonName);
            if (go == null)
            {
                Debug.LogWarning($"Button '{binding.buttonName}' not found.");
                continue;
            }

            Button btn = go.GetComponent<Button>();
            if (btn == null)
            {
                Debug.LogWarning($"'{binding.buttonName}' is missing Button component.");
                continue;
            }

            MethodInfo method = typeof(MenuController).GetMethod(binding.methodName);
            if (method == null)
            {
                Debug.LogWarning($"Method '{binding.methodName}' not found.");
                continue;
            }

            btn.onClick.RemoveAllListeners(); 
            btn.onClick.AddListener(() =>
            {
                Debug.Log($"[Binder] '{binding.buttonName}' → {binding.methodName}()");
                method.Invoke(menu, null);
            });
        }
    }
}
