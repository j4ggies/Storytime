# Storytime

## Jacob's Stuff

Press SPACE to activate animations.

Right now the animations only work once. You can open the `Spread`, then close the `Spread`, but after that it borks.

### Level structure

Each level is a `Spread`. Each `Spread` has 2 `Page`es. Each `Page` has a `PopupGroup`.

#### Spread

Each `Spread` must have a `Spread` script component. You must set 3 properties of this script using the Unity inspector window:

1. Left Page: A reference to the `Spread`'s left `Page`
2. Right Page: A reference to the `Spread`'s right `Page`
3. Spine Point: A reference to a `Transform` (can be empty `GameObject`). Should be positioned somewhere on the spine of the book. `Page`es rotate around this point.

#### Page

Each `Page` should be a `GameObject` with a `MeshRenderer` (i.e. a default cube). Be sure to remove any physics components such as `Collider`s.
Each `Page should contain an empty `GameObject` which serves as a parent for all of the `PopupGroup`s.
Pages should lay flat in the XZ plane. They should have no rotation.

#### PopupGroup

Each `PopupGroup` should be an empty `GameObject` with a `PopupGroup` script attached. You must set 2 properties of this script using the Unity inspector window:

1. Page: A reference to the `Page` object described above.
2. Pivot Point: A reference to a `Transform` (can be empty `GameObject`). Similar to the Spine Point. The point which the objects will rotate around. In general this should be the "bottom" of your model.

Each `PopupGroup` can contain a number of objects. Right now I'm just using default cubes which have been scaled and colored.
These objects should lay flat in the XZ plane. They should intersect with the `Page` which they belong to.
Be sure to remove any physics components such as colliders. The `PopupGroup` script will instantiate physics colliders for us.

### Pitfalls

In case you get weird behavior, here's some things to look out for:

* Ensure the Y positions of `Spread`s are 0
* Ensure the Y positions of `Page`s are 0
* Ensure the position of popup group parent objects (the thing that is a child of `Page` and parent of `PopupGroup`) is <0, 0, 0>
* Ensure the Y positions of `PopupGroup`s are 0
* Ensure the Y positions of your objects within `PopupGroups` are 0
* Ensure the rotation of all objects are <0, 0, 0>

These are not strict rules. It might work fine with nonzero positions or rotations. I have just found that I sometimes encounter weird behavior due to unexpected relative positions. Just be careful. **When in doubt, reset positions and rotations!**

Ideally we will fix our code to avoid some of the strange behavior we encounter.