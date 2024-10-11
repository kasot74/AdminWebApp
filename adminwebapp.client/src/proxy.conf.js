const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` : 'http://localhost:8987';

//const target = 'http://localhost:8987'

const PROXY_CONFIG = [
  {
    context: [
      "/api",
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
