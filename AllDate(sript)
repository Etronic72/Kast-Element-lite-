# AllDate.cs

## Description

This class manages all game data, including inventory, resources, and objects. It loads data from JSON, handles UI updates, and manages crafting.

## Variables

*   `OnButtonClicked`: (public delegate void ButtonClickedHandler(Item itemData); public event ButtonClickedHandler) Event triggered when an item button is clicked.
*   `jsonFilePath`: (public string) Path to the JSON file containing item data.
*   `itemPrefab`: (public GameObject) Prefab for displaying inventory items.
*   `parentTransform_item`: (public Transform) Parent transform for inventory items.
*   `itemPrefab2`: (public GameObject) Prefab for displaying resources.
*   `parentTransform_resources`: (public Transform) Parent transform for resources.
*   `itemPrefab3`: (public GameObject) Prefab for displaying objects.
*   `parentTransform_object`: (public Transform) Parent transform for objects.
*   `itemdata`: (public List<Item>) List to store general item data loaded from JSON.
*   `itemdata_inv`: (public List<inv>) List for inventory items.
*   `itemdata_res`: (public List<res>) List for resources.
*   `itemdata_obj`: (public List<obj>) List for objects.
*   `sv`: (private Save) Object for saving game data.
*   `mainScript`: (private MainScript) Reference to the MainScript for balance interactions.
*   `totalSellValue`: (public int) Total value from selling all resources and objects.
*   `resValue`: (public int) Value from selling resources.
*   `objValue`: (public int) Value from selling objects.
*   `Creat`: (public bool) Flag indicating if crafting is possible (unused).
*   `open`: (public int) Flag indicating if save data should be loaded (1=yes, 0=no).

## Methods

*   `Start()`: (void) Called when the script starts.
    *   Loads data from PlayerPrefs if available.
    *   Loads JSON data for inventory, resources, and objects.
    *   Instantiates UI elements based on loaded data.
    *   Links TextMeshProUGUI "Count" elements to inventory items.
    *   Starts coroutines for data saving and automatic resource/object generation.
    *   Sets the "Buy" text on the purchase buttons.
*   `Update()`: (void) Called every frame.
    *   Updates `totalSellValue` based on `resValue` and `objValue`.
*   `SaveData()`: (public IEnumerator) Coroutine that saves data periodically.
    *   Calls `OnApplicationQuit()` every 1 second.
*   `JSonLoadInventory(GameObject prefab, Transform parent)`: (public void) Loads inventory data from JSON.
    *   Constructs the full file path.
    *   Reads the JSON data from the file.
    *   Deserializes JSON into `ItemsList`.
    *   Creates UI elements for each item.
*   `JSonLoadResources(GameObject prefab, Transform parent)`: (public void) Loads resource data from JSON.
    *   Constructs the full file path.
    *   Reads the JSON data from the file.
    *   Deserializes JSON into `ItemsList`.
    *   Creates UI elements for each resource.
    *   Sets the sprite for each resource's image.
    *   Adds a listener to the resource's button.
*   `OnResourceButtonClicked(res resourceItem)`: (private void) Handles resource button clicks.
    *   Finds the `MainScript` and calls its `HandleResourceButtonClick()` method.
*   `UpdateResourceCountOverTime(res resourceItem)`: (private IEnumerator) Coroutine that automatically updates the resource count.
    *   Increments resource count if `isBuy` is 1 and the time interval has passed.
    *   Updates the fill amount for the background.
*   `JSonLoadObject(GameObject prefab, Transform parent)`: (public void) Loads object data from JSON.
    *   Constructs the full file path.
    *   Reads the JSON data from the file.
    *   Deserializes JSON into `ItemsList`.
    *   Creates UI elements for each object.
    *   Sets the sprite for each object's image.
    *   Adds a listener to the object's button.
*   `OnObjectButtonClicked(obj objectItem)`: (private void) Handles object button clicks.
    *   Finds the `MainScript` and calls its `HandleObjectButtonClick()` method.
*   `UpdateObjectCountOverTime(obj objectItem)`: (private IEnumerator) Coroutine that automatically updates the object count.
    *   Increments object count if `isBuy` is 1 and the time interval has passed.
    *   Calls `CraftObject()` to perform the object's crafting.
    *   Updates the fill amount for the background.
