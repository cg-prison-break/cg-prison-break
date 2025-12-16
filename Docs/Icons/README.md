# Creating Item Icons

This README describes the standardized workflow for creating icons for items.

## Requirements

- Access to the **IconScene**
- A finished item (e.g. prefab or 3D object)
- The corresponding Scriptable Object for the item

## Workflow

### 1. Add the item to the IconScene

- Open the **IconScene**.
- Drag your item into the scene.
- Set the item’s position to **(0, 0, 0)**.

### 2. Position the item relative to the camera

- Move the item **away from the camera in the camera’s viewing direction**.
- Make sure the entire object is visible in the camera view and nothing is clipped.

### 3. Rotate the object

- Rotate the item to achieve the desired orientation for the icon.
- Choose a clear and consistent perspective.

### 4. Scale the object

- Scale the item so that it fills the image as much as possible.
- The object should reach close to the edges of the frame without being cut off.

### 5. Save the RenderTexture as PNG

- Open the **Project View**.
- Navigate to:  
  `Assets/Scenes/IconTexture`
- Right-click on the **IconTexture** render texture.
- Select **"Save RenderTexture to PNG"**.

### 6. Remove the background

- Right-click on the generated PNG image.
- Select **"Remove Pure Black Background"**.

### 7. Adjust texture settings

- Select the generated image.
- In the Inspector, set the following options:
    - **Texture Type:** `Sprite (2D and UI)`
    - **Sprite Mode:** `Single`
- Apply the changes.

### 8. Move the sprite to the Icons folder

- Move the sprite to:  
  `Assets/Icons`

### 9. Assign the icon to the Scriptable Object

- Open the item’s corresponding **Scriptable Object**.
- Assign the created sprite to the icon field.

### 10. Done

- The item icon is now correctly created and assigned.
- The icon can now be used in the UI or inventory.
