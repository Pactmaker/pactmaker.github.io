# Pactmaker

[![Build](https://github.com/Pactmaker/pactmaker.github.io/actions/workflows/build.yml/badge.svg)](https://github.com/Pactmaker/pactmaker.github.io/actions/workflows/build.yml)

[English](README.en-US.md) | [中文](README.zh_CN.md)

在线的 WIXOSS 中文卡牌数据库，现在就 [OPEN](https://pactmaker.github.io)！

## 开发

本项目使用 .NET 8。请确保您的系统已[安装](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)对应的SDK。

此外，您还需要[下载最新的卡牌数据](https://github.com/Pactmaker/wixoss-data-public/releases/latest)，并将其解压缩到 `wwwroot` 文件夹下。

以下展示了如何在 Linux 上运行本项目：

```bash
# 先从 wixoss-data-public release 下载 data.tar.gz
git clone https://github.com/Pactmaker/pactmaker.github.io.git
tar -xzvf data.tar.gz -C pactmaker.github.io.git/wwwroot
dotnet watch --project pactmaker.github.io.git
```

## 发布正式版本

请参阅[项目自带的 GitHub Workflow](.github/workflows/build.yml) 了解如何创建发布版本。

## 常见问题

0. 中文环境为什么项目要用英文？  
   我的系统无法在 VS Code 中使用中文输入法…  
   `README.zh-CN.md` 是先写了英文版，然后机器翻译成中文，最后在浏览器中进行了调整。

1. 为什么页面加载时用户界面会冻结？  
   这是因为 Blazor WebAssembly 是[单线程执行](https://stackoverflow.com/a/61865580)的，所以尽管我们使用异步方式访问数据，用户界面线程也会被 IO 操作所阻塞。  
   在 .NET 7 中，Blazor WebAssembly 获得了[试验性多线程支持](https://visualstudiomagazine.com/articles/2022/10/11/blazor-webassembly-net7.aspx)。不过，这需要设置[额外 HTTP 标头](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SharedArrayBuffer#security_requirements)，而 GitHub Pages 并不支持此类设置。

2. 卡牌数据怎么这么多错的！  
   垃圾进，垃圾出。¯\\\_(ツ)_/¯
