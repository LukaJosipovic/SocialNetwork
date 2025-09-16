export function checkDivScrollEnd(elementId, functionName, dotnetReference) {
    var div = document.getElementById(elementId);

    div.addEventListener('scroll', function () {
        var isScrolledToBottom = div.scrollHeight - div.clientHeight <= div.scrollTop + 1;

        if (isScrolledToBottom) {
            dotnetReference.invokeMethodAsync(functionName);
        }
    });
}