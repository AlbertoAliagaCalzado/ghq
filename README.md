# GiftedIQ - Sistema de Notificaciones en Tiempo Real 🚀

Este proyecto es una solución Full-Stack desarrollada como prueba técnica, enfocada en la creación de un sistema de notificaciones reactivo y en tiempo real. Está diseñado siguiendo los principios de la **Arquitectura Hexagonal (Puertos y Adaptadores)** y aplicando el patrón **CQRS**.

## 🛠️ Stack Tecnológico

**Backend:**

- **Framework:** .NET 10 (ASP.NET Core Web API)
- **Arquitectura:** Hexagonal + CQRS (MediatR)
- **Tiempo Real:** SignalR (WebSockets)
- **Persistencia:** Entity Framework Core (In-Memory Database)
- **Testing:** xUnit, Moq, WebApplicationFactory (Tests de Integración E2E)

**Frontend:**

- **Framework:** React 18 + TypeScript + Vite
- **Estilos:** Tailwind CSS v4
- **Estado:** React Context API + Custom Hooks
- **Conexión de Red:** Fetch API (Capa de Servicios) + @microsoft/signalr

**DevOps:**

- **Contenedores:** Docker, Docker Compose (Multi-stage builds)
- **Servidor Web Frontend:** Nginx

---

## 📋 Requisitos Previos

Si deseas ejecutar el proyecto de forma nativa, asegúrate de tener instalado:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) (v20 o superior)

Para la ejecución contenerizada solo necesitas:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (o Docker Engine + Docker Compose)

---

## 🚀 Cómo ejecutar el proyecto (Local nativo)

### 1. Ejecutar contenedor de Docker (API & Frontal Web)

Abre una terminal en la raíz del proyecto y ejecuta:

```bash
   docker-compose up --build -d
```

### 2. Accede a las aplicaciones

Abre una terminal en la raíz del proyecto y ejecuta:

- Frontend: http://localhost:5173
- Swagger (API): http://localhost:5294/swagger
