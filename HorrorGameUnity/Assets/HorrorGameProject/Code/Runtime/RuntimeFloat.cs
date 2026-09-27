using UnityEngine;

[CreateAssetMenu(fileName = "RuntimeFloat", menuName = "Scriptable Objects/Runtime/RuntimeFloat")]
public class RuntimeFloat : ScriptableObject
{
    [SerializeField] private float m_value;

    public float Value => m_value;

    public void SetValue(float value)
    {
        m_value = value;
    }
}
