# EntitasGenerics

An extension for EntitasCSharp that removes the need for code generation.

This project includes a conversion of the example: https://github.com/RomanZhu/Match-Line-Entitas-ECS

# Details #

* Designed to work on top of Entitas, without changes to the existing Entitas core.
* Currently using Entitas version 1.13.0
* The Good stuff is located in: "Assets\Libs\Entitas-Generics" folder.

# Differences #

##### Defining Contexts #####



# Known Issues #

* 'Indexed' Entity searches currently loops through for Equality versus the dictionary-based generated code in default Entitas.
* Event 'target' and 'priority' are not yet supported.