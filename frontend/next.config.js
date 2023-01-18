/** @type {import('next').NextConfig} */
const removeImports = require('next-remove-imports')();

const nextConfig = {
  reactStrictMode: true,
  swcMinify: true,
  output: "standalone",

  async rewrites() {
    return [
      {
        source: '/api/User/:id*',
        destination: `http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/identity/api/User/:id*`
      },
      {
        source: '/api/Chat/:id*',
        destination: `http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/direct-messaging/api/Message/to/:id*`
      }
    ]
  }
}

module.exports = removeImports(nextConfig);
