# unity-extras

## Installation

- Open file `manifest.json` in `your_repo/Packages`
- Under `dependencies` add following line:

```json
"trismegistus.core": "https://github.com/TrismegistusDevelopment/unity-extras.git#upm",
```

- Reopen your project in Unity

You _should_ commit `manifest.json`

## Content

- __Types__
  - __MinMaxFloat__ - ranged value with slider-style inspector
  - __UnityEvents__ - common Unity events
- __Tools__
  - __CombineMesh__ - combines meshes from different go with material grouping
  - __ScenePicker__ - inspector for picking Scene path from scene asset
  - __ScriptingDefineSymbolsTool__ - changing Scripting Define Symbols (`#DEFINE` analogue) from code in editor
- __Extensions__
  - __TextureExtensions__
    - Camera to Texture2D
    - RenderTexture to Texture2D
  - __TransformExtensions__
    - LocalToLocalMatrix using target Transform
    - LocalToLocalMatrix using target Matrix4x4
