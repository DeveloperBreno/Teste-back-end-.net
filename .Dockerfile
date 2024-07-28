FROM node:18-alpine

WORKDIR /app

# Instala as dependências do projeto
COPY package*.json ./
RUN npm install

# Copia os arquivos do projeto
COPY . .

# Builda o projeto Angular para produção
RUN ng build --prod

# Copia os arquivos buildados para a pasta dist
COPY dist/your-project-name /usr/share/nginx/html

# Instala o Nginx
RUN apk add --no-cache nginx

# Configura o Nginx para servir os arquivos estáticos
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
