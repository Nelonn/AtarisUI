# AtarisUI

Simple and modern topbar for Windows

![](https://i.imgur.com/vLdOXOe.png)

---

Keep all the data you need in sight with convenient widgets on right side of TopBar

![](https://i.imgur.com/K6U3NGt.png)

The title of the selected window is always displayed on the left side of the TopBar

![](https://i.imgur.com/6aYeCdN.png)

Everything is configured using a single json configuration file (`%AppData%/AtarisUI/config.json`)

## Extentions

You can create your own TopBar widgets by developing an extension

- The widget class must be public and implement the "AtarisUI.IWidget" interface.
- In addition, the widget class must contain a constructor that takes a settings object of type "dynamic" as an argument.
- The extension must be a dll .NET class library.
- Extensions are installed by being located in the `%AppData%/AtarisUI/extensions` folder

## Default widgets

By default, TopBar has the following pre-installed widgets:

---

Stable:

- [AtarisUI.Widgets.Clock](#AtarisUI.Widgets.Clock)
- [AtarisUI.Widgets.CPUUsage](#AtarisUI.Widgets.CPUUsage)
- [AtarisUI.Widgets.RAMUsageText](#AtarisUI.Widgets.RAMUsageText)
- [AtarisUI.Widgets.Text](#AtarisUI.Widgets.Text)
- [AtarisUI.Widgets.Shortcut](#AtarisUI.Widgets.Shortcut)

Please fix :)

- [AtarisUI.Widgets.Tray](#AtarisUI.Widgets.Tray)
- [AtarisUI.Widgets.Audio](#AtarisUI.Widgets.Audio)
- [AtarisUI.Widgets.Bluetooth](#AtarisUI.Widgets.Bluetooth)
- [AtarisUI.Widgets.TextFromFile](#AtarisUI.Widgets.TextFromFile)
- [AtarisUI.Widgets.RAMUsageBar](#AtarisUI.Widgets.RAMUsageBar)

---

### AtarisUI.Widgets.Clock

Displays the current time

**Settings:**

- `format` (string) - Date format | Default: `"MMM d h:m tt"`
- `text_color` (string) - HEX color code | Default: `"#FFFFFF"`

---

### AtarisUI.Widgets.CPUUsage

Displays the processor utilization

---

### AtarisUI.Widgets.RAMUsageText

Displays the current RAM usage in percent

---

### AtarisUI.Widgets.Text

Displays custom text

**Settings:**

- `color` (string) - HEX color code | Default: `"#ffffff"`
- `text` (string) - Text to display | Default: `""`
- `font_size` (int) - Font size | Default: `14`

---

### AtarisUI.Widgets.Shortcut

Shortcut to lunch app or run command in one click

**Settings:**

- `command` (string) - Path to executable or cmd command | Default: `"explorer.exe"`
- `icon` (string) - Path to icon file | Optional,  by default icon extracting from target executable
- `title` (string) - Tooltip text | Optional,  by default equals `command` value

---

### AtarisUI.Widgets.Tray

Shows tray icons from taskbar

**WARNING: Not working**

---

### AtarisUI.Widgets.Audio

Audio control like in taskbar

**WARNING: Allows you to change only the volume((**

---

### AtarisUI.Widgets.Bluetooth

Bluetooth control like in taskbar

**WARNING: Not working**

---

### AtarisUI.Widgets.TextFromFile

Displays contents of text file (refresh every 500ms)

**Settings:**

- `color` (string) - HEX color code | Default: `"#BBBBBB"`
- `file` (string) - Path to text file | Default: `""`

---

### AtarisUI.Widgets.RAMUsageBar

Displays the current RAM usage

**Settings:**

- `color` (string) - HEX color code or `"from-percentage"` | Default: `"#BBBBBB"`
- `background-color` (string) - HEX color code | Default: `"#33BBBBBB"`
- `width` (int) - Width of the progress bar| Default: `60`
- `height` (int) - Height of the progress bar| Default: `10`
- `radius` (int) - Radius of the progress bar rectangle | Default: `2`