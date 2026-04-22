1. Separación Física en Múltiples Proyectos (Arquitectura Hexagonal / Clean Architecture)

- Decisión: Dividir la solución en cuatro proyectos distintos (Domain, Application, Infrastructure, Api) en lugar de usar carpetas dentro de un solo proyecto monolítico.

- Justificación: Esta es la forma más estricta y segura de garantizar la separación clara de las capas de dominio, aplicación, infraestructura y presentación. Al usar proyectos separados, el propio compilador de C# restringe las referencias circulares o indebidas.

- Trade-off: Añade un poco de complejidad inicial en la gestión de la solución frente a un monolito de un solo proyecto, pero garantiza que la arquitectura escale de forma limpia.

2. Aislamiento Absoluto del Dominio

- Decisión: La capa de Dominio no tiene ninguna referencia a paquetes de bases de datos, librerías de UI, a nada exterior.

- Justificación: Se cumple estrictamente con la regla de que el dominio debe ser independiente de frameworks y librerías externas. El dominio solo expone abstracciones (interfaces/puertos), y nunca depende de implementaciones.

- Trade-off: Obliga a mapear datos o inyectar dependencias en capas superiores, pero protege las reglas de negocio.

3. Implementación de CQRS (Command/Query Responsibility Segregation)

- Decisión: Estructurar la capa de aplicación dividiendo las operaciones en Commands (escrituras) y Queries (lecturas).

- Justificación: Permite que las operaciones de lectura se optimicen de forma independiente a la lógica compleja de escritura.

- Trade-off: Incrementa la cantidad de clases y archivos (cada operación requiere su propio Request y Handler), pero mejora drásticamente la mantenibilidad y facilita el testing unitario.

4. Estrategia de Manejo de Errores Orientada al Dominio

- Decisión: Crear una carpeta Exceptions pura en el Dominio para las reglas de negocio y planificar un Middleware global en la capa de API.

- Justificación: Nos asegura cumplir con el requisito de tener un manejo de errores explícito y legible, evitando dejar excepciones sin capturar. Las excepciones del dominio comunicarán qué regla de negocio falló, y el middleware de la API las traducirá a códigos HTTP estandarizados (400, 404, etc.) sin filtrar detalles internos.

5. Rich Domain Model vs Modelo Anémico

- Decisión: Las propiedades de la entidad Notification tienen setters privados (private set). El estado interno solo puede modificarse a través de métodos de comportamiento explícitos, como MarkAsRead().

- Justificación: Se evita el "Modelo de Dominio Anémico", donde las entidades son bolsas de datos que dejan las reglas de negocio desprotegidas.

- Trade-off: Requiere escribir un poco más de código en comparación con modelos CRUD tradicionales con getters y setters públicos, y requiere un mapeo cuidadoso cuando se usan ORMs.

6. Uso de record para Value Objects (Objetos de Valor)

- Decisión: Utilizar el tipo record de C# 9+ para definir los Value Objects, como NotificationId.

- Justificación: En DDD, un Value Object se caracteriza por no tener identidad propia (dos objetos con los mismos valores son el mismo objeto) y por ser inmutable. Los records en C# proporcionan inmutabilidad por defecto y evaluación de igualdad basada en el valor (value-based equality) de forma nativa, ahorrándonos sobrescribir los métodos Equals y GetHashCode manualmente.

- Trade-off: Ninguno significativo.

7. Patrón Factory Method para la Creación de Entidades

- Decisión: Ocultar el constructor de Notification (haciéndolo privado) y exponer un método estático Create() para instanciar la entidad.

- Justificación: Un constructor público estándar no siempre puede expresar claramente la intención de negocio ni manejar lógicas complejas de validación inicial. El Factory Method asegura que es imposible instanciar una notificación en un estado inválido (por ejemplo, sin destinatario o con un mensaje nulo) y centraliza la lógica de creación (como la asignación del NotificationId y la fecha CreatedAt).

- Trade-off: Se deben usar métodos Create() en lugar de instanciar entidades directamente con "new".

8. Puertos (Interfaces) Definidos en el Dominio

- Decisión: La interfaz INotificationRepository reside en la capa de Dominio, aunque su implementación real interactuará con la base de datos.

- Justificación: Esta es la base de la Arquitectura Hexagonal. El Dominio "dicta" los contratos (puertos) que necesita para funcionar. La capa de Infraestructura debe adaptarse a estos contratos. Así, el dominio no sabe ni le importa dónde se guardan los datos ni en qué infraestructura.

