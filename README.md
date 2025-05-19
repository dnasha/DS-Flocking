# DS-Flocking

This is a tech demo of how to implement flocking logic (based on the corresponding intermediate difficulty Unity Learn tutorial).
It goes beyond the tech demo by adding a 3d drone-style viewer so you can examine the flocking from all angles.
Additionally, a predator has also been added (not included in Unity tutorial).

To run the project locally, download the repo, and create a new Unity project from the folder "Project & Source Code".
You will find around a dozen customizable variables to play with in the flock manager object.

WARNING: Flocking (at least the way implemented here) is very computationally heavy at greater counts of entities.
You can optimize this further by changing how often each flock entity ticks in Flock.cs Update() function with random decision probabilities.
The fish are currently set to an exaggerated and more resource-heavy update rate for an easier visualization.
