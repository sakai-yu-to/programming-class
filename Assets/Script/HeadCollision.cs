using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Cinemachine.CinemachineOrbitalTransposer;

public class HeadCollision : MonoBehaviour
{
    public PlayerMove playermove;


    public Tilemap tilemap; // 参照するTilemap
    public TileBase coinedBlockTile; // 表示したい新しいタイル


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CoinBlock"))
        {
            Debug.Log("coinBlock collided");

            // コライダーの最も近いポイントを取得
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Action(hitPoint);
        }

        if (other.CompareTag("Dossun"))
        {
            playermove.hitDossunFlag = true;
            Debug.Log("Player hit Dossun! hitDossunFlag set to true");
        }

        if (other.CompareTag("changeMode"))
        {
            Gamemanager.instance.isHard = !Gamemanager.instance.isHard;
            Debug.Log(Gamemanager.instance.isHard);
        }

        if (other.CompareTag("Killer"))
        {
            playermove.enemyBounce = false;
            playermove.isJump = false;
        }

    }

    private void Action(Vector3 hitPoint)
    {
        Vector3Int tilePos = tilemap.WorldToCell(hitPoint);

        Debug.Log($"Collision point in world coordinates: {hitPoint}");
        Debug.Log($"Tile position in tilemap coordinates: {tilePos}");

        Vector3Int closestTilePos = FindClosestTilePosition(tilePos);

        if (tilemap.GetTile(closestTilePos) != null)
        {
            tilemap.SetTile(closestTilePos, null);
            Debug.Log($"Tile at {closestTilePos} deleted");
            playermove.getCoin = true;
            Debug.Log("Coin increased");
        }
        else
        {
            Debug.Log("No tile found near collision point.");
        }
    }

    private Vector3Int FindClosestTilePosition(Vector3Int startPos)
    {
        int range = 1;
        Vector3Int closestTilePos = startPos;
        float minDistance = float.MaxValue;

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int checkPos = new Vector3Int(startPos.x + x, startPos.y + y);

                if (tilemap.GetTile(checkPos) != null)
                {
                    Vector3 tileCenter = tilemap.GetCellCenterWorld(checkPos);
                    float distance = Vector3.Distance(tilemap.CellToWorld(startPos), tileCenter);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestTilePos = checkPos;
                    }
                }
            }
        }

        return closestTilePos;
    }

    void Start()
    {
      
    }

    void Update()
    {
        
    }
}