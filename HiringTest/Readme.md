# Ejercicio de prueba

Se tiene una arquitectura cliente servidor, empleando .Net Core Web api y Blazor Server como cliente. Se emplea una base de datos en memoria. La versión de .Net Core empleada es la 6.x.

## Servidor
* Se requiere implementar una funcionalidad para insertar nuevos elementos "TestEntity". 
* Se requiere implementar una funcionalidad para listar los elementos de la tabla "TestEntity".
* Se requiere implementar una funcionalidad para eliminar los elementos de la tabla "TestEntity".
* Se requiere implementar una funcionalidad para actualizar los elementos de la tabla "TestEntity".
* Se requiere implementar una funcionalidad para mostrar los detalles de un elemento de la tabla "TestEntity" por su identificador.
* Se requiere implementar un servidor de SignalR para que los clientes se conecten a él. 
* Se requiere notificar a los clientes mediante SignalR cada vez que la entidad "TestEntity" sea modificada (Agregar, Editar, Eliminar).
* Se requiere conectar a un Servibus de azure para recibir mensajes.
* Se requiere implementar una funcionalidad donde se debe recibir el mensaje del Servicebus y luego notificar a los clientes conectados al SignalR el nuevo mensaje.


### Cadena de conexión del Servicebus:
```
Endpoint=sb://nft-servicebus-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aCOMWo3cGQK7oAdRC782T5d7AXaFOix8FMIZ4Qwu874=
```
### Queue name donde se enviarán los mensajes:
```
message
```
## Cliente

* Se requiere implementar una vista que permita listar los elementos retornados desde el API.
* Se requiere implementar una vista para agregar un nuevo elemento a la lista.
* Se requiere implementar una opción para eliminar un elemento de la lista.
* Se requiere implementar una vista para actualizar los datos de un elemento.
* Se requiere implementar una vista para mostrar los detalles de un elemento.
* Se requiere conectar el sistema al SinalR del servidor y escuchar los push que se envíen.
* Se requiere mostrar una mensaje cuando se reciba un push desde signalR de mensaje.
* Se requiere actualizar los elementos del listado cada vez que un push de actualización sean enviado por SignalR (Agregar, Editar, Eliminar).

## Autor
Ninja Fantasy Trader