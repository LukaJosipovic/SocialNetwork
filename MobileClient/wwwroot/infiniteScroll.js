export function checkDivScrollEnd(elementId, functionName, dotnetReference) {
    var div = document.getElementById(elementId);

    div.addEventListener('scroll', function () {
        var isScrolledToBottom = div.scrollHeight - div.clientHeight <= div.scrollTop + 1;

        if (isScrolledToBottom) {
            dotnetReference.invokeMethodAsync(functionName);
        }
    });
}

export function checkPostDivScrollEnd(elementId, functionName, dotnetReference) {

    const div = document.getElementById(elementId);

    if (!div)
        return;

    let isLoading = false;

    async function handleScroll() {

        if (isLoading)
            return;

        const threshold = 200;

        const scrollPosition = div.scrollTop + div.clientHeight;
        const triggerPoint = div.scrollHeight - threshold;

        console.log("scrollTop:", div.scrollTop);
        console.log("clientHeight:", div.clientHeight);
        console.log("scrollHeight:", div.scrollHeight);
        console.log("scrollPosition:", scrollPosition);
        console.log("triggerPoint:", triggerPoint);

        if (scrollPosition >= triggerPoint) {

            console.log("LOAD MORE");

            isLoading = true;

            try {
                await dotnetReference.invokeMethodAsync(functionName);
            }
            finally {
                setTimeout(() => {
                    isLoading = false;
                }, 500);
            }
        }
    }

    div.addEventListener('scroll', handleScroll);

    // Initial check
    handleScroll();
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