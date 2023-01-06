// Write your JavaScript code.

const anchors = new AnchorJS();
// DOMContentLoaded was tested to be the best place to call anchors.add()
window.addEventListener('DOMContentLoaded', function (event) {
    anchors.options.placement = "left";
    // default to adding h2, h3, h4, h5, and h6
    anchors.add();
})
