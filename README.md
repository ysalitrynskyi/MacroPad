# MacroPad — software for cheap Chinese USB macro keypads (Windows)

A free, open-source replacement for the configuration tools that come with the small **USB macro keypads / mini keyboards** sold on AliExpress, Amazon, Temu and eBay: the 1–12 key pads with one to three rotary **knobs**, often listed as *"MINI KeyBoard"*, *"3 key 1 knob macro keyboard"*, *"one-handed programmable keypad"*, *"volume knob keypad"*, or the **SDINNOVATION SIDE-KEYBOARD**.

Use it to put a keyboard shortcut, a media key, a mouse click or scroll, or an LED control on every key and every knob direction. The mapping is stored **in the keypad itself**, so the pad keeps working on any computer, with nothing running in the background.

This is a fork of [rOzzy1987/MacroPad](https://github.com/rOzzy1987/MacroPad). Version 1.1 adds:

- **SDINNOVATION / SDCX / Huali keypads** (the ones the vendor configures in a browser at sdcx-tech.com or huali-tech.com, or with `SDTech.Options`), including the **SIDE-KEYBOARD** (`VID 6D7B`, `PID DCFA`, three keys and one knob).
- **What's on the keypad now:** the app reads every key's current setting back from the pad and confirms each upload.
- **Scroll speed:** a knob turn or a key press can scroll 1–10 wheel steps.
- **LED brightness and speed** for single-colour and animated effects, plus keys that change the keypad's own LEDs (brightness up/down, next effect, next colour, speed up/down).
- A clearer window: labelled toolbar, numbered steps, and a picker for keys your keyboard doesn't have (F13–F24, keypad keys).
- The community fixes that were waiting upstream: the window no longer collapses on normal-DPI screens (#44), mouse functions no longer crash (#28), a 4-key layout (#26), and the project is on .NET 8.

## Download

Get the latest build from **[Releases](https://github.com/ysalitrynskyi/MacroPad/releases/latest)**:

- `MacroPad-<version>-win-x64-standalone.zip`: runs on any 64-bit Windows 10/11, with nothing else to install.
- `MacroPad-<version>-win-net8.zip`: smaller, needs the [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (Windows asks to install it on first start if it's missing).

Unzip anywhere and run `RSoft.MacroPad.exe`. No installer, no driver.

## Supported keypads

The app finds the pad by its USB vendor and product id. Windows shows them in Device Manager → the keypad's *USB Input Device* → Details → Hardware Ids, as `VID_xxxx&PID_xxxx`.

| USB id (hex) | In `config.txt` (decimal) | What it usually is | Protocol | Tested |
|---|---|---|---|---|
| `6D7B:DCFA` | `28027:56570` | SDINNOVATION **SIDE-KEYBOARD**, 3 keys + 1 knob | WebHub | yes, this fork |
| `1189:8890` | `4489:34960` | "MINI KeyBoard", 3 keys + 1 knob (also a 4-key variant) | Legacy | yes, upstream |
| `1189:8840` | `4489:34880` | Mini typewriter-style pad, 6 keys + 2 knobs | Extended | community |
| `1189:8830`–`8833` | `4489:34864`–`34867` | 6/9/12 keys with 1–3 knobs | Extended | not verified |
| `1189:8810` | `4489:34832` | Extended-protocol pads | Extended | not verified |

Not listed? Add a line to `config.txt` (format below). Pads with only keyboard interfaces and no vendor-defined HID interface are configured some other way and can't be reached by any tool of this kind.

## Quick start

1. Plug in the keypad and start the app. The status bar turns green and the right layout is picked; for WebHub pads the panel on the right shows what every key does right now.
2. Click a key in the picture, or a knob: its **centre** is the press, its **left and right sides** are the turns.
3. Under *Key setup*, choose what it should do: record a shortcut (● then press the keys), pick one from the list (≡), choose a media key, a mouse action and its scroll speed, or set the LEDs.
4. Click **Upload to keypad** (Ctrl+S). The status bar confirms what the key now does.

### Common questions

- **The bundled `MINI_KeyBoard.exe` doesn't find my pad, or is in Chinese.** This app replaces it. Check the USB id against the table above.
- **Can the knob change the volume faster?** Not on the WebHub firmware: a media key entry has no repeat count (tested; one volume step per detent). The knob *can* scroll faster: set the Mouse tab's scroll speed.
- **Can I dim the LEDs?** Yes, on WebHub pads: the LED tab's brightness slider works with a single colour. For the palette effects the pad cycles its own colours.
- **Does it need admin rights or a driver?** No. It talks to the pad through the standard Windows HID driver.
- **Mac or Linux?** This app is Windows-only. For macOS there is [Knurl](https://github.com/dozzenn/knurl).

## Credits

Original app by Mihály Rozovits ([rOzzy1987/MacroPad](https://github.com/rOzzy1987/MacroPad)), GPL-3.0. Community pull requests merged here are by akiijauto (#44), Ryan Bernstein (#28) and RyanWor (#26). The WebHub protocol notes come from [Knurl](https://github.com/dozzenn/knurl). The SIDE-KEYBOARD support, read-back, scroll speed and LED controls were added in this fork and verified on the hardware.

---

*The original project's guide follows.*

## Original introduction

So you've ordered a chinese macro keypad and the software supplied doesn't make any sense to you? That was my problem as well...

![image](https://github.com/rOzzy1987/MacroPad/assets/617600/adf5b698-9ba4-4060-ade0-1fb078cac21c)

Enter the RSoft MacroPad!

![image](https://github.com/rOzzy1987/MacroPad/assets/617600/5fd74dc1-b420-4388-be8b-f427a05bedca)

## GUI
The main funcitons of the GUI are displayed on the image below
![image](https://github.com/rOzzy1987/MacroPad/assets/617600/857b150b-291a-419f-82b2-e10fad5ed53f)

The key sequence box is displaying keys to be sent by the macro keypad. You can see which modifiers are also sent with each keystroke, and on which sides were they used.
The example below shows `LeftCtrl + LeftShift + F5` `Enter`

![image](https://github.com/rOzzy1987/MacroPad/assets/617600/0e21d542-9a4e-44ef-bc9b-868b0bf810ac)

The key difference to the original software is the this app translates hardware scancodes. This means if you have a non-US keyboard, you still can record keystrokes and will get the same keypresses when using the keypad 

## Usage
### Starting
Open the app and connect your macro keypad.
 
<sub>The software will configure the first keypad it finds, so if you have multiple, disconnect the others while starting the app.</sub>

If there is an existing configuration for your device, the app will select the layout for you. 

If you can't see your keypad's visuals on screen or what you see is not your keypad you can select another one from the menu using the ![image](https://github.com/rOzzy1987/MacroPad/assets/617600/d5ec2fd0-3729-40ef-aad6-aa7030fdadf3)

 icon. If you can't find any, that resembles your keypad, you can just select the _***12 buttons 3 knobs***_ layout, and use that (this is just a visual representation, doesn't affect operation)


### Operation
#### Selecting key to configure
Find the key on the visual that you want to configure and click on it. For knobs, click on the center part for the push function, or click on either side for the rotation function. The selected part should be highlighted.

If your keypad supports layers, select the layer as well with the radio buttons above.

#### Selecting the function
You can either set the LED mode or the selected key's function. To choose just select the appropriate tab in the _Key setup_ box.

##### LED mode
<sub>This section is incomplete due to insufficient stock of testing devices. I.e. I (the author) just bought the cheapest device that doesn't support a lot of features others do. For example this 3 button 1 knob keypad doesn't support colors, and only has 3 modes, on being the Off state</sub>

##### Key sequence
You can record a key sequence for a button that will be played back using the record ![image](https://github.com/rOzzy1987/MacroPad/assets/617600/f605970c-4aa9-4aeb-84aa-b4ff88b8f164) button. If you want to correct a typo or want to start over just use the backspace
![image](https://github.com/rOzzy1987/MacroPad/assets/617600/7e396ae4-1db1-4bfc-a4b5-d19c1af4af29) and clear ![image](https://github.com/rOzzy1987/MacroPad/assets/617600/2ec5a021-479d-4a49-a881-7573ca38bbb8) buttons.

The app records keys and modifiers together.

**NOTE:**
The 3 button 1 knob keypad only stores modifiers for the first key in the sequence! Others should work as intended.

##### Media keys
Support is limited, but it works just by selecting the media key you need

##### Mouse
Select the button or scroll direction you need and add modifier keys as needed

<sub>The 3 button 1 knob keypad only supports the left side modifiers, but others should support all</sub>

#### Saving to keypad
To save your settings just click the upload button  ![image](https://github.com/rOzzy1987/MacroPad/assets/617600/79128933-1b53-419a-a90d-3a04515bce8c)

## Contribute
### layouts.txt
If you have a keypad that is not represented in this software, you can define your own layout in _layouts.txt_. If you do, and it works, please add it in a pull request, or just send it to me.

This file is storing visual representations and metadata only, it will not limit or change anything in the communication between the keypad and your computer.

#### File format
This file is a list of layout definitions
- You can comment a line using `//`
- Whitespace-insensitive
- Indentation is optional

Layout definition:
```
Layout: {LayoutName}
	{VendorId}:{ProductId},[{VendorId2}:{ProductId2}, ...]
	{LayerNumbers}:{MaxCharactersInSequence}:{DelaySupport}:{LedColorSupport}:{LedModeCount}
	{ButtonsAndKnobs}	
```

Button format:
```
B{ButtonId},{PosX},{PosY},[{SizeX},{SizeY}]
```

Knob format:
```
K{KnobId},{PosX},{PosY},[{SizeX},{SizeY}]
```

SizeX and SizeY have a default value of 20 and roughly translate to mm units  
ButtonId can be between 1-12  
KnobId can be between 1-3

### config.txt
This file tells what kind of devices to look for. The app is looking for a device with the given VendorId and ProductId. Then, it will search for a HID device under that, which has the given path parameter present in it's path


#### File Format

This file is a list supported devices, each is a single line
- You can comment a line using `//`
- Whitespace-insensitive
- Indentation is optional
```
{VendorId}:{ProductId},{PathFragment},{ProtocolVersion}
```

ProtocolVersion can be 0 (Legacy) or 1 (Extended). Apart from one device (the 3 button 1 knob) all devices use the extended protocol

ProtocolVersion 2 (WebHub) is for a different family of keypads: the SDINNOVATION / SDCX / Huali pads, which their vendor configures with a browser based WebHID tool. The SIDE-KEYBOARD (`28027:56570`, 3 buttons 1 knob) is configured this way out of the box. On these keypads
- every key or knob action holds a single keystroke (with modifiers) or a media key
- mouse functions are not supported
- the LED tab sets the backlight effect (off, solid, breathing, blink, tide) and its color

The WebHub protocol notes come from [Knurl](https://github.com/dozzenn/knurl), a macOS port of this app, and were verified here against a SIDE-KEYBOARD by writing mappings and reading them back.

#### Adding your own
Find your device in Windows' Device Manager:
- `View` > `Devices by type`
- In the device tree find `Human Interface Devices` > `USB Input Device` 
  - You will find several of these so start by looking at their hardware ids. Write them down 
    - Double click device and select the `Details` tab
    - Select `Hardware Ids` from the Properties dropdown. You'll find the VendorID, ProductID and PathFragment in the format of `USB\VID_{VendorId}&PID_{ProductID}&{PathFragment}`of `USB\VID_{VendorId}&PID_{ProductID}&REV_0000&{PathFragment}`
  - Disconnect the keypad and check which ones disappeared from the tree. Use this information to write a new line in the config file
- For the ProtocolVersion your best bet would be `1` since most of these keypads are using that. If that doesn't work, try `0`.

### Bug reporting
If you encounter any bugs, please consider creating a ticket in the issues serction on github or contact me via email.

Also please consider sharing 
- a photo of your keypad
- a screenshot of the app just after connecting the keypad
- the `hid.log` file and the `error.log` file in your app's root directory

## Disclaimer
Please note that I am but one single developer upset about the unfriendliness of the original software shipped with my keypad. My intention is to make good, usable software, but I did, do and will make mistakes. Theoretically this software may not cause any harm to your computer, or any peripherals whatsoever, but still: Use at your own risk!

## Contact me
For this fork (SIDE-KEYBOARD / WebHub support, read-back, scroll speed, LED controls) open an issue at https://github.com/ysalitrynskyi/MacroPad/issues. For the original app:

Drop a mail to rozovits.mihaly@gmail.com
