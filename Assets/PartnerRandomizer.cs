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
    public Vector3 playerPos => player.transform.position;
    Vector2 randomizationExtents = new Vector2(9f, 9f);
    public static PartnerRandomizer instance;
    public delegate void OnRandomize();
    public OnRandomize onRandomizeHandler;
    public int maxPartnerCount = 10;
    int tagCounter = 0;
    int numOfPartners = 1;
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
        if (onRandomizeHandler != null) onRandomizeHandler();

        if (tagCounter > 5)
        {
            numOfPartners = Mathf.Clamp(numOfPartners + 1, 0, maxPartnerCount);
            newRuleExplainer.gameObject.SetActive(true);
        }
        for(int i = 0; i < numOfPartners; i++)
        {
            Vector3 pos = new Vector3(
        Random.Range(-randomizationExtents.x / 2f, randomizationExtents.x / 2f)
        , 0
        , Random.Range(-randomizationExtents.x / 2f, randomizationExtents.x / 2f));
            var g = GameObject.Instantiate(partnerPrefab);
            g.SetActive(true);
            var gTarget = g.GetComponent<TagTarget>();
            gTarget.AddSelf();
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
