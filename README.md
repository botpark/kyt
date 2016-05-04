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

### Conexión Socket
WebSocket es un protocolo que al igual que http es usado para la comunicación entre el cliente y servidor, pero en este caso, http solo functiona en una sola dirección; el cliente realiza una petición y el servidor responde. Mientras que con sockets se abre un canal de comunicación en el que el servidor emite data cada ves que algo cambia.

Para crear una sencilla conexión websocket desde JavaScript, es necesario crear la siguiente estancia.

```js
	var kyt = new WebSocket('ws://127.0.0.1:2020')
```

### Emitiendo Eventos
Una ves conectado el socket a nuestro servidor socket, que se encuentra corriendo como servicio de windows. podemos mandarle mensajes, para que este los interprete, lo que deja por consiguiente cada uno de los siguientes mensajes a emitir.

#### connect
Permite conectar el dispositivo RFID, a su ves arranca un hilo en el servidor, que permite la detección de tags, pero queda en un estado pausado.
```js
	kyt.send("connect");
```

#### disconnect
Permite desconectar al dispositivo RFID, y a su ves mata todos los hilos existentes, necesarios para la detección de tags.
```js
	kyt.send("disconnect");
```

### start
Inicia la detección de tags, la data emitida sera constante hasta que se decida parar.
```js
	kyt.send("start");
```

### pause
Permite dejar de detectar tags.
```js
	kyt.send("pause");
```

### single
Permite detectar solo un tag
```js
	kyt.send("single");
```

### reset
Es usado en caso de encontrarse una desconexión por parte de la estancia del socket y el dispositivo en este caso ya se encontraria conectado.
```js
	kyt.send("reset");
```

### default
En caso de emitirse un evento no registrado, ejemplo;
```js
	kyt.send("pepe");
```
El callback encargado de interpretar, estos eventos simplemente sera ignorado, y retornara un mensaje como el siguiente:

```js
	var foo = "Evento No Registrado";
```

## Recibiendo data
Ahora que ya hemos hablado de como el cliente emite X cantidad de eventos, es el turno del servidor y de como recibe la data el cliente.

```js
kyt.onmessage = function (evt) {
      console.log(evt.data);
}
```

donde ```"evt"``` contiene toda la data emitida por el servidor.

### connect
En el caso de haber emitido un ```"connect"```, la data que recibe el cliente es de las siguientes tres formas:

#### Primera
Para cuando se conecta por primera vez.
```json
{
    "type"   : "connect",
    "payload": {
        "state"  : "true",
        "mensaje": "Conectando Dispositivo..."
    } 
}
```

#### Segunda
Cuando ya existe una conexión

```json
{
    "type"   : "connect",
    "payload": {
        "state"  : "true",
        "mensaje": "Ya se encuentra conectado..."
    } 
}
```

#### Tercera
En caso de que un error ocurra.

```json
{
    "type"   : "connect",
    "payload": {
        "state"  : "false",
        "mensaje": "El mensaje de error determinado por el servidor"
    } 
}
```

### disconnect
Para cuando se emite un ```"disconnect"```, el cliente recibe lo siguente en el callback principal.

#### Primero
Cuando se realiza una desconexión de forma exitosa.

```json
{
    "type"   : "disconnect",
    "payload": {
        "state"  : "true",
        "mensaje": "Desconectando Dispositivo..."
    } 
}
```

#### Segundo
En caso que el dispositivo ya se encuentre desconectado.

```json
{
    "type"   : "disconnect",
    "payload": {
        "state"  : "true",
        "mensaje": "Ya se encuentra desconectado..."
    } 
}
```

#### Tercero
En caso de error.

```json
 {
    "type"   : "disconnect",
    "payload": {
        "state"  : "false",
        "mensaje": "El mensaje de error determinado por el servidor"
    } 
}
```

### start
Recibe la data de los tags de manera constante al emitirse el evento ```"start"```.

```json
{
    "type": "tag",
    "payload": {
        "tag": "E2003020250F0275199048EB",
        "ant": "4"
    }
}
```

### single
Recibe solo un tag al emitir el evento ```"single"```.

```json
{
    "type": "tag",
    "payload": {
        "tag": "E2003020250F0275199048EB",
        "ant": "4"
    }
}
```


