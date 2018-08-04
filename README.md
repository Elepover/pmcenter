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

During the first run, `pmcenter` will automatically generate the `pmcenter.json` file for you. Change the settings to set up.

| Key | Type | Description |
| :---- | :----- | ----:|
| `APIKey` | `String` | Your Telegram bot's API key |
| `OwnerID` | `Long` | The owner's Telegram user ID |
| `StartMessage` | `String` | What the other users see when they send the `/start` command to the bot |
| `AutoBan` | `Boolean` | Decides whether the flood-banning feature is enabled or not |

## Start

After all these, you can start your own `pmcenter` safely by using this command:
`dotnet pmcenter.dll`

You can write a `systemd service` to keep it running, even after the host machine's rebooting.
