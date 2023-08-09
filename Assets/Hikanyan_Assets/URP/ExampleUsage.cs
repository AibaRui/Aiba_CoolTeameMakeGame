using UnityEngine;
using Hikanyan_Assets.ShaderGraph;

public class ExampleUsage : MonoBehaviour
{
    public PostEffectAddFeature postEffectFeature; // インスペクターから割り当て

    public bool isEnabled = true; // インスペクターから割り当て
    private void Update()
    {
        // postEffectFeature のインスタンスが正しく割り当てられているか確認
        if (postEffectFeature != null)
        {
            // ポストエフェクトを有効にする
            postEffectFeature.SetEffectEnabled(isEnabled);
        }
        else
        {
            Debug.LogWarning("PostEffectAddFeature is not assigned.");
        }
    }
}