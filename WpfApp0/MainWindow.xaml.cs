using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace InteractiveMap
{
    public partial class MainWindow : Window
    {
        private string currentRoom;
        private Dictionary<string, List<string>> roomConnections;

        public MainWindow()
        {
            InitializeComponent();
            InitializeRoomConnections();
            currentRoom = "Room 1";
            DisplayRoomInfo(currentRoom);
        }

        private void InitializeRoomConnections()
        {
            roomConnections = new Dictionary<string, List<string>>
            {
                { "Room 1", new List<string> { "Room 2", "Room 5" } },
                { "Room 2", new List<string> { "Room 1", "Room 3", "Room 6" } },
                { "Room 3", new List<string> { "Room 2", "Room 4", "Room 7" } },
                { "Room 4", new List<string> { "Room 3", "Room 8" } },
                { "Room 5", new List<string> { "Room 1", "Room 6", "Room 9" } },
                { "Room 6", new List<string> { "Room 2", "Room 5", "Room 7", "Room 10" } },
                { "Room 7", new List<string> { "Room 3", "Room 6", "Room 8", "Room 11" } },
                { "Room 8", new List<string> { "Room 4", "Room 7", "Room 12" } },
                { "Room 9", new List<string> { "Room 5", "Room 10" } },
                { "Room 10", new List<string> { "Room 6", "Room 9", "Room 11" } },
                { "Room 11", new List<string> { "Room 7", "Room 10", "Room 12" } },
                { "Room 12", new List<string> { "Room 8", "Room 11" } }
            };
        }

        private void MapCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(MapCanvas);
            foreach (UIElement element in MapCanvas.Children)
            {
                if (element is Rectangle rect && rect.Tag != null)
                {
                    double left = Canvas.GetLeft(rect);
                    double top = Canvas.GetTop(rect);
                    if (clickPosition.X >= left && clickPosition.X <= left + rect.Width &&
                        clickPosition.Y >= top && clickPosition.Y <= top + rect.Height)
                    {
                        string clickedRoom = rect.Tag.ToString();
                        if (rect.Fill.ToString() == "Gray") 
                        {
                            HandleCorridorClick(left, top);
                        }
                        else if (IsValidRoomTransition(clickedRoom))
                        {
                            currentRoom = clickedRoom;
                            DisplayRoomInfo(currentRoom);
                        }
                        break;
                    }
                }
            }
        }

        private bool IsValidRoomTransition(string clickedRoom)
        {
            return roomConnections[currentRoom].Contains(clickedRoom);
        }

        private void DisplayRoomInfo(string roomName)
        {
            RoomInfo.Text = $"You are in {roomName}";
        }

        private void HandleCorridorClick(double left, double top)
        {
            string adjacentRoom1 = GetAdjacentRoom(left, top, -150, 0);
            string adjacentRoom2 = GetAdjacentRoom(left, top, 150, 0);
            string adjacentRoom3 = GetAdjacentRoom(left, top, 0, -150);
            string adjacentRoom4 = GetAdjacentRoom(left, top, 0, 150);

            if (adjacentRoom1 != null && roomConnections[currentRoom].Contains(adjacentRoom1))
            {
                currentRoom = adjacentRoom1;
                DisplayRoomInfo(currentRoom);
            }
            else if (adjacentRoom2 != null && roomConnections[currentRoom].Contains(adjacentRoom2))
            {
                currentRoom = adjacentRoom2;
                DisplayRoomInfo(currentRoom);
            }
            else if (adjacentRoom3 != null && roomConnections[currentRoom].Contains(adjacentRoom3))
            {
                currentRoom = adjacentRoom3;
                DisplayRoomInfo(currentRoom);
            }
            else if (adjacentRoom4 != null && roomConnections[currentRoom].Contains(adjacentRoom4))
            {
                currentRoom = adjacentRoom4;
                DisplayRoomInfo(currentRoom);
            }
        }

        private string GetAdjacentRoom(double left, double top, double offsetX, double offsetY)
        {
            foreach (UIElement element in MapCanvas.Children)
            {
                if (element is Rectangle rect && rect.Tag != null && rect.Fill.ToString() != "Gray")
                {
                    double rectLeft = Canvas.GetLeft(rect);
                    double rectTop = Canvas.GetTop(rect);
                    if (rectLeft == left + offsetX && rectTop == top + offsetY)
                    {
                        return rect.Tag.ToString();
                    }
                }
            }
            return null;
        }
    }
}