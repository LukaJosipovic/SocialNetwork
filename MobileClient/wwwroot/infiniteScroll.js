export function checkDivScrollEnd(elementId, functionName, dotnetReference) {
    var div = document.getElementById(elementId);

    div.addEventListener('scroll', function () {
        var isScrolledToBottom = div.scrollHeight - div.clientHeight <= div.scrollTop + 1;

        if (isScrolledToBottom) {
            dotnetReference.invokeMethodAsync(functionName);
        }
    });
}

export function checkDivScrollTop(elementId, functionName, dotnetReference) {
    var div = document.getElementById(elementId);

    div.addEventListener('scroll', function () {
        if (div.scrollTop <= 1) {
            dotnetReference.invokeMethodAsync(functionName);
        }
    });
}

export function getScrollHeight(elementId) {
    const div = document.getElementById(elementId);
    return div ? div.scrollHeight : 0;
}

export function restoreScrollPosition(elementId, oldHeight) {
    const div = document.getElementById(elementId);
    if (!div) return;

    const newHeight = div.scrollHeight;
    div.scrollTop = newHeight - oldHeight;
}