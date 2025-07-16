using UnityEngine;
using UnityEngine.UI;

public class MappingManager : MonoBehaviour
{
    public MonoBehaviour[] controlMappings; 
    public ToggleGroup toggleGroup;         
    public Toggle[] toggles;              

    private SimpleControlMapping current;
    private int activeIndex = -1;

    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].group = toggleGroup;
            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    SetMapping(index);
                }
            });
        }
        if (toggles.Length > 0 && toggles[0].isOn)
        {
            SetMapping(0);
        }
    }

    public void SetMapping(int index)
    {
        if (index < 0 || index >= controlMappings.Length) return;

        current?.Disable();

        if (controlMappings[index] is SimpleControlMapping next)
        {
            current = next;
            current.Enable();
            activeIndex = index;

            Debug.Log($"Switched to mapping {index}: {next.GetType().Name}");
        }
        else
        {
            Debug.LogError($"Mapping {index} does not implement IControlMapping.");
        }
    }
}
