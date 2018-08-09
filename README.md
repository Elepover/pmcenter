# pmcenter [![Build status](https://ci.appveyor.com/api/projects/status/gmbdiackw0563980?svg=true)](https://ci.appveyor.com/project/Elepover/pmcenter)

A telegram bot helping you to process private messages.

## Setting Up Your Own `pmcenter`

Setting up `pmcenter` is not as hard as imagined. The following content will guide you through the installation process.

### Prerequisites

- Microsoft .NET Core (Runtime / SDK)
- Git (optional, not needed if you uses the binaries)

For officially supported operating systems, see [here](https://see.wtf/XxTlf).

For NOT officially supported operating systems, see [here](https://see.wtf/sIjUZ).

For Arch Linux, there's an official package.

### Build `pmcenter` Yourself

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

Run this script to download and initiate your own `pmcenter`:

```bash
mkdir pmcenter
cd pmcenter
wget https://ci.appveyor.com/api/projects/Elepover/pmcenter/artifacts/pmcenter.zip
unzip pmcenter.zip
dotnet pmcenter.dll
```

### Configuring

During the first run, `pmcenter` will automatically generate the `pmcenter.json` and `pmcenter_locale.json` file for you. Change the settings to set up.

#### `pmcenter` Settings

| Key | Type | Description |
| :---- | :----- | ----:|
| `APIKey` | `String` | Your Telegram bot's API key. |
| `OwnerID` | `Long` | The owner's Telegram user ID. |
| `AutoBan` | `Boolean` | Decides whether the flood-banning feature is enabled or not. |
| `Banned` | `Array` | Storage of banned users. |
| `ForwardingPaused` | `Boolean` | Decides whether the message forwarding is enabled or not. |
| `BannedKeywords` | `Array` | Storage of banned keywords. |
| `KeywordBanning` | `Boolean` | Decides whether the keyword banning feature is enabled or not. |

Note: After upgrades, you can send `/saveconf` command to the bot to fix missing new settings.

##### Note About Translations

- The `$1` variable in the `Message_ReplySuccessful` and `Message_BotStarted` key could be safely deleted, if you like.
- **Emojis** are supported, and were used by default.
- Currently the response of the `/info` command is unchangeable.
- Familiar with another language? Pull requests are welcome!

### Start

After all these, you can start your own `pmcenter` safely by using this command:

`dotnet pmcenter.dll`

You can write a `systemd service` to keep it running, even after the host machine's rebooting.

### Commands

| Command | Available to | Description |
| :---- | :---- | ----: |
| `/start` | Owner, Users | Display start message. |
| `/info` | Owner | Display the message's information. The command **must** be in reply to the target message. |
| `/ban` | Owner | Restrict the message's sender from contacting you again. |
| `/pardon` | Owner | Pardon the message's sender that you've banned before. |
| `/help` | Owner | Display the help message. |
| `/ping` | Owner | Test if the bot is working. |
| `/switchfw` | Owner | Switch message forwarding status. |
| `/switchbw` | Owner | Switch keyword banning status. |
| `/saveconf` | Owner | Manually save configurations and translations. Useful after upgrades. |
