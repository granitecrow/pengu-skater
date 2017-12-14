using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour {

    public PieceType type;
    private Piece currentPiece;

    public void Spawn()
    {
        int amountToChoose = 0;
        switch (type)
        {
            case PieceType.none:
                amountToChoose = 0;
                break;
            case PieceType.ramp:
                amountToChoose = LevelManager.Instance.ramps.Count;
                break;
            case PieceType.longblock:
                amountToChoose = LevelManager.Instance.longblocks.Count;
                break;
            case PieceType.jump:
                amountToChoose = LevelManager.Instance.jumps.Count;
                break;
            case PieceType.slide:
                amountToChoose = LevelManager.Instance.slides.Count;
                break;
            default:
                break;
        }
       
        currentPiece = LevelManager.Instance.GetPiece(type, Random.Range(0, amountToChoose)); //get me a piece from the pool
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    public void DeSpawn()
    {
        currentPiece.gameObject.SetActive(false);
    }

}
