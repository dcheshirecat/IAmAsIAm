// DRAFT: not canon until the owner approves it. Prologue "This Little Light", Sunday 1.
// vo-speakers: Maya=maya-05
// ─────────────────────────────────────────────────────────────────────────────
//  Sunday 1. Iowa City, 2003. Maya is 5.
//  Owner's beats: Maya goes "floaty" in the long service (a first, gentle depersonalization),
//  then is forgotten at church after it. Mira finds her. Small choices only (they change feel,
//  memory and the recap; the events always happen). The writing is a five-year-old's.
//
//  My guesses, for the owner to correct:
//   - The season (a grey, cold March morning).
//   - First Day School comes BEFORE worship (common in programmed Friends churches).
//   - The service: a hymn ("This Little Light of Mine", a public-domain spiritual, as the title echo),
//     a reading, Sarah's sermon, then a few minutes of open worship (silence), as many pastoral
//     Friends churches do.
//   - The smell of the building (wood polish, old hymnals, coffee) is planted here: it is the
//     adult trigger "meeting-house-smell". No trigger cue fires in the prologue.
//   - Mira teaches "feet on the floor", the first skill.
//   - Sarah is seen only from outside, through Maya. We never hear her reasons.
//   - Mira only says "She's been waiting a long while" to Sarah here; she speaks up properly on Sunday 2 (age 8).
// ─────────────────────────────────────────────────────────────────────────────

=== prologue_sunday1 ===
# cw: childhood-reference
# cw: neglect
# cw: child-alone
# cw: depersonalization
# place: home_maya
# shot: close
-> morning

= morning
It is Sunday. Sunday is church.

Maya is five. She can put on her own tights now. She can do her own shoes, too, if they are the ones with the sticky straps.

The house is cold. Mommy is in the kitchen, talking on the phone in her church voice.
Sarah: Yes. Yes, Mrs. Hines, I'll bring the budget sheets. No, I haven't forgotten. # vo: prologue-quaker_sarah_0001

Maya waits by the table. There is no bowl out. There is a coffee cup, and Mommy's Bible, and lots of papers with writing on them.

* [Ask for breakfast.]
    ~ record_choice("prologue.s1.breakfast", "asked", "You asked for breakfast. Mommy said there would be snacks at church.")
    Maya: Mommy? I'm hungry. # vo: prologue-quaker_maya-05_0002
    Mommy puts her hand over the phone.
    Sarah: There'll be snacks at church, honey. Get your coat. # vo: prologue-quaker_sarah_0003
    Then she takes her hand away, and she is talking to Mrs. Hines again.
* [Get a cracker from the cupboard.]
    ~ record_choice("prologue.s1.breakfast", "cracker", "You found a cracker in the cupboard by yourself.")
    Maya pulls a chair over to the cupboard. She is not supposed to climb. But Mommy is busy, and the crackers are right there.
    The cracker is a bit soft. She eats it anyway, standing on the chair.
    Nobody says get down.
* [Wait. Mommy is busy.]
    ~ record_choice("prologue.s1.breakfast", "waited", "You waited quietly, because Mommy was busy.")
    Maya waits. She is good at waiting. Everybody at church says so.
    Her tummy makes a noise. She holds it with both hands so it will be quiet.
-
Sarah: Shoes, Maya. We're late. # vo: prologue-quaker_sarah_0004

Maya's shoes are on the wrong feet. They feel funny, like two lefts. Mommy doesn't see. Maya doesn't say.
-> car

= car
# place: car
# shot: over-shoulder
In the car, Mommy talks to herself. She is practicing her sermon. She does it every Sunday.
Sarah: "Let your light shine." No. "Each of us carries a light." Each of us... # vo: prologue-quaker_sarah_0005

The windshield wipers go back and forth. Back and forth. The sky is the color of dishwater.

* [Count the red cars.]
    ~ record_choice("prologue.s1.car", "counted", "In the car, you counted red cars.")
    One red car. Two. A truck that is almost red, but Maya decides it doesn't count.
    By the time they get to church, she has five. Five, like her.
* [Ask Mommy a question.]
    ~ record_choice("prologue.s1.car", "asked", "In the car, you asked Mommy where light comes from. She didn't hear.")
    Maya: Mommy, where does light come from? # vo: prologue-quaker_maya-05_0006
    Sarah: Each of us carries... hm? In a minute, sweetheart. # vo: prologue-quaker_sarah_0007
    Maya waits for the minute. It doesn't come.
