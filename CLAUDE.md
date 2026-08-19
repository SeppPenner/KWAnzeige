# Project rules for Claude

## What this is

KWAnzeige is a small Windows Forms application that shows the current ISO 8601 calendar week in a
read only text box. It has a language combo box (German and English), a minimize button and nothing
else. The whole application is one form. It is shipped as an Inno Setup installer, it is **not** a
NuGet package: no `GeneratePackageOnBuild`, no push script.

One solution `src/KWAnzeige.sln` with exactly two projects:

- `src/KWAnzeige/KWAnzeige.csproj`, `OutputType` `WinExe`, `UseWindowsForms`, the application.
- `src/KWAnzeige.Tests/KWAnzeige.Tests.csproj`, MSTest, added in version 1.0.8.0.

Layout inside `src/KWAnzeige`:

- `Program.cs`: the `Main` method, `[STAThread]`, `Application.Run(new Main())`.
- `Main.cs`: the form logic. `MainLoad` starts the timer, `TimerTick` refreshes the text box,
  `InitializeLanguageManager` and `LoadLanguagesToCombo` set up the languages,
  `ComboBoxLanguageSelectedIndexChanged` and `OnLanguageChanged` handle a language switch,
  `ButtonMinimize_Click` minimizes the window.
- `CalendarWeekCalculator.cs`: the calendar week calculation, the only piece of the application that
  is not tied to the form. It lives outside `Main.cs` so that it can be tested.
- `Main.Designer.cs` plus `Main.resx`: designer generated code and the embedded form icon. Do not
  hand edit the layout, the designer owns it.
- `GlobalUsings.cs`: all usings of the project, including the alias `Timer`.
- `languages/de-DE.xml` and `languages/en-US.xml`: three keys each (`TodaysCW`, `Minimize`,
  `Title`), copied to the output directory with `CopyToOutputDirectory=Always`.
- `License.txt` and `Calendar.ico`: shipped next to the executable, the icon is also the
  `ApplicationIcon`.

Layout inside `src/KWAnzeige.Tests`:

- `CalendarWeekCalculatorTests.cs`: the documented ISO 8601 edge cases as `DataRow`s, every day of
  one week, the comparison against `ISOWeek.GetWeekOfYear` of the framework for every day from 1900
  to 2100, the allowed range, the time of day and the current culture.
- `GlobalUsings.cs`: all usings of the test project.

Everything else in the repository:

- `Setup/KWAnzeige-Setup.iss`: the Inno Setup script.
- `Setup/build-setup-files.bat`: deletes every `bin` and `obj` below `src`, publishes the
  application self contained for win-x64 to `src/KWAnzeige/bin/publish` and removes the `*.pdb`
  files. It does **not** compile the installer, that is a separate `ISCC.exe` run. A correct publish
  is 274 files and about 118 MB, which makes the installer about 35 MB. If it comes out at 9 files
  and 1 MB, the `--self-contained` switch got lost and the target machine would need an installed
  .NET runtime.
- `Setup/KWAnzeige-Setup.exe`: the built installer, not tracked, it hangs on the GitHub release of
  its version tag.
- `README.md` (the only user documentation), `Changelog.md`, `License.txt` (MIT),
  `Screenshot_DE.PNG`, `Screenshot_EN.PNG`, `.gitattributes` and `.gitignore`.
- `src/.editorconfig` and `src/KWAnzeige.sln.DotSettings`.

There is no `Updating.md`, no `HowToUse.md`, no `.github` folder and no pipeline file.

## Build

```powershell
dotnet build src/KWAnzeige.sln -c Release
```

```powershell
dotnet test src/KWAnzeige.sln -c Release
```

- Single target framework `net10.0-windows` in both projects, no multi-targeting.
  `RuntimeIdentifiers` is `win-x64` in the application, because the publish for the installer is a
  win-x64 publish.
- All build properties live directly in the two `.csproj` files and are duplicated there. There is
  **no** `Directory.Build.props` in this repository.
- `TreatWarningsAsErrors` is enabled, so every warning breaks the build, NuGet warnings (`NU****`)
  from restore included. A clean build reports zero warnings, keep it that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.8-1` for the first
  commit after tag `1.0.7`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. Several private feeds are configured globally on this machine. If one of
  them answers 404 for public packages, restore fails with `NU1301`. Then build with an explicit
  source: `dotnet build src/KWAnzeige.sln --source https://api.nuget.org/v3/index.json`.
