using System.Collections;
using UnityEngine;

public class Fps : MonoBehaviour
{
    public float x;
    public float y;
    public float width;
    public float height;
    public int fontSize;

    private float count;

    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnGUI()
    {
        GUIStyle myLabelStyle = null;
        if (myLabelStyle == null)
        {
            myLabelStyle = new GUIStyle(GUI.skin.label);
            myLabelStyle.fontSize = fontSize;
        }
        GUI.Label(new Rect(x, y, width, height), "FPS: " + Mathf.Round(count), myLabelStyle);
    }
}