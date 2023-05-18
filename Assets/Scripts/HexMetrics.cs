using UnityEngine;

public enum HexEdgeType
{
    Flat, Slope, Cliff
}

public static class HexMetrics
{
    public const int chunkSizeX = 5, chunkSizeZ = 5; 
    
    public const float outerRadius = 10f;
    // 内径  cos30°
    public const float innerRadius = outerRadius * 0.866025404f;

    // hex 边隙 参数 0.75 - 0.95
    public const float solidFactor = 0.8f;

    public const float blendFactor = 1f - solidFactor;
    // 高度单位步长
    public const float elevationStep = 3f;
    // 梯田坡度 单位步长
    public const int terracesPerSlope = 2;
    public const int terracesSteps = terracesPerSlope * 2 + 1;
    // 梯田水平坐标 单位步长
    public const float horizontalTerraceStepSize = 1f / terracesSteps;
    // 梯田竖直坐标 单位步长
    public const float verticalTerraceSepSize = 1f / (terracesPerSlope + 1);

    public static Texture2D noiseSource;
    // 噪声扰动力度
    public const float cellPerturbStrength = 4f;
    // 纹理采样scale
    public const float noiseScale = 0.003f;

    public const float elevationPerturbStrength = 1.5f;

    static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, - 0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };

    // 获取三角面片第一个角
    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return corners[(int)direction];
    }

    // 获取三角面片第二个角
    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return corners[(int)direction + 1];
    }

    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return corners[(int)direction] * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner(HexDirection direction)
    {
        return corners[(int)direction + 1] * solidFactor;
    }

    public static Vector3 GetBridge(HexDirection direction)
    {
        return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
    }

    public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step)
    {
        float h = step * HexMetrics.horizontalTerraceStepSize;
        a.x += (b.x - a.x) * h;
        a.z += (b.z - a.z) * h;
        float v = ((step + 1) / 2) * HexMetrics.verticalTerraceSepSize;
        a.y += (b.y - a.y) * v;
        return a;
    }

    public static Color TerraceLerp(Color a, Color b, int step)
    {
        return Color.Lerp(a, b, step * HexMetrics.horizontalTerraceStepSize);
    }

    public static HexEdgeType GetEdgeType(int elevation1, int elevation2)
    {
        if (elevation1 == elevation2)
        {
            return HexEdgeType.Flat;
        }
        int delta = elevation2 - elevation1;
        if (delta == 1 || delta == -1)
        {
            return HexEdgeType.Slope;
        }
        return HexEdgeType.Cliff;
    }

    public static Vector4 SampleNoise(Vector3 position)
    {
        return noiseSource.GetPixelBilinear(position.x * noiseScale, position.z * noiseScale);

    }
}
