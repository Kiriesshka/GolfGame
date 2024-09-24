using UnityEngine;
using KiriesshkaData;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class MakeGraphics : MonoBehaviour
{
    private DataSaver dS;
    public Volume v;
    private Bloom bloom;
    private FilmGrain grain;
    private DepthOfField depth;
    private LiftGammaGain lift;
    public UniversalRenderPipelineAsset urpa;
    public void Start()
    {
        if (v.profile.TryGet(out bloom));
        if (v.profile.TryGet(out grain));
        if (v.profile.TryGet(out depth)) ;
        if (v.profile.TryGet(out lift)) ;
        dS = new DataSaver();
        dS.fileName = "GRAPHICS";
        dS.fileExtension = ".txt";
        if (dS.IsSaveFileExsists())
        {
            dS.Load();
            SetGraphics(dS.GetString("TIER"));
        }
        
    }
    public void SetGraphics(string tier)
    {
        Debug.Log("Set graphics to " + tier);
        if(tier == "LOW")
        {
            bloom.active = false;
            grain.active = false;
            depth.active = false;
            lift.active = false;
            urpa.renderScale = 0.6f;
            urpa.shadowDistance = 0;
        }
        else if (tier == "NORMAL")
        {
            bloom.active = false;
            grain.active = true;
            depth.active = false;
            lift.active = true;
            urpa.renderScale = 0.8f;
            urpa.shadowDistance = 25;
        }
        else if(tier == "HIGH")
        {
            bloom.active = true;
            grain.active = true;
            depth.active = true;
            lift.active = true;
            urpa.renderScale = 0.8f;
            urpa.shadowDistance = 50;
        }
    }
}
