using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Array of all available menus
    public BaseMenu[] allMenus;

    // Enum for all the possible menu states
    public enum MenuStates
    {
        MainMenu,
        Settings,
        Pause,
        InGame
    }

    // The initial state of the menu (default is MainMenu)
    public MenuStates initialState = MenuStates.MainMenu;

    // Dictionary to map menu states to actual BaseMenu objects
    Dictionary<MenuStates, BaseMenu> menuDictionary = new Dictionary<MenuStates, BaseMenu>();

    // Stack to keep track of the menu history (used for jumping back)
    Stack<MenuStates> menuStack = new Stack<MenuStates>();

    // Reference to the current active menu state
    BaseMenu currentState;

    // Start is called before the first frame update
    void Start()
    {
        // Populate the dictionary with menus
        if (allMenus == null) return;

        foreach (BaseMenu menu in allMenus)
        {
            if (menu == null) continue;

            menu.InitState(this);

            if (!menuDictionary.ContainsKey(menu.state))
            {
                menuDictionary.Add(menu.state, menu);
            }
        }

        // Set the initial state of the menu and register the menu controller with the GameManager
        SetActiveState(initialState);
        GameManager.Instance.SetMenuController(this);
    }

    // Jump back to the previous menu state
    public void JumpBack()
    {
        if (menuStack.Count <= 1)
        {
            SetActiveState(MenuStates.MainMenu); // If there's no previous state, go to MainMenu
        }
        else
        {
            menuStack.Pop(); // Remove current state
            SetActiveState(menuStack.Peek(), true); // Activate the previous state
        }
    }

    // Set the active state of the menu
    public void SetActiveState(MenuStates newState, bool isJumpingBack = false)
    {
        if (!menuDictionary.ContainsKey(newState)) return; // If the state is not in the dictionary, return
        if (currentState == menuDictionary[newState]) return; // If the state is already active, do nothing

        // Deactivate the current menu state
        if (currentState != null)
        {
            currentState.ExitState();
            currentState.gameObject.SetActive(false);
        }

        // Activate the new state
        currentState = menuDictionary[newState];
        currentState.gameObject.SetActive(true);
        currentState.EnterState();

        // Add the new state to the stack unless it's a jump back
        if (!isJumpingBack) menuStack.Push(newState);

        // Handle game pause/resume based on the menu state
        HandlePauseState(newState);
    }

    // Handle game pause state depending on the menu state
    private void HandlePauseState(MenuStates state)
    {
        switch (state)
        {
            case MenuStates.Pause:
                Time.timeScale = 0f; // Pause the game when Pause menu is active
                break;
            case MenuStates.InGame:
                Time.timeScale = 1f; // Resume the game when in-game
                break;
            default:
                // In any other menu state (MainMenu, Settings), keep the game paused
                Time.timeScale = 0f;
                break;
        }
    }
}
