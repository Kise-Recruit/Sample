using System.Collections;
using UnityEngine;

public class UltimateTextureCreater : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Camera ultimateTmplateCamera;

    public IEnumerator CreateRenderTexture()
    {
        ultimateTmplateCamera.enabled = true;
        ultimateTmplateCamera.targetTexture = renderTexture;
        yield return null;
        ultimateTmplateCamera.targetTexture = null;
        ultimateTmplateCamera.enabled = false;
    }
}
