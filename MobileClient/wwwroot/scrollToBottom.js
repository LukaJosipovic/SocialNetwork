//window.scrollToBottom = (elementId) => {
//    const el = document.getElementById(elementId);
//    if (el) {
//        el.scrollTop = el.scrollHeight;
//    }
//};

window.scrollToBottom = (id) => {
    requestAnimationFrame(() => {
        debugger;
        const el = document.getElementById(id);
        if (el) {
            el.scrollTop = el.scrollHeight;
        }
    });
};

window.lockScroll = function (lock) {
    document.body.style.overflow = lock ? 'hidden' : '';
};