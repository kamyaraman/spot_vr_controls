using UnityEngine;
using TMPro;

public class Labeller : MonoBehaviour
{
    public Transform controller;

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
        Transform[] labels = { aXLabel, bYLabel, joystickLabel, oculusMenuLabel, indexLabel, gripLabel };

        for (int i = 1; i < labels.Length; i++)
        {
            labels[i].rotation = labels[0].rotation;
        }

        Vector3 toController = (controller.position - Camera.main.transform.position).normalized;
        float gazeDelta = Vector3.Angle(Camera.main.transform.forward, toController);
        foreach (Transform label in labels)
        {
            label.gameObject.SetActive(gazeDelta <= 15f);
        }
    }
}
