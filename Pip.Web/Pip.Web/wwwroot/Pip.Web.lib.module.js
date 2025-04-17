

function onEnhancedLoad(state) {
    console.log("enhanced load");
}

function onEnhancedNavigationStart(state) {
    console.log("navigation start");

    let p = new Promise(res => {
        state.resolve = res;
    });

    navigation.addEventListener("navigate", e => {
        e.intercept({
            scroll: 'manual',
            handler: () => p
        });
    }, { once: true });

    navigation.navigate(location.href);
}

function onEnhancedNavigationEnd(state) {
    console.log("navigation end")

    state.resolve();
}


export function afterWebStarted(blazor) {

    console.log("after Web Started");

    var state = {};

    blazor.addEventListener("enhancedload", () => onEnhancedLoad(state));

    blazor.addEventListener("enhancednavigationstart", () => onEnhancedNavigationStart(state));

    blazor.addEventListener("enhancednavigationend", () => onEnhancedNavigationEnd(state));
}