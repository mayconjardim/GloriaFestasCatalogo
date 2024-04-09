// 📜 This is the "service-worker.published.js" file of your Blazor PWA.

// 👇 Add these line to accept the message from this library.
self.addEventListener('message', event => {
    if (event.data?.type === 'SKIP_WAITING') self.skipWaiting();
});
...