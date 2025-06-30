using UnityEngine;
using TMPro;

public class Labeller : MonoBehaviour
{
    public Transform aXLabel,
        bYLabel,
        joystickLabel,
        oculusMenuLabel,
        indexLabel,
        gripLabel;

    public string aXText,
        bYText,
        joystickText,
        oculusMenuText,
        indexText,
        gripText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        GetTMPText(aXLabel).text = aXText;
        GetTMPText(bYLabel).text = bYText;
        GetTMPText(joystickLabel).text = joystickText;
        GetTMPText(oculusMenuLabel).text = oculusMenuText;
        GetTMPText(indexLabel).text = indexText;
        GetTMPText(gripLabel).text = gripText;
    }

    private TMP_Text GetTMPText(Transform transform)
    {
        return transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        bYLabel.rotation
            = joystickLabel.rotation
            = oculusMenuLabel.rotation
            = indexLabel.rotation
            = gripLabel.rotation
            = aXLabel.rotation;
    }
}
