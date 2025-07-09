using RosSharp.RosBridgeClient;
using TMPro;
using UnityEngine;

public class ControlFlowManager : MonoBehaviour
{
    private ControlFlow activeFlow = new SampleFlow();

    public GameObject rosControllerOne, dummyGripper;
    public TMP_Text infoText;
    public GameObject[] labelObjs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeFlow.spotOne = new SpotInterface(rosControllerOne, dummyGripper);

        var labels = new TMP_Text[labelObjs.Length];
        for (int i = 0; i < labelObjs.Length; i++)
            labels[i] = labelObjs[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
        activeFlow.labels = labels;
        activeFlow.infoText = infoText;

        activeFlow.Start();
    }

    // Update is called once per frame
    void Update()
    {
        activeFlow.SuperUpdate();    
    }
}
