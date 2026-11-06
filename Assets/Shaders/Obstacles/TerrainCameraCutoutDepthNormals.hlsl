#ifndef TERRAIN_CAMERA_CUTOUT_DEPTH_NORMALS_INCLUDED
#define TERRAIN_CAMERA_CUTOUT_DEPTH_NORMALS_INCLUDED

#define DepthNormalOnlyFragment TerrainOriginalDepthNormalOnlyFragment
#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitDepthNormalsPass.hlsl"
#undef DepthNormalOnlyFragment

#include "TerrainCameraCutout.hlsl"

void DepthNormalOnlyFragment(VaryingsDepthNormal IN, out half4 outNormalWS : SV_Target0
#ifdef _WRITE_RENDERING_LAYERS
    , out uint outRenderingLayers : SV_Target1
#endif
)
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
    ApplyTerrainCameraCutout(IN.clipPos);
    TerrainOriginalDepthNormalOnlyFragment(IN, outNormalWS
#ifdef _WRITE_RENDERING_LAYERS
        , outRenderingLayers
#endif
    );
}

#endif
