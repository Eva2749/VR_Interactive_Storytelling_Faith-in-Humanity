# Faith (in) humanity: A VR-driven Interactive Storytelling Game Adapted From *The Three-Body Problem*


See the full documentation here: https://dhabiakm.github.io/Alt-Realities-Final-Project/

# Project Description
### Inspiration and Questions to Explore
Our project is inspired by the popular science fiction novel *The Three-Body Problem*, whose plot focuses on extraterrestrial forces that are trying to destroy Earth. Our take on it, however, was not to replicate the storyline nor the idea itself but rather zoom into the people aspect of the story. How would different people react when they realize that everything would collapse? What's the best direction for humans to go and who we should be listening to? As the player, you'll be enrountering various groups of interest and be left with the decision to decide the future of the human beings. What decision will you make? Is that a right choice? 

We twisted the story background a little bit to intensify the moral problem. The main character is a prestigious inventor who has the supermacy to determine humans' future. Faced with a dying Earth and the invitation from an unknown extraterrestrial, the inventor is left with a moral question - should we trust the aliens and select a group of human elites to join them? Or should we reject the aliens' invitation and stay on the planet, which is doomed to death? Either decision would lead to huge loss of humans' life and with little hope. It's a gamble. 

### User Experience
Our goal is to create a playable experience through which the player would interact with people with different points of views in terms of the future of the dying Earth, before the player makes the final decision. The core experience is focused on immersive storytelling through character building and environmental building, so the majority of the efforts is dedicated to ensuring a linear and progressive interaction sequence. 

# My Work
## Scene 3 - The Lab
![image](https://user-images.githubusercontent.com/71305489/182528984-55335637-d986-4c90-9788-16d6d2640784.png)
### Background
The lab scene is the player’s first encounter with the group of interest — the scientists. The inventor/player would be greeted by the representative of the scientist group - Joseph, who is also her friend. Joseph would show the hyper-growth concept to the inventor in an attempt to persuade the inventor into supporting the scientists.

### Interactions Sequence

- Greeted by Joseph: when the inventor enters the lab, Joseph would come over and greet her
- Follow Joseph and see him talking: after greeting, Joseph would ask the user to follow him over to the lab table. When the user comes, Joseph would introduce her to the concept of hyper-growth.
- Pour the bottle: Joseph would instruct the inventor to pick up the yellow bottle on the table and pour it inside the blue container on the robot's hand.
- See the plant grow: after the inventor successfully pours into the blue container, the inventor would see a pump transferring the yellow chemicals into the tube, and the plant will grow.
- Respond to Joseph: after the inventor sees the plant growing quickly, Joseph will ask her if such an effect has changed her mind. Then a UI would appear asking the inventor to respond (has two options). Joseph would respond accordingly.
- Leave the lab: the inventor received enough information and would like to take a walk outside to clear her head. Joseph would say goodbye to her and the inventor would go to the teleport portal and teleport to the outside world.

### Implementation

![image](https://user-images.githubusercontent.com/71305489/182591126-46b2e9ef-e12c-404c-8260-1cfe6e8a09ea.png)


## Scene 4 - The Outside
![image](https://user-images.githubusercontent.com/71305489/182640143-7ba23c4b-20d5-468a-a0a5-a5d64dfcdb84.png)

### Background
The outside scene is where the inventor encounters another two groups of people who hold vastly different opinions about the right choice about humankinds’ fate. One group is the panicked crowd, who would be running around, shouting or sighing in response to the desperate situation of the Earth. The inventor would see them, but won’t interact with them. Another group is the cult, who worship the unknown extraterrestrials, regarding them more as gods who would save humans. The cult would see the inventor and come to her to persuade her into trusting and contacting the aliens. The cult also hate the inventor because the inventor, who herself is also a scientist, is involved in the process of high-tech development at the expense of resources depletion.

### Interactions Sequence
- The inventor teleports to the outside area and sees people running and shouting around
- Three cults notice the inventor and run to her in sequence
- Three cults start talking to the inventor one after another
- After they finish talking, run back to their original place and start praying again

### Implementation
![image](https://user-images.githubusercontent.com/71305489/182640403-d1168e12-5316-4911-9e5a-468cb8e0cdc6.png)


# Challenges I've faced and how I solved them

## Technical Challenges
Our project is animation and interaction heavy. The hard part is not only about the detailed scripting logic, but also about linking everything together (animation, scripts, shaders, particles, etc). There are many details in each step that would require logics and systematic thinking to keep track of everything. Also, the Awake(), Start(), Update() and Coroutines should be considered carefully before using them because they’re directly related to the frames and would directly impact the visual effects.

Take the pouring interaction and plant growing animation as an example. The steps are explained in detail as follows:

![image](https://user-images.githubusercontent.com/71305489/182640767-f26d1402-3d75-4739-b788-8b9fb001e93a.png)

There are actually far more details than described above, like how much/fast the particles move, how to make the particles move within a certain boundary and when to kill them. Also, whether to update the animation in Update() or in a Coroutine would also impact the visual effects. There are a lot of detailed manipulations about the parameters to make them work as a whole. We also had a lot of issues with small missing elements or steps that would take us HOURS to figure out (ie. accidentally deleting a rigid body).

Another technical challenge we ran into was related to our characters. We had character that we would’ve loved to use but because of how high-poly they were, they destroyed the oculus’ performance causing us to have to choose to let them go.

One last one to mention was implementing the dialogue, we did waste a week trying to integrate ink, when going with a much more simpler hardcoded option was more than sufficient enough for our project.

## Expecting User Behavior
Following the previous section, there are also challenges about expecting user behavior. For example, in the lab scene, there are many expected behaviors that users may not necessarily follow. Another mind-map to show the problems and solutions:

![image](https://user-images.githubusercontent.com/71305489/182640874-1d4169fe-fc82-451d-b354-2868f03a43d6.png)

The detailed expectations of possible user behavior and proper limitation/intervention could protect the whole user experience without breaking the fourth wall.

## Storytelling Challenges
Our project was a very ambitious one, totally at 7 scenes that the user experiences. The challenges appeared soon after, when we realized the considerable amount of work that we had to pour into each scene to make it work in the grander scheme of the tale itself. We had to find a way to deliver a playable experience to the users taking into account our time and skill restraints. The user initially was meant to feel small and hopeless, not knowing what to decide. Instead we twisted in a way that would form a more empowered user, one that is aware of what is happening and confident of the power that they are given. Our focus shifted to have the player listen to all the different voices before making the decision. When the time comes, the user sees those same voices, reminded of the weight of the situation and that the choice is not only affecting them, but many others as well.

A compromise we had to make lies in the fact that our original characters were a bit too much for the oculus to handle (we ran into a performance issue), this made us decide to lower the amount of NPCs that we have, only keeping those that were crucial to the story line. In the outside scene for example (we would have loved to have more screaming people), we had to significantly reduce the crowd size.

Another storytelling challenge also came with the two ending scenes, we already had built so many interactions so the question was: do we add more interactions? or do we leave space for the user to contemplate? We chose the latter, due to the lack of time and to try to have open ended scenes where the user can find themselves imagining what the consequences of their choices will be.



