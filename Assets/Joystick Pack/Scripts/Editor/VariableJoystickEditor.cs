﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(VariableJoystick))]
public class VariableJoystickEditor : JoystickEditor
{
    private SerializedProperty moveThreshold;
    private SerializedProperty joystickType;

    protected override void OnEnable()
    {
        base.OnEnable();
        moveThreshold = serializedObject.FindProperty("moveThreshold");
        joystickType = serializedObject.FindProperty("joystickType");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (background != null)
        {
            RectTransform backgroundRect = (RectTransform)background.objectReferenceValue;
            backgroundRect.pivot = center;
        }
    }

    protected override void DrawValues()
    {
        base.DrawValues();
        EditorGUILayout.PropertyField(moveThreshold, new GUIContent("Move Threshold", "The distance away from the center input has to be before the __joystick begins to move."));
        EditorGUILayout.PropertyField(joystickType, new GUIContent("Joystick ItemKey", "The type of __joystick the variable __joystick is current using."));
    }
}