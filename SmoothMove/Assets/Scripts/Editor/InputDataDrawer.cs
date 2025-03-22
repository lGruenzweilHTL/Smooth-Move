using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FlightController.InputData))]
public class InputDataDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the property label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label, EditorStyles.boldLabel);

        // Save the original indent level and set it to 0
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Draw the mode field
        var modeProperty = property.FindPropertyRelative(nameof(FlightController.InputData.mode));
        position.height = EditorGUI.GetPropertyHeight(modeProperty);
        EditorGUI.PropertyField(position, modeProperty);

        // Get the current mode value
        var mode = (FlightController.InputMode)modeProperty.enumValueIndex;

        // Draw the speed field for all modes except Disabled
        if (mode != FlightController.InputMode.Disabled) {
            var speedProperty = property.FindPropertyRelative(nameof(FlightController.InputData.speed));
            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUI.GetPropertyHeight(speedProperty);
            EditorGUI.PropertyField(position, speedProperty);
        }

        // Draw the relevant fields based on the mode
        switch (mode) {
            case FlightController.InputMode.Axis:
                DrawAxisFields(ref position, property);
                break;
            case FlightController.InputMode.Buttons:
                DrawButtonFields(ref position, property);
                break;
            case FlightController.InputMode.Keys:
                DrawKeyFields(ref position, property);
                break;
        }

        // Restore the original indent level
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    private void DrawAxisFields(ref Rect position, SerializedProperty property) {
        // Draw the useRawAxis field
        var useRawAxisProperty = property.FindPropertyRelative(nameof(FlightController.InputData.useRawAxis));
        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
        position.height = EditorGUI.GetPropertyHeight(useRawAxisProperty);
        EditorGUI.PropertyField(position, useRawAxisProperty);

        // Draw the axisName field
        var axisNameProperty = property.FindPropertyRelative(nameof(FlightController.InputData.axisName));
        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
        position.height = EditorGUI.GetPropertyHeight(axisNameProperty);
        EditorGUI.PropertyField(position, axisNameProperty);
        
        // Draw the invert field
        var invertAxisProperty = property.FindPropertyRelative(nameof(FlightController.InputData.invert));
        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
        position.height = EditorGUI.GetPropertyHeight(invertAxisProperty);
        EditorGUI.PropertyField(position, invertAxisProperty);
    }

    private void DrawButtonFields(ref Rect position, SerializedProperty property) {
        // Draw the positiveButtonName field
        var positiveButtonNameProperty = property.FindPropertyRelative(nameof(FlightController.InputData.positiveButtonName));
        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
        position.height = EditorGUI.GetPropertyHeight(positiveButtonNameProperty);
        EditorGUI.PropertyField(position, positiveButtonNameProperty);

        // Draw the negativeButtonName field
        var negativeButtonNameProperty = property.FindPropertyRelative(nameof(FlightController.InputData.negativeButtonName));
        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
        position.height = EditorGUI.GetPropertyHeight(negativeButtonNameProperty);
        EditorGUI.PropertyField(position, negativeButtonNameProperty);
    }

    private void DrawKeyFields(ref Rect position, SerializedProperty property) {
        // Draw the positiveKey field
        var positiveKeyProperty = property.FindPropertyRelative(nameof(FlightController.InputData.positiveKey));
        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
        position.height = EditorGUI.GetPropertyHeight(positiveKeyProperty);
        EditorGUI.PropertyField(position, positiveKeyProperty);

        // Draw the negativeKey field
        var negativeKeyProperty = property.FindPropertyRelative(nameof(FlightController.InputData.negativeKey));
        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
        position.height = EditorGUI.GetPropertyHeight(negativeKeyProperty);
        EditorGUI.PropertyField(position, negativeKeyProperty);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        float height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(FlightController.InputData.mode))) + EditorGUIUtility.standardVerticalSpacing;
        var mode = (FlightController.InputMode)property.FindPropertyRelative(nameof(FlightController.InputData.mode)).enumValueIndex;

        if (mode != FlightController.InputMode.Disabled) {
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(FlightController.InputData.speed))) + EditorGUIUtility.standardVerticalSpacing;
        }

        switch (mode) {
            case FlightController.InputMode.Axis:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(FlightController.InputData.useRawAxis))) + EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(FlightController.InputData.axisName))) + EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(FlightController.InputData.invert))) + EditorGUIUtility.standardVerticalSpacing;
                break;
            case FlightController.InputMode.Buttons:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(FlightController.InputData.positiveButtonName))) + EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(FlightController.InputData.negativeButtonName))) + EditorGUIUtility.standardVerticalSpacing;
                break;
            case FlightController.InputMode.Keys:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(FlightController.InputData.positiveKey))) + EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(FlightController.InputData.negativeKey))) + EditorGUIUtility.standardVerticalSpacing;
                break;
        }

        return height;
    }
}