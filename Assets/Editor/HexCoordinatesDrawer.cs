using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinatesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HexCoordinates coordinates = new HexCoordinates(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue
            );

        GUIStyle style = new GUIStyle();
        style.padding = new RectOffset(18, 0, 4, 0);
        position = EditorGUI.PrefixLabel(position, label, style);
        GUIStyle style2 = new GUIStyle();
        style2.padding = new RectOffset(0, 0, 4, 0);
        GUI.Label(position, coordinates.ToString(), style2);
    }
}
