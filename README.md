# 🚀 Chat Cliente-Servidor en C#

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

Un sistema de chat básico implementado con sockets TCP que demuestra comunicación en red y programación concurrente.

## 🌟 Características Principales

### 💻 **Cliente**
- ✅ Conexión TCP con autenticación por nombre
- 📝 Menú interactivo para enviar mensajes
- 📨 Mensajería privada entre usuarios
- 🧵 Recepción asíncrona en hilo separado
- ✉️ Formato: `remitente\mensaje\destinatario\`

### 🖥️ **Servidor**
- 🔌 Gestión multi-cliente con hilos
- 📊 Lista dinámica de usuarios conectados
- 🔄 Reenvío inteligente de mensajes
- 🔔 Notificaciones de conexión/desconexión

## ⚙️ Funcionamiento

1. El servidor inicia en IP/puerto configurado
2. Los clientes se conectan con su nombre de usuario
3. Los usuarios envían mensajes privados
4. El servidor reenvía mensajes al destinatario
5. Notifica si el destinatario no está disponible

## 🛠️ Tecnologías Utilizadas

- 🕸️ Sockets TCP para comunicación
- 🧵 Threads para concurrencia
- 🔤 Codificación UTF-8
- 🏗️ Patrón Cliente-Servidor

## 📚 Propósito Educativo

Este proyecto demuestra:
- Programación de redes básicas
- Comunicación entre procesos
- Manejo de conexiones concurrentes
- Patrones de diseño en .NET
