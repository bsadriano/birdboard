# Use the official Node.js image as the build environment
FROM node:18 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the application
# RUN npm run build

# Use the official Nginx image to serve the static files
# FROM nginx:alpine

# Copy the built files from the build stage
# COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Default command to run Nginx
# CMD ["nginx", "-g", "daemon off;"]

CMD ["npm", "start"]
