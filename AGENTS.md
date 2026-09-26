# AGENTS.md — MacroPad (fork)

Windows configurator for cheap USB macro keypads with knobs. WinForms, .NET 8, GPL-3.0.
Fork of `rOzzy1987/MacroPad`, which has had no activity since 2024. The fork is the release line.

## Remotes and branches

| Remote | URL | Role |
|---|---|---|
| `origin` | `github.com/rOzzy1987/MacroPad` | upstream, read-only for us |
| `fork` | `github.com/ysalitrynskyi/MacroPad` | ours; issues, releases, Pages |

(A fresh clone from the fork has these names swapped. Check `git remote -v`.)

- **`main` on the fork is the product.** Pushing it **deploys the GitHub Pages site** from `docs/`
  (https://ysalitrynskyi.github.io/MacroPad/). Releases are cut from it by hand.
- `integrated` has been identical to `main` so far; it is where upstream PRs were merged.
- **`webhub-protocol` is the branch of upstream PR rOzzy1987/MacroPad#46.** Keep it to the WebHub
  protocol alone; features go to `main`. Protocol fixes go to both (cherry-pick the file).

Upstream PRs, as of 2026-09-26: #44, #28 and #26 are merged into `main`. #40 (dark theme) doesn't
compile as submitted. #39 writes hex USB ids into the decimal `config.txt`, so it can never match.

## Build and release

```bash
dotnet build src -c Debug
dotnet publish src/RSoft.MacroPad/RSoft.MacroPad.csproj -c Release -o out/net8 -p:DebugType=none
dotnet publish src/RSoft.MacroPad/RSoft.MacroPad.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=none -o out/standalone
```

Version: `<Version>`/`<AssemblyVersion>` in `src/RSoft.MacroPad/RSoft.MacroPad.csproj`. A release
ships both zips (`MacroPad-<v>-win-net8.zip`, `MacroPad-<v>-win-x64-standalone.zip`) with SHA-256
in the notes. The README's supported-keypad table and `docs/index.html` (keywords, JSON-LD
version) must be updated with it. They are what makes the project findable.

Files: LF line endings, `.cs` files carry a UTF-8 BOM. Keep both.

## Protocols

`config.txt` ids are **decimal** (`28027:56570` = `6D7B:DCFA`). The last field selects the protocol:
0 Legacy, 1 Extended (both `1189` pads, from upstream), 2 **WebHub**.

WebHub (SDINNOVATION SIDE-KEYBOARD `6D7B:DCFA` and other SDCX / Huali pads) was added here.
The hardware facts below cost real time to learn:

- 64-byte frames on report id 0, interface `mi_02` (usage page `FF00`). Byte 0 = `06`, byte 1 = sub-command.
- **The pad answers every frame, and stops accepting frames until the answer is read.** Writing
  without reading wedges it after one frame (the next `WriteFile` hangs), and only a replug
  clears it. HidLibrary's timed `Read` doesn't work on .NET 6+, hence `WebHubTransport`
  (raw overlapped handle).
- **Answers go to every open handle, and the pad pushes status of its own** (after an LED key).
  `WebHubTransport` flushes the queue and matches the reply: `AA`, the same sub-command (the key
  table read `08` is answered as `07`), and the same offset for `08`/`10`. Don't loosen this.
- **Never send a sub-command you don't know.** `0x5A` jumps into the bootloader. That's why the
  upstream report-id probe is skipped for WebHub.
- Key table: 4-byte entries at offset `4 × index`. Buttons are 0–15. Each knob has three slots
  from 16: press, right, left. Types: `0x10` mouse `[buttons, 0, wheel]`, `0x13` disabled,
  `0x1F` keypad function (1/2 brightness ±, 3 effect, 4 colour, 5/6 speed ±), `0x20` key
  `[mods, usage]`, `0x30` consumer `[usage LE]`.
- Measured on the hardware 2026-09-26:
  - A wheel value of n scrolls n steps, sent on both the press and the release of a detent.
  - A consumer entry has **no repeat count** (byte 3 is ignored), so volume can't go faster on
    the pad itself.
  - LEDs dim through the HSV value; the 0–4 brightness byte has no visible effect.
- The vendor's own definitions (LED modes, key layouts) are JSON chunks in the
  www.sdcx-tech.com Next.js bundle: `"./6d7b_dcfa.json":[id,chunk]` in `page-*.js`, and the
  chunk hash is in `webpack-*.js`.
- Protocol notes originate from github.com/dozzenn/knurl (macOS port). Credit it.

## Testing against real hardware

- **The owner's pad holds their live setup.** Read it freely. Write only after they say yes, and
  restore what you changed byte for byte. They also change it themselves while you work, so read
  it again before restoring, never from memory.
- **Don't kill `RSoft.MacroPad` by name.** Test builds share the exe name with the copy the owner
  has open. Stop by PID.
- Watching input: the consumer collection (`mi_00&col03`) can be read; the keyboard and mouse
  collections are exclusive to Windows, so use a `WH_MOUSE_LL` hook for wheel events.
- Driving the UI: UI Automation for tabs, radios and menu items; `PostMessage` clicks for the
  picture's zones. WinForms `TrackBar`/`NumericUpDown` expose no settable pattern, so click the
  UpDown's `hwnd` or post key messages.