* [Draw on the foggy window.]
    ~ record_choice("prologue.s1.car", "drew", "In the car, you drew a little house on the foggy window.")
    Maya draws a house on the window with her finger. A door, a window, a sun.
    The sun drips. She draws a smile on it so it looks happy anyway.
-
-> first_day_school

= first_day_school
# place: church_first_day_school
# shot: wide
The church smells like it always does. Wood polish, and old hymnbooks, and coffee from the big silver pot in the hall.

Maya likes the smell. It means: here.

Mommy goes one way, toward her office, very fast. Maya goes the other way, to First Day School.
Miss Eloise: Good morning, Maya! Come sit, we're making lanterns today. # vo: prologue-quaker_eloise_0008

Rebecca is there. Rebecca is Maya's friend. She has a purple hairband and she knows how to whistle.
Rebecca: Maya! Sit with me! # vo: prologue-quaker_rebecca_0009

So Maya sits with Rebecca. They share the glue. Rebecca's lantern is yellow. Maya's is yellow too, because Rebecca's is.

Then Jamie and Justin and Will come in, loud, with their coats still on.
Rebecca: Oh! Hi, Jamie! # vo: prologue-quaker_rebecca_0010
Rebecca picks up her lantern and goes to their table. She doesn't say bye. She just goes.

* [Follow her.]
    ~ record_choice("prologue.s1.rebecca", "followed", "When Rebecca went to sit with the boys, you followed her.")
    Maya takes her lantern and goes too. There isn't a chair. She stands at the end of the table.
    Rebecca: You can watch, Maya. # vo: prologue-quaker_rebecca_0011
    Rebecca says it nicely. Maya watches.
* [Stay where you are.]
    ~ record_choice("prologue.s1.rebecca", "stayed", "When Rebecca went to sit with the boys, you stayed where you were.")
    Maya stays. The glue is still here. She finishes her lantern by herself.
    Miss Eloise: That's lovely, Maya. Very neat. # vo: prologue-quaker_eloise_0012
    It is very neat. Maya is good at neat.
* (gave) [Give Rebecca your lantern.]
    ~ record_choice("prologue.s1.rebecca", "gave", "When Rebecca went to sit with the boys, you gave her your lantern.")
    Maya walks over and gives Rebecca her lantern. So Rebecca can have two.
    Rebecca: Thanks! # vo: prologue-quaker_rebecca_0013
    Rebecca puts it on the table with the others. She doesn't look at it again. But she said thanks.
-
Miss Eloise: Time for worship, everybody. Lanterns stay here. You can take them home after. # vo: prologue-quaker_eloise_0014
-> worship

= worship
# place: church_sanctuary
# shot: wide
The big room has long wooden benches called pews. Mommy is at the front, up high, behind the wooden stand.

Everybody else sits with their families. Maya sits in the front pew, by herself, where Mommy can see her.

Across the room is Mr. Carter, with Thomas and Reneé. Thomas and Reneé are big. They are almost grown-ups. Reneé's crutches lean on the end of the pew, the silver kind with cuffs. Mr. Carter has his arm along the back of the pew, behind them both, like a fence.

They sing. Maya knows this song.
This little light of mine, I'm gonna let it shine.
Maya sings it loud. Then she sings it quiet, because nobody else is loud.

Then Mommy talks. Mommy talks for a long time.
Sarah: Each of us carries a light. Even when we can't see it. Even when the room is dark... # vo: prologue-quaker_sarah_0015

Maya swings her feet. Her shoes are still on the wrong feet. The pew is hard.
-> floaty

= floaty
# shot: inner
Then everybody stops. It's the quiet part. In the quiet part, you don't talk. You wait for God.

Maya waits.

It is very, very quiet. A cough. The heater ticking. Somebody's tummy, but not Maya's this time.
~ set_experience("Depersonalization", 0.35)
~ nudge(0.15)

Maya looks at her hands in her lap. They look funny. Like they are somebody else's hands, a doll's hands, put there for her.

She wiggles a finger. It wiggles. But it feels far away, like it's at the other end of a long hall.

Her body is sitting on the pew. And Maya is... a little bit up. A little bit behind. Like a balloon on a string.

It doesn't hurt. It's floaty. It's a bit like a dream.

* [Hold onto something.]
    -> floaty_hold
