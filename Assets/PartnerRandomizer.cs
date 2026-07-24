using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerRandomizer : MonoBehaviour
{
    [SerializeField]
    GameObject partnerPrefab;
    [SerializeField]
    GameObject player;
    public Vector3 playerPos => player.transform.position;
    Vector2 randomizationExtents = new Vector2(9f, 9f);
    public static PartnerRandomizer instance;
    public delegate void OnRandomize();
    public OnRandomize onRandomizeHandler;
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
        Vector3 pos = new Vector3(
            Random.Range(-randomizationExtents.x / 2f, randomizationExtents.x / 2f)
            , 0
            , Random.Range(-randomizationExtents.x / 2f, randomizationExtents.x / 2f));
        var g = GameObject.Instantiate(partnerPrefab);
        g.SetActive(true);
        g.GetComponent<TagTarget>().AddSelf();
        g.transform.position = pos;
        Vector3 forward = (player.transform.position - pos);
        forward.y = 0f;
        //The model's coordinate space got fucked up during export. Just roll with it
        Quaternion rot = Quaternion.LookRotation(Vector3.up,-forward.normalized);
        g.transform.rotation = rot;
    }

}
