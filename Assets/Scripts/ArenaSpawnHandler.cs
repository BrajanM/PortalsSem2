using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSpawnHandler : MonoBehaviour
{
    public GameObject Arena;

    public static Vector3 LastArenaCenter;
    public static Vector3 PlayerPosition;
    public static bool SpawnNewArena;


    private List<Vector3> arenaCenterList = new List<Vector3>();
    private float arenaCenterOffset = 70f;
    private Vector3 newArenaCenter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnNewArena)
        {
            spawArena();
        }
    }

    private void spawArena()
    {
        float xOffset = LastArenaCenter.x - PlayerPosition.x;
        float zOffset = LastArenaCenter.z - PlayerPosition.z;

        

        if (Mathf.Abs(xOffset) > Mathf.Abs(zOffset))
        {
            if (xOffset >0)
            {
                newArenaCenter = new Vector3(LastArenaCenter.x - arenaCenterOffset,LastArenaCenter.y,LastArenaCenter.z);
            }
            else
            {
                newArenaCenter = new Vector3(LastArenaCenter.x + arenaCenterOffset, LastArenaCenter.y, LastArenaCenter.z);

            }
        }
        if (Mathf.Abs(xOffset) < Mathf.Abs(zOffset))
        {
            if (zOffset > 0)
            {
                newArenaCenter = new Vector3(LastArenaCenter.x, LastArenaCenter.y, LastArenaCenter.z - arenaCenterOffset);
            }
            else
            {
                newArenaCenter = new Vector3(LastArenaCenter.x, LastArenaCenter.y , LastArenaCenter.z + arenaCenterOffset);
            }
        }

        if (!arenaCenterList.Contains(newArenaCenter))
        {
            Instantiate(Arena, newArenaCenter, Arena.transform.rotation);
            arenaCenterList.Add(newArenaCenter);
            SpawnNewArena = false;
        }

    }

}
