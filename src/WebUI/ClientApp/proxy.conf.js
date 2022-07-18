const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:5001';

const PROXY_CONFIG = [
  {
    context: [
      "/_configuration",
      "/.well-known",
      "/Identity",
      "/connect",
      "/ApplyDatabaseMigrations",
      "/_framework",
      "/api",
      "/favicon.ico",
      "/ChatRooms",
      "/ChatMessages",
      "/ChatHub",
   ],
    target: target,
    ws: true,
    secure: false,
  }
]

module.exports = PROXY_CONFIG;
