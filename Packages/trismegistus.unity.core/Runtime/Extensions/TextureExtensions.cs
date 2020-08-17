using UnityEngine;

namespace Trismegistus.Core.Extensions {
    public static class TextureExtensions {
        public static Texture2D Texture2D(this RenderTexture rt, Camera camera) {
            var currentActiveRt = RenderTexture.active;

            RenderTexture.active = rt;
            
            camera.Render();
            
            var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            tex.SetPixel(24, 24, Color.red);
            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0, false);
            tex.Apply();

            
            RenderTexture.active = currentActiveRt;
            return tex;
        }

        public static Texture2D Texture2D(this Camera camera) => 
            Texture2D(camera.targetTexture, camera);
    }
}