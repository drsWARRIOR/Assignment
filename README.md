
# Assignment

Overview
-
This project based on the given assignment, focused on generating and interacting with 10x10 grid in Unity.

**Assignment 1**

- Generated 10x10 grid consist of cubes called as cells in project

- Each cells has a script attached to it, that contains information about it

- A raycast is performed from mouse to detect tile hover

- Tile Coordinate displayed on it

![Image](https://github.com/user-attachments/assets/0d840b1b-958f-4431-9228-82bf5ff59d2d)

**Assignment 2**

- A Unity tool with 10x10 toggleable button interface for obstacles and blocked cells

- An Obstacle Manager reads the data and places red spheres as obstacles.

![Image](https://github.com/user-attachments/assets/7df44938-a61c-41db-8796-b0858402c493)

![Image](https://github.com/user-attachments/assets/f42b80d0-7217-478b-903e-a0022fa1b474)

![Image](https://github.com/user-attachments/assets/02db3321-666d-43fa-aca3-6d5c9b82c763)



- The obstacle data is stored in a Scriptable Object

**Assignment 3**

- A player unit is placed on the grid.

![Image](https://github.com/user-attachments/assets/9ed56df7-981b-4d22-88ad-9748d2dff10d)

- The player moves to selected tiles while avoiding obstacles

- A Star Pathfinfing Algorithm is used

- Movement is animated, and input is disabled while the unit moves.

![Image](https://github.com/user-attachments/assets/44ed4691-04e8-40ba-8900-87a3c0221be3)

**Assignment 4**

- An enemy unit is placed on the grid.

- The enemy moves towards the player using the same pathfinding algorithm

- The enemy attempts to move to one of the four adjacent tiles near the player

![Image](https://github.com/user-attachments/assets/dabbed2d-f6ab-4bce-9ea3-7299173394dd)

Building Instructions
-

**Step 1**

- Clone the repository using the command below
 `git clone https://github.com/drsWARRIOR/Assignment.git`

 > **Note:** The project is created in Unity 6 (6000.0.028f1), ensure to use this version or above

**Step 2**

- Open the directory of project and go to Scenes folder inside the assests and Run Level.unity

- Run the commands below to open the project 

`cd .\Assignment\Assets\Scenes`

`.\Level.unity `

Author
-

Dev Verma
