# Pactmaker

[![Build](https://github.com/Pactmaker/pactmaker.github.io/actions/workflows/deploy.yaml/badge.svg)](https://github.com/Pactmaker/pactmaker.github.io/actions/workflows/deploy.yaml)

[English](README.en-US.md) | [中文](README.zh_CN.md)

Online WIXOSS Card Database, now [OPEN](https://pactmaker.github.io)!

## Development

This project uses .NET 8. Please make sure you have the corresponding SDK [installed](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) on your system.

Additionally, you will need to [download the latest card data](https://github.com/Pactmaker/wixoss-data/).

Below shows how to run this project on Linux:

```bash
git clone https://github.com/Pactmaker/pactmaker.github.io.git
git clone https://github.com/Pactmaker/wixoss-data.git
cd Pactmaker
make update_assets
make watch
```

## Publish production version

Please refer to the [included GitHub workflow](.github/workflows/deploy.yml) for how to create the release build.

## FAQ

0. Why English when most users speak Chinese?  
   The project was planned to be used with [Open!batoru](https://batoru.moe/).

1. Why the UI is frozen during page loading?  
   This is because Blazor WebAssembly is [singlethreaded](https://stackoverflow.com/a/61865580), so the UI thread is blocked by the IO operation, even when we use Async methods to access the data.

2. There are a lot of error in your card data!  
   Garbage in, garbage out. ¯\\\_(ツ)_/¯
