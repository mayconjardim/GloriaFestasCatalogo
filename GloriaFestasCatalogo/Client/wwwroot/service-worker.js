const cacheName = 'my-app-v1.0';

self.addEventListener('install', (event) => {
    // Aqui voc� pode pr�-carregar recursos no cache, se necess�rio
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
    // Limpar caches antigos, se necess�rio
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
            // Se houver uma vers�o mais recente dispon�vel, for�a a atualiza��o do cache
            if (navigator.onLine) {
                return fetch(event.request).then((response) => {
                    const cacheCopy = response.clone();
                    caches.open(cacheName).then((cache) => {
                        cache.put(event.request, cacheCopy);
                    });
                    return response;
                });
            }
            // Se n�o houver uma vers�o mais recente, retorna a resposta do cache
            return response || fetch(event.request);
        })
    );
});

// Adiciona um ouvinte para mensagens vindas da sua aplica��o
self.addEventListener('message', (event) => {
    if (event.data.action === 'skipWaiting') {
        self.skipWaiting();
    }
});
