using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_Menu;
    [SerializeField] private GameObject m_Option;

    private bool m_isMenu = false;

    private InputProvider m_input;

    private void Awake()
    {
        m_input = new();

        m_input.OnMenu += OnInputMenu;
    }

    private void Start()
    {
        CloseMenu();
    }

    private void OnEnable()
    {
        m_input.Enable();
    }

    private void OnDisable()
    {
        m_input.Disable();
    }

    private void OnDestroy()
    {
        m_input.OnMenu -= OnInputMenu;
    }

    private void OnInputMenu()
    {

        if(!m_isMenu)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
        }
    }

    public void OpenMenu()
    {
        ViewMenu(true);
        ViewOption(false);

        m_isMenu = true;

    }

    public void CloseMenu()
    {
        ViewMenu(false);
        ViewOption(false);

        m_isMenu = false;
    }

    public void CloseTouchMenu()
    {
        ViewMenu(false);
    }

    public void CloseTouchOption()
    {
        ViewOption(false);
    }

    public void OpenOption()
    {
        ViewOption(true);
    }

    public void CloseOption()
    {
        ViewOption(false);
    }

    private void ViewMenu(bool isOpen)
    {
        m_Menu.SetActive(isOpen);
    }

    private void ViewOption(bool isOpen)
    {
        m_Option.SetActive(isOpen);
    }
}
