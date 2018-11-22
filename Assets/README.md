# Readme

Scene switching

## Flow

```
                 /--> Level 1
Main --> Menu  -|
                 \--> Quit
```

**Main:** initialzes all important game objects, etc. before loading main menu

**Menu:** start level or quit application

In between each scene, display loading canvas. Loading canvas is part of the `SceneController` prefab, which is a singleton object. `SceneController` persists between scenes with `DontDestroyOnLoad()`.

## Running

Make sure that the scenes are in the following order in build settings:

1. `Main`

2. `Menu`

3. `Level_001`

## TODO

Loading animations:

- Shader for "shimmer" effect on sprite

- Fade in/out when enabling/disabling loading canvas
