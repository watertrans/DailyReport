const fs = require('fs');
module.exports = {
  devServer: {
    port: '8443',
    public: 'localhost:8443',
    https: {
        key: fs.readFileSync('./certs/localhost.key'),
        cert: fs.readFileSync('./certs/localhost.cert'),
    },
  },
  configureWebpack: {
      devtool: 'source-map',
  },
}