# Slap City Custom SFX

This mod allows you to override virtually every sound effect in Slap City. Replacement can be done per character, or globally across characters who share sounds. Replacing a sound is as simple as obtaining an audio file (in ogg vorbis format), naming it after the sound you want to replace, and placing it in the correct folder!

## Installation

If your game isn't modded with BepInEx, DO THAT FIRST! Simply go to the [latest BepInEx release](https://github.com/BepInEx/BepInEx/releases) and extract `BepInEx_x64_5.4.15.0.zip` directly into your game's folder, then run the game once to install BepinEx properly. Your folder should look similar to the following:

<p align="center">
  <img src="https://i.imgur.com/6jndUWB.png" width="550" title="BepInEx Installed" alt="BepInEx Installed">
</p>

Next, download the [latest release of this mod](https://github.com/DeadlyKitten/SlapCity-CustomSFX/releases/latest) and place the dll in `BepInEx/plugins`.

The mod is now installed!

## Uninstallation

Delete `BepInEx/plugins/CustomSFX.dll`.

## Usage

Before doing anything else, make sure you run the game at least once with the mod installed (you don't have to play a match, simply going to the main menu will suffice). This will generate a folder for each character under `BepInEx/Custom SFX`.

To override a sound effect, simply place a sound file (all sounds **must** be in ogg vorbis format, mp3 will not work!) into the desired character's folder with the same name as the sound effect you want to replace. You'll find a text file in each character's folder that contains a list of sound effect IDs and the moves that utilize them.

Alternatively, you can place sound effects into the `BepInEx/Custom SFX/global` folder to override sound effects that do not correspond with a particular character, or to override a sound effect with a particular ID that is shared by many characters (specific character folders take precedence).

To override a sound effect with multiple audio files, create a folder with the name of the sound effect you wish to override. The mod will choose from these files at random every time the sound effect is played. This works with both individual characters as well as global sound effects.

<p align="center">
  <img src="https://i.imgur.com/iNOM0sb.png" width="550" title="BepInEx/Custom SFX/global" alt="example image">
</p>

-------------------------------------

### Notes

- All sounds **must** be in ogg vorbis format. Other formats like mp3 **will not** work. This is due to a limitation with earlier versions of Unity where it is unable to load mp3 files.
- The text files located in each folder contain all of the available sound IDs that you can replace, as well as the moves that utilize them. However, it may take some trial and error to figure out how to replace a particular sound.
- You might need to edit sounds for various reasons, such as fixing the timing or altering the volume. I recommend Audacity for this, as it's free, easy to use, and can load most audio formats and export ogg files.
- You can use the globals folder to edit sounds for multiple characters (e.g. the `stronghit` sound or the `ko` sound). Individual characters take precedence over global replacement.
  - i.e. if you have a `ko.ogg` in the globals folder and in fishbunjin's folder, fishbunjin will use the file in his folder while every other character uses the one in the globals folder (unless they override it as well).
  
------------------------------

#### Basic Move ID Reference

The following table contains some common move IDs and move they correspond with:

| Move ID | Move Name |
| :-----: | :-------: |
| ftilt | forward tilt|
| utilt | up tilt |
| dtilt | down tilt |
| fstr / fsmash | forward strong |
| ustr / usmash | up strong |
| dstr / dsmash | down strong |
| strair | air strong |
| spn | neutral special |
| spf | forward special |
| spu | up special |
| spd | down special |
| nair | neutral aerial |
| fair | forward aerial |
| bair | backward aerial |
| uair | upward aerial |
| dair | downward aerial |
| fthrow | forward throw |
| dthrow | down throw |
| uthrow | up throw |
| bthrow | back throw |
| getupatk | getup attack|
| climbedgeatk | ledge getup attack |