- Trade-off: Puede requerir lógica adicional en la capa de infraestructura para mapear los modelos de base de datos a las entidades puras del dominio, pero así se asegura el aislamiento total de la lógica de negocio.

9. Implementación del mediador vía librería MediatR

- Decisión: Despachar los Commands y Queries desde la API hacia la capa de Aplicación con MediatR.

- Justificación: Permite mantener los controladores de la API limpios y enfocados únicamente en recibir peticiones HTTP. En lugar de inyectar decenas de servicios en un controlador, se inyecta solo el mediador. Esto también garantiza que cada Command sea manejado por un único Handler.

- Trade-off: Añade complejidad al seguir el flujo del código, ya que la llamada pasa por la interfaz de MediatR.

10. Unit of Work Controlado por la Aplicación

- Decisión: Definir una interfaz IUnitOfWork en la capa de Aplicación para confirmar transacciones (SaveChangesAsync), separada de los repositorios.

- Justificación: Los repositorios (INotificationRepository) solo deben encargarse de agregar o buscar entidades, no de guardar cambios en la base de datos de forma aislada. Es la capa de Aplicación (a través del Handler) la que conoce cuándo es seguro confirmar la transacción completa.

- Trade-off: Requiere inyectar dos dependencias (NotificationRepository y UnitOfWork) en los Handlers de comandos de escritura, pero asegura que si se agregan múltiples acciones en un mismo Handler, todas se guarden en una sola transacción.

11. Uso estricto de DTOs (Data Transfer Objects)

- Decisión: Nunca exponer las Entidades de Dominio (Notification) directamente a través de la API. En su lugar, se utilizan los objetos de transferencia de datos (DTOs) en la capa de Aplicación.

- Justificación: Eliminar la posibilidad de exposición de datos sensibles. Si en el futuro se agrega cualquier propiedad a la entidad Notification, el frontend no se verá afectado. Además, permite adaptar la estructura de los datos específicamente para las necesidades de la UI.

- Trade-off: Requiere escribir código adicional (boilerplate) en los Handlers para mapear las propiedades de la entidad al DTO.

12. Traducción de estados a códigos HTTP

- Decisión: El controlador se encarga explícitamente de evaluar el resultado del Query Handler (si devuelve null, un JSON, etc.) y traducirlo al código de estado HTTP semánticamente correcto (ej. 404 Not Found).

- Justificación: Mantiene la capa de Aplicación separada de la web. Los Handlers no tienen que saber nada sobre HTTP. Es responsabilidad exclusiva de la capa de presentación interpretar ese resultado y cumplir con los estándares de diseño.

- Trade-off: El controlador debe contener un mínimo de lógica condicional en lugar de simplemente pasar los datos.

13. Persistencia Ignorante (Persistence Ignorance) mediante EF Core

- Decisión: Mantener las entidades de dominio completamente libres de atributos o decoradores (como [Table], [Key], [Column]). En su lugar, toda la configuración de la base de datos se realiza en la capa de Infraestructura usando el ModelBuilder del DbContext.

- Justificación: Cumple con el requisito de que el dominio sea independiente de librerías externas. Si mañana se decide cambiar o migrar a una nueva base de datos, Notification no sufrirá ni un cambio.

- Trade-off: Requiere escribir y mantener la configuración de mapeo manualmente en el DbContext (dos archivos en lugar de uno).

14. Mapeo con Value Conversions

Decisión: Utilizar el mecanismo de conversiones HasConversion para traducir NotificationId a un tipo primitivo GUID al guardar en la base de datos, y viceversa al leer.

Justificación: Permite mantener el tipado fuerte en el dominio (ayuda al programador), mientras se garantiza la eficiencia de las consultas en la base de datos.

Trade-off: Es necesario configurar estas conversiones explícitamente por cada Value Object.

15. Uso de Base de Datos In-Memory

- Decisión: Utilizar la memoria interna de la API para el desarrollo inicial y la demostración del funcionamiento.

- Justificación: Se requiere que el "happy path" sea demostrable y se permite el uso de datos semilla. Una base de datos en memoria reduce drásticamente la fricción inicial para levantar el proyecto en local (no requiere instalar SQL Server ni configurar contenedores Docker complejos de base de datos de forma inmediata), permitiendo evaluar la arquitectura y el código funcional rápidamente.

- Trade-off: No es una base de datos relacional real (no valida restricciones de integridad referencial complejas), por lo que antes de ir a producción, se entiende que debería cambiar por un proveedor de base de datos.