- Tests are MSTest, in the single test project `src/KWAnzeige.Tests`, which follows the same package
  set as the sibling repositories: `Microsoft.NET.Test.Sdk`, `MSTest.TestAdapter`,
  `MSTest.TestFramework`, `coverlet.collector` and `GitVersion.MsBuild`. `dotnet test` runs 25 tests,
  they need no network, write nothing and leave the working tree untouched. The test project sets
  `UseWindowsForms` as well, because it references the application project. Never claim a test run
  happened without running it.
- Only `CalendarWeekCalculator` is covered, the form itself is not. Beyond the tests, a behaviour
  change is verified by publishing and starting the executable and reading the window through UI
  automation, which is the only way to see the text box content from a script:

  ```powershell
  Add-Type -AssemblyName UIAutomationClient, UIAutomationTypes
  ```

  A correct run shows the window title `Aktuelle Kalenderwoche`, the label
  `Aktuelle Kalenderwoche:`, the button `Minimieren` and the current calendar week in the text box.

## Code conventions

Follow the surrounding code, it is consistent throughout every hand written file:

- File header comment block with `<copyright file="..." company="Hämmer Electronics">` and a
  `<summary>`, then the file-scoped namespace.
- XML doc comments on every type and every member, private members included, no exceptions.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into `GlobalUsings.cs`, inside the existing `#pragma warning disable
  IDE0065` block, never at the top of a file. The editorconfig requires usings inside the namespace
  (`csharp_using_directive_placement=inside_namespace:warning`), which global usings cannot satisfy,
  that is what the pragma is for. Do not add other pragmas. The comment text in that block is
  German because Visual Studio generated it, leave it alone.
- Fields, properties, methods and events are always accessed with `this.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning`).
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8, file scoped namespaces, `System` usings sorted first and `IDE0005` as warning. Analyzer
  warnings are fixed, not silenced.
- `Main.Designer.cs` is exempt from all of this, it keeps the designer style with fully qualified
  type names and the block namespace.
- The language XML files are UTF-8 without BOM, CRLF, and indented with tabs. Edit them with a
  script that preserves that, not with a tool that reformats.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **The `Timer` alias and its `SynchronizingObject`.** `GlobalUsings.cs` contains
  `global using Timer = System.Timers.Timer;`, which is needed because
  `System.Windows.Forms.Timer` would otherwise win through `ImplicitUsings`. That timer raises
  `Elapsed` on a ThreadPool thread, and `TimerTick` writes to a control, so `MainLoad` sets
  `SynchronizingObject = this`. Removing that line reintroduces an illegal cross thread call that
  stays invisible without an attached debugger, because `CheckForIllegalCrossThreadCalls` defaults
  to `Debugger.IsAttached`. Switching to `System.Windows.Forms.Timer` instead would also be correct,
  but then `Elapsed` has to become `Tick` and the alias has to go.
- **The timer runs every second for a value that changes weekly.** `MainLoad` sets
  `Interval = 1000`. That is not about the calendar week itself, it is the only mechanism that
  notices the rollover at midnight while the window stays open.
- **The German texts appear through a chain of accidents.** `InitializeLanguageManager` calls
  `SetCurrentLanguage("de-DE")` **before** it subscribes to `OnLanguageChanged`, so that first event
  is lost and the texts stay at the designer defaults (`Today's CW`). They only get translated
  because `LoadLanguagesToCombo` afterwards sets `SelectedIndex = 0`, which raises
  `SelectedIndexChanged` and sets the language a second time, this time with a subscriber attached.
  That index 0 is German depends on the file order the language library returns, `de-DE.xml` before
  `en-US.xml`. Adding a language file that sorts before `de-DE.xml` would change the startup
  language.
- **The window title comes from the language file**, key `Title`, not from GitVersion. There is no
  version anywhere in the user interface.
- **`GetWord` returns `null` for an unknown key** and does not fall back to another language. A new
  key has to go into **both** language files, otherwise one language shows an empty control.
- **Mixed control naming in the designer.** The fields `ButtonMinimize` and `LabelCW` are
  PascalCase, `textBoxCW`, `comboBoxLanguage` and `tableLayoutPanelMain` are camelCase, and the
  generated comments above them still say `buttonMinimize` and `labelCW`. `ButtonMinimize_Click`
  carries an underscore while `MainLoad`, `TimerTick` and `ComboBoxLanguageSelectedIndexChanged` do
  not. Renaming means touching the designer file, leave it.
