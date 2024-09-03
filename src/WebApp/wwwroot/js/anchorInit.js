if (typeof AnchorJS === 'function') {
    const anchors = new AnchorJS();
    document.addEventListener('DOMContentLoaded', function () {
        anchors.options.placement = "left";
        anchors.add('h2:not(.no-anchor), h3:not(.no-anchor)');
    })
}
