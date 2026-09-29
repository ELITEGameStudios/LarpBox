# LarpBox

An subtle imitation of killbox for an assignment.
Noah Simmons, 100982633

Press Space to play, you must survive the timer each round evading different enemies and navigate across different spawn patterns and Map environments. Good Larp.

The singleton design pattern is present in The use of a GameManager (Denoted as LarpManager in source code), and LarpMessageManager (UI announcement system). These are utilized in many different ways, but the LarpMessageManager is simplest to show in some form of diagram

Any Class -> Calls LarpMessageManager.Instance...
Returns: The single class of LarpMessageManager to announce things to the world
If many things are announced at once, the latest Announce function call to the singleton will be prioritized over all others.

Both of these places are great cases for the singleton design pattern because only one instance of these classes should serve as the chief authors of both the gamestate+status, as well as what the message UI element is saying. Any ambiguity or other instances for these are redundant and/or are prone to skewed and broken game states and messages, collapsing the very project it tries to serve.

The Factory design pattern is most prevalent in the maps/spawn system, using the interface known as ILarp (I couldnt find a better alias for maps). 
In LarpManager.Instance.TransitionMap()
// Selects random map in mapsList
calls Initialize() from ILarp
Map Class handles Initialization however it pleases

In LarpManager.Instance.BeginRound()
calls StartCoroutine(_currentLarp.SpawnCoroutine()), where _currentLarp is an ILarp interface
Map class handles spawning however they want. (DiscreteSpawnsLarp.SpawnCoroutine() or RadialSpawnsLarp.SpawnCoroutine())

The factory design pattern is good for this map/spawn system because it allows different spawn systems to be created for each maps different needs or patterns it wants to support. While you could get away with just hand placing spawns everywhere, having this extra functionality adds expandability and faster creation of really cool and interesting patterns which would otherwise be tedious to build.

Another place in which elements of the factory design pattern is implemented is my enemy classes through ILarpemy, which doesnt take advantage in the same way that the map system does, but still includes a key difference between the two given enemies, and keeps the system expandable in the same way that can be said for the map system.
