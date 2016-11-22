Drumkit XNA
===========

Drumkit XNA is a virtual drumkit that lets you play percussion sounds by 
tapping sound pads. The application contains two views for playing, a simple
view with 2D pads and a whole 3D like drumset to play with. You can record 
your beats and play them back afterwards. It is also possible to play the 
drums on top of your last recording. The application has been developed purely
on top of XNA with Microsoft Visual Studio 2010 Express for Windows Phone and 
tested to work on Nokia Lumia 800. 

Even though XNA Framework apps cannot be compiled or upgraded to target
Windows Phone OS 8.0, XNA Game Studio 4.0 apps that target Windows Phone
OS 7.1 remain fully supported and continue to run on Windows Phone 8 devices.
Compatibility with Windows Phone 8 has been verified using Microsoft Visual
Studio Express 2012 for Windows Phone and Nokia Lumia 820 and Nokia Lumia 920 
devices.

![Combined screenshots](doc/drumkit.png?raw=true)

The application has been ported from Java Drumkit example, available at  
http://www.developer.nokia.com/info/sw.nokia.com/id/1fe81968-abbf-48d1-a137-a203094611b2/MIDP_Java_Drumkit_Example_v1_0_en.zip.html 
At the same time, also a Qt version has been developed for Symbian and Nokia 
N9 devices, available at: 
http://www.developer.nokia.com/info/sw.nokia.com/id/57f87a44-0408-41c5-9d60-e2491d6793fb/Drumkit.html

This example application is hosted in GitHub:
https://github.com/Microsoft/drumkit-wp

For more information on implementation and porting, visit the wiki pages:
https://github.com/Microsoft/drumkit-wp/wiki

This project is compatible with Windows Phone 7 and Windows Phone 8 devices.


What's new in version 1.2
-------------------------

* Removed dependency to Nokia Pure Text font.


1. Instructions
--------------------------------------------------------------------------------

Before building the project, make sure you have the following installed:
* Windows 7 or later
* Microsoft Visual Studio 2010 Express for Windows Phone
* The Windows Phone Developer Tools January 2011 Update:
  http://download.microsoft.com/download/6/D/6/6D66958D-891B-4C0E-BC32-2DFC41917B11/WindowsPhoneDeveloperResources_en-US_Patch1.msp
* Windows Phone Developer Tools Fix:
  http://download.microsoft.com/download/6/D/6/6D66958D-891B-4C0E-BC32-2DFC41917B11/VS10-KB2486994-x86.exe
* Compatibility with Windows 8 and Microsoft Visual Studio Express 2012 for 
  Windows Phone has been verified

**Building:**

1. Open the SLN file: File > Open Project, select the file `WPDrumkit.sln`.
2. Select the target, for example 'Windows Phone 7 Emulator'.
3. Press F5 to build the project and run it on the Windows Phone Emulator.

**Deploying to phone:**

Please see official documentation for deploying and testing applications on
Windows Phone devices:
http://msdn.microsoft.com/en-us/library/gg588378%28v=vs.92%29.aspx


2. Implementation
--------------------------------------------------------------------------------

**Folders:**

* The root folder contains the project file, the license information and this file (README.md).
* `WPDrumkit`: Root folder for the implementation files.  
    * `Properties`: Application property files.
* `WPDrumkitContent`: Root folder for the application content, font file.  
    * `Audio`: Audio files.
    * `Images`: Root folder for images, background & splash screen.
        * `Buttons`: Button graphics
        * `Drumset`: Drumset graphics
        * `Menu`: Menu graphics

**Important files and classes:**

| File | Description |
| ---- | ----------- |
| `PadView.cs` | Class that controls the functionality of pad view. |
| `DrumsetView.cs` | Class that controls the functionality of drumset view. |
| `Pad.cs` | Class responsible of playing the sounds. |
| `Recorder.cs` | Records the drum strokes. |


3. License
--------------------------------------------------------------------------------

See the license text file delivered with this project. The license file is also
available online at
https://github.com/Microsoft/drumkit-wp/blob/master/Licence.txt


4. Version history
-------------------------------------------------------------------------------

* Version 1.2.0: Removed dependency to Nokia Pure Text font
* Version 1.1.0: First publication at developer.nokia.com based on 1.0.1 release
* Version 1.0.1: Refactoring, comments and some minor bug fixes
* Version 1.0.0: First version with both pad view and drumset view
* Version 0.2.0: Pad view
