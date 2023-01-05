---
name: Bug report / 错误汇报
about: If you need help / 如果你需要帮助
title: ""
labels: bug
assignees: ""
---

**Describe the bug / Bug 描述**
Describe what the bug is
简单描述一下是什么 bug

**Error Log / 错误日志**
Please paste the error log of ASFEnhance, for example
请在此粘贴 ASFEnhance 的错误日志, 示例如下

```txt
ASFenhance 遇到错误, 日志如下
==========================================
 - 原始消息: TEST
 - Access: Owner
 - ASF 版本: 5.4.0.3
 - 插件版本: 1.7.5.0
==========================================
{
  "EULA": true,
  "Statistic": true,
  "DevFeature": false
}
==========================================
 - 错误类型: System.Exception
 - 错误消息: Exception of type 'System.Exception' was thrown.
   at ASFEnhance.ASFEnhance.ResponseCommand(Bot bot, EAccess access, String message, String[] args, UInt64 steamId)
   at ASFEnhance.ASFEnhance.OnBotCommand(Bot bot, EAccess access, String message, String[] args, UInt64 steamId)
```
