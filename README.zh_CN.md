# Pactmaker

[![Build](https://github.com/Pactmaker/pactmaker.github.io/actions/workflows/deploy.yaml/badge.svg)](https://github.com/Pactmaker/pactmaker.github.io/actions/workflows/deploy.yaml)

[English](README.en-US.md) | [中文](README.zh_CN.md)

在线的 WIXOSS 卡牌数据库，现在就 [OPEN](https://pactmaker.github.io)！

## 开发

本项目使用 .NET 8。请确保您的系统已[安装](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)对应的SDK。

此外，您还需要[下载最新的卡牌数据](https://github.com/Pactmaker/wixoss-data/)。

以下展示了如何在 Linux 上运行本项目：

```bash
git clone https://github.com/Pactmaker/pactmaker.github.io.git
git clone https://github.com/Pactmaker/wixoss-data.git
cd Pactmaker
make update_assets
make watch
```

## 发布正式版本

请参阅[项目自带的 GitHub Workflow](.github/workflows/deploy.yml)了解如何创建发布版本。

## 常见问题

0. 为什么项目要用英文？  
   等[Open!batoru](https://batoru.moe/)等到花儿都谢了。

1. 为什么页面加载时用户界面会冻结？  
   这是因为 Blazor WebAssembly 是[单线程执行](https://stackoverflow.com/a/61865580)的，所以尽管我们使用异步方式访问数据，用户界面线程也会被 IO 操作所阻塞。

2. 卡牌数据怎么这么多错的！  
   垃圾进，垃圾出。¯\\\_(ツ)_/¯
