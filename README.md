# Gestion_inmueble
Sistema de Gestión de Propiedades Inmobiliarias
4️⃣ Sistema de Gestión de Propiedades Inmobiliarias
📌 Descripción: Un software para que agentes inmobiliarios gestionen propiedades y clientes.

📂 Entidades:

Usuario (nombre, email, rol, contraseña)
Propiedad (nombre, dirección, precio, tipo, estado)
Cliente (nombre, email, teléfono, presupuesto)
Agente (usuario_id, teléfono, experiencia)
Contrato (propiedad_id, cliente_id, fecha, monto, duración)
Pago (cliente_id, propiedad_id, monto, estado)
Visita (cliente_id, propiedad_id, fecha, agente_id)
Comentario (usuario_id, propiedad_id, mensaje, calificación)
Notificación (usuario_id, mensaje, estado)
💼 Modelo de Negocio:

Cobro por publicación de propiedades.
