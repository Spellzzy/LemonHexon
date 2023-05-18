using UnityEngine;

public enum HexDirection
{
    NE, E, SE,
    SW, W, NW
}

public static class HexDirectionExtensions
{
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }
}

public class HexCell : MonoBehaviour
{
    // 坐标信息
    public HexCoordinates coordinates;
    // 所属chunk
    public HexGridChunk chunk;
    // 颜色
    Color color;
    public Color Color
    {
        get { return color; }
        set {
            if (color == value)
            {
                return;
            }

            color = value;
            Refresh();
        }
    }

    // 高度
    int elevation = int.MinValue;
    public int Elevation
    {
        get { return elevation; }
        set {
            if (elevation == value)
            {
                return;
            }
            
            elevation = value;

            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y += (HexMetrics.SampleNoise(position).y * 2f - 1f) * HexMetrics.elevationPerturbStrength;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            // uiPosition.z = elevation * -HexMetrics.elevationStep;
            uiPosition.z = -position.y;
            uiRect.localPosition = uiPosition;
            // 海拔高度发生变化 刷新网格
            Refresh();
        }
    }

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }

    }

    public RectTransform uiRect;

    [SerializeField]
    HexCell[] neighbors;

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)(direction.Opposite())] = this;
    }

    public HexEdgeType GetEdgeType(HexCell otherCell)
    {
        //return HexMatrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
        return HexMetrics.GetEdgeType(elevation, otherCell.elevation);
    }

    public HexEdgeType GetHexEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
    }

    void Refresh()
    {
        if (chunk)
        {
            chunk.Refresh();
            for (int i = 0; i < neighbors.Length; i++)
            {
                HexCell neighbor = neighbors[i];
                if (neighbor != null && neighbor.chunk != null)
                {
                    neighbor.chunk.Refresh();
                }
            }
        }
    }
}