16. Middleware Global de Excepciones

- Decisión: Implementar un middleware personalizado para la captura global de excepciones en el pipeline de la API.

- Justificación: Garantiza que el cliente siempre reciba una respuesta en formato JSON coherente, incluso ante errores inesperados. Esto evita la fuga de información sensible y captura todos los posibles errores.

- Trade-off: Centralizar el manejo de errores puede ocultar detalles específicos si no se categorizan bien las excepciones, pero mejora drásticamente la mantenibilidad y la experiencia del usuario de la API.

17. Comunicación en Tiempo Real mediante SignalR

- Decisión: Implementar WebSockets usando SignalR y exponer un Hub para el envío de eventos en tiempo real al cliente.

- Justificación: Se requiere proveer notificaciones push / web en tiempo real. SignalR maneja automáticamente la negociación de protocolos y facilita la gestión de conexiones concurrentes. Se sigue respetando la Arquitectura Hexagonal.

- Trade-off: Añade un estado de conexión mantenido en el servidor. Si la aplicación escala a múltiples servicios en el futuro, requerirá configurar un sistema de colas para sincronizar los mensajes entre los distintos servidores.

18. Separación en Componentes Frontend

- Decisión: Aislar toda la lógica de estado y red (SignalR, Fetch API) fuera de la capa de interfaz, centralizándola en Contextos, y mantener los componentes visuales (NotificationBadge, NotificationList) como elementos estrictamente presentacionales.

- Justificación: Los componentes tienen responsabilidad única. Así, se garantiza que los componentes visuales sean "ignorantes" sobre el origen de los datos (no saben si vienen de un WebSocket, un mock o una API REST). Esto mejora drásticamente la mantenibilidad, facilita la creación de tests unitarios visuales y permite que el diseño escale sin enredarse con la lógica de negocio.

- Trade-off: Requiere una mayor fragmentación del código inicial, lo que exige una navegación ligeramente más compleja por la estructura de carpetas, pero es vital para la limpieza al escalar el proyecto.

19. Encapsulación del Estado Global mediante Custom Hooks (useNotifications)

- Decisión: Prohibir la exportación e importación directa del NotificationContext en los componentes de la vista, obligando a su consumo exclusivamente a través del hook personalizado useNotifications.

- Justificación: Actúa como un mecanismo de seguridad. Al encapsular useContext dentro del Custom Hook, se puede incluir lógica para verificar que se está llamando dentro de su Provider correspondiente, aislando los errores del servicio en su interior.

- Trade-off: Añade unas pocas líneas de código repetitivo en el contexto para definir el hook, pero el beneficio de prevenir errores silenciosos en tiempo de ejecución (como un contexto devolviendo undefined) supera ampliamente el desarrollo inicial.

20. Abstracción de Llamadas de Red (API Wrapper)

- Decisión: Extraer toda la lógica de peticiones HTTP (fetch, cabeceras, manejo de URLs) desde los componentes de React hacia una capa de servicios aislada en api/NotificationsApi.

- Justificación: Centraliza la configuración de la API en un único lugar. Si en el futuro se requiere añadir un Token de Autorización (JWT) en los headers, o se migra de fetch a axios, el cambio se hará en un solo archivo Los componentes de React ahora solo se encargan de la vista y delegar la acción, reduciendo el ruido.

21. Estrategia de Pruebas Unitarias (Backend)

- Decisión: Implementar pruebas unitarias utilizando xUnit y Moq, enfocando el esfuerzo principalmente en la capa de Aplicación (Command/Query Handlers) y Dominio (Entidades).

- Justificación: Al tener una Arquitectura Hexagonal, es sencillo aislar completamente la capa de aplicación inyectando mocks de las interfaces de infraestructura. Esto asegura que los tests sean extremadamente rápidos, predecibles y que validen estrictamente las reglas de negocio sin dependencias externas.

- Trade-off: Las pruebas unitarias con mocks no garantizan que la integración real con SignalR funcione, para ello existen pruebas de integración.

22. Tests de Integración End-to-End en Memoria (Backend)

- Decisión: Utilizar WebApplicationFactory para levantar la API en memoria durante las pruebas de integración, aislando componentes de infraestructura externos (SignalR).

- Justificación: Valida que el pipeline completo (Controladores, Routing, Middlewares de error, Inyección de Dependencias, Validación JSON) funciona correctamente en conjunto. Sustituir SignalR por un mock en este punto evita la inestabilidad de abrir puertos y sockets reales.
