using UnityEngine;

public class Grid3D
{
    private int width;
    private int height;
    private int depth; // Nueva dimensión para la profundidad
    private float cellSize;
    private int[,,] gridArray; // Array tridimensional

    public Grid3D(int width, int height, int depth, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.depth = depth; // Asignar profundidad
        this.cellSize = cellSize;

        gridArray = new int[width, height, depth]; // Crear el array tridimensional

        // Dibujar el grid en 3D
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    // Dibujar líneas en el eje Y-Z
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), Color.blue, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.blue, 100f);

                    // Dibujar líneas en el eje X-Z
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.blue, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.blue, 100f);

                    // Dibujar líneas en el eje X-Y
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.blue, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), Color.blue, 100f);
                }
            }
        }
    }

    // Método para obtener la posición en el mundo
    private Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize;
    }
}