* [Float.]
    ~ record_choice("prologue.s1.floaty", "floated", "In the quiet part of worship, you went floaty, and let yourself float.")
    ~ set_experience("Depersonalization", 0.5)
    Maya lets herself float. Up, and up. The quiet goes on, and on.
    She can see the top of her own head, nearly. A little girl in a blue dress, on a pew, very still. Very good.
    -> floaty_end

= floaty_hold
Maya looks for something to hold onto.
* [The wood of the pew.]
    ~ record_choice("prologue.s1.floaty", "wood", "In the quiet part of worship, you went floaty, and held onto the wood of the pew.")
    She puts her hand flat on the pew. The wood has lines in it, like a fingerprint. She follows one line with her finger. It goes around a knot, like a river going around a rock.
* [The buckle on her shoe.]
    ~ record_choice("prologue.s1.floaty", "buckle", "In the quiet part of worship, you went floaty, and held onto the buckle on your shoe.")
    She touches the strap on her shoe. Scratchy side. Smooth side. Scratchy side.
* [The light on the floor.]
    ~ record_choice("prologue.s1.floaty", "light", "In the quiet part of worship, you went floaty, and watched the light on the floor.")
    There is a square of light on the floor from the high window. Tiny dust is dancing in it. Maya watches one piece of dust all the way down.
-
~ set_experience("Depersonalization", 0.15)
~ nudge(-0.1)
A little bit of her comes back down. Not all the way. But some.
-> floaty_end

= floaty_end
Then Mommy says something, and everybody stands up, and it's over.

Maya stands up too. Her legs are pins and needles. Her body did the standing. She's not sure she did.
~ set_experience("Depersonalization", 0.1)
-> forgotten

= forgotten
# place: church_fellowship_hall
# shot: wide
After church there are snacks in the hall. Grown-ups hold coffee cups up high, where Maya can't knock them.

Mommy is in the middle of everybody. Everybody wants Mommy.

Mrs. Hines: Pastor Sarah, have you got a minute? It's about the roof fund. # vo: prologue-quaker_hines_0016
Mr. Jones: And the board needs to talk about the pledge numbers before next week. # vo: prologue-quaker_jones_0017
Sarah: Of course. Let's go to my office. # vo: prologue-quaker_sarah_0018

Mommy bends down to Maya, quick.
Sarah: Wait right here, sweetheart. Five minutes. Be good. # vo: prologue-quaker_sarah_0019

Then she's gone, between the coats.

Maya waits right here.

The Carters leave. Mr. Carter does up Reneé's scarf, even though she's big, and holds the door while she swings through on her crutches. Then everybody else leaves too, in ones and twos. The coffee pot gets washed. The lights in the hall go off, all except one.

The square of light on the floor slides slowly across the floor, and up the wall.
~ set_experience("TimeLoss", 0.2)

Maya is counting the squares in the window. Eleven, twelve... and then the light is in a different place. Higher. She doesn't remember it moving.

That's funny. She was only counting.

