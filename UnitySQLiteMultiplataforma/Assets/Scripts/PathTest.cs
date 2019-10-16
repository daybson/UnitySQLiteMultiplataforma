using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathTest : MonoBehaviour
{
    public Text text;

    void Start()
    {
        text.text = $"DATA PATH: {Application.dataPath}";
        text.text += $"\n\nPERSISTENT DATA PATH: {Application.persistentDataPath}";
        text.text += $"\n\nSTREAMING ASSETS PATH: {Application.streamingAssetsPath}";
    }

}
