using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    void OnSceneGUI()
    {
        // get the chosen game object
        MapGenerator mapGen = target as MapGenerator;

        if (mapGen == null)
            return;


    }

}
