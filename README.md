🎮 ChromaQuest: Time Warp
Overview

"ChromaQuest: Time Warp" is a 2D Unity game featuring a distinctive mechanic where players switch between worlds by inverting the map's colors. The game challenges players with a time limit, an engaging enemy system, and a dynamic score tracking feature.
1. 🗺️ Game Map Design
- 🛠️ Create the Map

    Method: Unity's Tilemap system.
    Details: Design custom tiles or use assets from the Unity Asset Store.

- 🌈 Inverting Colors for Alternate World

    Method: Shader or script.
    Trigger: Player action (e.g., key press).

2. 👤 Player Character
- 🖼️ Player Sprite

    Source: Custom design or Unity Asset Store.

- 🎮 Movement Controls

    Implementation: Script for keyboard input control.

3. ⏲️ Time Limit Implementation
- 🕒 Timer System

    Function: Countdown timer starting from 2 minutes.
    Display: Visible on the game's UI.

- 🏁 Game Over

    Condition: Timer runs out.
    Result: End or restart the level.

4. 🌍 Switching Between Worlds
- 🔁 World Switch Mechanism

    Method: Button press to invert colors.
    Implementation: Shader parameter changes or color inversion filter on camera.

5. 👾 Enemy System
- 🎨 Creating Enemies

    Task: Design and add enemy sprites.

- 🔀 Random Spawning

    Method: Unity's Instantiate() function.
    Pattern: Random locations on the map.

- 🎯 Tracking Kills

    Feature: Count and display the number of enemies killed on the UI.

6. ✨ Additional Features and Polish
- 📊 Score System

    Basis: Enemies killed and time survived.

- 🎨 UI Design

    Elements: Timer, score, and other relevant information.

- 🔊 Audio

    Components: Background music and sound effects for various actions.

🤝 Contributing

Contributions to "InvertQuest: Time's Echo" are welcome. Please ensure you follow the existing project structure and coding standards.