# Documentacion de Pruebas — BookStork

## Configuracion del entorno de pruebas

| Herramienta | Version | Rol |
|---|---|---|
| Vitest | 4.x | Ejecutor de pruebas, reemplaza Jest en proyectos Vite |
| @testing-library/react | 16.x | Renderizado de componentes en jsdom |
| @testing-library/user-event | 14.x | Simulacion de eventos del usuario |
| @testing-library/jest-dom | 6.x | Matchers adicionales como toBeInTheDocument |
| jsdom | 29.x | Simulacion del DOM del navegador en Node.js |

Archivo de configuracion relevante: `vite.config.ts`

```ts
test: {
  environment: "jsdom",   // Simula el DOM del navegador
  globals: true,          // Permite usar describe/it/expect sin importarlos
  setupFiles: "./src/test/setup.ts",  // Carga jest-dom antes de cada suite
}
```

Archivo `src/test/renderWithProviders.tsx`:
Funcion auxiliar que envuelve el componente bajo prueba en `MemoryRouter` y
`SearchProvider`. Se usa en todos los tests de componentes para evitar errores
por contexto o enrutamiento faltante.

---

## Parte 1 — Diseno de Casos de Prueba: Componente `BookCard`

### Descripcion del componente

`BookCard` recibe un objeto `Book` y renderiza una tarjeta con portada, titulo,
autor y anio de publicacion. La tarjeta es un enlace que navega a
`/books/:workId` al hacer clic.

### Casos de prueba documentados

**Caso 1 — Renderizado del titulo**

- Archivo: `src/test/components/BookCard.test.tsx`
- Que se asegura: el titulo del libro aparece visible en el DOM.
- Limite: no verifica estilos ni posicion visual; solo presencia del texto.
- Tipo: prueba de renderizado.

```
Dado un libro con titulo "Dune"
Cuando se renderiza BookCard
Entonces el texto "Dune" debe estar en el documento
```

---

**Caso 2 — Renderizado del autor**

- Que se asegura: el primer elemento del arreglo `author_name` se muestra.
- Limite: solo verifica el primer autor; si hay varios, los demas no se comprueban aqui.
- Tipo: prueba de renderizado condicional.

```
Dado un libro con author_name: ["Frank Herbert"]
Cuando se renderiza BookCard
Entonces el texto "Frank Herbert" debe estar en el documento
```

---

**Caso 3 — Renderizado del anio de publicacion**

- Que se asegura: el campo `first_publish_year` se muestra cuando existe.
- Limite: no verifica formato ni localizacion del numero.
- Tipo: prueba de renderizado condicional.

```
Dado un libro con first_publish_year: 1965
Cuando se renderiza BookCard
Entonces el texto "1965" debe estar en el documento
```

---

**Caso 4 — Campos opcionales ausentes**

- Que se asegura: cuando `author_name` y `first_publish_year` no se proporcionan,
  no aparecen en el DOM. El titulo sigue siendo visible.
- Limite: no verifica que el espacio visual quede correctamente ajustado.
- Tipo: prueba de renderizado con datos incompletos.

```
Dado un libro sin author_name ni first_publish_year
Cuando se renderiza BookCard
Entonces el titulo debe estar en el documento
Y el autor y el anio NO deben estar en el documento
```

---

**Caso 5 — Generacion del enlace de navegacion**

- Que se asegura: el atributo `href` del enlace corresponde al workId
  extraido del campo `key` del libro.
- Limite: verifica solo el atributo href; no simula el clic ni comprueba
  que la navegacion llega a la pagina correcta.
- Tipo: prueba de logica de transformacion de datos.

```
Dado un libro con key: "/works/OL82563W"
Cuando se renderiza BookCard
Entonces el enlace debe tener href="/books/OL82563W"
```

---

**Caso 6 — Imagen de portada con alt text**

- Que se asegura: cuando existe `cover_i`, se renderiza una imagen con la URL
  correcta de Open Library y un texto alternativo accesible.
- Limite: no verifica que la imagen carga correctamente en la red; solo
  que el atributo src y alt son los esperados.
- Tipo: prueba de accesibilidad y logica de URL.

