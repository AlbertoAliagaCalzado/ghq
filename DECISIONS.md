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
