# ♟️🔴 Chess and Connect

**Chess and Connect** es una aplicación web multijugador en tiempo real que permite jugar partidas de **Ajedrez** y **Conecta 4**, ya sea contra otros jugadores conectados o contra un bot inteligente.  

Este proyecto fue desarrollado como parte del segundo trimestre del ciclo formativo de **Desarrollo de Aplicaciones Multiplataforma (DAM)**, con el objetivo de aplicar de forma práctica los conocimientos adquiridos sobre desarrollo web fullstack moderno, comunicación en tiempo real y experiencia de usuario.

Diseñado y construido en equipo, busca ofrecer una experiencia divertida, dinámica y accesible para todos los usuarios.

---

## 📦 Tecnologías Utilizadas

### 🔙 Backend

| Tecnología               | Descripción                                                                           |
|--------------------------|---------------------------------------------------------------------------------------|
| **ASP.NET Core 8.0**     | Framework principal del servidor, manejando API REST y WebSockets                     |
| **C#**                   | Lenguaje de programación para toda la lógica del backend                             |
| **WebSocket nativo**     | Comunicación bidireccional en tiempo real entre jugadores                            |
| **Entity Framework Core**| ORM para gestionar bases de datos con **SQLite** y **MySQL**                         |
| **JWT (Json Web Tokens)**| Autenticación segura basada en tokens                                                |
| **Swagger**              | Documentación interactiva de la API                                                  |
| **F23.StringSimilarity** | Comparación de cadenas para búsqueda inteligente de usuarios                         |

### 🔜 Frontend

| Tecnología         | Descripción                                                                     |
|--------------------|---------------------------------------------------------------------------------|
| **Angular 19**     | Framework frontend usado para construir una SPA moderna y escalable             |
| **TypeScript**     | Desarrollo tipado y robusto del lado del cliente                               |
| **TailwindCSS**    | Sistema de estilos utilitarios para interfaces rápidas y responsivas           |
| **RxJS**           | Programación reactiva y manejo eficiente de eventos en tiempo real             |
| **SweetAlert2**    | Notificaciones y diálogos visuales personalizados                              |
| **Service Worker** | Permite usar la app como una **PWA** (Progresiva e instalable desde el navegador) |

---

## 🎮 Juegos Disponibles

La plataforma ofrece dos modos de juego: **Ajedrez** y **Conecta 4**. Ambos comparten las mismas funcionalidades:

- Jugar contra otro jugador en línea
- Jugar contra un bot
- Usar el sistema de emparejamiento automático
- Comunicación por chat en tiempo real
- Temporizador individual
- Solicitud de tablas o rendición
- Detección automática de desconexión

### 🧠 Ajedrez

- Tablero orientado según el color del jugador
- Movimientos válidos resaltados
- Temporizador individual para cada jugador
- Chat y notificaciones integradas
- Manejo de eventos como solicitud de revancha, desconexión del rival, etc.

![Vista general del juego](assets/chess1.jpg)
![Resultado](assets/chess2.jpg)
![Desconexión](assets/chess4.jpg)

### 🔴 Conecta 4

- Tablero clásico de 6x7
- Mecánica adaptada con lógica de validación de victoria
- Mismas opciones de juego, chat y notificaciones que en Ajedrez

![Conecta 4](assets/connect1.jpg)

---

## 📋 Menú y Navegación

Desde el menú principal el usuario puede:

- Elegir el modo de juego
- Gestionar su perfil y sus partidas
- Ver y administrar su lista de amigos
- Buscar otros jugadores mediante búsqueda inteligente
- Enviar, aceptar o rechazar solicitudes de amistad

![Menú principal](assets/menu1.jpg)
![Búsqueda de amigos](assets/menu2.jpg)
![Enviar solicitud](assets/menu3.jpg)
![Gestionar solicitudes](assets/menu4.jpg)

---

## 👤 Vista de Usuario

- Visualización de perfil propio y de otros jugadores
- Edición de datos personales (solo en tu perfil)
- Envío y eliminación de amistades
- Historial de partidas con paginación
- Filtro entre partidas de Ajedrez y Conecta 4

![Perfil de usuario](assets/user1.jpg)
![Paginación](assets/user2.jpg)
![Añadir amigo](assets/user3.jpg)
![Eliminar amigo](assets/user4.jpg)

---

## 🛠️ Panel de Administración

Para los usuarios con rol de **administrador**, existe un panel especial desde donde pueden:

- Ver a todos los usuarios registrados
- Cambiar roles (usuario/admin)
- Banear usuarios de la plataforma

Los usuarios baneados solo tendrán acceso a la pantalla principal y se les mostrará un mensaje con opción para apelar su caso.

![Panel de administración](assets/admin1.jpg)
![Apelación](assets/admin2.jpg)

---

## 🌐 Demo en Línea

El proyecto se encuentra desplegado en la web:

🔗 **[Ver demo](https://chess-connect-mejora.vercel.app/)**

- **Frontend**: Vercel  
- **Backend**: MonsterASP.NET

---

## 💡 Principales Funcionalidades

- 🎮 Partidas en tiempo real de Ajedrez y Conecta 4
- 👥 Registro y login con JWT
- 💬 Chat en tiempo real con WebSockets
- 🧩 Sistema de emparejamiento automático
- 📱 Interfaz responsive para móviles y escritorio
- 🧠 Lógica validada de movimiento, reglas y victoria
- 👨‍💼 Panel administrativo completo
- 🔍 Búsqueda inteligente de usuarios por similitud


