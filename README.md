# pmcenter [![Build status](https://ci.appveyor.com/api/projects/status/gmbdiackw0563980?svg=true)](https://ci.appveyor.com/project/Elepover/pmcenter)

A telegram bot helping you to process private messages.

## Documentation Language

- English

- [简体中文](https://github.com/Elepover/pmcenter/blob/master/README_zh.md)

## Table of Contents

> - [🔨 Features](#features)
> - [📻 Setting Up Your Own `pmcenter`](#setting-up-your-own-pmcenter)
>   - [⚙️ Prerequisites](#prerequisites)
>   - [📥 Build `pmcenter` Yourself](#build-pmcenter-yourself)
>   - [📩 Use Pre-compiled Binaries](#use-pre-compiled-binaries)
>   - [🐠 Use Docker](#use-docker)
> - [🔧 Configuring](#configuring)
>   - [⚒️ `pmcenter` Settings](#pmcenter-settings)
>     - [📄 Note About Translations](#note-about-translations)
> - [🚀 Starting](#starting)
> - [🔩 Commands](#commands)
> - [🔺 Disclaimer](#disclaimer)

## Features

- 🚉 Cross-platform support from Windows to Linux and Raspian
- 🛡️ High stability and availability
- 📡 SOCKS5 proxy support
- 🚧 Powerful anti-spamming system
  - 🗨️ Multiple filtering modes
    - Regular expression mode
    - Full word matching mode
  - 👤 Blocking by User ID
  - 🔄 Automatic blocking by message count
  - ☑️ Support for manual blocking
- 📺 Daily maintenance, all on Telegram
  - 📥 Automatic updates
    - ⏱ Optional auto update check
    - ➡️ Manual update check
  - 🔄 Restarting bot
  - 🌐 Switching languages
  - 🗃 Reading/Writing configurations
  - 💻 Getting system status
- ℹ️ Real message source display
- ... and more!

## Setting Up Your Own `pmcenter`

The following content will guide you through the installation process.

### Prerequisites

- Microsoft .NET Core (Runtime / SDK)
- Git (optional, not needed if you uses the binaries)

For officially supported operating systems, see [here](https://see.wtf/XxTlf).

For NOT officially supported operating systems, see [here](https://see.wtf/sIjUZ).

For Arch Linux, there's an official package.

For `Raspian` or other distributions based on `armv7` architecture, run this following script as `root`:

```bash
apt-get install curl libunwind8 gettext
curl -sSL -o dotnet.tar.gz https://download.microsoft.com/download/9/1/7/917308D9-6C92-4DA5-B4B1-B4A19451E2D2/dotnet-runtime-2.1.0-linux-arm.tar.gz
mkdir -p /opt/dotnet && sudo tar zxf dotnet.tar.gz -C /opt/dotnet
ln -s /opt/dotnet/dotnet /usr/local/bin
rm dotnet.tar.gz
dotnet --info
```

### Build `pmcenter` Yourself

**You need to install .NET Core _SDK_ and _Runtime_ to finish this.**

Run this script to clone, build and initiate your own `pmcenter`:

```bash
git clone https://github.com/Elepover/pmcenter.git --depth=1
cd pmcenter/pmcenter
dotnet restore
dotnet publish --configuration Release
cp -R bin/Release/netcoreapp2.1/publish ../
cd .. && mv publish build
cd build
dotnet pmcenter.dll
```

The compiled binaries will be put in the `pmcenter/build` folder in your working directory.

### Use Pre-compiled Binaries

**Only .NET Core _Runtime_ is required in this step.**

Run this script to download and initiate your own `pmcenter`:

```bash
mkdir pmcenter
cd pmcenter
wget https://ci.appveyor.com/api/projects/Elepover/pmcenter/artifacts/pmcenter.zip
unzip pmcenter.zip
dotnet pmcenter.dll
```

### Use Docker

DO NOT COPY & PASTE. You need to edit it before executing.

```bash
wget https://raw.githubusercontent.com/Elepover/pmcenter/master/Dockfile
docker build . -t pmcenter
docker run -v PATH_OF_YOUR_pmcenter.json:/opt/pmcenter/pmcenter.json pmcenter
```

That's all.

## Configuring

During the first run, `pmcenter` will automatically generate the `pmcenter.json` and `pmcenter_locale.json` file for you. Change the settings to set up.

Or, use setup wizard:

`dotnet pmcenter.dll --setup`

*Note: Running `pmcenter` with `root` privileges is STRONGLY UNRECOMMENDED. We recommend you run pmcenter as a regular user.*

### `pmcenter` Settings

| Key | Type | Description |
| :---- | :----- | ----:|
| `APIKey` | `String` | Your Telegram bot's API key. |
| `OwnerID` | `Long` | The owner's Telegram user ID. |
| `EnableCc` | `Boolean` | Decides whether cc feature is enabled or not. |
| `Cc` | `Array` | Other users/groups/channels to receive forwarded messages. |
| `AutoBan` | `Boolean` | Decides whether the flood-banning feature is enabled or not. |
| `AutoBanThreshold` | `Int` | How many messages in 0.5m will be banned. |
| `Banned` | `Array` | Storage of banned users. |
| `ForwardingPaused` | `Boolean` | Decides whether the message forwarding is enabled or not. |
| `BannedKeywords` | `Array` | Storage of banned keywords. |
| `KeywordBanning` | `Boolean` | Decides whether the keyword banning feature is enabled or not. |
| `KeywordAutoBan` | `Boolean` | Automatically bans the matching user. |
| `EnableRegex` | `Boolean` | Enables regex matching in keywords banning or not. |
| `AutoLangUpdate` | `Boolean` | Allows the bot to automatically update language file after updating. |
| `LangURL` | `String` | URL to the updated language file. |
| `DisableNotifications` | `Boolean` | Enable/Disable notifications. |
| `EnableRepliedConfirmation` | `Boolean` | Enable/Disable "reply successful" messages. |
| `EnableForwardedConfirmation` | `Boolean` | Enable/Disable "forwarded to owner" messages. |
| `EnableAutoUpdateCheck` | `Boolean` | Enable/Disable automatic update check. |
| `UseUsernameInMsgInfo` | `Boolean` | Decides whether to display user's nickname in message details or not. |
| `AnonymousForward` | `Boolean` | Enable/Disable anonymous forwarding (BETA, text messages only) |
| `DonateString` | `string` | Text to show when users send the /donate command. Leave empty to disable this feature. |
| `LowPerformanceMode` | `boolean` | Tune pmcenter for low-end hardware like a Raspberry Pi. |
| `DetailedMsgLogging` | `Boolean` | Enable/Disable detailed message information output. |
| `Socks5Proxies` | `Array` | List of SOCKS5 proxies. |
| `UseProxy` | `Boolean` | Use/Not to use SOCKS5 proxy. |
| `ResolveHostnamesLocally` | `Boolean` | Use/Not to use remote server to resolve domain names. |
| `CatchAllExceptions` | `Boolean` | Decides whether to forward all exceptions to the owner or not. |

#### Proxy Configuration

| Key | Type | Description |
| :---- | :----- | ----:|
| `ServerName` | `String` | Server hostname. |
| `ServerPort` | `Int` | Server port. |
| `Username` | `String` | Proxy user name. |
| `ProxyPass` | `String` | Proxy password. |

Tip: After upgrades, you can send `/saveconf` command to the bot to fix missing new settings.

#### Note About Translations

- The variables like `$1` in the `Message_ReplySuccessful` and other keys could be safely deleted, if you like.
- **Emojis** are supported, and were used by default.
- Currently the response of the `/info` command is unchangeable.
- Familiar with another language? Pull requests are welcome!

## Starting

After all these, you can start your own `pmcenter` safely by using this command:

`dotnet pmcenter.dll`

You can write a `systemd service` to keep it running, even after the host machine's rebooting.

## Commands

| Command | Available to | Description |
| :---- | :---- | ----: |
| `/start` | Owner, Users | Display start message. |
| `/donate` | Owner, Users | Display donate information. |
| `/info` | Owner | Display the message's information. The command **must** be in reply to the target message. |
| `/ban` | Owner | Restrict the message's sender from contacting you again. |
| `/banid <ID>` | Owner | Restrict a sender by his/her ID. |
| `/pardon` | Owner | Pardon the message's sender that you've banned before. |
| `/pardonid <ID>` | Owner | Pardon a sender by his/her ID. |
| `/help` | Owner | Display the help message. |
| `/ping` | Owner | Test if the bot is working. |
| `/switchfw` | Owner | Switch message forwarding status. |
| `/switchbw` | Owner | Switch keyword banning status. |
| `/switchnf` | Owner | Switch notifications status. |
| `/switchlang <URL>` | Owner | Switch language file. |
| `/detectperm` | Owner | Test access to configurations. |
| `/backup` | Owner | Backup configurations. |
| `/editconf <CONF>` | Owner | Manually edit configurations and translations w/ JSON-formatted text. |
| `/saveconf` | Owner | Manually save configurations and translations. Useful after upgrades. |
| `/readconf` | Owner | Reload configurations without restarting bot. |
| `/resetconf` | Owner | Reset configurations. |
| `/uptime` | Owner | Get system uptime info. |
| `/update` | Owner | Check for updates and update bot if available. |
| `/chkupdate` | Owner | Only check for updates. |
| `/catconf` | Owner | Get current configurations. |
| `/restart` | Owner | Restart bot. |
| `/status` | Owner | Get host device's status. |
| `/perform` | Owner | Start performance test. |

Please note: `/restart` command only works with a daemon that auto-restarts pmcenter when it exits. pmcenter cannot restart by itself.

# Disclaimer

The program is licensed under Apache License _(Version 2.0. Dependencies are licensed under MIT License)_ and comes with **ABSOLUTELY NO WARRANTY**. By using the program in any way, you acknowledge and confirm that the developer of the program is **NOT RESPONSIBLE** for service outage, data loss or any other rare unlisted incident caused by the program.
