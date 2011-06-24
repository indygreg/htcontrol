# htcontrol - Home Theater Controls

This project contains code to control your home theater. Well, it is really optimized to control *my* home theater. I'm not sure how useful others will find it. It is available to the masses in case anybody finds any component useful for his or her own project.

# My Home Theater

My home theater consists of the following components:

* Pioneer PDP-5080 Kuru plasma television
* Emotiva MMC-1 pre/pro
* Emotiva XMC-5 amplifier
* Oppo BDB-83 Special Edition (SE) universal Bluray player
* AT&T U-Verse Motorola DVR
* Windows 7 PC (HTPC)
* Microsoft Kinect
* Salk Song Series speakers (SongTowers, SongCenter, and SongSurround)

## Connectivity

The pre/pro, television, and Bluray player are all connected to the HTPC via USB/RS-232 adapters. The DVR has only IR control. (There are HTTP control services available in the Microsoft Mediaroom software, which U-Verse runs. However, AT&T doesn't make them available to end users.) The amplifier has a power trigger cable connected to the pre/pro and its power state is thus directly linked. The Kinect is connected to the HTPC.

The audio/video cabling between components is chaotic. My particular pre/pro doesn't process audio over HDMI. However, it can pass the signal through unchanged. The DVR and Bluray player are connected to both the pre/pro's HDMI inputs. The pre/pro HDMI output is connected to the TV. The Bluray player has its 8 channel analog out going to the pre/pro (only 6 channels are in use because I don't have a 7.X setup). The Bluray player also has a dedicated stereo output wired to the pre/pro. This supposedly offers higher audio quality. The HTPC has an HDMI cable going to the TV and an optical audio cable going to the pre/pro. The DVR has an optical audio cable going to the pre/pro.

# Features

The goal of this project is to make my home theater easier to use. The wiring is complicated due to restrictions of my current equipment and my desire for optimal audio and video quality. This makes switching inputs extremely painful.

## Intelligent Source Detection

My home theater recognizes when you are trying to do something and reactions accordingly. For example, if the system is powered off and the Bluray player is powered on (notification sent via RS-232), the pre/pro is powered on and switched to the appropriate input. When a disc is inserted, if it is a video disc, the TV is powered on and everything is switched to the proper input.

If the OK or channel buttons are pressed on the DVR remote, the system automatically poweres on (if necessary) and switches all inputs for DVR/television viewing.

In a nutshell, when you do something directly related to behavior for certain activities, the system configures itself for that activity. Before, you would have to power on up to 3 components (using as many remotes) and perform manual input switching. This was very complicated to the uninitiated.

# Software Components

## SerialControl

C# library for interacting with devices via RS-232. Contains code for controlling my Pioneer Kuro PDP-5080, Emotiva MMC-1 pre/pro, and Oppo BDB-83 SE universal Bluray player.

## EmotivaControl

C# Forms applications for interfacing with Emotiva MMC-1

## SpeechControl

Provides speech recognition functionality.

## IrControl

Library for receiving and transmitting infrared (IR) codes used by my home theater. Most control is done via RS-232, but some legacy devices are present.

## HTControl

Main logic driver for *my* home theater. This is the daemon that ties all the components together.
