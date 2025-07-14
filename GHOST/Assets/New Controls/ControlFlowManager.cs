using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlFlowManager : MonoBehaviour
{
    private ControlFlow initLeftFlow = new SampleDriveFlow(), initRightFlow = new SampleDriveFlow();

    public GameObject rosConnectorOne, dummyGripperOne, handViewOne,
        rosConnectorTwo, dummyGripperTwo, handViewTwo,
        leftHandAnchor, rightHandAnchor;
    public SkinnedMeshRenderer leftHandRenderer, rightHandRenderer;
    public GameObject[] leftLabelObjs, rightLabelObjs;
    public TMP_Text leftInfoText, rightInfoText;

    private readonly Dictionary<ControlFlow.Button, OVRInput.Button> leftButtonOvrMapping = new()
    {
        { ControlFlow.Button.AOrX, OVRInput.Button.Three },
        { ControlFlow.Button.BOrY, OVRInput.Button.Four },
        { ControlFlow.Button.Joystick, OVRInput.Button.PrimaryThumbstick },
        { ControlFlow.Button.Trigger, OVRInput.Button.PrimaryIndexTrigger },
        { ControlFlow.Button.Grip, OVRInput.Button.PrimaryHandTrigger }
    };

    private readonly Dictionary<ControlFlow.Button, OVRInput.Button> rightButtonOvrMapping = new()
    {
        { ControlFlow.Button.AOrX, OVRInput.Button.One },
        { ControlFlow.Button.BOrY, OVRInput.Button.Two },
        { ControlFlow.Button.Joystick, OVRInput.Button.SecondaryThumbstick },
        { ControlFlow.Button.Trigger, OVRInput.Button.SecondaryIndexTrigger },
        { ControlFlow.Button.Grip, OVRInput.Button.SecondaryHandTrigger }
    };

    private ControlFlow leftFlow, rightFlow;

    private SpotInterface spotOne, spotTwo;
    private TMP_Text[] leftLabels, rightLabels;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        spotOne = new(rosConnectorOne, dummyGripperOne, handViewOne);
        spotTwo = new(rosConnectorTwo, dummyGripperTwo, handViewTwo);

        leftLabels = new TMP_Text[leftLabelObjs.Length];
        for (int i = 0; i < leftLabelObjs.Length; i++)
            leftLabels[i] = GetLabel(leftLabelObjs[i]);

        rightLabels = new TMP_Text[rightLabelObjs.Length];
        for (int i = 0; i < rightLabelObjs.Length; i++)
            rightLabels[i] = GetLabel(rightLabelObjs[i]);

        leftFlow = initLeftFlow;
        rightFlow = initRightFlow;
        TransitionLeft(initLeftFlow);
        TransitionRight(initRightFlow);
    }

    private TMP_Text GetLabel(GameObject obj)
    {
        return obj.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
    }

    private void TransitionLeft(ControlFlow flow)
    {
        flow.SuperStart(
            spotOne,
            leftButtonOvrMapping,
            OVRInput.Axis2D.PrimaryThumbstick,
            leftHandAnchor,
            leftHandRenderer,
            leftLabels,
            leftInfoText,
            leftFlow.infoTextLines,
            TransitionLeft
            );
        leftFlow = flow;
    }

    private void TransitionRight(ControlFlow flow)
    {
        flow.SuperStart(
           spotOne,
           rightButtonOvrMapping,
           OVRInput.Axis2D.SecondaryThumbstick,
           rightHandAnchor,
           rightHandRenderer,
           rightLabels,
           rightInfoText,
           rightFlow.infoTextLines,
           TransitionRight
           );
        rightFlow = flow;
    }

    // Update is called once per frame
    void Update()
    {
        leftFlow.SuperUpdate();
        rightFlow.SuperUpdate();
    }
}
