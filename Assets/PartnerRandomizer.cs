using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartnerRandomizer : MonoBehaviour
{
    [SerializeField]
    GameObject partnerPrefab;
    [SerializeField]
    GameObject player;
    [SerializeField]
    Image newRuleExplainer;
    [SerializeField]
    GameObject wallGroup;
    [SerializeField]
    GameObject[] possibleFurniture;
    [SerializeField]
    GameObject windows;
    public Vector3 playerPos => player.transform.position;
    public static PartnerRandomizer instance;
    public delegate void OnRandomize();
    public OnRandomize onRandomizeHandler;
    public int maxPartnerCount = 10;
    public static HashSet<Vector2Int> OccupiedCellSet = new HashSet<Vector2Int>();
    const int maxCellBounds = 24;
    int tagCounter = 0;
    int numOfPartners = 1;
    int numOfAddedFurniture = 2;
    int cellBounds = 4;
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public void Start()
    {
        RandomizePartnerPlacement();
    }
    public void RandomizePartnerPlacement()
    {
        OccupiedCellSet.Clear();
        if (onRandomizeHandler != null) onRandomizeHandler();

        if (tagCounter > 5)
        {          
            numOfPartners = Mathf.Clamp(numOfPartners + 1, 0, maxPartnerCount);
            newRuleExplainer.gameObject.SetActive(true);
            int prevBounds = cellBounds;
            if (tagCounter > 10)
            {
                var window = GameObject.Instantiate(windows, windows.transform.parent);
                window.transform.localPosition += Vector3.right * 0.0214f * cellBounds / 2;
                window = GameObject.Instantiate(windows, windows.transform.parent);
                window.transform.localPosition -= Vector3.right * 0.0214f * cellBounds / 2;
                if (cellBounds == maxCellBounds)
                {
                    //Go to Win Screen
                    newRuleExplainer.gameObject.SetActive(false);
                    CountdownTracker.Instance.EndGame();
                    return;
                }
                cellBounds = Mathf.Clamp(cellBounds + 2, 0, maxCellBounds);
                numOfAddedFurniture += 2;
            }
            if(cellBounds != prevBounds)
            {
                foreach (Transform w in wallGroup.transform)
                {
                    Vector3 dir = w.position;
                    dir.y = 0;
                    dir = dir.normalized;
                    w.position += dir;
                }
                for (int i = 0; i < numOfAddedFurniture; i++)
                {
                    int rand = Random.Range(0, possibleFurniture.Length);
                    GameObject furniture = GameObject.Instantiate(possibleFurniture[rand]);
                    Vector2Int cell = new Vector2Int(
                        Random.Range(-cellBounds / 2, cellBounds / 2),
                        Random.Range(-cellBounds / 2, cellBounds / 2));
                    while (OccupiedCellSet.Contains(cell))
                    {
                        cell = new Vector2Int(
                            Random.Range(-cellBounds / 2, cellBounds / 2),
                            Random.Range(-cellBounds / 2, cellBounds / 2));
                    }
                    furniture.transform.position = new Vector3(cell.x + .5f, 0f, cell.y + .5f);
                    OccupiedCellSet.Add(cell);

                }
            }
        }
        
        for(int i = 0; i < numOfPartners; i++)
        {

            Vector2Int cell = new Vector2Int(
                Random.Range(-cellBounds / 2, cellBounds / 2),
                Random.Range(-cellBounds / 2, cellBounds / 2));
            while (OccupiedCellSet.Contains(cell))
            {
                cell = new Vector2Int(
                    Random.Range(-cellBounds / 2, cellBounds / 2),
                    Random.Range(-cellBounds / 2, cellBounds / 2));
            }
            Vector2 innerCellPos = new Vector2(
                Random.Range(cell.x, cell.x + 1f),
                Random.Range(cell.y, cell.y + 1f));
            Vector3 pos = new Vector3(
            innerCellPos.x
            , 0
            , innerCellPos.y);
            var g = GameObject.Instantiate(partnerPrefab);
            g.SetActive(true);
            var gTarget = g.GetComponent<TagTarget>();
            gTarget.AddSelf();
            gTarget.cell = cell;
            if(i == 0)
            {
                gTarget.isReal = true;
                gTarget.DisableChromaKey();
            }
            else
            {
                float hue = Random.Range(-90f, 200f);
                if (hue < 0) hue += 360f;
                hue /= 360f;
                float sat = Random.Range(75f, 100f) / 100f;
                float val = Random.Range(75f, 100f) / 100f;
                Color randColor = Color.HSVToRGB(hue, sat, val);
                gTarget.SetReplacementColor(randColor);
            }
            g.transform.position = pos;
            Vector3 forward = (player.transform.position - pos);
            forward.y = 0f;
            //The model's coordinate space got fucked up during export. Just roll with it
            Quaternion rot = Quaternion.LookRotation(Vector3.up, -forward.normalized);
            g.transform.rotation = rot;
        }

        tagCounter++;
    }

}
