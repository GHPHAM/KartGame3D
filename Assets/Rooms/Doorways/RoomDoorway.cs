using UnityEngine;

public class RoomDoorway : MonoBehaviour
{
    public bool isEntrance;
    public bool isExit;

    public void AssignRoom(RoomController room)
    {
        room.onRoomCleared += onRoomClear;
    }

    public void onRoomClear()
    {
        OpenDoor();
        //todo open door
        //GameObject roomPrefab = getRoom();
    }

    void OpenDoor()
    {
        //play animation to open door
    }
}
