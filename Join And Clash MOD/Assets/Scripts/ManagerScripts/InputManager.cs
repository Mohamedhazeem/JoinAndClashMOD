using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public delegate void MouseCallback();
    public event MouseCallback OnMouseHold, OnMouseDown, OnMouseUp;

    public delegate void MouseDragCallback(float X);
    public event MouseDragCallback OnMouseDrag;

    [Header("Ortho Graphic Camera")]
    [SerializeField] private Camera orthographicCamera;

    private Vector3 MouseStartPosition;
    private Vector3 MouseCurrentPosition;

    private void Awake()
    {
        AssignInstance();
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
    void Update()
    {
        HoldAndDrag();
    }
    private void HoldAndDrag()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.Menu && UIManager.instance.currentMenuState == UIManager.MenuState.BeforeStart && PlayerManager.instance.currentPlayerStates == PlayerStates.Idle && Input.GetMouseButtonDown(0))
        {
            GameManager.instance.SwitchGameStates();
            UIManager.instance.SwitchUiState();
        }

        if (GameManager.instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.instance.currentPlayerStates == PlayerStates.Idle && Input.GetMouseButtonDown(0))
        {
            PlayerManager.instance.SwitchPlayerStates();
            OnMouseDown?.Invoke();
            MouseStartPosition = orthographicCamera.ScreenToWorldPoint(Input.mousePosition);
            MouseStartPosition.y = PlayerManager.instance.currentPlayer.transform.position.y;

        }
        else if (Input.GetMouseButton(0) && GameManager.instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.instance.currentPlayerStates == PlayerStates.Running)
        {
            OnMouseHold?.Invoke();
            MouseCurrentPosition = orthographicCamera.ScreenToWorldPoint(Input.mousePosition);
            MouseCurrentPosition.y = PlayerManager.instance.currentPlayer.transform.position.y;

            var difference = MouseCurrentPosition - MouseStartPosition;
            OnMouseDrag(difference.x);

        }
        else if (Input.GetMouseButtonUp(0) && GameManager.instance.currentGameState == GameManager.GameState.GamePlay && PlayerManager.instance.currentPlayerStates == PlayerStates.Running)
        {
            
            PlayerManager.instance.SwitchPlayerStates();
            OnMouseUp?.Invoke();
        }
    }
}

