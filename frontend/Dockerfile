FROM node:18 AS build

ARG VITE_APP_API_URL
ENV VITE_APP_API_URL $VITE_APP_API_URL

WORKDIR /app

COPY frontend/package*.json ./

RUN npm install

COPY frontend/ .

RUN npm run build

FROM nginx:alpine

COPY --from=build /app/dist /usr/share/nginx/html
COPY --from=build /app/nginx/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
