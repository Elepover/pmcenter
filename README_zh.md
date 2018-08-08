# pmcenter [![Build status](https://ci.appveyor.com/api/projects/status/gmbdiackw0563980?svg=true)](https://ci.appveyor.com/project/Elepover/pmcenter)

一个帮你处理私人聊天消息的 Telegram 机器人。

## 搭建你自己的 `pmcenter` 机器人

搭建 `pmcenter` 并非想象中的如此艰难，以下教程将指导你完成搭建工作。

### 环境要求

- Microsoft .NET Core (运行时 / SDK)
- Git (可选，若下载 CI 编译二进制文件则不需要)

对于微软官方支持系统，请看[此处](https://see.wtf/XxTlf)；

对于非微软官方支持系统，请看[此处](https://see.wtf/sIjUZ)；

Arch Linux 可直接安装 `dotnet-runtime` 包。

### 自行编译 `pmcenter`

运行此脚本来 clone, 编译及运行 `pmcenter`:

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

编译好的二进制文件将放在您当前目录中的 `pmcenter/build` 文件夹里。

### 使用 CI 预编译二进制文件

运行此脚本来下载和运行 `pmcenter`:

```bash
mkdir pmcenter
cd pmcenter
wget https://ci.appveyor.com/api/projects/Elepover/pmcenter/artifacts/pmcenter.zip
unzip pmcenter.zip
dotnet pmcenter.dll
```

### 配置

首次启动，`pmcenter` 将自动生成 `pmcenter.json` 和 `pmcenter_locale.json` 文件，修改文件来修改配置。

#### `pmcenter` 设置

| 项目 | 类型 | 描述 |
| :---- | :----- | ----:|
| `APIKey` | `String` | 你的 Telegram 机器人 API 密钥 |
| `OwnerID` | `Long` | 使用者的 Telegram ID |
| `AutoBan` | `Boolean` | 是否自动封禁刷屏用户 |
| `ForwardingPaused` | `Boolean` | 是否启用消息转发 |

#### `pmcenter` 翻译文件

| 项目 | 出现于 |
| :---- | ----: |
| `Message_CommandNotReplying` | 当所有者未回复任何消息时 |
| `Message_CommandNotReplyingValidMessage` | 当所有者回复非转发消息时 |
| `Message_Help` | 当所有者发送 `/help` 命令时 |
| `Message_OwnerStart` | 当用户向机器人发送 `/start` 命令时 |
| `Message_ReplySuccessful` | 当消息回复成功时 |
| `Message_UserBanned` | 当所有者 `/ban` 用户时 |
| `Message_UserPardoned` | 当所有者 `/pardon` 用户时 |
| `Message_UserStartDefault` | 当所有者向机器人发送 `/start` 命令时 |
| `Message_PingReply` | 当所有者向机器人发送 `/ping` 命令时 |
| `Message_ServicePaused` | 当所有者向机器人发送 `/switchfw` 命令且转发**已暂停**时 |
| `Message_ServiceResumed` | 当所有者向机器人发送 `/switchfw` 命令且转发**已恢复**时 |
| `Message_UserServicePaused` | 当用户向机器人发送消息而转发已暂停时 |
| `Message_BotStarted` | 当机器人启动成功时 |

##### 注意事项

- `Message_ReplySuccessful` 及 `Message_BotStarted` 项中的 `$1` 可安全删除。
- 支持 **Emojis** 且默认启用。
- 目前 `/info` 命令的回复尚且无法更改。

### 启动

完成上述操作后，可以使用以下命令安全启动 `pmcenter`:

`dotnet pmcenter.dll`

您也可以编写一个 `systemd 服务` 来保证其在主机重启后仍能保持运行。

### 命令

| 命令 | 可用于 | 描述 |
| :---- | :---- | ----: |
| `/start` | 所有者, 用户 | 显示启动消息 |
| `/info` | 所有者 | 显示所回复的消息信息 |
| `/ban` | 所有者 | 阻止该发送者再次联系您 |
| `/pardon` | 所有者 | 解封此发送者 |
| `/help` | 所有者 | 显示帮助消息 |
| `/ping` | 所有者 | 测试机器人工作状态 |
| `/switchfw` | 所有者 | 启用/暂停消息转发 |
