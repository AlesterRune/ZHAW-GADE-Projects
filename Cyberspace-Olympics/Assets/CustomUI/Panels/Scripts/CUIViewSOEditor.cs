using UnityEditor;

namespace CUI.Panels
{
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CUIViewSO))]
    public class CUIViewSOEditor : Editor
    {
        private Editor _editor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();
            
            var theme = serializedObject.FindProperty("theme");
            CreateCachedEditor(theme.objectReferenceValue, null, ref _editor);
            _editor.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}