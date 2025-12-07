# Multi-stage build for Vite React app

# Build static assets
FROM node:24-alpine AS builder
WORKDIR /app

# Install dependencies
COPY package.json ./
RUN npm install

# Copy source
COPY . .

ARG VITE_API_BASE_URL
ARG VITE_APP_TITLE
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}
ENV VITE_APP_TITLE=${VITE_APP_TITLE}

# Build the app
RUN npm run build

# Serve with Nginx
FROM nginx:1.27-alpine AS runner

# Copy SPA-friendly nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built assets
COPY --from=builder /app/dist /usr/share/nginx/html

EXPOSE 80

# Install curl for healthcheck
RUN apk add --no-cache curl

# Health check (fails if non-200 or connection refused)
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl -fsS http://127.0.0.1:80/ || exit 1

CMD ["nginx", "-g", "daemon off;"]
