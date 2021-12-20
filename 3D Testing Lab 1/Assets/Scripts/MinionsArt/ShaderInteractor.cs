using UnityEngine;

public class ShaderInteractor : MonoBehaviour
{
    // This is for Grass Shader Interaction
    void Update()
    {
        Shader.SetGlobalVector("_PositionMoving", transform.position);
    }
}
