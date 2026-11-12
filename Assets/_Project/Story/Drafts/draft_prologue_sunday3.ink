// DRAFT: not canon until the owner approves it. Prologue "This Little Light", Sunday 3.
// ─────────────────────────────────────────────────────────────────────────────
//  Sunday 3. Iowa City, 2009. Maya is 11.
//  Owner's beats: Maya "performs okay" (neat clothes, a smile, good answers), and Mira believes
//  it. Mira moves away. Small choices only. The writing is an eleven-year-old's.
//  This Sunday also holds Maya's first real time loss (the prologue eases the player into it).
//
//  My guesses, for the owner to correct:
//   - Spring (May). Mira, now about 74, is moving to Oregon to live near her daughter.
//   - Mira gives Maya a smooth stone from her garden ("something to hold"): the grounding-object skill.
//   - Mira gives Maya her new address; Maya means to write and doesn't.
//   - The first time loss: after the goodbye, Maya is suddenly at home in her room, with no memory of
//     the drive. (Nina is 11, "formed to be the minister's perfect, quiet child": her presence is felt
//     here as the performing, but she isn't named until Sunday 4.)
// ─────────────────────────────────────────────────────────────────────────────

=== prologue_sunday3 ===
# cw: childhood-reference
# cw: neglect
# cw: depersonalization
# cw: time-loss
# place: home_maya
# shot: close
-> morning

= morning
Maya is eleven, and she has a system.

Saturday night: iron the blue dress, the one with the collar. Hang it on the closet door. Shoes polished with a paper towel. Hair brushed one hundred times, then the braid, then the clip.

Sunday morning: up before Mom. Toast, if there's bread. Teeth. Dress. Smile, in the bathroom mirror, until it looks right.

Mom says "Don't you look nice" every Sunday. It's the best part of the week.

Sarah: Don't you look nice. Grab the bulletins, would you, sweetheart? They're on the printer.

Maya grabs the bulletins. She is very good at this.
-> church

= church
# place: church_sanctuary
# shot: wide
At church, Maya hands out the bulletins at the door. She knows everybody's name.

Mrs. Hines: Good morning, Maya. Aren't you a credit to your mother.
Maya: Good morning, Mrs. Hines. I like your brooch.
Mr. Jones: There's our girl.
Maya: Good morning, Mr. Jones. How's your knee?

Rebecca comes in with her family, and Jamie and Justin and Will right behind. Rebecca has her hair in a high ponytail now, like the older girls.
Rebecca: Hey, Maya.
Maya: Hey! I like your ponytail.
Rebecca: Thanks. Will, wait up!

The Carters come in last, like always. Mr. Carter holds the door. Reneé swings through on her crutches, and gives Maya a little nod, the kind big kids give little kids when they're being nice.

Maya smiles for everybody. Her face is starting to ache. It's fine. She's used to it.
~ set_experience("Depersonalization", 0.25)

Sometimes, lately, it's like there's somebody else doing the smiling. Somebody very good at it. Maya just watches from a little way behind her own eyes, and lets her.
-> news

= news
# place: church_fellowship_hall
# shot: wide
After worship, Mira is waiting by the coffee, like always. But today she has her coat on already.
Mira: Maya, dear. Will you sit with me a minute? I've some news, and I wanted you to hear it from me.

They sit in the two chairs by the window. Mira's chairs.
Mira: I'm moving to Oregon. To be near my daughter, Ellen. My legs aren't what they were, and she worries.
Mira: I leave on Thursday.

Thursday. That's four days.

