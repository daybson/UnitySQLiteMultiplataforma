using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathTest : MonoBehaviour
{
    public Text uiText;

    void Start()
    {
        uiText.text = $"Data Path:{Application.dataPath}\n\n Persistent DataPath:{Application.persistentDataPath}\n\nStreaming Assets:{Application.streamingAssetsPath}";
    }
}
