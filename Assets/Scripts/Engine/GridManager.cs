using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int depth;
    public float cellSize = 1f;
    private int[,,] gridArray;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Más de una instancia de GridManager detectada. Destruyendo la nueva instancia.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Opcional: Asegúrate de que el objeto persista entre escenas
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        gridArray = new int[width, height, depth];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    gridArray[x, y, z] = 0;
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), Color.blue, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.blue, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.blue, 100f);
                }
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize;
    }

    public bool PlantAtPosition(Vector3 worldPosition)
    {
        GetGridPosition(worldPosition, out int x, out int y, out int z);

        if (!IsValidGridPosition(x, y, z))
        {
            Debug.LogWarning($"Coordenadas fuera de los límites: ({x}, {y}, {z})");
            return false;
        }

        if (gridArray[x, y, z] == 0)
        {
            gridArray[x, y, z] = 1;
            return true;
        }
        else
        {
            Debug.LogWarning($"La celda ya está ocupada: ({x}, {y}, {z})");
        }

        return false;
    }


    private void GetGridPosition(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
        z = Mathf.FloorToInt(worldPosition.z / cellSize);
    }


    private bool IsValidGridPosition(int x, int y, int z)
    {
        return x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < depth;
    }
}
