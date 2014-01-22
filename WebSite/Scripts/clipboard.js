define(["config", "ZeroClipboard"], function(config, ZeroClipboard) {
    ZeroClipboard.setDefaults({ moviePath: config.baseUrl + "Scripts/ZeroClipboard.swf" });

    return function(target) {
        return new ZeroClipboard(target);
    };
});