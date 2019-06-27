using System.Collections.Generic;
using System.Linq;
using Trismegistus.Core.Extensions;
using UnityEngine;

namespace Trismegistus.Core.Tools {
    /// <summary>
    ///     Example
    /// </summary>
    public class CombineMesh : MonoBehaviour {
        [SerializeField] private bool auto;
        [SerializeField] private bool eMergeSubMeshes = true;
        [SerializeField] private bool eUseMatrices = true;

        private void Start() {
            if (auto) Utils.CombineAll(gameObject, eMergeSubMeshes, eUseMatrices);
        }
    }

    /// <summary>
    ///     Mesh combining tools
    /// </summary>
    public static class Utils {
        /// <summary>
        ///     Combines all meshes by unique main material for each LOD (if exists)
        /// </summary>
        /// <param name="root">Parent gameObject</param>
        /// <param name="mergeSubMeshes">
        ///     Defines whether Meshes should be combined into a single sub-mesh.
        ///     <seealso cref="Mesh.CombineMeshes" />
        /// </param>
        /// <param name="useMatrices">
        ///     Defines whether the transforms supplied in the CombineInstance array should be used or
        ///     ignored.<seealso cref="Mesh.CombineMeshes" />
        /// </param>
        public static void CombineAll(GameObject root, bool mergeSubMeshes = true, bool useMatrices = true) {
            var lodGroup = root.GetComponent<LODGroup>();

            var gameObjects = new List<Renderer[]>();

            if (lodGroup != null)
                gameObjects.AddRange(lodGroup.GetLODs()
                    .Select(loD => loD.renderers));
            else
                gameObjects.Add(root.GetComponentsInChildren<Renderer>());

            foreach (var meshRenderers in gameObjects) CombineGroup(meshRenderers, mergeSubMeshes, useMatrices);
        }

        /// <summary>
        ///     Combines meshes from specific renderers by main material
        /// </summary>
        /// <param name="renderers"></param>
        /// <param name="mergeSubMeshes">
        ///     Defines whether Meshes should be combined into a single sub-mesh.
        ///     <seealso cref="Mesh.CombineMeshes" />
        /// </param>
        /// <param name="useMatrices">
        ///     Defines whether the transforms supplied in the CombineInstance array should be used or
        ///     ignored.<seealso cref="Mesh.CombineMeshes" />
        /// </param>
        public static void CombineGroup(Renderer[] renderers, bool mergeSubMeshes, bool useMatrices) {
            if (renderers == null || renderers.Length < 1) {
                Debug.Log("No renderers found");
                return;
            }

            Debug.Log($"Found {renderers.Length} renderers");

            var uniqueMaterials = renderers.Select(x => x.sharedMaterial).Distinct();
            var enumerable = uniqueMaterials as Material[] ?? uniqueMaterials.ToArray();
            Debug.Log($"Found {enumerable.Count()} unique materials");

            var matMeshes = enumerable.Select(mat => renderers.Where(x => x.sharedMaterial == mat)
                    .Select(x => x.GetComponent<MeshFilter>())
                    .ToArray())
                .ToList();

            foreach (var meshFilters in matMeshes) Combine(meshFilters, mergeSubMeshes, useMatrices);
        }

        /// <summary>
        ///     Combines meshes
        /// </summary>
        /// <param name="meshFilters"></param>
        /// <param name="mergeSubMeshes">
        ///     Defines whether Meshes should be combined into a single sub-mesh.
        ///     <seealso cref="Mesh.CombineMeshes" />
        /// </param>
        /// <param name="useMatrices">
        ///     Defines whether the transforms supplied in the CombineInstance array should be used or
        ///     ignored.<seealso cref="Mesh.CombineMeshes" />
        /// </param>
        public static void Combine(IReadOnlyList<MeshFilter> meshFilters, bool mergeSubMeshes, bool useMatrices) {
            if (meshFilters == null || meshFilters.Count < 1) {
                Debug.Log("No meshFilters found");
                return;
            }

            var combine = new CombineInstance[meshFilters.Count];

            var m = meshFilters[0];
            var t = m.transform;
            var mainMatrix = t.localToWorldMatrix;

            combine[0].mesh = m.sharedMesh;
            combine[0].transform = Matrix4x4.identity;


            for (var i = 1; i < meshFilters.Count; i++) {
                combine[i].mesh = meshFilters[i].sharedMesh;

                combine[i].transform = meshFilters[i].transform.LocalToLocalMatrix(mainMatrix);

                Object.Destroy(meshFilters[i].gameObject);
            }

            m.mesh = new Mesh();

            m.mesh.CombineMeshes(combine, mergeSubMeshes, useMatrices);
        }
    }
}