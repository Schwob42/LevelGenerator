# Level Generator for Firefighter VR Training

This repository contains a prototype for a **level generator** used in a **virtual reality training** environment for firefighters. The goal of the project is to create a dynamic and random floor plan with rooms, corridors, fire, smoke, and injured persons to simulate realistic emergency situations for firefighter training.

## Table of Contents
- [Installation](#installation)
- [Usage](#usage)
- [Settings](#settings)
- [Algorithm Overview](#algorithm-overview)
- [Corridor and Room Elements](#corridor-and-room-elements)

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Schwob42/LevelGenerator.git

2. Unity mit Version und den Assets für Feuer, Rauch und dem Dummy...

## Usage:
  This VR training prototype is used to simulate environments where firefighters can practice navigating through random building layouts, responding to different emergency situations, and improving spatial awareness.
  - VR Simulation: Firefighters navigate through a smoke-filled building to simulate real-life emergency scenarios.
  - Dynamic Floor Generation: The system randomly generates rooms and corridors based on various parameters and user settings.

## Settings
  The user can adjust the following parameters for the environment generation:
  - Maximum depth and width of the floor (in meters).
  - Starting point for the algorithm (the access door to the floor).
  - Whether rooms and corridors should be generated or only one type.
  - Minimum number of corridor elements before a curve or intersection.
  - Probability for placing specific corridor elements (0-100%).
  - Room dimensions (in elements).
  - Option to use a seed to generate identical results.

## Algorithm Overview
The algorithm for generating the layout of the floor is based on a maze generation approach. It uses the following steps:
- Start with an empty floor grid and place the starting point.
- Use the random selection of corridor and room elements to populate the grid.
- Continuously check for conflicts in placement using Boolean operations.
- Optionally, rooms and obstacles can be placed in corridors.

## Corridor and Room Elements
The layout consists of several corridor and room types, including:
- Corridor elements: Straight corridor, corridor with a door, curves, T-junctions, X-junctions, start/end.
- Room elements: Corner, alcove, open space, and different wall configurations.

<p float="left">
	<img src="./Sources/Images/StartEnd.png" width="200" height="250"/>
	<img src="./Sources/Images/Corridor.png" width="200" height="250" />
	<img src="./Sources/Images/CorridorDoor.png" width="200" height="250" />
</p>
<p float="left">
	<img src="./Sources/Images/Corner.png" width="200" height="250" />
	<img src="./Sources/Images/TCrossing.png" width="200" height="250" />
	<img src="./Sources/Images/XCrossing.png" width="200" height="250" />
</p>






