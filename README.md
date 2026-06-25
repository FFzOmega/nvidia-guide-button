# nvidia-guide-button
simple tray application to press ctrl+home when you press the guide button on a directinput controller (i used the 8bitdo pro 3 on this project) and when pressing guide twice it will press F20 (which is my screenshot button) and holding for 1500 ms the guide button it will press F17 (which is the save the last x Minutes).
you cannot change the keys nor the guide button if doesn't match your Dinput directly on the compiled app itself, but you can alter the code.... i really made for myself but thought a lost soul could be interested in this in some capacity, you can take the source code and feed to GPT and ask him to make what you need, then just need to compile, which GPT can also teach.
i wanted something easy to work with and runs on background, lightweight and i can just transfer to other computers, this code was written in C#, if anyone want to improve this mostly useless project feel free to, just keep it open for other ppl too :)

final considerations:

to be clear, i know absolutely nothing of coding so this is 100% gpt work, i only assisted with debugging, and the project idea, with that said, don't expect any kind of troubleshooting or updates to fix issues because i don't know how to code so i cannot promisse to fix stuff i know nothing about;

all credits to the dependencies:

### .NET 10
- Copyright © Microsoft
- Used as the application runtime and WinForms framework.

### SharpDX
- Used for DirectInput controller detection.
- Repository: https://github.com/sharpdx/SharpDX
- License: MIT

### GregsStack.InputSimulatorStandard
- Used to generate keyboard input through the Windows `SendInput` API.
- Repository: https://github.com/GregsStack/InputSimulatorStandard
- License: MIT

## License

this app is licensed under the GNU General Public License v3.0 (GPL-3.0).

Although this project is licensed under GPL-3.0, it makes use of third-party libraries that are distributed under the MIT License. Those libraries remain under their respective licenses.

thanks for all of the Open Source community <3
