using UnityEngine;

namespace Trismegistus.Core.Extensions {
    public static class TransformExtensions {
        public static Matrix4x4 LocalToLocalMatrix(this Transform t1, Transform t2) {
            return t2.localToWorldMatrix.inverse * t1.localToWorldMatrix;
        }
        
        public static Matrix4x4 LocalToLocalMatrix(this Transform t1, Matrix4x4 targetMatrix) {
            return targetMatrix.inverse * t1.localToWorldMatrix;
        }
    }
}