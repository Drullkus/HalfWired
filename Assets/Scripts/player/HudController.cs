using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    public int hits = 0;
    
    [SerializeField] private TextMeshProUGUI hitsText;

    public void Reset()
    {
        SetHits(0);
    }

    public void AddHit(int hit = 1)
    {
        SetHits(hit + hits);
    }

    private void SetHits(int hits)
    {
        this.hits = hits;
        hitsText.text = $"Hits: {hits}";
    }

}
