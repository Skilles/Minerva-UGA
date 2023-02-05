const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:5459';


const onError = (err, req, resp, target) => {
    console.error(`${err.message}`);
}

console.log(`Proxying to ${target}...`);

module.exports = function (app) {
  const appProxy = createProxyMiddleware({
    target: target,
    // Handle errors to prevent the proxy middleware from crashing when
    // the ASP NET Core webserver is unavailable
    onError: onError,
    secure: false,
    // Uncomment this line to add support for proxying websockets
    //ws: true, 
    headers: {
      Connection: 'Keep-Alive'
    },
    changeOrigin: true,
  });

  app.use('/api', appProxy);
};