* [Go and knock on Mommy's office door.]
    ~ record_choice("prologue.s1.waiting", "knocked", "When you were left waiting, you went to Mommy's office door. You could hear talking. You didn't knock.")
    Maya walks down the hall to Mommy's door. It's closed. She can hear talking behind it. Numbers, and Mrs. Hines laughing.
    She lifts her hand to knock. Then she puts it down. Mommy said wait right here. This isn't right here.
    She goes back.
* [Get your lantern from First Day School.]
    ~ record_choice("prologue.s1.waiting", "lantern", "When you were left waiting, you fetched your lantern to keep you company.")
    Maya gets {first_day_school.gave: Rebecca's lantern instead, the yellow one, because Rebecca forgot it | her lantern}. She sits back down right here, and holds it in her lap.
    It isn't a real lantern. It doesn't light up. But it's something.
* [Stay. Be good.]
    ~ record_choice("prologue.s1.waiting", "stayed", "When you were left waiting, you stayed exactly where Mommy said. You were very good.")
    Maya stays. She doesn't move at all. If she is very good, Mommy will come.
-
Her tummy hurts now. A lot.
-> mira

= mira
# place: church_fellowship_hall
# shot: close
Then, footsteps. Slow ones, with a click, like a cane.

An old lady comes into the hall. She has white hair in a bun and a cardigan the color of moss. She was looking for her gloves, but she's found Maya instead.
Mira: Well, hello. Are you all on your own in here? # vo: prologue-quaker_mira_0020

Maya knows her face from church. Not her name.
Maya: I'm waiting. Mommy said five minutes. # vo: prologue-quaker_maya-05_0021
Mira: Did she. Hm. # vo: prologue-quaker_mira_0022

The old lady looks at the clock on the wall. She doesn't say anything about the clock. She sits down next to Maya, slow, holding the back of the chair.
Mira: I'm Mira. I'll wait with you, if that's all right. I'm good at waiting too. # vo: prologue-quaker_mira_0023

She has crackers in her cardigan pocket, in a napkin, from snack time.
Mira: Would you like these? I always take too many. # vo: prologue-quaker_mira_0024

* [Take them. Say you're really hungry.]
    ~ record_choice("prologue.s1.mira", "hungry", "You told Mira you were really hungry. She gave you all her crackers.")
    Maya: I'm really, really hungry. # vo: prologue-quaker_maya-05_0025
    Mira gives her all the crackers. Then she finds a little box of raisins in her bag, too.
    Mira: Have you had any lunch, dear? # vo: prologue-quaker_mira_0026
    Maya shakes her head. Mira's mouth goes thin, like a line drawn with a ruler. But her voice stays soft.
    Mira: Well. We'll fix that. # vo: prologue-quaker_mira_0027
* [Take one. Say you're fine.]
    ~ record_choice("prologue.s1.mira", "fine", "Mira offered you crackers. You took one, and said you were fine.")
    Maya: Just one. I'm fine. # vo: prologue-quaker_maya-05_0028
    Mira: Of course. Take one. And then, if it turns out you're a two-cracker sort of person, take another. # vo: prologue-quaker_mira_0029
    Maya takes all of them in the end. Mira doesn't say anything. She just nods, like that's what she thought.
* [Don't say anything. Lean on her arm.]
    ~ record_choice("prologue.s1.mira", "leaned", "You didn't say anything to Mira. You leaned on her arm, and she let you.")
    Maya doesn't say anything. She leans against the old lady's arm. The cardigan is soft and smells like soap.
    Mira stays very still, the way you stay still when a bird lands near you.
    Then, slowly, she puts the crackers in Maya's lap.
-
They wait. Maya eats. Mira hums the song from church, very low. This little light of mine.

Maya: Mira? Sometimes I go floaty. # vo: prologue-quaker_maya-05_0030
Mira: Floaty? # vo: prologue-quaker_mira_0031
Maya: Like a balloon. In the quiet part. # vo: prologue-quaker_maya-05_0032

Mira thinks about that for a long time. She doesn't laugh.
Mira: When I feel far away, do you know what I do? I put my feet flat on the floor, and I push, just a little. And I say hello to the floor. # vo: prologue-quaker_mira_0033
Mira: Shall we try? Shoes and all. # vo: prologue-quaker_mira_0034

Feet on the floor. # exercise: feet-on-floor
~ learn_skill("feet-on-floor")
~ set_experience("Depersonalization", 0.0)
~ nudge(-0.15)

Maya: Hello, floor. # vo: prologue-quaker_maya-05_0035
Mira: Hello, floor. There. Isn't it nice that it holds us up, without being asked. # vo: prologue-quaker_mira_0036

Mira looks down at Maya's feet. She bends, slowly, and swaps the shoes onto the right feet. She doesn't say anything about that either.
-> sarah_returns

= sarah_returns
Then Mommy's door opens, down the hall. Voices, and coats.

Mommy comes in fast. She sees Mira and stops.
Sarah: Oh! Mira. You're a lifesaver, thank you so much. We got so caught up. # vo: prologue-quaker_sarah_0037

Mira stands, slow.
Mira: She's been waiting a long while, Sarah. # vo: prologue-quaker_mira_0038
Sarah: I know, I know. It's been one of those days. Maya, coat. Say thank you to Mira. # vo: prologue-quaker_sarah_0039
Maya: Thank you, Mira. # vo: prologue-quaker_maya-05_0040
Mira: Thank you for keeping me company, Maya. # vo: prologue-quaker_mira_0041

At the door, Maya looks back. Mira is still standing in the hall, holding the back of the chair, watching them go.
-> home

= home
# place: car
# shot: over-shoulder
In the car, Mommy is on the phone again, with the phone squashed against her shoulder.

Maya puts her feet flat on the floor of the car. She pushes, just a little.

Hello, floor.
~ journal("Note", "Sunday. Mira. Hello, floor.")
-> END
