using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_Menu;
    [SerializeField] private GameObject m_Option;

    public void InputOnMenu()
    {
        OnMenu(true);
        OnOption(false);
    }

    public void InputOffMenu()
    {
        OnMenu(false);
        OnOption(false);
    }

    public void OnCloseTouchMenu()
    {
        OnMenu(false);
    }

    public void OnCloseTouchOption()
    {
        OnOption(false);
    }

    public void OpenOption()
    {
        OnOption(true);
    }

    public void CloseOption()
    {
        OnOption(false);
    }

    private void OnMenu(bool isOpen)
    {
        m_Menu.SetActive(isOpen);
    }

    private void OnOption(bool isOpen)
    {
        m_Option.SetActive(isOpen);
    }
}
