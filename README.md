# Craftopia StreamIntegration
A mod to integrate twitch events into actions in game

## CraftopiaStreamIntegration
This is the mod part of the integration, it uses [BepInEx][1] to load into the game

## CraftopiaActions
The actions that the integration app uses to send to the game


## CraftopiaPatcher
A preload patcher for use with [BepInEx][1] preloaded, it adds a property for custom mob names and rewrites a method to use that if set.

[1]: https://github.com/BepInEx/BepInEx