# MainScript.cs

## Description

This script manages the core game logic, including the player's balance, UI, panel switching, button click handling (purchasing resources and objects, selling everything), and data saving.

## Variables

*   `balance`: (public int) The player's current balance.
*   `balanceText`: (public TextMeshProUGUI) The text field to display the player's balance in the UI.
*   `ObjectPanel`: (public GameObject) The panel containing objects.
*   `ResourcesPanel`: (public GameObject) The panel containing resources.
*   `InventoryPanel`: (public GameObject) The panel containing the inventory.
*   `OPtionPanel`: (public GameObject) The panel with options.
*   `ObjectButton`: (public Button) The button to open the object panel.
*   `ResourcesButton`: (public Button) The button to open the resource panel.
*   `InventoryButton`: (public Button) The button to open the inventory panel.
*   `OPtionButton`: (public Button) The button to open the option panel.
*   `AllSell`: (public Button) The button to sell everything.
*   `AllSellText`: (public TextMeshProUGUI) The text field to display the total sale amount.
*   `CountElement`: (public int) An element counter.
*   `CountElement_text`: (public Text) Text to display the element counter.
*   `bal`: (private Save2) An object for saving balance data.
*   `NotEnoughMoneyColor`: (public Color) The text color when the player doesn't have enough money.
*   `EnoughMoneyColor`: (public Color) The text color when the player has enough money.
*   `allDateScript`: (private AllDate) A reference to the AllDate script to access game data.

## Methods

*   `Awake()`: (private void) Called when the object is loaded. Empty in this code.
*   `Start()`: (void) Called when the script starts.
    *   Loads balance data from PlayerPrefs if it exists.
    *   Adds event handlers to buttons for switching panels.
    *   Finds the `AllDate` script in the scene.
*   `Update()`: (void) Called every frame.
    *   Updates the balance text in the UI.
    *   Updates the total sell amount text in the UI.
    *   Calls `OnApplicationQuit()` to save data (better to use `OnApplicationQuit` for reliability).
*   `UpdateResourceUI()`: (private void) Method to update the UI for resources.
    *   Iterates through all resources in `allDateScript.itemdata_res`.
    *   Updates the name and level text of the resource.
    *   Updates the text on the purchase button based on the player's money.
    *   Changes the button color based on the player's money.
*   `UpdateObjectUI()`: (private void) Method to update the UI for objects.
    *   Iterates through all objects in `allDateScript.itemdata_obj`.
    *   Updates the name and level text of the object.
    *   Updates the text on the purchase button based on the player's money.
    *   Changes the button color based on the player's money.
*   `SetButtonColor(Button button, Color color)`: (private void) Method to change the color of a button.
    *   Takes a button and a color.
    *   Changes the button color.
*   `TogglePanel(GameObject panel)`: (void) Method to toggle the visibility of panels.
    *   Takes the panel to display.
    *   Hides all panels and displays the given panel.
*   `HandleResourceButtonClick(res resourceItem)`: (public void) Handler for clicking a resource button.
    *   Takes a `res resourceItem` object.
    *   Updates the resource UI.
    *   Checks if the player has enough money to buy the resource.
    *   If there is enough money:
        *   Increases the resource level.
        *   Sets the `isBuy` flag to 1.
        *   Deducts money from the balance.
        *   Increases the resource price.
        *   Updates the text on the purchase button.
        *   Increases `CountElement` if the resource level is 1.
    *   If there isn't enough money, prints a message to the console.
    *   Updates the resource UI.
*   `HandleObjectButtonClick(obj objectItem)`: (public void) Handler for clicking an object button.
    *   Takes an `obj objectItem` object.
    *   Checks if the player has enough money to buy the object.
    *   If there is enough money:
        *   Checks if there are enough resources to craft the object, if the object has a crafting recipe.
        *   If there are enough resources (or no crafting is required):
            *   Sets the `isBuy` flag to 1.
            *   Increases the object level.
            *   Deducts money from the balance.
            *   Increases the object price.
            *   Updates the text on the purchase button.
            *   Increases `CountElement` if the object level is 1.
            *   Calls `allDateScript.CraftObject(objectItem)` to perform the crafting, if required.
    *   If there are not enough resources, prints a message to the console.
    *   If there isn't enough money, prints a message to the console.
    *   Updates the object UI.
*   `SellAll()`: (public void) Method to sell all resources and objects.
    *   Increases the player's balance by the total sale value of all resources and objects.
    *   Resets the sell values in `allDateScript`.
    *   Updates the `AllSellText` text.
    *   Resets the count of all resources, objects, and inventory items.
*   `OnApplicationQuit()`: (private void) Method called when exiting the application.
    *   Saves the current balance to `bal.balance_save`.
    *   Saves the `bal` object to PlayerPrefs as JSON.

## Classes

*   `Save2`: (public class) Class to save player balance data.
    *   `balance_save`: (public int) Saved balance.
