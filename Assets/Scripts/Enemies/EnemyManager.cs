using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ.Player;
using GGJ.Levels;
using GGJ.Interactables;
using GGJ.Utilities.FolderGeneration;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private enum EnemyType { Bomb, Turret };

    [SerializeField]
    private EnemyBase[] enemyPrefabs;

    protected static Transform _currentPlayer;

    [SerializeField] Vector2Int enemyCountRange = new Vector2Int(3, 8);
    
    void OnEnable()
    {
        //DoorInteractable.LoadNewRoom += OnLoadNewRoom;
        RoomManager.OnNewRoomLoaded += OnLoadNewRoom;
    }

    void OnDisable()
    {
        RoomManager.OnNewRoomLoaded -= OnLoadNewRoom;
    }

    private void OnLoadNewRoom(int roomIndex)
    {
        if(roomIndex < 0)
            return;
        
        Debug.Log("EnemyManager--OnLoadNewRoom");
        SpawnEnemies(Random.Range(enemyCountRange.x,enemyCountRange.y+1));
    }

    public void SpawnEnemies(int number)
    {
        for(int i=0;i < number; i++)
        {
            SpawnEnemy( (EnemyType)Random.Range(0,enemyPrefabs.Length) );
        }
    }

    // Used when switching rooms
    public void DespawnEnemies()
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        for(int i=0;i<enemies.Length;i++)
        {
            Destroy(enemies[i].gameObject);
        }
    }

    private void SpawnEnemy(EnemyType type)
    {    
        if(_currentPlayer == null)
            _currentPlayer = FindObjectOfType<PlayerHealth>().transform;
        
        // Get a random distance from player
        Vector2 randomDir;
        Vector3 newPos;
        float distance;
        // Sample navmesh
        NavMeshHit pt;
        int tries = 0;
        do
        {
            if(tries > 10)
                return;
            randomDir = Random.insideUnitCircle.normalized;
            newPos = new Vector3(randomDir.x,0,randomDir.y);
            distance = Random.Range(10.0f,20.0f);
            newPos = newPos * distance + _currentPlayer.position;
            tries++;
        }while(!NavMesh.SamplePosition(newPos, out pt, 2.0f, 1));
        
        // Place enemy
        EnemyBase enemyPrefab = enemyPrefabs[(int)type];
        EnemyBase enemyObj = Instantiate(enemyPrefab, pt.position + Vector3.up*0.5f, Quaternion.identity, RoomManager.CurrentRoom.transform);

    }

}