```
Dado un libro con cover_i: 12345 y titulo "Dune"
Cuando se renderiza BookCard
Entonces debe existir una imagen con:
  alt = "Cover of Dune"
  src = "https://covers.openlibrary.org/b/id/12345-M.jpg"
```

---

**Caso 7 — Sin imagen cuando no hay cover_i**

- Que se asegura: cuando el libro no tiene `cover_i`, no se renderiza ninguna
  imagen en el DOM.
- Limite: no verifica que el marcador de posicion visual (el icono de libro)
  se muestre correctamente.
- Tipo: prueba de renderizado condicional.

```
Dado un libro sin cover_i
Cuando se renderiza BookCard
Entonces no debe existir ninguna imagen en el documento
```

---

## Parte 1 — Diseno de Casos de Prueba: Componente `HomePage`

### Descripcion del componente

`HomePage` presenta un formulario de busqueda con un campo de texto y un boton.
Al enviar, llama a `setQuery` del contexto y navega a `/books`. Tambien ofrece
botones de sugerencia predefinidos que hacen lo mismo al hacer clic.

### Casos de prueba documentados

**Caso 1 — Titulo principal visible**

- Que se asegura: el encabezado de la pagina existe en el DOM con el rol
  semantico correcto (heading).
- Limite: no verifica el nivel del encabezado (h1 vs h2).
- Tipo: prueba de renderizado.

```
Cuando se renderiza HomePage
Entonces debe existir un heading con el texto "Discover Your Next Read"
```

---

**Caso 2 — Campo de busqueda y boton presentes**

- Que se asegura: el input con su placeholder y el boton de envio existen
  en el DOM y son accesibles por nombre.
- Limite: no verifica estilos ni disposicion visual.
- Tipo: prueba de renderizado y accesibilidad.

```
Cuando se renderiza HomePage
Entonces debe existir un input con placeholder "Title, author, or keyword..."
Y debe existir un boton con nombre "Search"
```

---

**Caso 3 — Chips de sugerencias renderizados**

- Que se asegura: los cinco chips de sugerencia predefinidos (Dune, 1984,
  The Hobbit, Sapiens, Harry Potter) se renderizan como botones accesibles.
- Limite: no verifica el orden ni el estilo de los chips.
- Tipo: prueba de renderizado de lista.

```
Cuando se renderiza HomePage
Entonces deben existir botones con los textos:
  "Dune", "1984", "The Hobbit", "Sapiens", "Harry Potter"
```

---

**Caso 4 — El input refleja lo que escribe el usuario**

- Que se asegura: el estado controlado del input se actualiza cuando el
  usuario escribe, mostrando el valor correcto.
- Limite: no verifica debounce ni validacion en tiempo real.
- Tipo: prueba de interaccion de formulario.

```
Dado que el usuario escribe "Orwell" en el input
Cuando se inspecciona el valor del campo
Entonces el input debe tener el valor "Orwell"
```

---

**Caso 5 — No navega con formulario vacio**

- Que se asegura: al hacer clic en Search sin texto, la navegacion no ocurre
  y la pagina de inicio sigue visible.
- Limite: no verifica mensajes de error de validacion; solo que el componente
  no cambia de ruta.
- Tipo: prueba de validacion de formulario.

```
Dado que el input esta vacio
Cuando el usuario hace clic en el boton "Search"
Entonces el heading "Discover Your Next Read" debe seguir en el documento
```

---

**Caso 6 — No navega con solo espacios en blanco**

- Que se asegura: la funcion `trim()` en `handleSubmit` descarta entradas
  que son visualmente no vacias pero semanticamente vacias.
- Limite: cubre solo espacios ASCII; no cubre tabs ni saltos de linea.
- Tipo: prueba de caso borde de validacion.

```
Dado que el usuario escribe "   " (solo espacios) en el input
Cuando el usuario envia el formulario
Entonces el heading "Discover Your Next Read" debe seguir en el documento
```

---

## Parte 2 — Probando el hook `useFetch`

### Como se aborda la prueba de un hook que obtiene datos

Un hook no puede renderizarse directamente. `@testing-library/react` provee
`renderHook`, que monta el hook en un componente anonimo y expone su valor
de retorno a traves de `result.current`.

```ts
const { result } = renderHook(() => useFetch("https://api.ejemplo.com/libros"));
await waitFor(() => expect(result.current.loading).toBe(false));
```

