FROM node:22-alpine AS build
WORKDIR /src
COPY ./package*.json ./
RUN npm install
COPY ./ ./
RUN npm run build -- --configuration=docker

FROM nginx:alpine AS app
WORKDIR /app
COPY --from=build /src/dist/frontend/. /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
