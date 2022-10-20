-> start_knot

=== start_knot ===
Hey! It's you isn't it? What do you want this time?

*** Alcohol. $alcohol
-> alcohol
*** Money. $money
-> money
*** Just Swimming. $swimming
-> swimming

=== alcohol ===
Alcohol? You think I'd give you any of that after the last time?
    -> knot_1

=== money ===
You're asking for money again? How many times do I have to say no for you to get it?
    -> knot_1
    
=== swimming ===
Yeah, sure, sure. You're just going to ask for something later aren't you? What is it this time? Alcohol? Money?
    -> knot_1
    
=== knot_1 ===
Is that all? Because if it is, you should just scram! Get out! I don't have anything to give to the likes of you!
{swimming: If you're going to swim then get on with it already! I have nothing more to say to you. -> END}
{not swimming:
    {money: And you haven't paid me back from last time!}
    {alcohol: Remember how you crashed our party drunk?!}
}

*** The Computer. $computer
-> knot_2

*** I'm a Swimmer. $swimming
-> knot_1

=== knot_2 ===
Your computer? Oh I think I remember that old thing. You sold it off with a bunch of other stuff to pay off your debts.
How do I remember? It's because it was you sold it off while you were drunk and had a party at your house.

*** The Game Console. $games
-> knot_3

=== knot_3 ===
Yeah, we used to play games on your console. All the guys in the neighborhood would sit around your couch and drink all night long.
We'd play and drink and play and drink...
But that's all in the past now.
Maybe we got along once but I just can't keep giving you money to waste. You don't even live here anymore.
Get out.

-> END