* [Say it's great news.]
    ~ record_choice("prologue.s3.news", "great", "When Mira said she was moving away, you told her it was great news.")
    Maya: That's great! Oregon is really pretty. We did it in social studies.
    Mira looks at her for a long moment.
    Mira: It is pretty. I'll send you pictures.
* [Ask her not to go.]
    ~ record_choice("prologue.s3.news", "asked", "When Mira said she was moving away, you almost asked her not to go. You asked about the weather instead.")
    Maya opens her mouth to say don't go.
    What comes out is:
    Maya: Does it rain a lot there?
    Mira: Oh, all the time. I'll grow moss, like my cardigan.
    Maya laughs, because Mira wants her to.
* [Say nothing.]
    ~ record_choice("prologue.s3.news", "nothing", "When Mira said she was moving away, you didn't say anything for a long time.")
    Maya doesn't say anything. She looks at the window. There's a fly on it, walking up and up and falling down.
    Mira waits. She's always been good at waiting.
-
-> the_question

= the_question
Then Mira takes Maya's hand, the way she did when Maya was little.
Mira: Maya. Before I go, I want to ask you something, and I want a true answer. Are you all right? At home? Really?

This is the moment. Maya knows it is.

She could tell her. About the crackers. About Wednesday nights, and Thursday nights, and the time the power went out and she sat in the dark for three hours with the phone in her lap. About going floaty, which happens nearly every day now.

But Mira is leaving on Thursday. And she looks so tired. And if Maya tells, Mira will be sad all the way to Oregon. And Mom might get in trouble.

And the somebody who is good at smiling is already smiling.
~ set_experience("Depersonalization", 0.4)

* [Tell her you're fine.]
    ~ record_choice("prologue.s3.question", "fine", "Mira asked if you were really all right. You told her you were fine.")
    Maya: I'm fine! Really. I'm good at looking after myself. Mom says I'm very independent.
* [Tell her it's better now.]
    ~ record_choice("prologue.s3.question", "better", "Mira asked if you were really all right. You told her things were better now.")
    Maya: It's better now. I'm older. I can make pasta and everything.
* [Start to tell her, then stop.]
    ~ record_choice("prologue.s3.question", "almost", "Mira asked if you were really all right. You almost told her. Then you said you were fine.")
    Maya: Sometimes it's...
    She stops. Mira's eyes are so kind. They'd break.
    Maya: Sometimes it's boring, that's all. When Mom's at meetings. But I read a lot. I'm fine.
-
Mira searches her face. Maya keeps the smile very still, like holding a full glass.

Then Mira lets out a long breath, and her shoulders come down.
Mira: Well. You've grown into a very capable young lady. I'm proud of you.
Mira: I'm glad. I'm so glad, Maya.

She believes it. Maya can see her believe it.

It should feel like winning.
-> gift

= gift
Mira opens her handbag and takes out something wrapped in a handkerchief.

It's a stone. Grey, and flat, and smooth as soap, with a white line all the way around it like a ribbon.
Mira: From my garden. I used to hold it in meeting, when my mind wandered off. You can have it now. For when you feel far away.

The stone is warm from her bag. It's heavy for its size, in a good way.

Something to hold. # exercise: grounding-object
~ learn_skill("grounding-object")
~ nudge(-0.1)

Mira writes her new address on a card, in her shaky blue pen.
Mira: Write to me. Tell me about school, and books, and everything.
Maya: I will. I promise.

Mira hugs her. She smells like soap and coffee, the way she always has.
~ journal("Note", "Mira's stone. Mira's address in Oregon. Write to her.")
-> lost

= lost
# shot: inner
Mira walks to the door with her cane. At the door she turns and lifts her hand.

Maya lifts hers.
~ set_experience("Depersonalization", 0.7)
~ nudge(0.3)

And then.
~ set_experience("TimeLoss", 0.8)
~ open_gap("prologue-s3-goodbye", "stone,dress,dark")

...

Maya is sitting on her bed.

It's dark outside. Her blue dress is hung up on the closet door, neatly, on its hanger. She's in her pajamas. There's a plate on her desk with toast crumbs on it.

She doesn't remember the drive home. She doesn't remember taking off the dress, or making toast, or the sun going down.

The last thing she remembers is Mira's hand, lifting, at the door.

* [Check the time.]
    ~ find_clue("prologue-s3-goodbye", "dark")
    ~ record_choice("prologue.s3.lost", "time", "The first time you lost time, you checked the clock. Nearly nine hours were gone.")
    The clock on her desk says 8:47. Church ended at noon.
    Almost nine hours. Gone, like a page torn out.
* [Look for the stone.]
    ~ find_clue("prologue-s3-goodbye", "stone")
    ~ record_choice("prologue.s3.lost", "stone", "The first time you lost time, you looked for Mira's stone. It was on your pillow.")
    The stone is on her pillow, right in the middle, like somebody put it there on purpose so she'd find it.
    She doesn't remember putting it there.
* [Look at the dress.]
    ~ find_clue("prologue-s3-goodbye", "dress")
    ~ record_choice("prologue.s3.lost", "dress", "The first time you lost time, you looked at your dress. Someone had hung it up perfectly.")
    The dress is hung up perfectly. Collar straight. Sash tied in a bow around the hanger.
    Maya never ties the sash like that. It's very neat. Neater than she does it.
-
~ set_experience("Derealization", 0.3)
The room looks like her room. It looks exactly like her room. That's what's wrong with it.

She holds very still and listens. The house is empty. Mom's car isn't in the drive. Thursday is the board, but this is Sunday, so it's something else.

* [Hold the stone.]
    Maya finds Mira's stone and holds it in both hands. It's cool now. Heavy. Real.
* [Feet on the floor.]
    Maya puts her feet flat on the floor and pushes. Hello, floor.
-
~ set_experience("Derealization", 0.0)
~ set_experience("TimeLoss", 0.0)
~ nudge(-0.2)

She doesn't tell anybody. There isn't anybody to tell.

She means to write to Mira on Monday, and on Tuesday, and every day after. She writes "Dear Mira" on a piece of paper four different times. She never finishes. What would she say?

She keeps the stone, though. She keeps it in her pocket, every Sunday.
-> END
