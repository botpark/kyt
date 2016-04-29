# Kyt

Kyt es un servicio de windows para la gestión del dispositivo RFID PT-3L01Z. Como puente de cominicación el servicio instancia un servidor socket que es el encargado de emitir y de recibir data.

## Prerequisitos
1. Es necesario .NET framework 4.5
2. Tener instalado Visual Studio Installer, este se puede obtener mediante la busqueda de plugins y extensiones de Visual Studio.
3. Es necesario Visual Studio 2015 Comunity Edition, para un mejor rendimiento, en compilación.

## Instalación

### Modo Desarrollo
En este modo simplemente se corre el sevicio en modo debug por Visual Studio y queda listo para ser consumido.

### Modo Procucción
Se brinda un instalador, en el siguiente enlace; [kyt Installer](http://www.google.com)

## API
Texto de prueba

### Conexión Socket
WebSocket es un protocolo que al igual que http es usado para la comunicación entre el cliente y el servidor, pero en este caso, http solo functiona en una sola dirección; el cliente realiza una petición y el servidor responde. Mientras que con sockets se abre un canal de comunicación en el que el servidor emite data cada ves que encuentra que algo cambia.

Para crear una sencilla conexión websocket desde JavaScript, es necesario crear la siguiente estancia.

```js
	var socket = new WebSocket('ws://127.0.0.1:2020')
```

### Emitiendo Eventos
Una ves conectado el socket a nuestro servidor socket, que se encuentra corriendo como servicio de windows. podemos mandarle mensajes, para que este los interprete, lo que deja por consiguiente cada uno de los siguientes mensajes a emitir.

#### connect
Permite conectar el dispositivo RFID, a su ves arranca un hilo en el servidor, que permite la detección de tags, pero queda en un estado pausado.
```js
	socket.emit("connect");
```

#### disconnect
Permite desconectar al dispositivo RFID, y a su ves mata todos los hilos existentes, necesarios para la detección de tags.
```js
	socket.emit("disconnect");
```

### start
Inicia la detección de tags, la data emitida sera constante hasta que se decida parar.
```js
	socket.emit("start");
```

### pause
Permite dejar de detectar tags.
```js
	socket.emit("pause");
```

### pause
Permite detectar solo un tag
```js
	socket.emit("single");
```

### reset
Es usado en caso de encontrarse una desconexión por parte de la estancia del socket y el dispositivo en este caso ya se encontraria conectado.
```js
	socket.emit("reset");
```

### default
En caso de emitirse un evento no registrado, ejemplo;
```js
	socket.emit("pepe");
```
El callback encargado de interpretar, estos eventos simplemente sera ignorado, y retornara un mensaje como el siguiente:

```js
	var foo = "Evento No Registrado";
```

## Ejemplos


