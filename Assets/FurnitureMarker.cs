using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureMarker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PartnerRandomizer.instance.onRandomizeHandler += MarkOccupency;
    }
    void MarkOccupency()
    {
        Vector2Int cell = new Vector2Int(
            Mathf.FloorToInt(this.transform.position.x),
            Mathf.FloorToInt(this.transform.position.z)
            );
        PartnerRandomizer.OccupiedCellSet.Add(cell);
    }
}