*   `CheckCraftRequirements(obj objectItem)`: (public bool) Checks if there are enough resources to craft an object.
    *   Iterates through the `craft` List, checking if the player has enough items for each resource.
    *   Returns `true` if enough resources are available, `false` otherwise.
*   `CraftObject(obj objectItem)`: (public void) Performs the crafting operation.
    *   Subtracts the required resources from the player's inventory.
    *   Adjusts the `resValue`.
    *   Logs a message to the console.
*   `OnApplicationQuit()`: (private void) Saves the game data using PlayerPrefs.
    *   Saves data for levels, counts, prices, and buy flags.
    *   Creates arrays to hold the data.
    *   Populates the arrays.
    *   Sets the JSON string in PlayerPrefs.

## Classes

*   `Item`: (public class) Represents a general item in the game.
    *   `id`: (public int) Unique identifier.
    *   `name`: (public string) Item name.
    *   `lvl`: (public int) Item level.
    *   `time`: (public int) Generation time.
    *   `price`: (public int) Item price.
    *   `sell`: (public int) Item selling price.
    *   `count`: (public int) Item count.
    *   `isBuy`: (public int) Buy flag.
    *   `craft`: (public List<CraftItem>) List of `CraftItem` needed for crafting.
    *   `image`: (public Image) Reference to an Image component (populated from `images`).
    *   `images`: (public string) Path to the image.
    *   `Background`: (public Image) Background Image component for fill amount.
    *   `button`: (public Button) Reference to the button.
*   `ItemsList`: (public class) A list of items.
    *   `items`: (public List<Item>) List of `Item`.
*   `inv`: (public class) Represents an inventory item.
    *   `name`: (public string) Item name.
    *   `count`: (public int) Item count.
    *   `countText`: (public TextMeshProUGUI) Text field for displaying count.
*   `res`: (public class) Represents a resource.
    *   `id`: (public int) Unique identifier.
    *   `name`: (public string) Name.
    *   `lvl`: (public int) Level.
    *   `time`: (public int) Generation time.
    *   `sell`: (public int) Sell value.
    *   `price`: (public int) Price.
    *   `count`: (public int) Count.
    *   `up`: (public Button) Button (unused).
    *   `isBuy`: (public int) Buy flag.
    *   `nameText`: (public TextMeshProUGUI) Reference to name text field.
    *   `descriptionText`: (public TextMeshProUGUI) Reference to description text field.
    *   `sellText`: (public TextMeshProUGUI) Reference to sell text field.
    *   `image`: (public Image) Reference to Image component.
    *   `images`: (public string) Image Path.
    *   `Background`: (public Image) Background Image component.
    *   `button`: (public Button) Reference to Button component.
*   `obj`: (public class) Represents an object.
    *   `id`: (public int) Unique identifier.
    *   `name`: (public string) Name.
    *   `lvl`: (public int) Level.
    *   `time`: (public int) Generation time.
    *   `sell`: (public int) Sell value.
    *   `price`: (public int) Price.
    *   `isBuy`: (public int) Buy flag.
    *   `count`: (public int) Count.
    *   `craft`: (public List<CraftItem>) List of crafting ingredients.
    *   `nameText`: (public TextMeshProUGUI) Reference to name text field.
    *   `descriptionText`: (public TextMeshProUGUI) Reference to description text field.
    *   `sellText`: (public TextMeshProUGUI) Reference to sell text field.
    *   `image`: (public Image) Reference to Image component.
    *   `images`: (public string) Image Path.
    *   `Background`: (public Image) Background Image component.
    *   `button`: (public Button) Reference to Button component.
*   `CraftItem`: (public class) Represents an item required for crafting.
    *   `name`: (public string) Item name.
    *   `count`: (public int) Required item count.
*   `Save`: (public class) Class for saving the game state.
    *   `open_save`: (public int) Flag for loading saves.
    *   `lvl_save`: (public int[]) Array for item levels.
    *   `count_save`: (public int[]) Array for item counts.
    *   `isBuy_save`: (public int[]) Array for item buy flags.
    *   `price_save`: (public int[]) Array for item prices.
