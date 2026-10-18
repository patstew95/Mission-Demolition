# Mission-Demolition
Game Dev Fall 26

WEBGL link: https://play.unity.com/en/games/b00c0ace-6187-4581-a166-77a6e85e6b1b/webgl-builds

Improvement: I added a wind system to Mission Demolition to make each shot more strategic. At the start of every level, the game picks a random wind strength between -2 and 5. In testing I found stronger headwinds prevented the projectile from flying far enough. The current conditions appear on screen as a tailwind or headwind. While a projectile is in flight, the wind applies a constant horizontal force to it. A tailwind carries shots farther, and a headwind holds them back, so the player has to read the conditions and adjust their aim to compensate.