- **`Calendar.ico` exists twice**, as `src/Calendar.ico` and as `src/KWAnzeige/Calendar.ico`, with
  identical bytes. Only the one inside the project folder is used, `ApplicationIcon` and the
  `SetupIconFile` of the `.iss` both point at it. The copy in `src` is unused.
- **`License.txt` exists twice**, in the repository root and as `src/KWAnzeige/License.txt`, with
  identical bytes. The `.iss` uses the project copy as `LicenseFile` and it is copied next to the
  executable, the root copy is the one GitHub and the README link.
- **The installer is not part of the repository.** `.gitignore` line 6 is `*.exe`, and up to and
  including 1.0.8 `Setup/KWAnzeige-Setup.exe` got into the commits with `git add -f` anyway. It hangs
  on the GitHub release of its version tag now, do not add it back.
- **`MyAppPublisher` has a trailing dot.** The `.iss` says `H\xe4mmer Electronics.`, everywhere
  else in the repository the company is written without it. It ends up in the installer metadata
  that way.
- **The quick launch icon task is gone.** `OnlyBelowVersion: 0,6.1` limited it to Windows 7 and older,
  so it never triggered, and its `{userappdata}` path was what made Inno Setup warn about a per user
  area in a per machine install. The compile is warning free now, keep it that way.
- **AppVeyor badge without CI in the repository.** `README.md` links an AppVeyor build that is
  configured outside of this repository.
- **`src/KWAnzeige.sln.DotSettings`** is tracked and holds nothing but a ReSharper user dictionary
  (`Anzeige`, `H_00E4mmer`, `Todays`). Leave it alone.
- **`.gitattributes` is the unmodified Visual Studio template**, every rule below `* text=auto` is
  commented out. Any binary file that must not be normalized needs its own rule added.
- **`HaemmerElectronics.SeppPenner.Language` 1.1.9 does not exist on nuget.org.** The changelog of
  the sibling repository `CSharpLanguageManager` lists it, but only up to 1.1.8 was ever pushed.
  `dotnet list package --outdated` is right when it names 1.1.8 as the newest version.

## Releasing

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.8.0 (2026-08-13)** : Short description.`
3. Set `MyAppVersion` in `Setup/KWAnzeige-Setup.iss` to the same four part version.
4. Commit that.
5. Tag the commit with the plain three part version number, no `v` prefix (`1.0.8`, `1.0.7`, ...).
   The existing tags are lightweight tags, create new ones the same way.
6. **Only now** build the installer, the tag has to exist first. Otherwise GitVersion burns a
   prerelease version such as `1.0.8-2+Branch.master.Sha...` into the shipped executable. Run
   `Setup/build-setup-files.bat`, then `ISCC.exe` on `Setup/KWAnzeige-Setup.iss`.
7. Push the commits and the tag.
8. Attach `Setup/KWAnzeige-Setup.exe` to the GitHub release of that tag. **Never commit the
   installer.** `Setup/` is the `OutputDir` of the Inno Setup script, so the file lands there during
   the build and `.gitignore` covers it afterwards.

The version in the `Changelog.md` has four parts (`1.0.8.0`), the tag has three (`1.0.8`).

Running `build-setup-files.bat` from a tool shell needs care: this environment sets
`NoDefaultCurrentDirectoryInExePath`, so cmd does not find the batch file by name alone, and the
`cd ..\src` inside it is relative to the start directory. Use `cd /d` into `Setup` and
`call .\build-setup-files.bat`. A double click or a normal console is unaffected.

For step 8 there is no `gh` on this machine. The GitHub API does the job, with the token that
`git push` already uses, so nothing has to be stored anywhere:

```bash
c=$(printf "protocol=https\nhost=github.com\n\n" | git credential fill)
tok=$(printf "%s" "$c" | grep '^password=' | cut -d= -f2-)
id=$(curl -s -X POST -H "Authorization: Bearer $tok" \
  https://api.github.com/repos/SeppPenner/KWAnzeige/releases \
  -d '{"tag_name":"1.0.9","name":"1.0.9"}' | grep -m1 '"id"' | tr -dc 0-9)
curl -s -X POST -H "Authorization: Bearer $tok" -H "Content-Type: application/octet-stream" \
  --data-binary @Setup/KWAnzeige-Setup.exe \
  "https://uploads.github.com/repos/SeppPenner/KWAnzeige/releases/$id/assets?name=KWAnzeige-Setup.exe"
```

Never print that token, and never write it into a file.

## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies) always use real umlauts and ß, never ASCII
  transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and configuration keys
  stay unchanged where umlauts are technically undesirable.
