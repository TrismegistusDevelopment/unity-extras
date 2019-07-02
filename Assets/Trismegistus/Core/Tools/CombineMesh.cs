using System;
using System.Collections.Generic;
using System.Linq;
using Trismegistus.Core.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Trismegistus.Core.Tools {
    /// <summary>
    ///     Example
    /// </summary>
    public class CombineMesh : MonoBehaviour {
        [SerializeField] private bool auto = false;
        [SerializeField] private CombineMeshSettings settings = new CombineMeshSettings(true);

        private void Start() {
            if (auto) CombineMeshUtils.CombineAll(gameObject, settings);
        }
    }

    [Serializable]
    public struct CombineMeshSettings {
        public bool MergeSubMeshes;
        public bool UseMatrices;
        public bool SkipMultiMaterials;

        public CombineMeshSettings(bool mergeSubMeshes = true, bool useMatrices = true,
            bool skipMultiMaterials = false) {
            MergeSubMeshes = mergeSubMeshes;
            UseMatrices = useMatrices;
            SkipMultiMaterials = skipMultiMaterials;
        }
    }

    /// <summary>
    ///     Mesh combining tools
    /// </summary>
    public static class CombineMeshUtils {
        /// <summary>
        ///     Combines all meshes by unique main material for each LOD (if exists)
        /// </summary>
        /// <param name="root">Parent gameObject</param>
        /// <param name="settings"></param>
        public static void CombineAll(GameObject root, CombineMeshSettings settings) {
            var lodGroup = root.GetComponent<LODGroup>();

            var gameObjects = new List<Renderer[]>();

            if (lodGroup != null)
                gameObjects.AddRange(lodGroup.GetLODs()
                    .Select(loD => loD.renderers));
            else
                gameObjects.Add(root.GetComponentsInChildren<Renderer>());

            foreach (var meshRenderers in gameObjects) {
                var renderers = meshRenderers
                    .Where(r => r != null)
                    .Where(r => r.sharedMaterial != null)
                    .ToArray();
                if (settings.SkipMultiMaterials)
                    renderers = renderers.Where(r => r.sharedMaterials.Count() == 1).ToArray();
                CombineGroup(
                    renderers,
                    settings);
            }
        }

        /// <summary>
        ///     Combines meshes from specific renderers by main material
        /// </summary>
        /// <param name="renderers"></param>
        /// <param name="settings"></param>
        public static void CombineGroup(Renderer[] renderers, CombineMeshSettings settings) {
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

            foreach (var meshFilters in matMeshes) Combine(meshFilters.Where(mf => mf != null).ToArray(), settings);
        }

        /// <summary>
        ///     Combines meshes
        /// </summary>
        /// <param name="meshFilters"></param>
        /// <param name="settings"></param>
        public static void Combine(IReadOnlyList<MeshFilter> meshFilters, CombineMeshSettings settings) {
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

            m.mesh.CombineMeshes(combine, settings.MergeSubMeshes, settings.UseMatrices);
        }
    }
}