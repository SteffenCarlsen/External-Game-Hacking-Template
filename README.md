# External-Game-Hacking-Template
[![Windows](https://img.shields.io/badge/platform-Windows-0078d7.svg?style=plastic)](https://en.wikipedia.org/wiki/Microsoft_Windows) 
[![License](https://img.shields.io/github/license/danielkrupinski/Osiris.svg?style=plastic)](LICENSE) 

Free Open-source External-game-hacking template, for making the development of external cheats easier and to have the code-base support you from the start.

## Features
* **Extensions**
    * **CacheExtensions** - Easy to use datacaching, for easier data-access-optimization
    * **KeyboardExtensions** - Easy to use wrapper around GetAsyncKeyState
    * **MathExtensions** - General purpose math functions
    * **VectorExtensions** - Vector extensions to System.Numeric.Vectors
    * **MemoryExtensions** - Memory extensions to manipulate structures and byte arrays
    * **CacheExtensions** - Easy to use datacaching, for easier data-access-optimization
    * **MouseExtensions** - Send-input wrapper for mouseinput
    * **ProcessExtensions** - General extensions for interacting with the Process Class
    * **SettingsExtensions** - Wrapper around Newtonsoft.Json for easier reading and writing json files
    * **SteamExtensions** - Getting the steam folder location can sometimes be a bitch, this makes it easy using reg-keys
* **Memory Library**
    * **Read/Write memory** - Generic implementation of Read/Write memory
    * **ReadMatrix & ReadString** - Reading objects unable to be read using the generic implementation
* **Settings**
    * **Config system** - Easy to use and extend config system
* **Windows imports**
    * **User32** - Most of the needed imports from User32.dll, ready to use
    * **Kernel32** - Most of the needed imports from Kernel.dll, ready to use
    
# TODO:
* Low-level keyboard and mouse hooks
* Include Managers/Features and implement KISS example
