# Pactmaker

[![Build](https://github.com/Pactmaker/pactmaker.github.io/actions/workflows/build.yml/badge.svg)](https://github.com/Pactmaker/pactmaker.github.io/actions/workflows/build.yml)

[English](README.en-US.md) | [中文](README.zh_CN.md)

Online WIXOSS CN Card Database, now [OPEN](https://pactmaker.github.io)!

## Development

This project uses .NET 8. Please make sure you have the corresponding SDK [installed](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) on your system.

Additionally, you will need to [download the latest card data](https://github.com/Pactmaker/wixoss-data-public/releases/latest), and extract it under `wwwroot` folder.

Below shows how to run this project on Linux:

```bash
# Download data.tar.gz from wixoss-data-public release first
git clone https://github.com/Pactmaker/pactmaker.github.io.git
tar -xzvf data.tar.gz -C pactmaker.github.io.git/wwwroot
dotnet watch --project pactmaker.github.io.git
```

## Publish production version

Please refer to the [included GitHub workflow](.github/workflows/build.yml) for how to create the release build.

## FAQ

0. Why English when this is about THE CHINESE envirnonment?  
   I can't get Chinese IME working in VS Code...  
   For `README.zh-CN.md` I first wrote English version, then machine translated to Chinese, and made final adjustment in browser.

1. Why the UI is frozen during page loading?  
   This is because Blazor WebAssembly is [singlethreaded](https://stackoverflow.com/a/61865580), so the UI thread is blocked by the IO operation, even when we use Async methods to access the data.  
   In .NET 7 Blazor WebAssembly gained the [experimental multithreading support](https://visualstudiomagazine.com/articles/2022/10/11/blazor-webassembly-net7.aspx). However, this requires [additional HTTP headers](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SharedArrayBuffer#security_requirements) being set, which is not supported in GitHub Pages.

2. There are a lot of error in your card data!  
   Garbage in, garbage out. ¯\\\_(ツ)_/¯
