using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagTarget : MonoBehaviour
{
    [SerializeField]
    Image tagPrompt;
    [SerializeField]
    private AudioClip barkClip;
    [SerializeField]
    private Renderer hoodRenderer;
    [SerializeField]
    private Renderer clothesRenderer;

    static HashSet<TagTarget> activeTargetSet = new HashSet<TagTarget>();
    bool isClosest = false;
    public bool isReal = false;

    // Update is called once per frame
    void Update()
    {
        float dist = (PartnerRandomizer.instance.playerPos - this.transform.position).magnitude;
        int closestTargetID = -1;
        float minDist = 999f;
        float otherDist = 0f;
        foreach(var target in activeTargetSet)
        {
            if (target.GetHashCode() == this.GetHashCode()) continue;
            otherDist = (PartnerRandomizer.instance.playerPos - target.transform.position).magnitude;
            if (minDist > otherDist)
            {
                closestTargetID = target.GetHashCode();
                minDist = otherDist;
            }
        }
        if (dist < minDist) isClosest = true;
        else isClosest = false;
        if (isClosest && dist < 1f)
        {
            tagPrompt.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                CountdownTracker.Instance.playerSource.PlayOneShot(barkClip);
                if (isReal) CountdownTracker.Instance.ResetCountdown();
                else CountdownTracker.Instance.TriggerLoss();
                RemoveSelf();
            }
        }
        else if(isClosest)
        {
            tagPrompt.gameObject.SetActive(false);
        }
    }
    public void AddSelf()
    {
        activeTargetSet.Add(this);
        PartnerRandomizer.instance.onRandomizeHandler += RemoveSelf;
    }
    void RemoveSelf()
    {
        activeTargetSet.Remove(this);
        Destroy(this.gameObject);
        PartnerRandomizer.instance.onRandomizeHandler -= RemoveSelf;

    }
    public void SetReplacementColor(Color rColor)
    {
        hoodRenderer.material.SetColor(Shader.PropertyToID("_ReplacementColor"), rColor);
        clothesRenderer.material.SetColor(Shader.PropertyToID("_ReplacementColor"), rColor);
    }
    public void DisableChromaKey()
    {
        hoodRenderer.material.SetFloat(Shader.PropertyToID("_Threshold"), 0f);
        clothesRenderer.material.SetFloat(Shader.PropertyToID("_Threshold"), 0f);
    }
}
