# Flocking Experiments

I did this a long while back during AI courses at Full Sail University as a way to implement boids in my spare time (as I absolutely love the simple-implementation-yields-complex-results style of coding swarm and flock simulations) while we were learning about it.

The "Queueing" option actually has a specific story behind it as well: at one point during class I was asked to write a 'queueing' behavior for boids. I hadn't heard of queueing before (and the algorithm I made is actually very much different from the algorithm expected), so I defaulted the word to the british use of it, i.e. making a single-file line. After I was done writing it down, I decided to copy it to a notepad for later addition into this project. I was expecting it to either not compile, not work, or both.

What I *wasn't* expecting was for it to compile and work perfectly without editation. I'll say I was pretty surprised by it.

Closing notes, I wouldn't recommend implementing boids this way. Though doing this as a per-agent simulation has perks like organic swarm merging, it is definitely a *per-agent* sim and naturally has the drawbacks of eventually resulting in an excessive amount of objects running more code than is reasonably efficient for the amount. Realistically I'd suggest implementing the standard, per-swarm way where the swarm object controls all the movement for a single swarm, and maybe ad-hoc realistic-looking swarm merging back in if you just absolutely need it.

Addendum: For anyone looking to open this file, the Unity version is 5.1.3f1 or at the very least 5.1.3
