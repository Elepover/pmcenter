File structure of pmcenter's source code:

```
|- pmcenter - Source code main directory
   |- BotCommands - Every bot command's processing logic
   |  |- ...
   |- CommandLines - Every commandline's processing logic
   |  |- ...
   |- Configurations - Configurations' processing logics
   |  |- ...
   |- Enums - pmcenter's self-defined enums
   |  |- ...
   |- Interfaces - pmcenter's self-defined interfaces
   |  |- ...
   |- Methods - Useful functions
   |  |- Database - Extracting/writing things from/to local database
   |  |  |- Checking - For extracting things from database
   |  |  |  |- ...
   |  |  |- Writing - For writing things to database
   |  |  |  |- ...
   |  |- NetworkTest - Testing network quality, used by some commands
   |  |  |- ...
   |  |- Threads - Threads' logics
   |  |  |- ...
   |  |- ...
   |- BotProcess.cs - Bot's master logic
   |- CommandLineRouter.cs - Commandlines' router
   |- CommandLines.cs - Loading commandlines to memory
   |- CommandManager.cs - Bot commands' router
   |- Program.cs - Main entry of pmcenter
   |- Setup.cs - Setup wizard's processing logic
   |- Template.cs - As its name
   |- Vars.cs - Constants/variables storage
```
