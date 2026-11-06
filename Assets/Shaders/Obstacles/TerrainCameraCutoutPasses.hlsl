#ifndef TERRAIN_CAMERA_CUTOUT_PASSES_INCLUDED
#define TERRAIN_CAMERA_CUTOUT_PASSES_INCLUDED

// Штатные проходы URP сохраняют смешивание слоёв, освещение и отверстия Terrain.
#define SplatmapFragment TerrainOriginalSplatmapFragment
#define DepthOnlyFragment TerrainOriginalDepthOnlyFragment
#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
#undef SplatmapFragment
#undef DepthOnlyFragment

#include "TerrainCameraCutout.hlsl"

#ifdef TERRAIN_GBUFFER
GBufferFragOutput SplatmapFragment(Varyings IN)
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
    ApplyTerrainCameraCutout(IN.clipPos);
    return TerrainOriginalSplatmapFragment(IN);
}
#else
void SplatmapFragment(Varyings IN, out half4 outColor : SV_Target0
#ifdef _WRITE_RENDERING_LAYERS
    , out uint outRenderingLayers : SV_Target1
#endif
)
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
    ApplyTerrainCameraCutout(IN.clipPos);
    TerrainOriginalSplatmapFragment(IN, outColor
#ifdef _WRITE_RENDERING_LAYERS
        , outRenderingLayers
#endif
    );
}
#endif

half4 DepthOnlyFragment(VaryingsLean IN) : SV_TARGET
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
#ifndef SCENESELECTIONPASS
    ApplyTerrainCameraCutout(IN.clipPos);
#endif
    return TerrainOriginalDepthOnlyFragment(IN);
}

#endif
