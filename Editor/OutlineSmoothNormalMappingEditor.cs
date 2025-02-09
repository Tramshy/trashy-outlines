using UnityEngine;
using UnityEditor;

namespace TrashyOutlines.OutlineEditor
{
    [CustomEditor(typeof(OutlineSmoothNormalMapping))]
    public class OutlineSmoothNormalMappingEditor : Editor
    {
        private string _errorMessage;
        private bool _showError;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            OutlineSmoothNormalMapping outlineSmoothNormal = (OutlineSmoothNormalMapping)target;

            outlineSmoothNormal.CurrentMesh = outlineSmoothNormal.TryGetComponent(out MeshFilter mF) ? mF.sharedMesh : outlineSmoothNormal.TryGetComponent(out SkinnedMeshRenderer sMF) ? sMF.sharedMesh : null;

            if (outlineSmoothNormal.CurrentMesh == null)
                return;

            if (GUILayout.Button("Recalculate Smooth Normals"))
            {
                outlineSmoothNormal.LoadSmoothNormalsToObject(out OutlineSmoothNormalMapping.ReasonForFailure reason);

                if (reason != OutlineSmoothNormalMapping.ReasonForFailure.NoFailure)
                {
                    switch (reason)
                    {
                        default:
                        case OutlineSmoothNormalMapping.ReasonForFailure.NoOutlineMaterial:
                            Debug.LogWarning("No outline found on: " + outlineSmoothNormal.gameObject.name + ".");

                            break;

                        case OutlineSmoothNormalMapping.ReasonForFailure.NoRenderer:
                            Debug.LogError("No renderer found on: " + outlineSmoothNormal.gameObject.name + ".");

                            _errorMessage = "No renderer found, add one to avoid unexpected behavior.";
                            _showError = true;

                            return;

                        case OutlineSmoothNormalMapping.ReasonForFailure.NoMaterials:
                            Debug.LogError("No materials found on: " + outlineSmoothNormal.gameObject.name + ".");

                            _errorMessage = "No materials found, at least one material slot is needed to use outlines.\nMore than one is needed to avoid unexpected behavior.";
                            _showError = true;

                            return;
                    }
                }

                outlineSmoothNormal.PreviousMesh = outlineSmoothNormal.CurrentMesh;

                Debug.Log("Smooth normals for: " + outlineSmoothNormal.gameObject.gameObject.name + ", have been recalculated successfully!");
            }

            if (_showError)
            {
                EditorGUILayout.HelpBox(_errorMessage, MessageType.Error);

                return;
            }

            if (outlineSmoothNormal.PreviousMesh != outlineSmoothNormal.CurrentMesh)
                EditorGUILayout.HelpBox("Mesh changes detected, recalculation of smooth normals is recommended.", MessageType.Warning);
        }
    }
}
