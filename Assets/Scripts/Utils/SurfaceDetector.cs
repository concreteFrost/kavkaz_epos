using UnityEngine;

public enum SurfaceType
{
    Grass = 0,
    Gravel = 1,
    Rock = 2,
    Wood = 3,
    Sand = 4,
}

public static class SurfaceDetector
{
    // Функция для определения имени поверхности под игроком
    public static SurfaceType GetSurfaceType(Vector3 position, float offset = 0f)
    {
        Vector3 rayStart = position + Vector3.up * offset;

        if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit))
            return SurfaceType.Wood; // поверхность по умолчанию

        // Terrain
        Terrain terrain = hit.collider.GetComponent<Terrain>();
        if (terrain != null)
        {
            string surface = GetTerrainSurfaceName(terrain, hit.point).ToLower();

            if (surface.Contains("grass")) return SurfaceType.Grass;
            if (surface.Contains("gravel")) return SurfaceType.Gravel;
            if (surface.Contains("sand") || surface.Contains("ground")) return SurfaceType.Sand;
            if (surface.Contains("rock")) return SurfaceType.Rock;

            return SurfaceType.Grass;
        }

        // Обычные объекты
        if (hit.collider.CompareTag("Wood"))
            return SurfaceType.Wood;

        if (hit.collider.CompareTag("Rock"))
        {
            Debug.Log("is rock");
            return SurfaceType.Rock;
        }
           

        return SurfaceType.Grass;
    }

    // Определение имени слоя террейна
    private static string GetTerrainSurfaceName(Terrain terrain, Vector3 position)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPosition = position - terrain.transform.position;

        float xNormalized = terrainPosition.x / terrainData.size.x;
        float zNormalized = terrainPosition.z / terrainData.size.z;

        int xMap = Mathf.FloorToInt(xNormalized * terrainData.alphamapWidth);
        int zMap = Mathf.FloorToInt(zNormalized * terrainData.alphamapHeight);

        float[,,] alphaMap = terrainData.GetAlphamaps(xMap, zMap, 1, 1);

        int strongestIndex = -1;
        float maxWeight = 0;

        for (int i = 0; i < alphaMap.GetLength(2); i++)
        {
            if (alphaMap[0, 0, i] > maxWeight)
            {
                strongestIndex = i;
                maxWeight = alphaMap[0, 0, i];
            }
        }

        if (strongestIndex >= 0 && strongestIndex < terrainData.terrainLayers.Length)
        {
            return terrainData.terrainLayers[strongestIndex].name;
        }

        return "Unknown";
    }
}
