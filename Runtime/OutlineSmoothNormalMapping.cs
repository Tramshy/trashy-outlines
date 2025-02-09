using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrashyOutlines
{
    public class OutlineSmoothNormalMapping : MonoBehaviour
    {
        public enum ReasonForFailure
        {
            NoFailure,
            NoRenderer,
            NoMaterials,
            NoOutlineMaterial
        }

        [HideInInspector] public Mesh CurrentMesh, PreviousMesh;

        public Vector3[] SmoothNormals;

        [SerializeField] private bool _shouldAddOutlineOnAwake;

        private void Awake()
        {
            if (SmoothNormals.Length == 0)
                return;

            if (_shouldAddOutlineOnAwake)
                AddOutlines();

            foreach (MeshFilter filter in GetComponentsInChildren<MeshFilter>())
            {
                List<Vector3> uv3 = new List<Vector3>();

                filter.sharedMesh.GetUVs(3, uv3);

                if (SmoothNormals.ToList() != uv3)
                    filter.sharedMesh.SetUVs(3, SmoothNormals);

                return;
            }

            foreach (SkinnedMeshRenderer skinnedMesh in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                List<Vector3> uv3 = new List<Vector3>();

                skinnedMesh.sharedMesh.GetUVs(3, uv3);

                if (SmoothNormals.ToList() != uv3)
                    skinnedMesh.sharedMesh.SetUVs(3, SmoothNormals);
            }
        }

        /// <summary>
        /// Add and outline to any object.
        /// </summary>
        /// <param name="outline">The outline to use, if left null it will use the default outline instance in resources</param>
        /// <param name="mask">The mask to use, if left null it will use the default mask instance in resources. This should almost always be left null, unless you are trying to achieve something very specific</param>
        public void AddOutlines(Material outline = null, Material mask = null)
        {
            outline = outline ?? Resources.Load<Material>(@"Materials/Outline");
            mask = mask ?? Resources.Load<Material>(@"Materials/Outline Mask");

            var renderer = GetComponentInChildren<Renderer>();

            if (renderer == null)
                return;

            var l = renderer.materials.ToList();
            l.Add(mask);
            l.Add(outline);
            renderer.materials = l.ToArray();
        }

        public void LoadSmoothNormalsToObject(out ReasonForFailure reason)
        {
            reason = ReasonForFailure.NoFailure;

            bool foundRenderer = false;

            if (TryGetComponent(out MeshRenderer renderer))
            {
                if (renderer.sharedMaterials.Length == 0)
                {
                    reason = ReasonForFailure.NoMaterials;

                    return;
                }

                bool found = false;

                foundRenderer = true;

                foreach (Material m in renderer.sharedMaterials)
                {
                    if (m.shader == Resources.Load<Shader>(@"TOutline"))
                    {
                        found = true;

                        break;
                    }
                }

                if (!found)
                    reason = ReasonForFailure.NoOutlineMaterial;
            }

            if (TryGetComponent(out SkinnedMeshRenderer skinnedRenderer))
            {
                if (skinnedRenderer.sharedMaterials.Length == 0)
                {
                    reason = ReasonForFailure.NoMaterials;

                    return;
                }

                bool found = false;

                foundRenderer = true;

                foreach (Material m in skinnedRenderer.sharedMaterials)
                {
                    if (m.shader == Resources.Load<Shader>(@"Outline ShaderGraph") || m.shader == Resources.Load<Shader>(@"OutlineHLSL"))
                    {
                        found = true;

                        break;
                    }
                }

                if (!found)
                    reason = ReasonForFailure.NoOutlineMaterial;
            }

            if (!foundRenderer)
            {
                reason = ReasonForFailure.NoRenderer;

                return;
            }

            foreach (MeshFilter filter in GetComponentsInChildren<MeshFilter>())
            {
                List<Vector3> smoothNormalMap = CalculateSmoothNormals(filter.sharedMesh);

                // Set UV coordinate at third texture slot to smooth normal map.
                filter.sharedMesh.SetUVs(3, smoothNormalMap);
                SmoothNormals = smoothNormalMap.ToArray();

                if (!TryGetComponent(out Renderer thisRenderer))
                {
                    reason = ReasonForFailure.NoRenderer;

                    return;
                }

                CombineSubmeshes(filter.sharedMesh, thisRenderer.sharedMaterials);

                return;
            }

            foreach (SkinnedMeshRenderer skinnedMesh in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                List<Vector3> smoothNormalMap = CalculateSmoothNormals(skinnedMesh.sharedMesh);

                // Set UV coordinate at third texture slot to smooth normal map.
                skinnedMesh.sharedMesh.SetUVs(3, smoothNormalMap);
                SmoothNormals = smoothNormalMap.ToArray();

                CombineSubmeshes(skinnedMesh.sharedMesh, skinnedRenderer.sharedMaterials);
            }

            return;
        }

        public List<Vector3> CalculateSmoothNormals(Mesh mesh)
        {
            /*
             * First creates a KeyValuePair for all vertices in a mesh based on the key of their position and the value of their index in the mesh vertices array.
             * Then creates a group for each KeyValuePair based on their Key, this basically creates storage for all vertices of the same position with their index position.
             */
            var vertexGroups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy((thisGroup) => thisGroup.Key);

            List<Vector3> smoothNormals = new List<Vector3>(mesh.normals);

            foreach (var group in vertexGroups)
            {
                // If the vertex is alone, then it will keep it's normal.
                if (group.Count() <= 1)
                    continue;

                Vector3 smoothNormal = Vector3.zero;

                foreach (KeyValuePair<Vector3, int> pair in group)
                {
                    // Add together all the normals of the vertices of the same position.
                    smoothNormal += smoothNormals[pair.Value];
                }

                smoothNormal.Normalize();

                foreach (KeyValuePair<Vector3, int> pair in group)
                {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }

        // TODO: Understand this, I stole this crap
        private void CombineSubmeshes(Mesh mesh, Material[] materials)
        {
            // Skip meshes with a single submesh
            if (mesh.subMeshCount == 1)
            {
                return;
            }

            // Skip if submesh count exceeds material count
            if (mesh.subMeshCount > materials.Length)
            {
                return;
            }

            mesh.subMeshCount++;
            mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        }
    }
}
