using UnityEngine;

namespace TodayOrdeal
{
    public class HelloWorldGUI : MonoBehaviour
    {
        private Rect windowRect;
        private bool isDragging = false;
        private Vector2 dragOffset;

        private void Start()
        {
            // Initialize the window rectangle to a default position and size
            windowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300); // 400x300
            DontDestroyOnLoad(this.gameObject);
        }

        void OnGUI()
        {
            // Handle mouse dragging
            HandleMouseDrag();

            // Draw the draggable window
            windowRect = GUI.Window(0, windowRect, DrawWindow, "Hello World Window");
        }

        private void DrawWindow(int windowID)
        {
            // Draw the table
            float cellWidth = windowRect.width / 2;  // Width of each cell
            float cellHeight = windowRect.height / 2; // Height of each cell

            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    // Calculate the position of each cell
                    float cellXPos = col * cellWidth;
                    float cellYPos = row * cellHeight + 25; // Offset for header

                    // Draw each cell with some content
                    GUI.Box(new Rect(cellXPos, cellYPos, cellWidth, cellHeight), $"Cell {row + 1},{col + 1}");
                }
            }

            // Make the window draggable
            GUI.DragWindow(new Rect(0, 0, windowRect.width, 25)); // Header height

            // Log a message to the console
            Debug.Log("GUI with a classic draggable table rendered.");
        }

        private void HandleMouseDrag()
        {
            // Start dragging
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && windowRect.Contains(Event.current.mousePosition))
            {
                isDragging = true;
                dragOffset = Event.current.mousePosition - new Vector2(windowRect.x, windowRect.y);
                Event.current.Use(); // Consume the event
            }

            // Stop dragging
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                isDragging = false;
            }

            // Drag the window
            if (isDragging)
            {
                windowRect.x = Event.current.mousePosition.x - dragOffset.x;
                windowRect.y = Event.current.mousePosition.y - dragOffset.y;
                Event.current.Use(); // Consume the event
            }
        }

        private void OnDestroy()
        {
            // Cleanup if necessary
        }
    }
}
