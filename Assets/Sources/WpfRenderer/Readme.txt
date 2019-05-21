
There is a hidden "cvs" folder here that has a web application
for testing the Entitas/Non-Unity code in this project.

By running it outside of the unity engine, normal line-by-line analysis tools such as ANTS Performance/Memory profiler or DotTrace can be used.

Note that for the Unity projects to build .dll files and be referenced properly, a setting in the VisualStudio (2019) Unity plugin needs to be changed:

Tools > Options > Tools for Unity > General > Misc (Group) > "Disable the Full build of Projects" to "False"

Inside the WPF project. the build configuration 'External' needs to be used so that the Unity projects can be built *with* 'ENTITAS_DISABLE_VISUAL_DEBUGGING' from the app solution, and *without* the define when built from within Unity. 

This pertains to the use of the Unity-Only entitas feature: 'Feature'. The feature base constructor in Entitas core causes an issue because it uses UnityEngine C++ internals and therefore throws an exception when run outside the Unity editor.