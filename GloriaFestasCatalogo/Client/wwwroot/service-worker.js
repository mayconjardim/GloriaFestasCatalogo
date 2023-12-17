self.addEventListener('fetch', (event) => {
    // This is a no-op fetch handler, consider removing or optimizing it
    event.respondWith(fetch(event.request));
});