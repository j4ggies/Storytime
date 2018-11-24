
All files I created or used are in the Art, Physics, Scenes, and Scripts folders inside the Assets folder. 
The ones not are Jacob stuff. You can import all the folders into your build, the Scenes folder isn't required so its optional.


#Movement

Object must have the Player script and the Player tag.

Double Jump - Hit Spacebar twice

Jump - Hit Spacebar once (Can change Jump Power in Player inspector)

Basic Movement - Arrow Keys


#Checkpoint

Character must collide with objects with the CheckPoint tag to intialize the respawn point and will then respawn if they collide 
with a object that has a FallDetector tag.


#Scoring System

Each coin with the Coin tag counts as 1 so your text will display how many that you collected. It is all handle by the object with the GameMaster tag that runs the gameMaster Script. Check the script inspector where you can drag the text object you want to use to display the number of coins collected by dragging it onto the PointText parameter. 




