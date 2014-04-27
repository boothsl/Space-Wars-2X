using UnityEngine;

public class DemoGUI : MonoBehaviour
{
    protected InteractiveMusic music;

    int themeIndex;
    int modeIndex;

    string theme = "0";
    string mode = "0";

    void Start()
    {
        music = GetComponent<InteractiveMusic>();
    }

    void OnGUI()
    {

        // Background box
        GUI.Box(new Rect(10, 10, 200, 210), "Actions");

        // Play and Stop
        GUILayout.BeginHorizontal();
        if (GUI.Button(new Rect(20, 50, 80, 20), "Play All"))
            music.Play();
        if (GUI.Button(new Rect(120, 50, 80, 20), "Stop All"))
            music.Stop();
        GUILayout.EndHorizontal();

        // ChangeMode
        GUILayout.BeginHorizontal();
        GUI.Label(new Rect(20, 110, 50, 20), "Theme");
        theme = GUI.TextField(new Rect(90, 110, 110, 20), theme);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.Label(new Rect(20, 140, 50, 20), "Mode");
        mode = GUI.TextField(new Rect(90, 140, 110, 20), mode);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUI.Button(new Rect(90, 170, 110, 20), "Change Mode"))
            if (int.TryParse(theme, out themeIndex) && int.TryParse(mode, out modeIndex))
                music.ChangeMode(themeIndex, modeIndex);
            else
                music.ChangeMode(theme, mode);
        GUILayout.EndHorizontal();
    }

}
