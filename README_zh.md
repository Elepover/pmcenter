# pmcenter

[![build status](https://ci.appveyor.com/api/projects/status/gmbdiackw0563980?svg=true)](https://ci.appveyor.com/project/Elepover/pmcenter) [![CodeFactor](https://www.codefactor.io/repository/github/elepover/pmcenter/badge)](https://www.codefactor.io/repository/github/elepover/pmcenter) [![telegram channel](https://img.shields.io/badge/telegram-channel-blue.svg)](https://t.me/pmcenter_devlog) ![license](https://img.shields.io/github/license/elepover/pmcenter.svg) ![language rank](https://img.shields.io/github/languages/top/elepover/pmcenter.svg?color=brightgreen) ![repo size in bytes](https://img.shields.io/github/repo-size/elepover/pmcenter.svg) ![environment](https://img.shields.io/badge/dotnet-v2.1-blue.svg) ![last commit](https://img.shields.io/github/last-commit/elepover/pmcenter.svg) ![status](https://img.shields.io/badge/status-maintaining-success.svg)

一个帮你处理私人聊天消息的 Telegram 机器人。

## 文档语言

- [English](https://github.com/Elepover/pmcenter/blob/master/README.md)

- 简体中文

## 目录

> - [🌲 分支](#分支)
> - [🔨 功能](#功能)
> - [📻 搭建你自己的 `pmcenter` 机器人](#搭建你自己的-pmcenter-机器人)
>   - [⚙️ 环境要求](#环境要求)
>   - [📥 自行编译 `pmcenter`](#自行编译-pmcenter)
>   - [📩 使用 CI 预编译二进制文件](#使用-ci-预编译二进制文件)
> - [🔧 配置](#配置)
>   - [⚒️ `pmcenter` 设置](#pmcenter-设置)
>     - [📄 注意事项](#注意事项)
>     - [⛵ 改变文件位置](#改变文件位置)
> - [🚀 启动](#启动)
> - [🔩 命令](#命令)
> - [🔺 免责声明](#免责声明)

## 分支

目前 `pmcenter` 有两个分支:

- `pmcenter-lazer`: 最新代码，最新功能，就像 Chrome Canary 一样。**不保证运行时稳定性**。
- `master`: 稳定分支，在 `pmcenter-lazer` 准备就绪后将被合并至该分支中。

## 功能

- 🚉 跨平台运行，涵盖 Windows, Linux 及 Raspian
- 🛡️ 高稳定性及容错率
- 📡 SOCKS5 代理支持
- 🚧 强大的的消息过滤系统
  - 💬 多模式的关键词过滤
    - 🔣 正则表达式模式
    - 🔠 全词匹配模式
  - 👤 用户 ID 屏蔽
  - 🔄 根据消息量的自动屏蔽
  - ☑️ 可手动屏蔽用户
- 📺 无需连接 SSH 即可在 TG 上完成常用维护
  - 📥 自动更新
    - 🕑 可选的自动更新检查
    - ➡️ 手动的更新检查
  - 🔄 重启机器人
  - 🌐 切换语言
  - 🗃 保存或读取配置
  - 💻 获取系统信息
- ℹ️ 真实转发消息源显示
- 🆔 支持 Telegram 5.5.0 引入的全新隐私转发模式
- …… 更多!

## 搭建你自己的 `pmcenter` 机器人

以下教程将指导你完成搭建工作。

### 环境要求

- Microsoft .NET Core (运行时 / SDK)
- Git (可选，若下载 CI 编译二进制文件则不需要)

对于微软官方支持系统，请看[此处](https://see.wtf/XxTlf)；

对于非微软官方支持系统，请看[此处](https://see.wtf/sIjUZ)；

Arch Linux 可直接安装 `dotnet-runtime` 包。

树莓派:

以 `root` 权限运行以下脚本:

```bash
apt-get install curl libunwind8 gettext
curl -sSL -o dotnet.tar.gz https://download.microsoft.com/download/9/1/7/917308D9-6C92-4DA5-B4B1-B4A19451E2D2/dotnet-runtime-2.1.0-linux-arm.tar.gz
mkdir -p /opt/dotnet && sudo tar zxf dotnet.tar.gz -C /opt/dotnet
ln -s /opt/dotnet/dotnet /usr/local/bin
rm dotnet.tar.gz
dotnet --info
```

### 自行编译 `pmcenter`

**您需要安装 .NET Core _SDK_ 及 _Runtime_ 才能完成此步。**

运行此脚本来 clone, 编译及运行 `pmcenter`:

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

编译好的二进制文件将放在您当前目录中的 `pmcenter/build` 文件夹里。

### 使用 CI 预编译二进制文件

**本步骤中，仅需要 .NET Core _Runtime_ 即可。**

运行此脚本来下载和运行 `pmcenter`:

```bash
mkdir pmcenter
cd pmcenter
wget https://ci.appveyor.com/api/projects/Elepover/pmcenter/artifacts/pmcenter.zip
unzip pmcenter.zip
dotnet pmcenter.dll
```

## 配置

首次启动，`pmcenter` 将自动生成 `pmcenter.json` 和 `pmcenter_locale.json` 文件，修改文件来修改配置。

使用设置向导:

`dotnet pmcenter.dll --setup`

*提示：不建议使用 `root` 权限运行 `pmcenter`，我们强烈推荐您使用普通用户权限执行 `pmcenter`。*

### `pmcenter` 设置

| 项目 | 类型 | 用户可编辑 | 描述 |
| :---- | :---- | :---- | ----:|
| `APIKey` | `String` | ✓ | 你的 Telegram 机器人 API 密钥 |
| `OwnerID` | `Long` | ✓ | 使用者的 Telegram ID |
| `EnableCc` | `Boolean` | ✓ | 是否启用 Cc 功能 |
| `Cc` | `Array` | ✓ | 其他消息接收者 |
| `AutoBan` | `Boolean` | ✓ | 是否自动封禁刷屏用户 |
| `AutoBanThreshold` | `Int` | ✓ | 30 秒内消息量阈值，超过将自动封禁 |
| `ForwardingPaused` | `Boolean` | ✓ | 是否启用消息转发 |
| `KeywordBanning` | `Boolean` | ✓ | 是否启用关键词屏蔽功能 |
| `KeywordAutoBan` | `Boolean` | ✓ | 是否自动封禁被屏蔽消息发送者 |
| `EnableRegex` | `Boolean` | ✓ | 是否启用关键字屏蔽中的正则表达式匹配 |
| `AutoLangUpdate` | `Boolean` | ✓ | 是否启用自动语言文件更新 |
| `LangURL` | `String` | ✓ | 新语言文件的 URL |
| `DisableNotifications` | `Boolean` | ✓ | 是否停用消息通知 |
| `EnableRepliedConfirmation` | `Boolean` | ✓ | 是否启用 “回复成功” 提示 |
| `EnableForwardedConfirmation` | `Boolean` | ✓ | 是否启用 “已转发” 提示 |
| `EnableAutoUpdateCheck` | `Boolean` | ✓ | 是否启用自动更新检查 |
| `UseUsernameInMsgInfo` | `Boolean` | ✓ | 是否在消息详情中显示用户昵称 |
| `AnonymousForward` | `Boolean` | ✓ | 是否启用匿名转发 (BETA, 仅支持纯文本消息) |
| `DonateString` | `String` | ✓ | 用户发送 /donate 指令时显示的消息，留空以关闭 |
| `LowPerformanceMode` | `Boolean` | ✓ | 调节 pmcenter 配置以适应低性能设备，如树莓派 |
| `DetailedMsgLogging` | `Boolean` | ✓ | 是否在收到每条消息时都输出消息详情 |
| `UseProxy` | `Boolean` | ✓ | 是否使用 SOCKS5 代理 |
| `ResolveHostnamesLocally` | `Boolean` | ✓ | 是否使用远程服务器解析域名 |
| `CatchAllExceptions` | `Boolean` | ✓ | 是否将所有错误转发给所有者 |
| `NoStartupMessage` | `Boolean` | ✓ | 是否停用 "启动成功" 消息 |
| `ContChatTarget` | `Long` | ✓ | 连续对话模式目标 |
| `EnableMsgLink` | `Boolean` | ✓ | 是否启用消息链接 |
| `AllowUserRetraction` | `Boolean` | ✓ | 是否允许消息撤回 |
| `ConfSyncInterval` | `Int` | ✓ | 指定自动保存间隔，单位毫秒 |
| `AdvancedLogging` | `Boolean` | ✓ | 如果启用，pmcenter 会在日志消息中附加输出时的代码文件及行号信息 |
| `DisableTimeDisplay` | `Boolean` | ✓ | 不在日志中显示时间 |
| `UpdateChannel` | `String` | ✓ | 选择更新频道 |
| `Statistics` | `Stats` | ✕ | 统计数据 |
| `Socks5Proxies` | `Array` | ✓ | SOCKS5 代理列表 |
| `BannedKeywords` | `Array` | ✓ | 屏蔽的关键字存储 |
| `Banned` | `Array` | ✓ | 封禁用户存储 |
| `MessageLinks` | `Array` | ✕ | 消息链接存储 |

#### 代理配置

| 项目 | 类型 | 描述 |
| :---- | :----- | ----:|
| `ServerName` | `String` | 服务器地址 |
| `ServerPort` | `Int` | 服务器端口 |
| `Username` | `String` | 代理用户名 |
| `ProxyPass` | `String` | 代理密码 |

提示：升级后，可向机器人发送 `/saveconf` 命令来自动补齐升级后缺少的新配置项。

#### 注意事项

- `Message_ReplySuccessful` 等各种翻译中类似 `$1` 的变量可安全删除。
- 支持 **Emojis** 且默认启用。
- 目前 `/info` 命令的回复尚且无法更改。
- 欢迎 Pull Requests.
- 切换中文语言包，只需发送 `/switchlang https://raw.githubusercontent.com/Elepover/pmcenter/master/locales/pmcenter_locale_zh.json`
- 在启用 `EnableMsgLink` 前请三思：虽然此功能允许您回复匿名转发消息及频道消息，但 pmcenter 的存储和内存占用将随消息量增长而增加，并将拖慢 pmcenter 操作配置文件时的速度。

#### 改变文件位置

在 1.5.85.174 或更高版本, pmcenter 会在启动时读取以下两个环境变量:

```
pmcenter_conf: pmcenter 配置文件路径。
pmcenter_lang: pmcenter 语言文件路径。
```

在以下三种情况下, pmcenter 仍将使用默认位置:

- 环境变量不存在。
- 无法读取环境变量。
- 指定的文件不存在。

## 启动

完成上述操作后，可以使用以下命令安全启动 `pmcenter`:

`dotnet pmcenter.dll`

您也可以编写一个 `systemd 服务` 来保证其在主机重启后仍能保持运行。

## 命令

| 命令 | 可用于 | 描述 |
| :---- | :---- | ----: |
| `/start` | 所有者, 用户 | 显示启动消息 |
| `/donate` | 所有者, 用户 | 显示捐赠信息 |
| `/info` | 所有者 | 显示所回复的消息信息 |
| `/ban` | 所有者 | 阻止该发送者再次联系您 |
| `/banid <ID>` | 所有者 | 通过 ID 封禁用户 |
| `/pardon` | 所有者 | 解封此发送者 |
| `/pardonid <ID>` | 所有者 | 通过 ID 解封用户 |
| `/help` | 所有者 | 显示帮助消息 |
| `/ping` | 所有者 | 测试机器人工作状态 |
| `/switchfw` | 所有者 | 启用/暂停消息转发 |
| `/switchbw` | 所有者 | 启用/停用关键字屏蔽 |
| `/switchnf` | 所有者 | 启用/停用消息通知 |
| `/switchlang <URL>` | 所有者 | 切换语言文件 |
| `/switchlangcode [code]` | 所有者 | 使用语言代码切换语言文件 |
| `/detectperm` | 所有者 | 测试配置文件写入权限 |
| `/backup` | 所有者 | 备份配置文件 |
| `/editconf <CONF>` | 所有者 | 手动保存 JSON 格式的配置及翻译 |
| `/saveconf` | 所有者 | 手动保存配置及翻译，可用于更新后补齐缺少的配置项 |
| `/readconf` | 所有者 | 在不重启机器人的情况下，重新载入配置 |
| `/resetconf` | 所有者 | 重置配置文件 |
| `/uptime` | 所有者 | 获取系统在线时间信息 |
| `/update` | 所有者 | 检查更新并自动更新（如果可用） |
| `/chkupdate` | 所有者 | 仅检查更新 |
| `/catconf` | 所有者 | 获取当前配置数据 |
| `/restart` | 所有者 | 重新启动机器人 |
| `/status` | 所有者 | 获取设备状态 |
| `/perform` | 所有者 | 启动性能测试 |
| `/testnetwork` | 所有者 | 测试到 pmcenter 所用服务器的延迟 |
| `/chat [ID]` | 所有者 | 进入持续对话模式 |
| `/stopchat` | 所有者 | 退出持续对话模式 |
| `/retract` | 所有者, 用户 | 撤回消息 |
| `/clearmessagelinks` | 所有者 | 清除消息链接 |
| `/getstats` | 所有者 | 显示统计数据 |

请注意: `/restart` 命令仅在有有效的守护进程，且其能在 pmcenter 退出后自动将其重启的情况下工作。pmcenter 无法自行重新启动。

# 已知问题

## OpenSSL 1.1 兼容性问题

此问题仅在 Linux 环境下出现，与 Windows 无关。

当使用旧版 (早于 PR [#34443](https://github.com/dotnet/corefx/pull/34443) (2019/02/14)) 的 .NET Core 2.1 (其与 OpenSSL 1.1+ 不兼容) 时, pmcenter 将会在建立安全连接时抛出以下错误:

```
System.Net.Http.HttpRequestException: The SSL connection could not be established, see inner exception. 
---> System.Security.Authentication.AuthenticationException: Authentication failed, see inner exception. 
---> System.TypeInitializationException: The type initializer for 'SslMethods' threw an exception. 
---> System.TypeInitializationException: The type initializer for 'Ssl' threw an exception. 
---> System.TypeInitializationException: The type initializer for 'SslInitializer' threw an exception. 
---> Interop+Crypto+OpenSslCryptographicException: error:0E076071:configuration file routines:MODULE_RUN:unknown module name
```

在 issue [#33179](https://github.com/dotnet/corefx/issues/33179) 中提及了以下解决方案:

1. 打开 `openssl.cnf` 并注释掉 `ssl_conf = ssl_sect` 一行。
2. 更新 (新于 #34443) 的 .NET Core 2.1 运行时。
3. 安装 OpenSSL 1.0.

# 免责声明

很抱歉，但鉴于某些事件，我们实在不得不加入这个章节，以至于独立成一个 commit 来提交。

本程序由 Apache License _(版本 2.0，依赖组件由 MIT License 授权)_ 授权。**不提供任何担保**。使用本程序即表明，您知情并同意：程序开发者不对此程序导致的任何服务中断、数据损失或任何少见未列出的事故负责。
