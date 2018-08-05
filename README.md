# pmcenter [![Build status](https://ci.appveyor.com/api/projects/status/gmbdiackw0563980?svg=true)](https://ci.appveyor.com/project/Elepover/pmcenter)

A telegram bot helping you to process private messages.

# Setting Up Your Own `pmcenter`

## Prerequirements

- Microsoft .NET Core (Runtime / SDK)
- Git (optional, not needed if you uses the binaries)

For officially supported operating systems, see https://see.wtf/XxTlf

For NOT officially supported operating systems, see https://see.wtf/sIjUZ

For Arch Linux, there's an official package.

## Build `pmcenter` Yourself

Run this script to clone, build and initiate your own `pmcenter`:

```bash
git clone https://github.com/Elepover/pmcenter.git --depth=1
cd pmcenter/pmcenter
dotnet restore
dotnet publish --configuration Release
cp -R bin/Release/publish ../
cd .. && mv publish build
cd build
dotnet pmcenter.dll
```

The compiled binaries will be put in the `pmcenter/build` folder in your working directory.

## Use Pre-compiled Binaries

Run this script to download and initiate your own `pmcenter`:

```bash
mkdir pmcenter
cd pmcenter
wget https://ci.appveyor.com/api/projects/Elepover/pmcenter/artifacts/pmcenter.zip
unzip pmcenter.zip
dotnet pmcenter.dll
```

## Configuring

During the first run, `pmcenter` will automatically generate the `pmcenter.json` and `pmcenter_locale.json` file for you. Change the settings to set up.

### `pmcenter` Settings

| Key | Type | Description |
| :---- | :----- | ----:|
| `APIKey` | `String` | Your Telegram bot's API key |
| `OwnerID` | `Long` | The owner's Telegram user ID |
| `StartMessage` | `String` | What the other users see when they send the `/start` command to the bot |
| `AutoBan` | `Boolean` | Decides whether the flood-banning feature is enabled or not |

### `pmcenter` Locales / Translations

| Key | Appears When |
| :---- | ----: |
| `Message_CommandNotReplying` | When the owner is not replying to a message. |
| `Message_CommandNotReplyingValidMessage` | When the owner is replying to a non-forwarded message. |
| `Message_Help` | When the owner asks for `/help`. |
| `Message_OwnerStart` | When the owner sends `/start` to the bot. |
| `Message_ReplySuccessful` | When replying is successfully completed. |
| `Message_UserBanned` | When the owner `/ban`s a user. |
| `Message_UserPardoned` | When the owner `/pardon`s a user. |
| `Message_UserStartDefault` | When a user sends `/start` to the bot. |

#### Note

- The `$1` variable in the `Message_ReplySuccessful` key could be safely deleted, if you like.
- **Emojis** are supported, and were used by default.
- Currently the response of the `/info` command is unchangeable.

## Start

After all these, you can start your own `pmcenter` safely by using this command:

`dotnet pmcenter.dll`

You can write a `systemd service` to keep it running, even after the host machine's rebooting.

## Commands

| Command | Available to | Description |
| :---- | :---- | ----: |
| `/start` | Owner, Users | Display start message. |
| `/info` | Owner | Display the message's information. The command **must** be in reply to the target message. |
| `/ban` | Owner | Restrict the message's sender from contacting you again. |
| `/pardon` | Owner | Pardon the message's sender that you've banned before. |
| `/help` | Owner | Display the help message. |