`waitFor` reintenta la asercion hasta que pasa o se agota el tiempo limite,
lo que permite esperar a que la promesa de fetch resuelva sin tiempos de espera
fijos.

### Que se debe simular

**1. La funcion global `fetch`**

`fetch` no existe en Node.js de forma nativa con el mismo comportamiento que
en el navegador. Se reemplaza con `vi.fn()`:

```ts
const fetchMock = vi.fn();
vi.stubGlobal("fetch", fetchMock);
```

Esto permite controlar exactamente que devuelve cada llamada sin tocar
ningun servidor real.

**2. El objeto `Response`**

`fetch` en el navegador devuelve un objeto `Response` con propiedades como
`ok`, `status` y el metodo `json()`. El mock debe replicar esta forma:

```ts
function okResponse(data: unknown) {
  return Promise.resolve({
    ok: true,
    status: 200,
    json: () => Promise.resolve(data),
  });
}

function errorResponse(status: number, statusText: string) {
  return Promise.resolve({
    ok: false,
    status,
    statusText,
    json: () => Promise.resolve({}),
  });
}
```

**3. AbortController**

`jsdom` incluye `AbortController`, por lo que no es necesario simularlo.
La prueba de cancelacion no es necesaria porque `AbortError` se ignora
intencionalmente en el hook.

**4. Lo que NO se simula**

- Temporizadores (`setTimeout`): el hook no tiene reintentos con retardo.
- El modulo completo de `fetch`: solo se simula la funcion global, no se
  mockeam modulos enteros, lo que mantiene el comportamiento del hook intacto.

### Nota sobre StrictMode

`@testing-library/react` v16 envuelve `renderHook` en `React.StrictMode` por
defecto. StrictMode ejecuta cada efecto dos veces en desarrollo para detectar
efectos secundarios no puros. Por esto, las aserciones de conteo de llamadas a
`fetch` usan conteos relativos en lugar de absolutos:

```ts
// Fragil — asume que fetch se llama exactamente 1 vez,
// pero StrictMode puede llamarlo 2 veces
expect(fetchMock).toHaveBeenCalledTimes(1);

// Robusto — verifica que refetch produjo al menos una llamada adicional
const llamadasAntes = fetchMock.mock.calls.length;
act(() => { result.current.refetch(); });
await waitFor(() => expect(result.current.loading).toBe(false));
expect(fetchMock.mock.calls.length).toBeGreaterThan(llamadasAntes);
```

### Casos de prueba documentados para `useFetch`

**Caso 1 — Estado inicial con URL**

- Que se asegura: el estado sincrono del primer renderizado es
  `loading=true`, `data=null`, `error=null`.
- Limite: usa una promesa que nunca resuelve para congelar el estado;
  no cubre el caso de URL vacia en el estado inicial.

**Caso 2 — Exito de la peticion**

- Que se asegura: tras una respuesta exitosa, `data` contiene el JSON
  parseado y `loading` es false.
- Limite: la URL y los datos son estaticos; no cubre respuestas paginadas.

**Caso 3 — Error HTTP (4xx / 5xx)**

- Que se asegura: una respuesta con `ok: false` produce `error` con el
  codigo de estado y `data=null`.
- Limite: el formato del mensaje de error depende del texto del servidor.

**Caso 4 — `enabled: false`**

- Que se asegura: el hook no llama a `fetch` cuando la opcion `enabled`
  es `false`.
- Limite: no verifica el comportamiento cuando `enabled` cambia de `false`
  a `true` despues del montaje.

**Caso 5 — `refetch` dispara una nueva peticion**

- Que se asegura: llamar a `refetch()` produce al menos una llamada
  adicional a `fetch` respecto al estado anterior.
- Limite: usa un conteo relativo por compatibilidad con StrictMode.

**Caso 6 — Error de red**

- Que se asegura: cuando `fetch` rechaza la promesa (sin conexion, DNS
  fallido), el campo `error` recibe el mensaje de la excepcion.
- Limite: el mensaje depende del texto del `Error` lanzado; en un
  navegador real el mensaje puede variar segun el sistema operativo.

---

## Parte 3 — Flujo de Autenticacion Basado en Tokens

El siguiente flujo describe los pasos desde que el usuario llena el formulario
de inicio de sesion hasta que realiza una peticion autenticada a la API.

