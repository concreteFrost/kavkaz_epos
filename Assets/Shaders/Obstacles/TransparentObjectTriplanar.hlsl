#ifndef TRANSPARENT_OBJECT_TRIPLANAR_INCLUDED
#define TRANSPARENT_OBJECT_TRIPLANAR_INCLUDED

// Амплитуда, как в исходном графе, задаётся в сантиметрах; высота берётся из G.
float2 TransparentObjectParallax(UnityTexture2D heightMap, float2 uv, float3 view, float amplitude)
{
    float height = SAMPLE_TEXTURE2D(heightMap.tex, heightMap.samplerstate, uv).g;
    view = normalize(view);
    return (height - 0.5) * amplitude * 0.01 * view.xy / (abs(view.z) + 0.42);
}

float4 TransparentObjectSample(UnityTexture2D map, float2 uvX, float2 uvY, float2 uvZ, float3 weights)
{
    return SAMPLE_TEXTURE2D(map.tex, map.samplerstate, uvX) * weights.x
        + SAMPLE_TEXTURE2D(map.tex, map.samplerstate, uvY) * weights.y
        + SAMPLE_TEXTURE2D(map.tex, map.samplerstate, uvZ) * weights.z;
}

void TransparentObjectTriplanar_float(
    UnityTexture2D BaseMap, UnityTexture2D NormalMap, UnityTexture2D MetallicMap,
    UnityTexture2D OcclusionMap, UnityTexture2D HeightMap,
    float3 Position, float3 Normal, float3 View, float3 Tangent, float3 Bitangent,
    float2 Tiling, float2 Offset, float HeightStrength, float Blend,
    out float4 Color, out float3 NormalTS, out float Metallic, out float Smoothness, out float Occlusion)
{
    float3 normal = normalize(Normal);
    float3 weights = pow(abs(normal), clamp(Blend, 1.0, 16.0));
    weights /= dot(weights, float3(1.0, 1.0, 1.0));

    float2 uvX = Position.zy * Tiling + Offset;
    float2 uvY = Position.xz * Tiling + Offset;
    float2 uvZ = Position.xy * Tiling + Offset;
    // Одинаковое смещение всех карт в каждой проекции сохраняет их совмещение.
    uvX += TransparentObjectParallax(HeightMap, uvX, float3(View.zy, View.x), HeightStrength);
    uvY += TransparentObjectParallax(HeightMap, uvY, float3(View.xz, View.y), HeightStrength);
    uvZ += TransparentObjectParallax(HeightMap, uvZ, View, HeightStrength);

    Color = TransparentObjectSample(BaseMap, uvX, uvY, uvZ, weights);
    float4 mask = TransparentObjectSample(MetallicMap, uvX, uvY, uvZ, weights);
    Metallic = mask.r;
    Smoothness = mask.a;
    Occlusion = TransparentObjectSample(OcclusionMap, uvX, uvY, uvZ, weights).g;

    // Смешивание нормалей по схеме штатного узла Triplanar Shader Graph.
    float3 nX = UnpackNormal(SAMPLE_TEXTURE2D(NormalMap.tex, NormalMap.samplerstate, uvX));
    float3 nY = UnpackNormal(SAMPLE_TEXTURE2D(NormalMap.tex, NormalMap.samplerstate, uvY));
    float3 nZ = UnpackNormal(SAMPLE_TEXTURE2D(NormalMap.tex, NormalMap.samplerstate, uvZ));
    nX = float3(nX.xy + normal.zy, abs(nX.z) * normal.x);
    nY = float3(nY.xy + normal.xz, abs(nY.z) * normal.y);
    nZ = float3(nZ.xy + normal.xy, abs(nZ.z) * normal.z);
    float3 normalWS = normalize(nX.zyx * weights.x + nY.xzy * weights.y + nZ.xyz * weights.z);
    NormalTS = normalize(float3(dot(normalWS, normalize(Tangent)),
        dot(normalWS, normalize(Bitangent)), dot(normalWS, normal)));
}

#endif
