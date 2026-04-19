FROM node:20-alpine AS build
WORKDIR /app

ARG VITE_API_URL=http://localhost:5000

COPY package*.json ./
RUN npm ci

COPY . .

RUN echo "export const BASE_URL = '${VITE_API_URL}';" > src/core/config/env.ts

RUN npm run build

FROM nginx:alpine AS runtime
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