### Paso 1 — El usuario envia el formulario

El componente `LoginForm` captura el correo y la contrasena. Antes de enviar,
se realiza validacion en el cliente: campos no vacios, formato de correo valido.
Si la validacion falla, se muestran mensajes de error sin contactar al servidor.

```
Usuario escribe correo y contrasena
-> Validacion del lado del cliente
-> Si invalido: mostrar error, detener flujo
-> Si valido: continuar al paso 2
```

### Paso 2 — Peticion de autenticacion al servidor

Se envia una peticion `POST` al endpoint de autenticacion con las credenciales
en el cuerpo en formato JSON. La contrasena nunca se almacena en el estado de
React despues de este punto.

```ts
const response = await fetch("/api/auth/login", {
  method: "POST",
  headers: { "Content-Type": "application/json" },
  body: JSON.stringify({ email, password }),
});
```

La comunicacion debe ocurrir siempre sobre HTTPS para proteger las credenciales
en transito.

### Paso 3 — El servidor valida las credenciales

El servidor verifica el correo contra la base de datos y compara la contrasena
con el hash almacenado usando un algoritmo seguro como bcrypt. Si las credenciales
son incorrectas, el servidor responde con `401 Unauthorized` y un mensaje generico
que no indica si el correo o la contrasena fue el campo incorrecto (para no dar
informacion a un atacante).

### Paso 4 — El servidor emite un token

Si las credenciales son validas, el servidor genera un token JWT firmado con una
clave secreta. El payload del token contiene datos no sensibles como el id del
usuario y su rol. El token tiene un tiempo de expiracion definido, por ejemplo
quince minutos para el access token.

```json
{
  "sub": "user-123",
  "role": "reader",
  "exp": 1234567890
}
```

El servidor puede tambien emitir un refresh token de vida mas larga, que se
almacena en una cookie `HttpOnly` para evitar acceso desde JavaScript.

### Paso 5 — El cliente recibe y almacena el token

El access token se recibe en el cuerpo de la respuesta y se guarda en memoria
(variable de modulo o estado de React). No se guarda en `localStorage` porque
es vulnerable a ataques XSS. El refresh token llega en la cookie `HttpOnly`
automaticamente.

```ts
const { accessToken } = await response.json();
// Se guarda en memoria, no en localStorage
setAccessToken(accessToken);
```

### Paso 6 — Peticion autenticada a la API

Para cualquier peticion subsecuente que requiera autorizacion, el access token
se incluye en el encabezado `Authorization` como Bearer token:

```ts
const data = await fetch("/api/books/favorites", {
  headers: {
    "Authorization": `Bearer ${accessToken}`,
    "Content-Type": "application/json",
  },
});
```

El servidor verifica la firma del token en cada peticion sin consultar la
base de datos. Si el token expiro, responde con `401` y el cliente usa el
refresh token para obtener un nuevo access token de forma transparente.

### Paso 7 — Cierre de sesion

Al cerrar sesion, el access token se elimina de la memoria del cliente y se
envia una peticion al servidor para invalidar el refresh token. Esto garantiza
que un token robado no pueda usarse indefinidamente.

```ts
// Limpiar estado del cliente
setAccessToken(null);

// Invalidar el refresh token en el servidor
await fetch("/api/auth/logout", { method: "POST" });
```

### Resumen del flujo

```
[Login Form]
     |
     v
[Validacion cliente]
     |
     v
POST /api/auth/login  { email, password }
     |
     v
[Servidor verifica hash de contrasena]
     |
     v
[Servidor emite JWT firmado + refresh token en cookie HttpOnly]
     |
     v
[Cliente guarda access token en memoria]
     |
     v
GET /api/books/favorites
  Authorization: Bearer <token>
     |
     v
[Servidor verifica firma del JWT]
     |
     v
[Respuesta con datos protegidos]
```

---

## Resultado final de las pruebas

```
Test Files: 3 passed
     Tests: 19 passed
  BookCard: 7 pruebas
  HomePage: 6 pruebas
  useFetch: 6 pruebas
```

Para ejecutar las pruebas:

```bash
npm run test          # Ejecuta una vez y muestra resultados
npm run test:watch    # Modo interactivo, re-ejecuta al guardar cambios
```