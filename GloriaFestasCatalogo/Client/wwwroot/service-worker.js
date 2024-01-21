const cacheName = 'my-app-v1.0';

self.addEventListener('install', (event) => {
    // Aqui você pode pré-carregar recursos no cache, se necessário
    event.waitUntil(
        caches.open(cacheName).then((cache) => {
            return cache.addAll([
                '/index.html',
                '/app.js',
                '/styles.css',
            ]);
        })
    );
});

self.addEventListener('activate', (event) => {
    // Limpar caches antigos, se necessário
    event.waitUntil(
        caches.keys().then((keys) => {
            return Promise.all(keys.map((key) => {
                if (key !== cacheName) {
                    return caches.delete(key);
                }
            }));
        })
    );
});

self.addEventListener('fetch', (event) => {
    event.respondWith(
        caches.match(event.request).then((response) => {
            // Se houver uma versão mais recente disponível, força a atualização do cache
            if (navigator.onLine) {
                return fetch(event.request).then((response) => {
                    const cacheCopy = response.clone();
                    caches.open(cacheName).then((cache) => {
                        cache.put(event.request, cacheCopy);
                    });
                    return response;
                });
            }
            // Se não houver uma versão mais recente, retorna a resposta do cache
            return response || fetch(event.request);
        })
    );
});

// Adiciona um ouvinte para mensagens vindas da sua aplicação
self.addEventListener('message', (event) => {
    if (event.data.action === 'skipWaiting') {
        self.skipWaiting();
    }
});
