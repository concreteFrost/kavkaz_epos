#ifndef TERRAIN_CAMERA_CUTOUT_INCLUDED
#define TERRAIN_CAMERA_CUTOUT_INCLUDED

// _CutoutSize, _Dissolve, множитель перед Dither, включение эффекта.
float4 _TerrainCutoutSettings;
// Смещение Alpha после Dither и _AlphaThreshold исходного графа.
float4 _TerrainCutoutAlpha;
// Граница выреза в пространстве камеры: более дальние поверхности сохраняются.
float _TerrainCutoutDepth;

void ApplyTerrainCameraCutout(float4 positionCS)
{
    if (_TerrainCutoutSettings.w == 0.0)
        return;

    float eyeDepth = IsPerspectiveProjection()
        ? LinearEyeDepth(positionCS.z, _ZBufferParams)
        : LinearDepthToEyeDepth(positionCS.z);
    if (eyeDepth >= _TerrainCutoutDepth)
        return;

    // Повторяет цепочку Screen Position -> Distance -> Smoothstep -> Multiply исходного графа.
    float2 screenUV = GetNormalizedScreenSpaceUV(positionCS.xy);
    float aspect = _ScreenParams.x / _ScreenParams.y;
    float radialDistance = length((screenUV - 0.5) / float2(1.0, aspect));
    float visibility = smoothstep(_TerrainCutoutSettings.x, _TerrainCutoutSettings.y, radialDistance)
        * _TerrainCutoutSettings.z;

    // Тот же Bayer 4x4 и порядок индексов, что у узла Shader Graph Dither.
    float2 ditherUV = screenUV * _ScreenParams.xy;
    const float thresholds[16] =
    {
         1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
        13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
         4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
        16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
    };
    uint index = (uint(ditherUV.x) % 4) * 4 + uint(ditherUV.y) % 4;
    clip(visibility - thresholds[index] + _TerrainCutoutAlpha.x - _TerrainCutoutAlpha.y);
}

#endif
