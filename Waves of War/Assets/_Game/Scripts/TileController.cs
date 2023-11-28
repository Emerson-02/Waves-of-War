using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tileToUse; 
    public Transform player;

 
    void Update()
    {
        if(player != null)
        {
            Vector3Int playerTilePosition = tilemap.WorldToCell(player.position);

            int leftBound = playerTilePosition.x - 50;
            int rightBound = playerTilePosition.x + 50;
            int bottomBound = playerTilePosition.y - 50;
            int topBound = playerTilePosition.y + 50;

            for (int x = leftBound; x <= rightBound; x++)
            {
                for (int y = bottomBound; y <= topBound; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);

                    if (!tilemap.HasTile(tilePosition))
                    {
                        tilemap.SetTile(tilePosition, tileToUse);
                    }
                    else
                    {
                        Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(tilePosition);
                        float distance = Vector3.Distance(player.position, tileWorldPosition);

                        if (distance > 100f)
                        {
                            tilemap.SetTile(tilePosition, null);
                        }
                    }
                }
            }
        }

        
    }
    
}
