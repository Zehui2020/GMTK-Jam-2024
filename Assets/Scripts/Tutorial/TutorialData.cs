using System.Collections.Generic;

using UnityEngine;

//This data will store each tutorial segment
[System.Serializable]
public class TutorialData
{
    public List<Vector3> ContainerPosition;

    [TextArea(5,5)]
    public List<string> InstructionsText;
}
