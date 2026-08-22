using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackDefinition))]
public class AttackDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AttackDefinition attack =
            (AttackDefinition)target;

        DrawDefaultInspector();

        EditorGUILayout.Space(12);

        DrawTimingPreview(attack);
    }

    private void DrawTimingPreview(
        AttackDefinition attack)
    {
        EditorGUILayout.LabelField(
            "Timing Preview",
            EditorStyles.boldLabel);

        if (attack.animationClip == null)
        {
            EditorGUILayout.HelpBox(
                "Assign an Animation Clip to preview timing.",
                MessageType.Info);

            return;
        }

        EditorGUILayout.LabelField(
            $"Frame Rate: {attack.FrameRate:F1} FPS");

        EditorGUILayout.LabelField(
            $"Frames in Clip: {attack.FrameCount}");

        EditorGUILayout.LabelField(
            $"Duration: {attack.Duration:F3} s");

        EditorGUILayout.LabelField(
            $"Animation Start: {attack.animationStartFrame}");

        EditorGUILayout.Space(6);

        DrawTiming(
            "Hit Start",
            attack.hitStartFrame,
            attack.HitStartFrame,
            attack.HitStart);

        DrawTiming(
            "Hit End",
            attack.hitEndFrame,
            attack.HitEndFrame,
            attack.HitEnd);

        EditorGUILayout.Space(4);

        DrawTiming(
            "Combo Start",
            attack.comboStartFrame,
            attack.ComboStartFrame,
            attack.ComboStart);

        DrawTiming(
            "Combo End",
            attack.comboEndFrame,
            attack.ComboEndFrame,
            attack.ComboEnd);

        EditorGUILayout.Space(4);

        DrawTiming(
            "Self Move Start",
            attack.selfMoveStartFrame,
            attack.SelfMoveStartFrame,
            attack.SelfMoveStart);

        DrawTiming(
            "Self Move End",
            attack.selfMoveEndFrame,
            attack.SelfMoveEndFrame,
            attack.SelfMoveEnd);

        EditorGUILayout.LabelField(
            "Self Move Force",
            attack.selfMoveForce.ToString("F2"));

        EditorGUILayout.Space(4);

        DrawTiming(
            "Trail Start",
            attack.trailStartFrame,
            attack.TrailStartFrame,
            attack.TrailStart);

        DrawTiming(
            "Trail End",
            attack.trailEndFrame,
            attack.TrailEndFrame,
            attack.TrailEnd);

        EditorGUILayout.Space(8);

        DrawWarnings(attack);
    }

    private void DrawTiming(
        string label,
        int originalFrame,
        int gameFrame,
        float time)
    {
        EditorGUILayout.LabelField(
            label,
            $"Original: {originalFrame}  |  " +
            $"Game: {gameFrame}  |  " +
            $"{time:F3} s");
    }

    private void DrawWarnings(
        AttackDefinition attack)
    {
        int frameCount = attack.FrameCount;

        if (attack.HitStartFrame < 0)
        {
            EditorGUILayout.HelpBox(
                $"Hit Start ({attack.hitStartFrame}) is before the animation start " +
                $"({attack.animationStartFrame}).",
                MessageType.Error);
        }

        if (attack.HitEndFrame < 0)
        {
            EditorGUILayout.HelpBox(
                $"Hit End ({attack.hitEndFrame}) is before the animation start " +
                $"({attack.animationStartFrame}).",
                MessageType.Error);
        }

        if (attack.HitStartFrame > frameCount)
        {
            EditorGUILayout.HelpBox(
                $"Hit Start ({attack.HitStartFrame}) is outside the clip " +
                $"({frameCount} frames).",
                MessageType.Warning);
        }

        if (attack.HitEndFrame > frameCount)
        {
            EditorGUILayout.HelpBox(
                $"Hit End ({attack.HitEndFrame}) is outside the clip " +
                $"({frameCount} frames).",
                MessageType.Warning);
        }

        if (attack.ComboStartFrame < 0 ||
            attack.ComboStartFrame > frameCount)
        {
            EditorGUILayout.HelpBox(
                $"Combo Start ({attack.ComboStartFrame}) is outside the clip.",
                MessageType.Warning);
        }

        if (attack.ComboEndFrame < 0 ||
            attack.ComboEndFrame > frameCount)
        {
            EditorGUILayout.HelpBox(
                $"Combo End ({attack.ComboEndFrame}) is outside the clip.",
                MessageType.Warning);
        }

        if (attack.HitEndFrame < attack.HitStartFrame)
        {
            EditorGUILayout.HelpBox(
                "Hit End is before Hit Start.",
                MessageType.Error);
        }

        if (attack.ComboEndFrame < attack.ComboStartFrame)
        {
            EditorGUILayout.HelpBox(
                "Combo End is before Combo Start.",
                MessageType.Error);
        }

        if (attack.SelfMoveStartFrame < 0)
        {
            EditorGUILayout.HelpBox(
                $"Self Move Start ({attack.selfMoveStartFrame}) " +
                $"is before the animation start " +
                $"({attack.animationStartFrame}).",
                MessageType.Error);
        }

        if (attack.SelfMoveEndFrame < 0)
        {
            EditorGUILayout.HelpBox(
                $"Self Move End ({attack.selfMoveEndFrame}) " +
                $"is before the animation start " +
                $"({attack.animationStartFrame}).",
                MessageType.Error);
        }

        if (attack.SelfMoveStartFrame > frameCount)
        {
            EditorGUILayout.HelpBox(
                $"Self Move Start ({attack.SelfMoveStartFrame}) " +
                $"is outside the clip ({frameCount} frames).",
                MessageType.Warning);
        }

        if (attack.SelfMoveEndFrame > frameCount)
        {
            EditorGUILayout.HelpBox(
                $"Self Move End ({attack.SelfMoveEndFrame}) " +
                $"is outside the clip ({frameCount} frames).",
                MessageType.Warning);
        }

        if (attack.SelfMoveEndFrame < attack.SelfMoveStartFrame)
        {
            EditorGUILayout.HelpBox(
                "Self Move End is before Self Move Start.",
                MessageType.Error);
        }

        if (attack.TrailEndFrame < attack.TrailStartFrame)
        {
            EditorGUILayout.HelpBox(
                "Trail End is before Trail Start.",
                MessageType.Error);
        }
    }
}