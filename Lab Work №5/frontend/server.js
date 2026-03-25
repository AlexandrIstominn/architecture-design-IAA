const express = require('express');
const { createProxyMiddleware } = require('http-proxy-middleware');
const path = require('path');

const app = express();
const PORT = process.env.PORT || 3000;
const API_URL = process.env.API_URL || 'http://localhost:5000';

// Прокси запросов к API (для работы в Docker и без CORS с одного origin)
app.use('/api', createProxyMiddleware({
  target: API_URL,
  changeOrigin: true,
  onProxyReq: (proxyReq, req, res) => {
    const userId = req.headers['x-user-id'] || '1';
    proxyReq.setHeader('X-User-Id', userId);
  },
}));

app.use(express.static(path.join(__dirname, 'public')));

app.get('*', (req, res) => {
  res.sendFile(path.join(__dirname, 'public', 'index.html'));
});

app.listen(PORT, '0.0.0.0', () => {
  console.log(`Frontend: http://0.0.0.0:${PORT}, API proxy → ${API_URL}`);
});
