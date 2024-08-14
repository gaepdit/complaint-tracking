// Write your JavaScript code.

const anchors = new AnchorJS();
// DOMContentLoaded was tested to be the best place to call anchors.add()
document.addEventListener('DOMContentLoaded', function () {
    anchors.options.placement = "left";
    // Add anchors to h2, h3, h4, h5, and h6 elements, but exclude any with the "no-anchor" class.
    anchors.add('h2:not(.no-anchor), h3:not(.no-anchor)');
})
