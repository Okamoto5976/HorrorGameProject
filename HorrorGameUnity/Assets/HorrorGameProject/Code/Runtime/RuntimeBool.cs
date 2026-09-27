using UnityEngine;

[CreateAssetMenu(fileName = "RuntimeBool", menuName = "Scriptable Objects/Runtime/RuntimeBool")]
public class RuntimeBool : ScriptableObject
{
    [SerializeField] private bool m_value;

    public bool Value => m_value;

    public void SetValue(bool value)
    {
        m_value = value;
    }
}
