# Family Library

## 项目简介

一个用于族文件管理系统的项目，该项目前端框架为 vue3.js。后端框架为.Net 8 微服务架构容器化部署。

## 安装说明

### 前端

进入 src/frontend 目录，运行以下命令

1. 安装依赖

```bash
pnpm install
```

2. 运行项目

```bash
pnpm run dev
```

### 后端

进入 src/backend 目录，运行以下命令 (在安装了 docker 和 docker compose 之后才能运行)

```bash
docker compose up -d
```

## 功能特点

### 后端
- dotnet8 微服务架构
- 采用 Envoy 作为网关
- 数据库使用 postgresql
