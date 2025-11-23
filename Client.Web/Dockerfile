# ==========================================================
# ETAPA 1: BUILD (Instala dependencias y compila la aplicación)
# ==========================================================
FROM node:20-alpine AS build

WORKDIR /app

# Copia package.json y package-lock.json para instalar dependencias
# Usa la ruta de tu proyecto: Client.Web
COPY Client.Web/package*.json ./

# Instala todas las dependencias
RUN npm install

# Copia el código fuente restante de Vue
COPY Client.Web/ ./

# Compila la aplicación Vue (genera la carpeta 'dist')
# Las variables como VUE_APP_API_URL son inyectadas aquí
RUN npm run build 

# ==========================================================
# ETAPA 2: RUNTIME (Servir los archivos estáticos con Nginx)
# ==========================================================
FROM nginx:stable-alpine AS final

# Copia los archivos compilados de la etapa 'build' a la carpeta de Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# *** IMPORTANTE: Copia la configuración de Nginx (nginx.conf) ***
# Esto permite que Nginx sepa cómo manejar las rutas de Vue (el fallback a index.html)
COPY Client.Web/nginx.conf /etc/nginx/conf.d/default.conf 

# Exponer el puerto interno de Nginx (80)
EXPOSE 80

# Comando para que Nginx se ejecute en primer plano
CMD ["nginx", "-g", "daemon off;"]