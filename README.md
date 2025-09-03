# Task Manager Angular

![Angular](https://img.shields.io/badge/Angular-E23237?style=for-the-badge&logo=angular&logoColor=white)
![Angular Material](https://img.shields.io/badge/Angular%20Material-0081CB?style=for-the-badge&logo=angular&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
<br>
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![ASP.NET](https://img.shields.io/badge/ASP.NET-512BD4?style=for-the-badge&logo=asp.dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

---

## Description

A simple full-stack **task manager** built using:

- 🖼️ **Angular**, **Angular Material** and **Bootstrap** for the frontend
- ⚙️ **C# ASP.NET Web API** for the backend
- 🗃️ **PostgreSQL** as the database
- 🐳 **Docker** to dockerize as the things

Not all tools were strictly necessary, some were included to explore best practices and expand our tech exposure.

---

## 🚀 Application Features

This project is a clean, full-stack implementation designed to showcase core development concepts.

It includes the following features organized by page:

🏠 Home Page

    A clear landing page with a brief overview of the project and a "Getting Started" section.

📝 Task Management

    ✅ Create & Save: Add new tasks.

    📝 View & Edit: A table view where you can see all tasks, with a dedicated page to edit an individual task.

    ❌ Delete: Remove one or more tasks from the database.

    🔍 Filtering & Sorting: Easily search, organize, and find tasks.

    ➡️ Pagination: Navigate through tasks with a paging system for a clean user experience.

    📌 Mark Fail: Tasks are automatically marked as failed if deadline passes.

📊 Statistics

    A simple statistics page with a single chart, demonstrating data visualization with Chart.js.

There's no authentication or user system, just a clean CRUD implementation to practice full-stack architecture and collaboration.

## 📋Getting Started

This repo contains a [frontend](./frontend) and a [backend.api](./backend.api) folder, each with its own `README.md` containing specific setup instructions.

To get started clone the repo:

```bash
git clone git@github.com:NedzadDonlagic04/task-manager-angular.git
cd task-manager-angular
```

After you setup the projects in the aforementioned folders you should be able to run everything without issue.

## Docker Compose Setup

This project uses Docker Compose to run the entire application stack.

### Configure Frontend and Backend

Follow the setup in these 2 projects before continuing.

### Configure Environment Variables

Before you start, you need to provide the credentials for the PostgreSQL database. For security, these are managed using an environment file.

Create a file named **.env.db** in the root directory of the project. If you change the name of this file make sure you **DO NOT COMMIT THIS FILE TO GIT.**

Contents of the file should be in this format:

```
DB_USER=your_db_username
DB_PASS=your_db_password
DB_PORT=5432
DB_NAME=your_db_name
```

### Run the Application

If you have a local PostgreSQL instance running, you may need to stop it to avoid port conflicts.

```bash
# Example for a Linux system
sudo systemctl stop postgresql
```

After configuring your environment variables, build and run the entire application with a single command.

The `--env-file` flag is necessary because your environment variables are stored in a file with a custom name.
If you leave the name just ass **.env** you don't need to pass it like this to the command.

```bash
docker compose --env-file .env.db up --build
```
