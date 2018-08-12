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

### 配置

首次启动，`pmcenter` 将自动生成 `pmcenter.json` 和 `pmcenter_locale.json` 文件，修改文件来修改配置。

#### `pmcenter` 设置

| 项目 | 类型 | 描述 |
| :---- | :----- | ----:|
| `APIKey` | `String` | 你的 Telegram 机器人 API 密钥 |
| `OwnerID` | `Long` | 使用者的 Telegram ID |
| `AutoBan` | `Boolean` | 是否自动封禁刷屏用户 |
| `AutoBanThreshold` | `Int` | 30 秒内消息量阈值，超过将自动封禁 |
| `Banned` | `Array` | 封禁用户存储 |
| `ForwardingPaused` | `Boolean` | 是否启用消息转发 |
| `BannedKeywords` | `Array` | 屏蔽的关键字存储 |
| `KeywordBanning` | `Boolean` | 是否启用关键词屏蔽功能 |
| `KeywordAutoBan` | `Boolean` | 是否自动封禁被屏蔽消息发送者 |
| `EnableRegex` | `Boolean` | 是否启用关键字屏蔽中的正则表达式匹配 |
| `RestartCommand` | `String` | 用于自动更新后重新启动机器人的命令 |
| `RestartArgs` | `String` | 重启命令的参数部分 |

提示：升级后，可向机器人发送 `/saveconf` 命令来自动补齐升级后缺少的新配置项。

##### 翻译注意事项

- `Message_ReplySuccessful` 及 `Message_BotStarted` 项中的 `$1` 可安全删除。
- 支持 **Emojis** 且默认启用。
- 目前 `/info` 命令的回复尚且无法更改。
- 欢迎 Pull Requests.

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
| `/switchbw` | 所有者 | 启用/停用关键字屏蔽 |
| `/saveconf` | 所有者 | 手动保存配置及翻译，可用于更新后补齐缺少的配置项 |
| `/readconf` | 所有者 | 在不重启机器人的情况下，重新载入配置 |
| `/uptime` | 所有者 | 获取系统在线时间信息 |
| `/update` | 所有者 | 检查更新并自动更新（如果可用） |
| `/chkupdate` | 所有者 | 仅检查更新 |
| `/catconf` | 所有者 | 获取当前配置数据 |
