# EntitasGenerics

An experiment to replace the code generation part of EntitasECS C# version with generics.

Currently working in the context of this Entitas example project: https://github.com/RomanZhu/Match-Line-Entitas-ECS

Note: work in progress.

* The code to replace generation is in the "Assets\Libs\Entitas-Generics" folder.

# Known Issues #

* 'Set<TComponent>' and related creates a new instance compared to reusing a pooled component in Entitas generated accessors.

* 'Indexed' Entity searches currently just loops through for Equality versus the dictionary-based generated code.

* Entity access also requiring a reference to the context is a bit awkward.

* Unique and Flag components are grouped onto hard-coded entities to avoid an entity search.