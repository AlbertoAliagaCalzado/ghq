1. Separación Física en Múltiples Proyectos (Arquitectura Hexagonal / Clean Architecture)
   - Decisión: Dividir la solución en cuatro proyectos distintos (Domain, Application, Infrastructure, Api) en lugar de usar carpetas dentro de un solo proyecto monolítico.

   - Justificación: Esta es la forma más estricta y segura en .NET de garantizar la separación clara de las capas de dominio, aplicación, infraestructura y presentación. Al usar proyectos separados, el propio compilador de C# restringe las referencias circulares o indebidas.

   - Trade-off: Añade un poco de complejidad inicial en la gestión de la solución frente a un monolito de un solo proyecto, pero garantiza que la arquitectura escale de forma limpia.

2. Aislamiento Absoluto del Dominio (Aplicación del Principio DIP)
   - Decisión: El proyecto GiftedIQ.Domain no tiene ninguna referencia a paquetes de bases de datos (como Entity Framework Core), librerías de UI, ni siquiera a los otros proyectos de la solución.

   - Justificación: Cumplimos estrictamente con la regla de que el dominio debe ser independiente de frameworks y librerías externas. Aplicamos el principio de Inversión de Dependencias (DIP) de SOLID: el dominio solo expone abstracciones (interfaces/puertos), y nunca depende de implementaciones.

   - Trade-off: Obliga a mapear datos o inyectar dependencias en capas superiores, pero protege las reglas de negocio.

3. Implementación de CQRS (Command/Query Responsibility Segregation)
   - Decisión: Estructurar la capa de aplicación dividiendo las operaciones en Commands (escrituras/mutaciones de estado) y Queries (lecturas).

   - Justificación: Respeta el Principio de Responsabilidad Única (SRP) de SOLID, permitiendo que las operaciones de lectura se optimicen de forma independiente a la lógica compleja de escritura.

   - Trade-off: Incrementa la cantidad de clases y archivos (cada operación requiere su propio Request y Handler), pero mejora drásticamente la mantenibilidad y facilita el testing unitario.

4. Estrategia de Manejo de Errores Orientada al Dominio
   - Decisión: Crear una carpeta Exceptions pura en el Dominio para las reglas de negocio y planificar un Middleware global en la capa de API.

   - Justificación: Nos asegura cumplir con el requisito de tener un manejo de errores explícito y coherente, evitando dejar excepciones sin capturar. Las excepciones del dominio comunicarán qué regla de negocio falló, y el middleware de la API las traducirá a códigos HTTP estandarizados (400, 404, etc.) sin filtrar detalles internos del servidor.
