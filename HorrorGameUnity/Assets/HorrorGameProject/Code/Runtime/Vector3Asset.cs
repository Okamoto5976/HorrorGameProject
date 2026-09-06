using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Asset", menuName = "Scriptable Objects/Runtime/Vector3Asset")]
public class Vector3Asset : ScriptableObject
{
    [SerializeField] private Vector3 m_value;

    public Vector3 Value => m_value;

    public void SetValue(Vector3 value)
    {
        m_value = value;
    }
}
