# pmcenter

[![build status](https://ci.appveyor.com/api/projects/status/gmbdiackw0563980?svg=true)](https://ci.appveyor.com/project/Elepover/pmcenter) [![telegram channel](https://img.shields.io/badge/telegram-channel-blue.svg)](https://t.me/pmcenter_devlog) ![license](https://img.shields.io/github/license/elepover/pmcenter.svg) ![language rank](https://img.shields.io/github/languages/top/elepover/pmcenter.svg?color=brightgreen) ![repo size in bytes](https://img.shields.io/github/repo-size/elepover/pmcenter.svg) ![environment](https://img.shields.io/badge/dotnet-v2.1-blue.svg) ![last commit](https://img.shields.io/github/last-commit/elepover/pmcenter.svg) ![status](https://img.shields.io/badge/status-maintaining-success.svg)

A telegram bot helping you process private messages.

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
>     - [📄 Note](#note)
>     - [⛵ Changing File Location](#changing-file-location)
> - [🚀 Starting](#starting)
> - [🔩 Commands](#commands)
> - [🔺 Disclaimer](#disclaimer)

## Features

- 🚉 Cross-platform support from Windows to Linux and Raspian
- 🛡️ High stability and availability
- 📡 SOCKS5 proxy support
- 🚧 Powerful anti-spamming system
  - 💬 Multiple filtering modes
    - 🔣 Regular expression mode
    - 🔠 Full word matching mode
  - 👤 Blocking by User ID
  - 🔄 Automatic blocking by message count
  - ☑️ Support for manual blocking
- 📺 Daily maintenance, all on Telegram
  - 📥 Automatic updates
    - 🕑 Optional auto update check
    - ➡️ Manual update check
  - 🔄 Restarting bot
  - 🌐 Switching languages
  - 🗃 Reading/Writing configurations
  - 💻 Getting system status
- ℹ️ Real message source display
- 🆔 Support the new privacy mode introduced in Telegram 5.5.0
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
wget https://raw.githubusercontent.com/Elepover/pmcenter/master/Dockerfile
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

| Key | Type | User Editable | Description |
| :---- | :---- | :---- | ----:|
| `APIKey` | `String`| ✓ | Your Telegram bot's API key. |
| `OwnerID` | `Long` | ✓ | The owner's Telegram user ID. |
| `EnableCc` | `Boolean` | ✓ | Decides whether cc feature is enabled or not. |
| `Cc` | `Array` | ✓ | Other users/groups/channels to receive forwarded messages. |
| `AutoBan` | `Boolean` | ✓ | Decides whether the flood-banning feature is enabled or not. |
| `AutoBanThreshold` | `Int` | ✓ | How many messages in 0.5m will be banned. |
| `ForwardingPaused` | `Boolean` | ✓ | Decides whether the message forwarding is enabled or not. |
| `KeywordBanning` | `Boolean` | ✓ | Decides whether the keyword banning feature is enabled or not. |
| `KeywordAutoBan` | `Boolean` | ✓ | Automatically bans the matching user. |
| `EnableRegex` | `Boolean` | ✓ | Enables regex matching in keywords banning or not. |
| `AutoLangUpdate` | `Boolean` | ✓ | Allows the bot to automatically update language file after updating. |
| `LangURL` | `String` | ✓ | URL to the updated language file. |
| `DisableNotifications` | `Boolean` | ✓ | Enable/Disable notifications. |
| `EnableRepliedConfirmation` | `Boolean` | ✓ | Enable/Disable "reply successful" messages. |
| `EnableForwardedConfirmation` | `Boolean` | ✓ | Enable/Disable "forwarded to owner" messages. |
| `EnableAutoUpdateCheck` | `Boolean` | ✓ | Enable/Disable automatic update check. |
| `UseUsernameInMsgInfo` | `Boolean` | ✓ | Decides whether to display user's nickname in message details or not. |
| `AnonymousForward` | `Boolean` | ✓ | Enable/Disable anonymous forwarding (BETA, text messages only) |
| `DonateString` | `String` | ✓ | Text to show when users send the /donate command. Leave empty to disable this feature. |
| `LowPerformanceMode` | `Boolean` | ✓ | Tune pmcenter for low-end hardware like a Raspberry Pi. |
| `DetailedMsgLogging` | `Boolean` | ✓ | Enable/Disable detailed message information output. |
| `UseProxy` | `Boolean` | ✓ | Use/Not to use SOCKS5 proxy. |
| `ResolveHostnamesLocally` | `Boolean` | ✓ | Use/Not to use remote server to resolve domain names. |
| `CatchAllExceptions` | `Boolean` | ✓ | Decides whether to forward all exceptions to the owner or not. |
| `NoStartupMessage` | `Boolean` | ✓ | Enable/Disable "startup complete" messages. |
| `ContChatTarget` | `Long` | ✕ | Target of Continued Conversation. |
| `EnableMsgLink` | `Boolean` | ✓ | Enable/Disable message links. |
| `AllowUserRetraction` | `Boolean` | ✓ | Enable/Disable message retraction. |
| `AllowUntrustedCerts` | `Boolean` | ✓ | Allow untrusted certificates. |
| `Statistics` | `Stats` | ✕ | Statistics data. |
| `Socks5Proxies` | `Array` | ✓ | List of SOCKS5 proxies. |
| `BannedKeywords` | `Array` | ✓ | Storage of banned keywords. |
| `Banned` | `Array` | ✓ | Storage of banned users. |
| `MessageLinks` | `Array` | ✕ | Storage of message links. |

#### Proxy Configuration

| Key | Type | Description |
| :---- | :----- | ----:|
| `ServerName` | `String` | Server hostname. |
| `ServerPort` | `Int` | Server port. |
| `Username` | `String` | Proxy user name. |
| `ProxyPass` | `String` | Proxy password. |

Tip: After upgrades, you can send `/saveconf` command to the bot to fix missing new settings.

#### Note

- The variables like `$1` in the `Message_ReplySuccessful` and other keys could be safely deleted, if you like.
- **Emojis** are supported, and were used by default.
- Currently the response of the `/info` command is unchangeable.
- Familiar with another language? Pull requests are welcome!
- Please think twice before turning on `EnableMsgLink`, it makes it possible for you to reply to messages that are forwarded anonymously or from channels, however, it will cost more and more of your storage space and memory as the storage grows and makes it slower for pmcenter to process configuration files.

#### Changing File Location

On version 1.5.85.174 and later, pmcenter will check these two following environment variables on startup:

```
pmcenter_conf: pmcenter's configurations file's location.
pmcenter_lang: pmcenter's language file's location.
```

On these 3 scenarios, pmcenter will still use default location:

- The environment variable doesn't exist.
- The environment variable cannot be accessed.
- File not found.

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
| `/switchlangcode [code]` | Owner | Switch language file by language code. |
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
| `/testnetwork` | Owner | Test latency to servers used by pmcenter. |
| `/chat [ID]` | Owner | Enter Continued Conversation mode with someone. |
| `/stopchat` | Owner | Leave Continued Conversation. |
| `/retract` | Owner, Users | Retract a message. |
| `/clearmessagelinks` | Owner | Clear message links. |
| `/getstats` | Owner | Get statistics data. |

Please note: `/restart` command only works with a daemon that auto-restarts pmcenter when it exits. pmcenter cannot restart by itself.

# Disclaimer

The program is licensed under Apache License _(Version 2.0. Dependencies are licensed under MIT License)_ and comes with **ABSOLUTELY NO WARRANTY**. By using the program in any way, you acknowledge and confirm that the developer of the program is **NOT RESPONSIBLE** for service outage, data loss or any other rare unlisted incident caused by the program.
