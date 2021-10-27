using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public enum MenuState
    {
        BeforeStart,
        Started
    }
    internal MenuState currentMenuState;

    [Header("Hold And Drag GameObject")]
    [SerializeField]private GameObject HoldAndDrag_Text;

    private void Awake()
    {
        AssignInstance();
    }
    private void Start()
    {
        currentMenuState = MenuState.BeforeStart;
        HoldAndDrag_Text.SetActive(true);
    }
    private void AssignInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void SwitchUiState()
    {
        switch (currentMenuState)
        {
            case MenuState.BeforeStart:
                currentMenuState = MenuState.Started;
                HoldAndDrag_Text.SetActive(false);
                break;

            case MenuState.Started:
                currentMenuState = MenuState.BeforeStart;
                HoldAndDrag_Text.SetActive(true);
                break;

            default:
                break;
        }
    }
}
