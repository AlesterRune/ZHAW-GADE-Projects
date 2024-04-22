using UnityEditor;

namespace CUI.Panels
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CUIView))]
    public class CUIViewEditor : Editor
    {

        private Editor _editor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();
            
            var viewData = serializedObject.FindProperty("viewData");
            CreateCachedEditor(viewData.objectReferenceValue, null, ref _editor);
            _editor.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}